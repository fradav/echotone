namespace Components

open Fable.Core
open Fable.Core.JsInterop
open Browser
open Browser.Types

open Oxpecker.Solid

open Types
open Data
open State
open Oxpecker.Solid.Imports
open System



[<AutoOpen>]
module Page =
    open Fable.Core.JsInterop

    type DateTime with
        [<Emit("$0.toLocaleDateString(undefined, $1)")>]
        member this.toLocaleDateString(options: obj) : string = jsNative

        [<Emit("$0.toLocaleTimeString(undefined, $1)")>]
        member this.toLocaleTimeString(options: obj) : string = jsNative


    let formatterDateOptions = {|
        weekday = "long"
        day = "numeric"
        month = "long"
        year = "numeric"
    |}

    let formatterTimeOptions = {|
        weekday = "long"
        day = "numeric"
        month = "long"
        year = "numeric"
        hour = "numeric"
        minute = "numeric"
    |}

    let formatDate (d: PagesT.Items.Data.Unit.Fr.Temporal) =
        match (d.datetime: obj), (d.date: obj) with
        | null, null -> None
        | dt, null -> Some $"{DateTime.Parse(string dt).toLocaleTimeString(formatterTimeOptions)}"
        | null, dt -> Some $"{DateTime.Parse(string dt).toLocaleDateString(formatterDateOptions)}"
        | _ -> None

    let formatSimpleDate (d: PagesT.Items.Data.Unit.Fr.Temporal) (date: string option) =
        match date, (d.description: obj) with
        | Some dt, des when (des |> isNull |> not) && (string des).Length > 0 -> Some $"{string des}, le {dt}"
        | Some dt, _ -> Some dt
        | None, des when (string des).Length > 0 -> des |> string |> Some
        | _ -> None

    let formatRange (dp: PagesT.Items.Data.Unit.Fr.Temporal array) =
        printfn "%A" dp
        match dp[0..1] with
        | [| d1; d2 |] ->
            let d1 = formatDate d1
            let d2 = formatDate d2
            match d1, d2 with
            | Some d1, Some d2 -> Some $"Du {d1} au {d2}"
            | _ -> None
        | _ -> None

    [<SolidComponent>]
    let ListDates (dates: PagesT.Items.Data.Unit.Fr.Temporal array) : HtmlElement =
        Fragment() {
            For(each = (dates |> Array.choose(fun d -> formatSimpleDate d (formatDate d)))) {
                yield
                    fun temporal index ->
                        div(class' = "text-gray-400 temporal") {
                            // "👁️‍🗨️ " + (temporal?datetime: DateTime).toLocaleDateString(formaDatetOptions)
                            "👁️‍🗨️ " + temporal
                        }
            }
        }

    [<SolidComponent>]
    let Vignette w (page: PagesT.Items) : HtmlElement =
        let mutable divRef: HTMLDivElement = Unchecked.defaultof<_>

        onMount(fun _ -> observer.observe(divRef))
        div(
            class' =
                "break-inside-avoid duration-1000 ease-in-out mb-10 bg-gray-100 shadow-2xl dark:shadow-zinc-800 rounded-3xl dark:bg-zinc-900"
        )
            .classList(
                {|
                    // ``animate-pulse`` = isToBounce page
                |}
            )
            .ref(fun e -> divRef <- e) {
            Cover w page
            div(class' = "p-5 has-temporal:animate-smallbounce", style = $"width: {w}px")
                .classList(
                    {|
                        ``animate-strongpulse`` = isToBounce page
                    |}
                ) {
                h3(class' = "text-gray-500") { page.data.unit.fr.title }
                h4() { page.data.unit.fr.short }
                Switch() {
                    Match(when' = hasTemporalRange page) {
                        div(class' = "text-gray-400 temporal") {
                            // "👁️‍🗨️ " + (temporal?datetime: DateTime).toLocaleDateString(formaDatetOptions)
                            "👁️‍🗨️ " + (formatRange page.data.unit.fr.temporal).Value
                        }
                        ListDates(page.data.unit.fr.temporal[2..])
                    }
                    Match(when' = (page |> hasTemporalRange |> not)) { ListDates(page.data.unit.fr.temporal) }
                }
            // HyphenatedText() { div(innerHTML = page.data.unit.fr.text) }
            }
        }
