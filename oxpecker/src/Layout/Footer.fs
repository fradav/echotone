namespace Layout

open Browser.Types
open Oxpecker.Solid
open Browser.Dom
open Browser
open Oxpecker.Solid.Router
open Fable.Core
open Fable.Core.JsInterop

open Data
open Components

[<AutoOpen>]
module Footer =
    let private classes = {|
        footerbar =
            "sticky py-8 border-t-2 b-0 border-gray-300 hocus:backdrop-opacity-80 hocus:translate-0 duration-500 ease-in-out flex flex-wrap justify-around items-center bottom-0 w-screen backdrop-blur-lg z-index-500  bg-gray-100/50 text-gray-700 p-4 dark:bg-gray-800/50 dark:text-white"
    |}
    // let newLetterSubscribe: unit -> HtmlElement =
    //     importDefault "../Components/NewsLetter.jsx"

    let mailIcon =
        html
            """<svg class="w-10 h-10" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
  <path d="M1.5 8.67v8.58a3 3 0 0 0 3 3h15a3 3 0 0 0 3-3V8.67l-8.928 5.493a3 3 0 0 1-3.144 0L1.5 8.67Z" />
  <path d="M22.5 6.908V6.75a3 3 0 0 0-3-3h-15a3 3 0 0 0-3 3v.158l9.714 5.978a1.5 1.5 0 0 0 1.572 0L22.5 6.908Z" />
</svg>
"""
    let instaIcon =
        html
            """<svg class="w-10 h-10" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" fill="currentColor"><!--!Font Awesome Free 6.7.2 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license/free Copyright 2025 Fonticons, Inc.--><path d="M224.1 141c-63.6 0-114.9 51.3-114.9 114.9s51.3 114.9 114.9 114.9S339 319.5 339 255.9 287.7 141 224.1 141zm0 189.6c-41.1 0-74.7-33.5-74.7-74.7s33.5-74.7 74.7-74.7 74.7 33.5 74.7 74.7-33.6 74.7-74.7 74.7zm146.4-194.3c0 14.9-12 26.8-26.8 26.8-14.9 0-26.8-12-26.8-26.8s12-26.8 26.8-26.8 26.8 12 26.8 26.8zm76.1 27.2c-1.7-35.9-9.9-67.7-36.2-93.9-26.2-26.2-58-34.4-93.9-36.2-37-2.1-147.9-2.1-184.9 0-35.8 1.7-67.6 9.9-93.9 36.1s-34.4 58-36.2 93.9c-2.1 37-2.1 147.9 0 184.9 1.7 35.9 9.9 67.7 36.2 93.9s58 34.4 93.9 36.2c37 2.1 147.9 2.1 184.9 0 35.9-1.7 67.7-9.9 93.9-36.2 26.2-26.2 34.4-58 36.2-93.9 2.1-37 2.1-147.8 0-184.8zM398.8 388c-7.8 19.6-22.9 34.7-42.6 42.6-29.5 11.7-99.5 9-132.1 9s-102.7 2.6-132.1-9c-19.6-7.8-34.7-22.9-42.6-42.6-11.7-29.5-9-99.5-9-132.1s-2.6-102.7 9-132.1c7.8-19.6 22.9-34.7 42.6-42.6 29.5-11.7 99.5-9 132.1-9s102.7-2.6 132.1 9c19.6 7.8 34.7 22.9 42.6 42.6 11.7 29.5 9 99.5 9 132.1s2.7 102.7-9 132.1z"/></svg>"""

    [<SolidComponent>]
    let Contact () =
        let mutable aref: HTMLAnchorElement = Unchecked.defaultof<_>

        onMount(fun _ ->
            let parts = [| "space"; "contact"; "echotone" |]
            let separator = System.String([| char 64 |]) // @ symbol from char code
            let dot = System.String([| char 46 |]) // . symbol from char code
            let email = parts[1] + separator + parts[2] + dot + parts[0]
            aref.href <- "mailto:" + email
        // Set text content separately to avoid having the email in href attribute
        )
        Fragment() {
            div(class' = "flex flex-col gap-5 items-end") {
                div(class' = "max-w-70 w-70 text-right") {
                    span(class' = "pl-3 float-right align-top") {
                        a(class' = "dark:text-gray-100", href = "mailto:toto@tata.com").ref(fun x -> aref <- x) {
                            mailIcon()
                        }
                        a(
                            class' = "dark:text-gray-100",
                            href = "https://www.instagram.com/echotone__/",
                            target = "_blank"
                        ) {
                            instaIcon()
                        }
                    }
                    "Échotone, espace d’art géré par des artistes"
                    br()
                    "6 Rue des Maquisards, 34190 GANGES"
                }
                div(class' = "flex space-x-4 h-10") {
                    For(each = Array.ofSeq sponsorsLogos) {
                        yield
                            fun l index ->
                                a(class' = "bg-white", href = l.url, target = "_blank") {
                                    img(src = l.src, alt = l.name, class' = "h-10")
                                }
                    }
                }
            }
        }

    [<SolidComponent>]
    let Footer () : HtmlElement =
        let overScrolled, setOverScrolled = createSignal false
        onMount(fun () ->
            let root = document.documentElement
            root.onwheel <-
                fun e ->
                    let scrolled = store.scrolled
                    let scrollEnd = abs(root.scrollHeight - root.clientHeight - root.scrollTop) <= 1.
                    if scrollEnd then
                        if not(overScrolled()) then
                            e.deltaY >= 0. |> setOverScrolled
                        else
                            e.deltaY < 0. |> not |> setOverScrolled
                    else
                        setOverScrolled false)

        footer(class' = classes.footerbar)
            .classList(
                {|
                    ``translate-y-[95%]`` = not(overScrolled())
                |}
            ) {
            Newsletter()
            Contact()
        }
