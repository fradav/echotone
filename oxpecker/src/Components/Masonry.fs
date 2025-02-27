namespace Components

open Browser.Dom
open Browser.MediaQueryListExtensions

open Oxpecker.Solid

[<AutoOpen>]
module Masonry =
    let private classes = {|
        masonry = "grid gap-4"
        masonryColumn = "grid gap-10 place-self-center h-full"
    |}

    type Breakpoint = { query: string; columns: int }
    let private masonryBreakpoints = [|
        {
            query = "(min-width: 1920px)"
            columns = 4
        }
        {
            query = "(min-width: 1536px)"
            columns = 3
        }
        {
            query = "(min-width: 1280px) and (max-width: 1536px)"
            columns = 3
        }
        {
            query = "(min-width: 1024px) and (max-width: 1280px)"
            columns = 2
        }
        {
            query = "(min-width: 768px) and (max-width: 1024px)"
            columns = 2
        }
        {
            query = "(max-width: 768px)"
            columns = 1
        }
    |]

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
        |> Array.map List.rev

    [<SolidComponent>]
    let Masonry (pages: Data.PagesT.Items seq) : HtmlElement =
        let columns, setColumns = createSignal 3

        createEffect(fun () -> printfn "Masonry columns: %i" (columns()))
        onMount(fun () ->
            masonryBreakpoints
            |> Array.iter(fun { query = query; columns = columns } ->
                let mql = window.matchMedia(query)
                if mql.matches then
                    setColumns columns
                mql.addEventListener("change", fun _ -> setColumns columns)))

        // let masonryExemple =
        //     [ 49; 378; 204; 398; 150; 400; 329; 90; 329; 500; 988; 200 ]
        //     |> List.ofSeq
        //     |> makeMasonry 4 id
        //     |> printfn "%A"

        div(class' = classes.masonry, style = $"grid-template-columns: repeat({columns()}, minmax(0, 1fr))") {
            For(each = (pages |> List.ofSeq |> makeMasonry (columns()) (getMedias >> getHeight 400))) {
                yield
                    fun col index ->
                        div(class' = classes.masonryColumn) {
                            For(each = Array.ofList col) { yield fun page index -> Cover 400 page }
                        }
            }
        }
