namespace Components

open Oxpecker.Solid

open Site.Imports
open Data

[<AutoOpen>]
module Masonry =
    let private classes =
        {| noCover = "w-100 bg-amber-300"
           masonry = "flex flex-wrap justify-center gap-4 m-5"
           slidingContainer = "relative w-[400px]"
           slidingCover = "relative h-56 overflow-hidden rounded-lg md:h-96"
           coverItem = "duration-1000 ease-in-out hover:blur-2xl"
           cover = "absolute block -translate-x-1/2 -translate-y-1/2 top-1/2 left-1/2 object-fill" |}

    [<SolidComponent>]
    let pageCoverSlugs (page: PagesT.Items) =
        let pagemedias = page.data.medias.iv |> Seq.filter _.``in`` |> Seq.collect _.medias

        let ratio =
            pagemedias
            |> Seq.tryHead
            |> Option.map (fun x -> mapSlugs.[x].height / mapSlugs.[x].width)

        pagemedias
        |> List.ofSeq
        |> function
            | [] -> div (class' = classes.noCover) { "No cover" }
            | medias ->
                let height = int (400. * (ratio |> Option.defaultValue 1.0))

                div(class' = classes.slidingContainer).data ("carousel", "slide") {
                    div (class' = classes.slidingCover) {
                        For(each = Array.ofSeq medias) {
                            yield
                                fun imgid index ->
                                    div(class' = classes.coverItem)
                                        .data("carousel-item", "")
                                        .classList ({| hidden = true |}) {
                                        img (
                                            class' = classes.cover,
                                            src = $"medias/{mapSlugs[imgid].slug}",
                                            style = $"width = 400px; height = {height}px"
                                        )
                                    }
                        }
                    }
                }

    [<SolidComponent>]
    let Masonry (pages: Data.PagesT.Items seq) : HtmlElement =
        onMount (fun () -> flowbite.initCarousels ())

        div (class' = classes.masonry) {
            For(each = Array.ofSeq pages) { yield fun (page: Data.PagesT.Items) index -> pageCoverSlugs page }
        }
