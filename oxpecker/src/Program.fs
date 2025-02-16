open Browser
open App
open Oxpecker.Solid
open Oxpecker.Solid.Router
open Fable.Core
open Fable.Core.JsInterop

importAll "./index.scss"

// import import.meta.env.MODE from vite config

[<Global("import.meta.env.BASE_URL")>]
let baseR: string = jsNative

printfn "baseR: %s" baseR

[<SolidComponent>]
let taggedRoute (tag: Tag) =
    Route(path = navItems.[tag].slug, component' = taggedPages tag)

// HMR doesn't work in Root for some reason
[<SolidComponent>]
let appRouter () =
    // Router(base' = baseR) {
    Router(base' = baseR) {
        Route(path = "/", component' = App)
        taggedRoute Tag.Programmation
        taggedRoute Tag.Atelier
        taggedRoute Tag.Boutique
        Route(path = "/about", component' = About)
        Route(path = "/contact", component' = Contact)
    }

render (appRouter, document.getElementById "root")
