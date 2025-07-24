namespace Components

open Fable.Core
open Fable.Core.JsInterop
open Browser.Types

open Oxpecker.Solid
open Oxpecker.Solid.Imports

open Types
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
            | Some x -> (w |> float) * (mapSlugAssets[x].height / mapSlugAssets[x].width) |> int

    let getWidth (h: int) (media: string) =
        (h |> float) * (mapSlugAssets[media].width / mapSlugAssets[media].height) |> int

    let getMediasCover (page: PagesT.Items) =
        page.data.medias.iv
        |> Seq.filter _.``in``
        |> Seq.collect _.medias
        |> Seq.filter(fun x -> mapSlugAssets[x].``type`` = AssetType.Image)

    let getMedias (page: PagesT.Items) =
        page.data.medias.iv
        |> Seq.filter(_.``inslides`` >> not)
        |> Seq.collect _.medias
        |> Seq.filter(fun x -> mapSlugAssets[x].``type`` = AssetType.Image)

    let getVideos (page: PagesT.Items) =
        let captions =
            page.data.medias.iv
            |> Seq.map _.caption
            |> Seq.filter(fun x -> not(isNullOrUndefined(x)))
        let vids =
            page.data.medias.iv
            |> Seq.collect _.medias
            |> Seq.filter(fun x -> mapSlugAssets[x].``type`` = AssetType.Video)
        if (Seq.isEmpty vids) then
            None, vids
        else if (not(Seq.isEmpty captions)) then
            Some(Seq.head captions), vids
        else
            None, vids

    [<SolidComponent>]
    let SlidingCover coverStyle (medias: string seq) : HtmlElement =
        let current, setCurrent = createSignal 0
        let delay () = System.Random().Next(5000, 8000)

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
                For(each = (medias |> Seq.filter(fun x -> mapSlugAssets[x].thumbnail.IsSome) |> Array.ofSeq)) {
                    yield
                        fun imgid index ->
                            div(class' = classes.coverItem)
                                .classList(
                                    {|
                                        ``opacity-100`` = (index() = current())
                                    |}
                                ) {
                                img(class' = classes.cover, src = mapSlugAssets[imgid].thumbnail.Value)
                                    .style'(coverStyle)
                            }
                }
            }
        }

    let swiperParams = {|
        loop = true
        navigation = true
        grabCursor = true
        autoHeight = true
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
    let Videos (page: PagesT.Items) : HtmlElement =
        let caption, pagevideos = getVideos page
        div(class' = "grid justify-center place-items-center gap-10 my-10") {
            if (caption.IsSome) then
                div() { p(class' = "px-20 text-sm") { string caption.Value } }
            else
                div()
            For(each = Array.ofSeq pagevideos) {
                yield fun vidid index -> video(class' = "max-w-1/2", src = mapSlugAssets[vidid].slug, controls = true)
            }
        }

    [<SolidComponent>]
    let CoverFlow (page: PagesT.Items) : HtmlElement =
        let zoom, setZoom = createSignal None
        let mutable swiperEl = Unchecked.defaultof<HTMLElement>

        onMount(fun _ ->
            Swiper.register()

            JS.Constructors.Object.assign(swiperEl, swiperParams) |> ignore
            swiperEl?initialize())

        let pagemedias = getMedias page
        pagemedias
        |> List.ofSeq
        |> function
            | [] -> div(class' = classes.noCover)
            | medias ->
                Fragment() {
                    div() {
                        div(class' = "fixed top-0 left-0 w-screen h-screen z-1000 duration-500 ease-in-out")
                            .classList(
                                {|
                                    ``collapse`` = zoom() = None
                                    ``bg-black/25 drop-shadow-2xl backdrop-blur-2xl`` = zoom() |> Option.isSome
                                |}
                            ) {
                            For(each = Array.ofSeq medias) {
                                yield
                                    fun imgid index ->
                                        img(
                                            class' = "scale-50 block z-1000 top-0 fixed",
                                            src = mapSlugAssets[imgid].slug,
                                            onClick = fun _ -> setZoom(None)
                                        )
                                            .classList(
                                                {|
                                                    ``duration-1000 ease-in-out h-11/12 w-11/12 object-contain top-[calc(1/24*100%)] left-[calc(1/24*100%)] scale-none`` =
                                                        zoom() = Some(index())
                                                    ``duration-500 ease-in-out collapse -translate-y-9/12`` =
                                                        zoom() = None || zoom() <> Some(index())
                                                |}
                                            )

                            }
                        }
                        SwiperContainer(class' = "py-6 md:py-12 w-full").ref(fun el -> swiperEl <- el) {


                            For(each = Array.ofSeq medias) {
                                yield
                                    fun imgid index ->
                                        SwiperSlide(
                                            class' = "bg-cover bg-center",
                                            onClick = (fun _ -> setZoom(Some(index())))
                                        ) {
                                            img(
                                                class' = "w-full h-[300px] md:h-[600px] object-contain",
                                                src = mapSlugAssets[imgid].slug
                                            )

                                        }
                            }
                        }
                    }
                }
