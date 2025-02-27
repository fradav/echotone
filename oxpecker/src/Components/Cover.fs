namespace Components

open Oxpecker.Solid

open Data

[<AutoOpen>]
module Cover =
    let private classes = {|
        noCover = "bg-amber-300"
        slidingContainer = "hover:blur-xs @xs:max-w-80"
        slidingCover = "relative h-56 overflow-hidden rounded-lg"
        coverItem = "duration-1000 ease-in-out opacity-0"
        // I want a fade out in the coverItem
        cover = "absolute block -translate-x-1/2 -translate-y-1/2 top-1/2 left-1/2 object-fill "
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
        let delay () = System.Random().Next(1500, 3000)

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
                                img(class' = classes.cover, src = $"medias/{mapSlugs[imgid].slug}").style'(coverStyle)
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
            | [] -> div(class' = classes.noCover).style'(coverStyle) { page.data.unit.fr.title }
            | medias -> SlidingCover coverStyle medias
