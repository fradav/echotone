namespace Components

open Oxpecker.Solid
open Oxpecker.Solid.Imports
open Browser.Types
open Data

[<AutoOpen>]
module Page =

    [<SolidComponent>]
    let Vignette w (page: PagesT.Items) : HtmlElement =
        let mutable divRef: HTMLDivElement = Unchecked.defaultof<_>

        onMount(fun _ -> observer.observe(divRef))
        div(
            class' =
                "break-inside-avoid break-after-avoid-page duration-1000 ease-in-out mb-10 bg-gray-100 shadow-2xl dark:shadow-cyan-800 rounded-3xl dark:bg-blue-950"
        )
            .ref(fun e -> divRef <- e) {
            Cover w page
            div(class' = "p-5", style = $"width: {w}px") {
                h3(class' = "text-gray-500") { page.data.unit.fr.title }
                h4() { page.data.unit.fr.short }
            // HyphenatedText() { div(innerHTML = page.data.unit.fr.text) }
            }

        }
