namespace Components

open Oxpecker.Solid
open Oxpecker.Solid.Router

open Types
open Data
open State

[<AutoOpen>]
module Masonry =
    open Browser.Types

    let rec transpose xs = [
        match xs with
        | [] -> failwith "cannot transpose a 0-by-n matrix"
        | [] :: xs -> yield! xs
        | xs ->
            let xsn = List.filter (List.isEmpty >> not) xs
            yield List.map List.head xsn
            yield! transpose(List.map List.tail xsn)
    ]

    let makeMasonry<'T> (cols: int) (getH: 'T -> float) (l: 'T list) : 'T option array =
        let heights = Array.zeroCreate cols
        let lists = Array.create cols []
        List.iter
            (fun x ->
                let minIndex = Array.findIndex (fun x -> x = Array.min heights) heights
                heights[minIndex] <- heights[minIndex] + getH x
                lists[minIndex] <- x :: lists.[minIndex])
            l
        lists
        |> Array.collect(List.map Some >> (fun x -> None :: x) >> List.rev >> List.toArray)

    [<SolidComponent>]
    let Masonry (pages: PagesT.Items seq) : HtmlElement =
        let len = Seq.length pages
        // Return a cleanup function for when the element is removed
        let listRef: HTMLAnchorElement array =
            Array.init len (fun _ -> Unchecked.defaultof<HTMLAnchorElement>)
        let starterList = pages |> Seq.mapi(fun i page -> i, page) |> Array.ofSeq
        let heights, setHeights = createSignal(Array.zeroCreate len)

        createEffect(fun () ->
            if breakColumns[store.breakpoint] > 0 then
                listRef |> Array.map(_.offsetHeight) |> setHeights)

        div(class' = "flex items-center justify-center") {
            div(class' = "gap-10 duration-1000 ease-in-out", style = "column-fill: auto")
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
                        (starterList
                         |> List.ofArray
                         |> makeMasonry (breakColumns[store.breakpoint]) (fst >> Array.get(heights())))
                ) {
                    yield
                        fun pagesome index ->
                            match pagesome with
                            | None -> div(class' = "content break-after-column")
                            | Some pagei ->
                                let page = snd pagei
                                let slug = page.data.id.iv
                                let href = (navTaggedItems[mapPageSlugToTag[slug]] |> fst |> _.slug) + "/" + slug
                                div(class' = "content").ref(fun el -> listRef[index()] <- el) {
                                    A(href = href) { Vignette 350 page }
                                }
                }
            }
        }
