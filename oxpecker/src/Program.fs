module Oxpecker

open Browser
open Oxpecker.Solid
open Oxpecker.Solid.Router
open Fable.Core
open Fable.Core.JsInterop
open Oxpecker.Solid.Meta

open Data
open App

let private classes: CssModules.src.index = importDefault "./index.module.css"
importAll "../index.css"

// let initFlowbite: unit -> unit = import "initFlowbite" "flowbite"
// importSideEffects "flowbite"

[<SolidComponent>]
let taggedRoute (tag: Tag) =
    Route(path = navItems.[tag].slug, component' = App(taggedPages tag))

[<SolidComponent>]
let NotFound () : HtmlElement =

    Fragment() {
        Title() { "404 - Not Found" }

        main () {

            div () {
                h1 () { "404 - Not Found" }
                p () { "Sorry, the page you are looking for does not exist." }
            }
        }
    }

let slugToTitle = mapLocationToTag >> (fun tag -> navItems[tag].title)

[<SolidComponent>]
let Layout (rootprops: RootProps) : HtmlElement =
    let location = useLocation ()
    let currentTag, setCurrentTag = location.pathname |> slugToTitle |> createSignal

    createEffect (fun () ->
        printfn "Location changed to %s" location.pathname

        if location.pathname.Length > 0 then
            location.pathname |> slugToTitle |> setCurrentTag)


    Fragment() {
        Title() { $"Échotone - {currentTag ()}" }
        rootprops.children
    }


// HMR doesn't work in Root for some reason
[<SolidComponent>]
let appRouter () =

    MetaProvider() {
        Router(base' = baseR, root = Layout) {
            Route(path = "/", component' = App(taggedPages Tag.Accueil))
            taggedRoute Tag.Programmation
            taggedRoute Tag.Atelier
            taggedRoute Tag.Boutique
            Route(path = "/about", component' = App AboutP)
            Route(path = "/contact", component' = App ContactP)
            // Add a catch-all route
            Route(path = "*", component' = NotFound)
        }
    }

render (appRouter, document.getElementById "root")
