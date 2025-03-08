namespace Layout

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Aria

open Types
open Data
open State
open Components

[<AutoOpen>]
module Header =
    open Fable.Core.JsInterop

    let private classes = {|
        navbar =
            "dark:bg-zinc-800/75 backdrop-blur-2xl sticky w-full z-100 top-0 start-0 bg-gray-100/75 overflow-x-clip"
        navbarContainer = "flex justify-around flex-wrap items-center"
        navbarMobileContainer = "flex items-center justify-between not-md:w-full p-4"
        logoLink = "max-w-[300px] w-[300px] flex items-center space-x-3 rtl:space-x-reverse"
        logo = "h-auto rounded-2xl px-1 py-2 dark:invert"
        hamburgerContainer = "flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse"
        hamburgerButton = "grid place-content-center w-20 h-20 pt-3 md:hidden"
        hamburgerIcon =
            "w-16 h-2 bg-black dark:bg-white rounded-full transition-all duration-150 before:content-[''] before:absolute before:w-16 before:h-2 before:bg-black dark:before:bg-white before:rounded-full before:-translate-y-4 before:-translate-x-8 before:transition-all before:duration-150 after:content-[''] after:absolute after:w-16 after:h-2 after:bg-black dark:after:bg-white after:rounded-full after:translate-y-4  after:-translate-x-8 after:transition-all after:duration-150"
        linkListContainer =
            "not-md:transition-transform not-md:duration-1000 not-md:ease-in-out items-center justify-between md:flex not-md:fixed not-md:top-20 md:w-auto md:order-1  m-4 not-md:rounded not-md:right-0"
        linkList =
            "flex flex-col p-4 md:p-0 mt-4 font-medium border border-gray-100 text-xl rounded-lg md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 dark:border-gray-700 not-md:bg-gradient-to-r from-gray-500/90 to-slate-900/90"
        item = "block py-2 px-3"
        itemActive =
            "text-white md:dark:text-zinc-400 md:text-zinc-300 md:p-0 not-md:bg-gray-400 dark:md:drop-shadow-2xl"
        itemInactive =
            "rounded-sm md:hover:bg-transparent md:dark:hocus:text-zinc-400 md:hocus:text-zinc-300 md:p-0 text-black dark:text-gray-200 dark:hocus:text-gray-50"
    |}

    let private menuOpened, setMenuOpened = createSignal false
    let private hasClicked, setHasClicked = createSignal false
    let private toggleMenu = menuOpened >> not >> setMenuOpened
    let private oldCurrentTag, setOldCurrentTag = createSignal Accueil

    [<SolidComponent>]
    let navBarLink (topic: Topic) : HtmlElement =
        let baseItem = topic |> getBaseItemFromTopic
        let href = baseItem.slug
        let title = baseItem.title

        li() {
            A(
                href = href,
                onClick =
                    (fun e ->

                        chooseTopic topic),
                activeClass = classes.itemActive,
                inactiveClass = classes.itemInactive,
                class' = classes.item,
                end' = (href = "/"),
                onAnimationEnd =
                    fun e ->
                        printfn "Animation Name: %s" e.animationName
                        if e.animationName = "flicker" then
                            // setOldCurrentTag store.globalLinkState
                            if store.screenType = Mobile then
                                setHasClicked true
                                setMenuOpened false
            // setHasClicked true
            )
                .classList(
                // createObj [
                //     classes.itemActive ==> true
                //     classes.itemInactive ==> true
                //     "animate-flicker" ==> false
                // ]
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
        // useBeforeLeave(fun e -> ())

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

                    div(
                        class' = classes.hamburgerContainer,
                        id = "navbar-trigger",
                        onClick =
                            (fun _ ->
                                setHasClicked true
                                toggleMenu())
                    ) {

                        button(
                            type' = "button",
                            // class' = classes.hamburgerButton,
                            class' = classes.hamburgerButton,
                            ariaControls = "navbar-sticky",
                            ariaExpanded = false
                        )
                            .classList
                            {|
                                ``hamburger-toggle`` = menuOpened()
                            |} {
                            span(class' = "sr-only") { "Open Main Menu" }
                            div(class' = classes.hamburgerIcon)

                        }
                    }
                }

                div(
                    class' = classes.linkListContainer,
                    id = "navbar-sticky",
                    onTransitionEnd =
                        fun e ->
                            printfn "Transition Menu: %b" (menuOpened())
                            if e.target = e.currentTarget then
                                setHasClicked false


                )
                    .classList
                    {|
                        ``not-md:collapse`` = not(menuOpened() || hasClicked())
                        ``not-md:translate-x-full`` = not(menuOpened())

                    |} {
                    ul(class' = classes.linkList) {
                        For(each = realNonEmptyTopics) { yield fun tag index -> navBarLink tag }
                        ThemeChooser()
                    }
                }


            }

        }
