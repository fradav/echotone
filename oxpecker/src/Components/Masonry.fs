namespace Components

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Data
open Browser.Types
open Browser
open Fable.Core

[<AutoOpen>]
module Masonry =
    let private classes = {|
        // masonry = "mx-auto flex gap-10 justify-center mb-30"
        // masonryColumn = "flex flex-col flex-nowrap gap-8"
        masonry = "gap-10 duration-1000 ease-in-out"
    |}

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

    // [<SolidComponent>]
    // let Masonry (pages: Data.PagesT.Items seq) : HtmlElement =
    //     div(class' = classes.masonry) {
    //         For(
    //             each =
    //                 (pages
    //                  |> List.ofSeq
    //                  |> makeMasonry (breakColumns[store.screenType]) (getMedias >> getHeight 400))
    //         ) {
    //             yield
    //                 fun col index ->
    //                     div(class' = classes.masonryColumn) {
    //                         For(each = Array.ofList col) {
    //                             yield fun page index -> A(href = page.data.id.iv) { Vignette 300 page }
    //                         }
    //                     }
    //         }
    //     }


    [<SolidComponent>]
    let Masonry (pages: Data.PagesT.Items seq) : HtmlElement =
        // Return a cleanup function for when the element is removed

        div(class' = "flex items-center justify-center") {
            div(class' = classes.masonry)
                .classList(
                    {|
                        ``columns-1`` = breakColumns[store.screenType] = 1
                        ``columns-2`` = breakColumns[store.screenType] = 2
                        ``columns-3`` = breakColumns[store.screenType] = 3
                        ``columns-4`` = breakColumns[store.screenType] = 4
                    |}
                ) {
                For(
                    each =
                        (Seq.concat(
                            seq {
                                pages
                                pages
                                pages
                            }
                         )
                         |> List.ofSeq
                         |> makeMasonry (breakColumns[store.screenType]) (getMedias >> getHeight 400))
                ) {
                    yield fun page index -> A(href = page.data.id.iv) { Vignette 350 page }
                }
            }
        }
