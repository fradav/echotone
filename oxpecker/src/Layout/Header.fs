namespace Layout

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Aria

open Fable.Core.JsInterop
open Data
open Components


[<AutoOpen>]
module Header =


    let private classes = {|
        navbar = "dark:md:bg-gray-900/75 backdrop-blur-2xl sticky w-full z-100  top-0 start-0 bg-transparent"
        navbarContainer = "flex justify-around flex-wrap items-center"
        navbarMobileContainer = "dark:not-md:bg-gray-900 flex items-center justify-between not-md:w-full p-4"
        logoLink = "max-w-[300px] w-[300px] flex items-center space-x-3 rtl:space-x-reverse"
        logo = "h-auto rounded-2xl px-1 py-2 dark:invert"
        hamburgerContainer = "flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse"
        hamburgerButton =
            "inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600 duration-500 ease-in-out"
        linkListContainer =
            "items-center justify-between md:flex not-md:fixed not-md:top-20 md:w-auto md:order-1 duration-1000 ease-in-out m-4 not-md:rounded not-md:right-0"
        linkList =
            "flex flex-col p-4 md:p-0 mt-4 font-medium border border-gray-100 text-xl rounded-lg md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 md:dark:bg-gray-900/75 dark:border-gray-700 not-md:bg-blue-200/75"
        item = "block py-2 px-3"
        itemActive = "text-white md:text-gray-300 md:p-0"
        itemInactive =
            "text-gray-900 rounded-sm md:hover:bg-transparent md:hocus:text-blue-700 md:p-0 dark:text-gray-200 dark:hover:text-gray-50"
    |}

    [<SolidComponent>]
    let navBarLink (tag: Tag) () : HtmlElement =
        let href = navItems[tag].slug
        let title = navItems[tag].title

        li() {
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

    // let menuIconSvg = html(importDefault "../Icons/Menu.svg?raw")
    let menuIconSvg =
        html
            """<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="size-6">
  <path d="M5.625 3.75a2.625 2.625 0 1 0 0 5.25h12.75a2.625 2.625 0 0 0 0-5.25H5.625ZM3.75 11.25a.75.75 0 0 0 0 1.5h16.5a.75.75 0 0 0 0-1.5H3.75ZM3 15.75a.75.75 0 0 1 .75-.75h16.5a.75.75 0 0 1 0 1.5H3.75a.75.75 0 0 1-.75-.75ZM3.75 18.75a.75.75 0 0 0 0 1.5h16.5a.75.75 0 0 0 0-1.5H3.75Z" />
</svg>
"""

    [<SolidComponent>]
    let Header () : HtmlElement =
        let triggeredMenu, setTriggeredMenu = createSignal false
        let toggleMenu _ =
            triggeredMenu() |> not |> setTriggeredMenu

        let calculateWidthLogo () =
            let minRes = 150
            let maxRes = 300

            let wH = 400.

            let ratioscrolled =
                maxRes - int(float(maxRes - minRes) * min (store.scrolled / wH) 1.)

            ratioscrolled

        nav(class' = classes.navbar) {
            div(class' = classes.navbarContainer) {
                div(class' = classes.navbarMobileContainer) {
                    A(href = "/", class' = classes.logoLink) {
                        img(
                            class' = classes.logo,
                            style = $"width: {calculateWidthLogo()}px;max-width: 100%%;",
                            src = "logo-full.svg",
                            alt = "Logo"
                        )
                    }

                    div(class' = classes.hamburgerContainer, id = "navbar-trigger", onClick = toggleMenu) {

                        button(
                            type' = "button",
                            class' = classes.hamburgerButton,
                            ariaControls = "navbar-sticky",
                            ariaExpanded = false
                        ) {
                            span(class' = "sr-only") { "Open Main Menu" }
                            // menuIconSvg()
                            span(id = "nav-icon1").classList({| ``open`` = triggeredMenu() |})
                        }
                    }
                }

                div(class' = classes.linkListContainer, id = "navbar-sticky")
                    .classList(
                        {|
                            ``not-md:translate-x-full not-md:invisible`` = not(triggeredMenu())
                            ``not-md:translate-x-0`` = triggeredMenu()
                        |}
                    ) {
                    ul(class' = classes.linkList) {
                        For(each = [| Tag.Programmation; Tag.Atelier; Tag.Boutique; Tag.Apropos; Tag.Contact |]) {
                            yield fun tag index -> navBarLink tag ()
                        }
                    }
                }


            }

        }
