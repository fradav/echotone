module App

open Oxpecker.Solid
open Oxpecker.Solid.Router
// open Fable.Core.JsInterop

open Data
open Layout

// let initFlowbite: unit -> unit = import "initFlowbite" "flowbite"


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
        div () { For(each = getPages navItems[tag].cmstag) { yield fun (page: Unit) index -> SolidUnit page } }
    }


[<SolidComponent>]
let App (page: unit -> HtmlElement) () : HtmlElement =
    // onMount initFlowbite

    Fragment() {
        Header()
        div (class' = "mt-30") { page () }
    }
