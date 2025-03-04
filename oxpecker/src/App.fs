module App

open Fable.Core.JsInterop

open Oxpecker.Solid
open Browser

open Data
open Layout
open Oxpecker.Solid.Imports
open Components
open Oxpecker.Solid.Router

[<SolidComponent>]
let SolidAboutContact (unit: AboutContact) : HtmlElement =
    Fragment() {
        // h1() { unit.title }
        h2() { unit.short }
        HyphenatedText() { div(innerHTML = unit.text) }

    // A(href = "/", class' = "block text-right") { "Retour" }
    }

[<SolidComponent>]
let SolidUnit (unit: Unit) : HtmlElement =
    Fragment() {
        h1() { unit.title }
        h2() { unit.short }
        HyphenatedText() { div(innerHTML = unit.text) }
    // A(href = "/", class' = "block text-right") { " Retour " }
    }

[<SolidComponent>]
let AboutP () : HtmlElement = SolidAboutContact about

[<SolidComponent>]
let ContactP () : HtmlElement = SolidAboutContact contact

let filterPages (tag: string) =
    pages.items |> Seq.filter(fun x -> x.data.unit.fr.tags |> Seq.contains tag)

let getPages (tag: string) =
    filterPages tag |> Seq.map(fun x -> x.data.unit.fr) |> Array.ofSeq

[<SolidComponent>]
let taggedPages (tag: Tag) () : HtmlElement =
    Masonry(filterPages navItems[tag].cmstag)


[<SolidComponent>]
let makePage (page: PagesT.Items) : HtmlElement =
    Fragment() {
        CoverFlow page
        SolidUnit page.data.unit.fr
    }


[<SolidComponent>]
let App (page: unit -> HtmlElement) () : HtmlElement =
    let delay, setDelay = createSignal true
    let handleScroll (e: Types.Event) =
        window.pageYOffset |> setStore.Path.Map(_.scrolled).Update

    onMount(fun () ->
        let timer = new System.Timers.Timer(1., AutoReset = false)
        timer.Elapsed.Add(fun _ -> setDelay false)
        timer.Start()
        // On page load or when changing themes, best to add inline in `head` to avoid FOUC
        let forceTheme = localStorage["theme"]
        printfn "forceTheme: %A" forceTheme
        document.documentElement.classList.toggle(
            "dark",
            (isIn "theme" localStorage && forceTheme = "dark")
            || (not(isIn "theme" localStorage)
                && window.matchMedia("(prefers-color-scheme: dark)").matches)
        )
        |> function
            | true -> localStorage["theme"] <- "dark"
            | false -> localStorage["theme"] <- "light"

        Dom.document.addEventListener("scroll", handleScroll)
        breakQueries
        |> Seq.iter(fun (breakpoint, query) ->
            let mql = window.matchMedia(query)
            if mql.matches then
                setStore.Path.Map(_.screenType).Update(breakpoint)
            mql.addEventListener(
                "change",
                fun e ->
                    if (e?matches) then
                        setStore.Path.Map(_.screenType).Update(breakpoint)
            ))
        |> ignore)
    onCleanup(fun () ->
        Dom.document.removeEventListener("scroll", handleScroll)
        breakQueries
        |> Seq.iter(fun (breakpoint, query) ->
            let mql = window.matchMedia(query)
            mql.removeEventListener("change", fun _ -> ()))
        |> ignore)

    Fragment() {
        Header()
        div(class' = "py-10 min-h-screen duration-1000 ease-in-out").classList({| ``opacity-0`` = delay() |}) { page() }
        Footer()
    }

[<SolidComponent>]
let Loading () : HtmlElement =
    Fragment() {
        Header()
        div(class' = "py-10 min-h-screen") { h3() { "Loading..." } }
        Footer()
    }
