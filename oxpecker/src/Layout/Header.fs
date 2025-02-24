module Layout

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Aria
open Fable.Core.JsInterop
open Browser

open Flowbite.Main
open Data

let private classes: CssModules.src.Layout.header =
    importDefault "./header.module.scss"

let commonclass' = "block py-2 px-3"

let activeclass' =
    "text-white bg-blue-700 rounded-sm md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500"

let inactiveclass' =
    "text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 md:dark:hover:text-blue-500 dark:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"

[<SolidComponent>]
let navBarLink (tag: Tag) () : HtmlElement =
    let href = navItems[tag].slug
    let title = navItems[tag].title

    li () {
        A(
            href = href,
            class' = commonclass',
            activeClass = activeclass',
            inactiveClass = inactiveclass',
            end' = (href = "/")
        ) {
            title
        }
    }


let menuIconSvg =
    """<svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 17 14">
<path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M1 1h15M1 7h15M1 13h15"/>
</svg>"""

[<SolidComponent>]
let Header () : HtmlElement =
    let triggeredMenu, setTriggeredMenu = createSignal false

    onMount (fun () ->
        let navlist = document.getElementById ("navbar-sticky")
        let navtrigger = document.getElementById ("navbar-trigger")

        let collapseObj =
            flowbite.Collapse.Create(targetEl = navlist, triggerEl = navtrigger)

        collapseObj.updateOnToggle (fun _ -> setTriggeredMenu (not (triggeredMenu ())))

        flowbite.initCollapses ())

    nav (
        class' =
            "bg-white dark:bg-gray-900 fixed w-full z-20 top-0 start-0 border-b border-gray-200 dark:border-gray-600"
    ) {
        div (class' = "max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4") {
            A(href = "/", class' = "flex items-center space-x-3 rtl:space-x-revers") {
                span (class' = "self-center text-2xl font-semibold whitespace-nowrap dark:text-white") {
                    img (
                        class' = "h-auto max-w-xs rounded-2xl px-1 py-2 dark:invert",
                        src = "logo-full.svg",
                        alt = "Logo"
                    )
                }
            }

            div (class' = "flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse", id = "navbar-trigger") {

                button(
                    type' = "button",
                    class' =
                        "inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600 ease-in-out duration-400",
                    ariaControls = "navbar-sticky",
                    ariaExpanded = false
                )
                    .classList({| ``-rotate-90`` = triggeredMenu () |})
                    .data ("collapse-toggle", "navbar-sticky") {
                    span (class' = "sr-only") { "Open Main Menu" }
                    span (innerHTML = menuIconSvg)
                }

            }

            div (class' = "hidden w-full md:block md:w-auto", id = "navbar-sticky") {
                ul (
                    class' =
                        "flex flex-col p-4 md:p-0 mt-4 font-medium border border-gray-100 rounded-lg bg-gray-50 md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700"
                ) {
                    For(
                        each =
                            [| Tag.Accueil
                               Tag.Programmation
                               Tag.Atelier
                               Tag.Boutique
                               Tag.Apropos
                               Tag.Contact |]
                    ) {
                        yield fun tag index -> navBarLink tag ()
                    }
                }
            }


        }

    }
