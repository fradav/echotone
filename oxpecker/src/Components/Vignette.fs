namespace Components

open Oxpecker.Solid
open Oxpecker.Solid.Imports
open Browser.Types
open Data

[<AutoOpen>]
module Page =
    open Fable.Core.JsInterop

    // formatter un datetime en français sous la forme "le 11 mars à 19h"
    let formatDateTime (datetime: string) =
        let date = datetime.Split("T").[0]
        let time = datetime.Split("T").[1].Split(":")
        let monthNum = int(date.Split("-").[1])
        let monthNames = [|
            "janvier"
            "février"
            "mars"
            "avril"
            "mai"
            "juin"
            "juillet"
            "août"
            "septembre"
            "octobre"
            "novembre"
            "décembre"
        |]
        let month = monthNames.[monthNum - 1] // Subtract 1 because months are 1-based but array is 0-based
        let day = date.Split("-").[2]
        let hour = time.[0]
        let minute = time.[1]
        $"le {day} {month} à {hour}h{minute}"

    [<SolidComponent>]
    let Vignette w (page: PagesT.Items) : HtmlElement =
        let mutable divRef: HTMLDivElement = Unchecked.defaultof<_>

        onMount(fun _ -> observer.observe(divRef))
        div(
            class' =
                "break-inside-avoid break-after-avoid-page duration-1000 ease-in-out mb-10 bg-gray-100 shadow-2xl dark:shadow-zinc-800 rounded-3xl dark:bg-zinc-900"
        )
            .classList(
                createObj [
                    "[&>.unit]:animate-smallbounce"
                    ==> ((page.data.id.iv = "n-u-collectif-residents-2025")
                         || (page.data.id.iv = "geoffrey-badel-residents-2024"))
                ]
            )
            .ref(fun e -> divRef <- e) {
            Cover w page
            div(class' = "p-5 unit", style = $"width: {w}px") {
                h3(class' = "text-gray-500") { page.data.unit.fr.title }
                h4() { page.data.unit.fr.short }
                For(each = page.data.unit.fr.temporal) {
                    yield
                        fun temporal index ->
                            div(class' = "text-gray-400") { "👁️‍🗨️ " + formatDateTime(temporal?datetime) }
                }
            // HyphenatedText() { div(innerHTML = page.data.unit.fr.text) }
            }

        }
