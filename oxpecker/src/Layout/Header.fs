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
        navbar =
            "dark:bg-gray-900/75 backdrop-blur-2xl md:sticky not-md:fixed w-full z-100 top-0 start-0 bg-transparent"
        navbarContainer = "flex justify-around flex-wrap items-center"
        navbarMobileContainer = "flex items-center justify-between not-md:w-full p-4"
        logoLink = "max-w-[300px] w-[300px] flex items-center space-x-3 rtl:space-x-reverse"
        logo = "h-auto rounded-2xl px-1 py-2 dark:invert"
        hamburgerContainer = "flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse"
        hamburgerButton =
            "inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600 duration-500 ease-in-out"
        linkListContainer =
            "items-center justify-between md:flex not-md:fixed not-md:top-20 md:w-auto md:order-1 duration-1000 ease-in-out m-4 not-md:rounded not-md:right-0"
        linkList =
            "flex flex-col p-4 md:p-0 mt-4 font-medium border border-gray-100 text-xl rounded-lg md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 dark:border-gray-700 not-md:bg-gradient-to-r from-gray-500/90 to-slate-900/90"
        item = "block py-2 px-3"
        itemActive =
            "text-white md:text-gray-300 md:p-0 not-md:bg-gray-400 dark:md:text-gray-700 dark:md:drop-shadow-2xl animate-flicker"
        itemInactive =
            "rounded-sm md:hover:bg-transparent md:hocus:text-blue-700 md:p-0 text-gray-200 dark:hover:text-gray-50"
    |}

    let toggleMenu _ =
        // printfn "toggleMenu called %A" store.menuOpened
        store.menuOpened |> not |> setStore.Path.Map(_.menuOpened).Update

    [<SolidComponent>]
    let navBarLink (tag: Tag) : HtmlElement =
        let href = navItems[tag].slug
        let title = navItems[tag].title
        let path = useLocation()

        createEffect(fun () ->
            if path.pathname = href then
                setStore.Path.Map(_.currentTag).Update tag)

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
        let timer = new System.Timers.Timer(AutoReset = false)

        onCleanup(fun () ->
            timer.Interval <- 1.
            timer.Elapsed.Add(fun _ -> setStore.Path.Map(_.menuOpened).Update false)
            timer.Start())

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
                            // class' = classes.hamburgerButton,
                            class' = "grid place-content-center w-20 h-20 pt-3 md:hidden",
                            ariaControls = "navbar-sticky",
                            ariaExpanded = false
                        )
                            .classList(
                                {|
                                    ``hamburger-toggle`` = store.menuOpened
                                |}
                            ) {
                            span(class' = "sr-only") { "Open Main Menu" }
                            // menuIconSvg()
                            div(
                                class' =
                                    "w-16 h-2 bg-black dark:bg-white rounded-full transition-all duration-150 before:content-[''] before:absolute before:w-16 before:h-2 before:bg-black dark:before:bg-white before:rounded-full before:-translate-y-4 before:-translate-x-8 before:transition-all before:duration-150 after:content-[''] after:absolute after:w-16 after:h-2 after:bg-black dark:after:bg-white after:rounded-full after:translate-y-4  after:-translate-x-8 after:transition-all after:duration-150"
                            )

                        }
                    }
                }

                div(class' = classes.linkListContainer, id = "navbar-sticky")
                    .classList(
                        {|
                            ``not-md:translate-x-full not-md:invisible`` = not(store.menuOpened)

                        |}
                    ) {
                    ul(class' = classes.linkList) {
                        For(each = realNonEmptyTags) { yield fun tag index -> navBarLink tag }
                    }
                }


            }

        }
