module App

open Fable.Core.JsInterop
open Browser

open Oxpecker.Solid
open Oxpecker.Solid.Router

open Types
open Data
open State
open Layout
open Oxpecker.Solid.Imports
open Components

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
        HyphenatedText() { div(innerHTML = if isNull unit.text then "" else (string unit.text)) }
    // A(href = "/", class' = "block text-right") { " Retour " }
    }

[<SolidComponent>]
let AboutP () : HtmlElement = SolidAboutContact about

[<SolidComponent>]
let ContactP () : HtmlElement = SolidAboutContact contact

[<SolidComponent>]
let taggedPages (taggedtopic: TaggedTopic) () : HtmlElement = Masonry(getPagesForTopic taggedtopic)

[<SolidComponent>]
let makePage (page: PagesT.Items) : HtmlElement =
    Fragment() {
        CoverFlow page
        SolidUnit page.data.unit.fr
    }

let delay, setDelay = createSignal false
let transition, setTransition = createSignal false
let newRoute, setNewRoute = createSignal ""

[<SolidComponent>]
let App (page: unit -> HtmlElement) () : HtmlElement =

    let navigate = useNavigate()
    let location = useLocation()
    // useBeforeLeave(fun e ->
    //     printfn "Leaving %s for %s" location.pathname (newRoute())
    //     if not(transition()) then
    //         setTransition true
    //         e.preventDefault()
    //         setNewRoute(string e.``to``)
    //         setDelay true
    //     else
    //         setTransition false)

    createEffect(fun () ->
        if store.breakpoint = Xs || store.breakpoint = Sm then
            setStore.Path.Map(_.screenType).Update(Mobile)
        else
            setStore.Path.Map(_.screenType).Update(Desktop))

    let handleScroll (e: Types.Event) =
        window.pageYOffset |> setStore.Path.Map(_.scrolled).Update

    onMount(fun () ->
        // window.setTimeout((fun () -> setDelay false), 500) |> ignore

        Dom.document.addEventListener("scroll", handleScroll)
        breakQueries
        |> Seq.iter(fun (breakpoint, query) ->
            let mql = window.matchMedia(query)
            if mql.matches then
                setStore.Path.Map(_.breakpoint).Update(breakpoint)
            mql.addEventListener(
                "change",
                fun e ->
                    if (e?matches) then
                        setStore.Path.Map(_.breakpoint).Update(breakpoint)
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
        div(
            class' = "py-10 min-h-screen duration-1000 ease-in-out",
            onTransitionEnd = fun e -> ()
        // printfn "Transition: endded at %s, newRoute is %s" location.pathname (newRoute())
        // if
        //     (e.target) = e.currentTarget
        //     && (location.pathname <> newRoute() && location.pathname <> (newRoute() + "/"))
        // then
        //     navigate.Invoke(newRoute())

        )

            .classList({| ``opacity-0`` = delay() |}) {
            page()
        }
        Footer()
    }

[<SolidComponent>]
let Loading () : HtmlElement =
    Fragment() {
        Header()
        div(class' = "py-10 min-h-screen") { h3() { "Loading..." } }
        Footer()
    }
