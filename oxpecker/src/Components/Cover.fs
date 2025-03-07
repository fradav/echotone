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
        coverItem = "transition-opacity duration-[3s] ease-in-out opacity-0"
        cover = "absolute block -translate-x-1/2 -translate-y-1/2 top-1/2 left-1/2 object-cover "
    |}

    let getHeight (w: int) (pagemedias: string seq) =
        pagemedias
        |> Seq.tryHead
        |> function
            | None -> w
            | Some x -> (w |> float) * (mapSlugs.[x].height / mapSlugs.[x].width) |> int

    let getMediasCover (page: PagesT.Items) =
        page.data.medias.iv
        |> Seq.filter _.``in``
        |> Seq.collect _.medias
        |> Seq.filter(fun x -> mapSlugs.[x].``type`` = AssetType.Image)

    let getMedias (page: PagesT.Items) =
        page.data.medias.iv
        |> Seq.collect _.medias
        |> Seq.filter(fun x -> mapSlugs.[x].``type`` = AssetType.Image)

    let getVideos (page: PagesT.Items) =
        page.data.medias.iv
        |> Seq.collect _.medias
        |> Seq.filter(fun x -> mapSlugs.[x].``type`` = AssetType.Video)

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
                                img(class' = classes.cover, src = mapSlugs[imgid].thumbnail).style'(coverStyle)
                            }
                }
            }
        }


    [<SolidComponent>]
    let Cover w (page: PagesT.Items) : HtmlElement =
        let pagemedias = getMediasCover page
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
        let zoom, setZoom = createSignal None
        let delayZoom, setDelayZoom = createSignal false
        let timer = new System.Timers.Timer(AutoReset = false)
        createEffect(fun _ ->
            if Option.isSome(zoom()) then
                timer.Interval <- 1.
                setDelayZoom false
                timer.Elapsed.Add(fun _ -> setDelayZoom true)
                timer.Start())
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
                // zoom = {| maxRatio = 5 |}
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
            swiperEl?initialize())

        let pagemedias = getMedias page
        let pagevideos = getVideos page
        pagemedias
        |> List.ofSeq
        |> function
            | [] -> div(class' = classes.noCover)
            | medias ->
                Fragment() {
                    div() {
                        For(each = Array.ofSeq pagevideos) {
                            yield
                                fun vidid index ->
                                    video(
                                        class' = "w-full h-[300px] md:h-[600px] object-contain",
                                        src = mapSlugs[vidid].slug,
                                        controls = true
                                    )
                        }
                    }
                    div() {
                        div(class' = "fixed top-0 left-0 w-screen h-screen duration-500 ease-in-out")
                            .classList(
                                {|
                                    ``hidden`` = zoom() = None
                                    ``bg-black/25 drop-shadow-2xl backdrop-blur-2xl z-500`` =
                                        zoom() |> Option.isSome && delayZoom()
                                |}
                            ) {
                            For(each = Array.ofSeq medias) {
                                yield
                                    fun imgid index ->
                                        img(
                                            class' = "duration-1000 ease-in-out scale-50",
                                            src = mapSlugs[imgid].slug,
                                            onClick = fun _ -> setZoom(None)
                                        )
                                            .classList(
                                                {|
                                                    ``h-11/12 w-11/12 object-contain fixed top-[calc(1/24*100%)] left-[calc(1/24*100%)] block z-1000 scale-none`` =
                                                        zoom() = Some(index()) && delayZoom()
                                                    ``collapse`` = zoom() = None || zoom() <> Some(index())
                                                |}
                                            )

                            }
                        }
                        SwiperContainer(class' = "py-6 md:py-12 w-full") {


                            For(each = Array.ofSeq medias) {
                                yield
                                    fun imgid index ->
                                        SwiperSlide(
                                            class' = "bg-cover bg-center",
                                            onClick = (fun _ -> setZoom(Some(index())))
                                        ) {
                                            img(
                                                class' = "w-full h-[300px] md:h-[600px] object-contain",
                                                src = mapSlugs[imgid].slug
                                            )

                                        }
                            }
                        }

                    }
                }
