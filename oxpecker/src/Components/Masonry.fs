namespace Components

open Oxpecker.Solid
open Oxpecker.Solid.Router

open Types
open Data
open State

[<AutoOpen>]
module Masonry =

    let makeMasonry<'T> (cols: int) (getH: 'T -> int) (l: 'T list) =
        List.fold
            (fun (heights, acc) x ->
                let minIndex = Array.findIndex (fun x -> x = Array.min heights) heights
                let newHeights =
                    Array.mapi (fun i h -> if i = minIndex then h + getH x else h) heights
                let newLists = Array.mapi (fun i ol -> if i = minIndex then x :: ol else ol) acc
                newHeights, newLists)
            (Array.zeroCreate cols, Array.create cols [])
            l
        |> snd
        // |> Array.map List.rev
        |> Array.collect(List.rev >> List.toArray)

    [<SolidComponent>]
    let Masonry (pages: PagesT.Items seq) : HtmlElement =
        // Return a cleanup function for when the element is removed

        div(class' = "flex items-center justify-center") {
            div(class' = "gap-10 duration-1000 ease-in-out")
                .classList(
                    {|
                        ``columns-1`` = breakColumns[store.breakpoint] = 1
                        ``columns-2`` = breakColumns[store.breakpoint] = 2
                        ``columns-3`` = breakColumns[store.breakpoint] = 3
                        ``columns-4`` = breakColumns[store.breakpoint] = 4
                    |}
                ) {
                For(
                    each =
                        (Seq.concat(seq { pages })
                         |> List.ofSeq
                         |> makeMasonry (breakColumns[store.breakpoint]) (getMedias >> getHeight 400))
                ) {
                    yield
                        fun page index ->
                            let slug = page.data.id.iv
                            let href = (navTaggedItems[mapPageSlugToTag[slug]] |> fst |> _.slug) + "/" + slug
                            A(href = href) { Vignette 350 page }
                }
            }
        }
