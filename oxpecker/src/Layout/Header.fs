namespace Layout

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Aria

open Browser

open Site.Imports
open Data
open Components

[<AutoOpen>]
module Header =
    let private classes =
        {| navbar =
            "dark:md:bg-gray-900 fixed w-full z-100  top-0 start-0 border-b border-gray-200 dark:border-gray-600 bg-transparent"
           navbarContainer = "flex justify-around flex-wrap items-center"
           navbarMobileContainer = "dark:not-md:bg-gray-900 flex items-center justify-between not-md:w-full p-4"
           logoLink = "max-w-[300px] w-[300px] flex items-center space-x-3 rtl:space-x-reverse"
           logo = "h-auto rounded-2xl px-1 py-2 dark:invert"
           hamburgerContainer = "flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse"
           hamburgerButton =
            "inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600 duration-500 ease-in-out"
           linkListContainer =
            "items-center justify-between w-full md:flex md:w-auto md:order-1 duration-1000 ease-in-out m-4 not-md:rounded"
           linkList =
            "flex flex-col p-4 md:p-0 mt-4 font-medium border border-gray-100 rounded-lg bg-gray-50 md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700"
           item = "block py-2 px-3"
           itemActive =
            "text-white bg-blue-700 rounded-sm md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500"
           itemInactive =
            "text-gray-900 rounded-sm hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 md:dark:hover:text-blue-500 dark:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700" |}

    [<SolidComponent>]
    let navBarLink (tag: Tag) () : HtmlElement =
        let href = navItems[tag].slug
        let title = navItems[tag].title

        li () {
            A(
                href = href,
                class' = classes.item,
                activeClass = classes.itemActive,
                inactiveClass = classes.itemInactive,
                end' = (href = "/")
            ) {
                title
            }
        }

    let menuIconSvg =
        html
            """<svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 17 14">
        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M1 1h15M1 7h15M1 13h15"/>
    </svg>
    """

    [<SolidComponent>]
    let Header (scrolled: Accessor<float>) : HtmlElement =
        let triggeredMenu, setTriggeredMenu = createSignal false
        // printfn "Name of the classe : %s" classes.linkListContainer

        let calculateWidthLogo () =
            let minRes = 150
            let maxRes = 300
            // let wH = document.body.scrollHeight
            let wH = 400.

            let ratioscrolled =
                maxRes - int (float (maxRes - minRes) * (min (scrolled () / wH) 1.))

            printfn "ratioscrolled: %i" ratioscrolled
            ratioscrolled

        // let mutable mutableButton: Types.HTMLButtonElement = Unchecked.defaultof<_>

        onMount (fun () ->
            let navlist = document.getElementById ("navbar-sticky")
            let navtrigger = document.getElementById ("navbar-trigger")

            let collapseObj =
                flowbite.Collapse.Create(targetEl = navlist, triggerEl = navtrigger)

            collapseObj.updateOnToggle (fun _ ->
                navlist.classList.remove ("hidden")
                setTriggeredMenu (not (triggeredMenu ())))

            navlist.classList.remove ("hidden")

        )

        nav (class' = classes.navbar) {
            div (class' = classes.navbarContainer) {
                div (class' = classes.navbarMobileContainer) {
                    A(href = "/", class' = classes.logoLink) {
                        img (
                            class' = classes.logo,
                            style = $"width: {calculateWidthLogo ()}px;max-width: 100%%;",
                            src = "logo-full.svg",
                            alt = "Logo"
                        )
                    }

                    div (class' = classes.hamburgerContainer, id = "navbar-trigger") {

                        button(
                            type' = "button",
                            class' = classes.hamburgerButton,
                            ariaControls = "navbar-sticky",
                            ariaExpanded = false
                        )
                            .classList({| ``-rotate-90`` = triggeredMenu () |})
                            .data ("collapse-toggle", "navbar-sticky") {
                            span (class' = "sr-only") { "Open Main Menu" }
                            menuIconSvg ()
                        }
                    }
                }

                div(class' = classes.linkListContainer, id = "navbar-sticky")
                    .classList ({| ``not-md:opacity-0 not-md:pointer-events-none`` = not (triggeredMenu ()) |}) {
                    ul (class' = classes.linkList) {
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
