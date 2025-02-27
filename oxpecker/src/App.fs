module App

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Browser

open Data
open Layout
open Components

[<SolidComponent>]
let SolidAboutContact (unit: AboutContact) : HtmlElement =
    Fragment() {
        h1 () { unit.title }
        h2 () { unit.short }
        div (innerHTML = unit.text)
        A(href = "/", class' = "block text-right") { "Retour" }
    }

[<SolidComponent>]
let SolidUnit (unit: Unit) : HtmlElement =
    Fragment() {
        h1 () { unit.title }
        h2 () { unit.short }
        div (innerHTML = unit.text)
        A(href = "/", class' = "block text-right") { " Retour " }
    }

[<SolidComponent>]
let AboutP () : HtmlElement = SolidAboutContact about

[<SolidComponent>]
let ContactP () : HtmlElement = SolidAboutContact contact

let filterPages (tag: string) =
    pages.items |> Seq.filter (fun x -> x.data.unit.fr.tags |> Seq.contains tag)

let getPages (tag: string) =
    filterPages tag |> Seq.map (fun x -> x.data.unit.fr) |> Array.ofSeq

[<SolidComponent>]
let taggedPages (tag: Tag) () : HtmlElement =
    Fragment() {
        h1 () { navItems[tag].title }
        Masonry(filterPages navItems[tag].cmstag)
    // div () { For(each = getPages navItems[tag].cmstag) { yield fun (page: Unit) index -> SolidUnit page } }
    }


[<SolidComponent>]
let App (page: unit -> HtmlElement) () : HtmlElement =
    let mutable divRef: Types.HTMLDivElement = Unchecked.defaultof<_>
    let scrolled, setScrolled = createSignal 0.0

    let handleScroll (e: Types.Event) =
        printfn "scrolling to %f" window.pageYOffset
        window.pageYOffset |> setScrolled

    onMount (fun () -> Dom.document.addEventListener ("scroll", handleScroll) |> ignore)
    onCleanup (fun () -> Dom.document.removeEventListener ("scroll", handleScroll) |> ignore)

    Fragment() {
        Header(scrolled)
        div(class' = "mt-40").ref (fun el -> divRef <- el) { page () }
    }
