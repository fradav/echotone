namespace Components

open Oxpecker.Solid
open Oxpecker.Solid.Imports

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open Data

[<AutoOpen>]
module Cover =
    let private classes = {|
        noCover = "bg-amber-300"
        slidingContainer = "hover:blur-xs @xs:max-w-80"
        slidingCover = "relative h-56 overflow-hidden"
        coverItem = "duration-1000 ease-in-out opacity-0"
        cover = "absolute block -translate-x-1/2 -translate-y-1/2 top-1/2 left-1/2 object-cover "
    |}

    let getHeight (w: int) (pagemedias: string seq) =
        pagemedias
        |> Seq.tryHead
        |> function
            | None -> w
            | Some x -> (w |> float) * (mapSlugs.[x].height / mapSlugs.[x].width) |> int

    let getMedias (page: PagesT.Items) =
        page.data.medias.iv |> Seq.filter _.``in`` |> Seq.collect _.medias

    [<SolidComponent>]
    let SlidingCover coverStyle (medias: string seq) : HtmlElement =
        let current, setCurrent = createSignal 0
        let delay () = System.Random().Next(3000, 5000)

        let timer = new System.Timers.Timer(AutoReset = false)
        timer.Elapsed.Add(fun _ ->
            let nextItem = (current() + 1) % (Seq.length medias)
            setCurrent nextItem
            timer.Interval <- delay()
            timer.Start())

        onMount(fun _ ->
            timer.Interval <- delay()
            timer.Start())

        div(class' = classes.slidingContainer)
            // .data("carousel", if started() then "slide" else "none")
            .style'(coverStyle) {
            div(class' = classes.slidingCover).style'(coverStyle) {
                For(each = Array.ofSeq medias) {
                    yield
                        fun imgid index ->
                            div(class' = classes.coverItem)
                                .classList(
                                    {|
                                        ``opacity-100`` = (index() = current())
                                    |}
                                ) {
                                img(class' = classes.cover, src = $"medias/thumbnails/{mapSlugs[imgid].slug}")
                                    .style'(coverStyle)
                            }
                }
            }
        }


    [<SolidComponent>]
    let Cover w (page: PagesT.Items) : HtmlElement =
        let pagemedias = getMedias page
        let height = getHeight w pagemedias

        let coverStyle = {|
            width = $"{w}px"
            height = $"{height}px"
        |}

        pagemedias
        |> List.ofSeq
        |> function
            | [] -> div(class' = classes.noCover).style'(coverStyle)
            | medias -> SlidingCover coverStyle medias

    [<SolidComponent>]
    let CoverFlow (page: PagesT.Items) : HtmlElement =
        let zoom, setZoom = createSignal false
        onMount(fun _ ->
            Swiper.register()
            let swiperEl = document.querySelector("swiper-container")
            let swiperParams = {|
                loop = true
                navigation = true
                autoHeight = true
                grabCursor = true
                centeredSlides = true
                pagination = {| dynamicBullets = true |}
                mousewheel = true
                zoom = {| maxRatio = 5 |}
                breakpoints = {|
                    ``320`` = {|
                        slidesPerView = "1"
                        spaceBetween = 0
                    |}
                    ``640`` = {|
                        slidesPerView = "1.3"
                        spaceBetween = 2
                    |}
                    ``768`` = {|
                        slidesPerView = "1.5"
                        spaceBetween = 5
                    |}
                    ``1024`` = {|
                        slidesPerView = "1.6"
                        spaceBetween = 10
                    |}
                    ``1280`` = {|
                        slidesPerView = "1.7"
                        spaceBetween = 15
                    |}
                    ``1536`` = {|
                        slidesPerView = "1.8"
                        spaceBetween = 20
                    |}
                |}
            |}
            JS.Constructors.Object.assign(swiperEl, swiperParams) |> ignore
            printfn "Swiper initialized %A" swiperEl
            swiperEl?initialize())

        let pagemedias = getMedias page

        pagemedias
        |> List.ofSeq
        |> function
            | [] -> div(class' = classes.noCover)
            | medias ->
                SwiperContainer(class' = "py-6 md:py-12 w-full") {


                    For(each = Array.ofSeq medias) {
                        yield
                            fun imgid index ->
                                SwiperSlide(
                                    class' = "bg-cover bg-center"
                                // onClick = (fun _ -> setZoom(not(zoom()))
                                ) {
                                    img(
                                        class' = "w-full h-[300px] md:h-[600px] object-contain",
                                        src = $"medias/{mapSlugs[imgid].slug}"
                                    )
                                        .classList(
                                            {|
                                                ``scale-150 block z-1 fixed top-0 left-0`` = zoom()
                                            |}
                                        )

                                }
                    }
                }
