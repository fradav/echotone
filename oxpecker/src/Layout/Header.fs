namespace Layout

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Aria

open Fable.Core.JsInterop
open Browser.Types
open Browser
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

    let darkIcon =
        html
            """<svg id="theme-toggle-dark-icon" class="hidden w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path d="M17.293 13.293A8 8 0 016.707 2.707a8.001 8.001 0 1010.586 10.586z"></path></svg>"""

    let lightIcon =
        html
            """<svg id="theme-toggle-light-icon" class="hidden w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path d="M10 2a1 1 0 011 1v1a1 1 0 11-2 0V3a1 1 0 011-1zm4 8a4 4 0 11-8 0 4 4 0 018 0zm-.464 4.95l.707.707a1 1 0 001.414-1.414l-.707-.707a1 1 0 00-1.414 1.414zm2.12-10.607a1 1 0 010 1.414l-.706.707a1 1 0 11-1.414-1.414l.707-.707a1 1 0 011.414 0zM17 11a1 1 0 100-2h-1a1 1 0 100 2h1zm-7 4a1 1 0 011 1v1a1 1 0 11-2 0v-1a1 1 0 011-1zM5.05 6.464A1 1 0 106.465 5.05l-.708-.707a1 1 0 00-1.414 1.414l.707.707zm1.414 8.486l-.707.707a1 1 0 01-1.414-1.414l.707-.707a1 1 0 011.414 1.414zM4 11a1 1 0 100-2H3a1 1 0 000 2h1z" fill-rule="evenodd" clip-rule="evenodd"></path></svg>"""

    [<SolidComponent>]
    let Header () : HtmlElement =
        let timer = new System.Timers.Timer(AutoReset = false)
        onMount(fun _ ->
            let themeToggleDarkIcon = document.getElementById("theme-toggle-dark-icon")
            let themeToggleLightIcon = document.getElementById("theme-toggle-light-icon")
            if
                localStorage.getItem("theme") = "dark"
                || (not(isIn "theme" localStorage)
                    && window.matchMedia("(prefers-color-scheme: dark)").matches)
            then
                themeToggleDarkIcon.classList.remove("hidden")
                themeToggleLightIcon.classList.add("hidden")
            else
                themeToggleDarkIcon.classList.add("hidden")
                themeToggleLightIcon.classList.remove("hidden")
            let themeToggleBtn = document.getElementById("theme-toggle")
            themeToggleBtn.addEventListener(
                "click",
                fun _ ->
                    let themeToggleDarkIcon = document.getElementById("theme-toggle-dark-icon")
                    let themeToggleLightIcon = document.getElementById("theme-toggle-light-icon")
                    if themeToggleDarkIcon.classList.contains("hidden") then
                        themeToggleDarkIcon.classList.remove("hidden")
                        themeToggleLightIcon.classList.add("hidden")
                        localStorage.setItem("theme", "dark")
                        document.documentElement.classList.add("dark")
                    else
                        themeToggleDarkIcon.classList.add("hidden")
                        themeToggleLightIcon.classList.remove("hidden")
                        localStorage.setItem("theme", "light")
                        document.documentElement.classList.remove("dark")
            ))
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
                        button(
                            id = "theme-toggle",
                            type' = "button",
                            class' =
                                "h-5 w-5 text-gray-500 dark:text-gray-400 not-md:text-gray-200 not-md:place-self-end hover:bg-gray-100 dark:hover:bg-gray-700 focus:outline-none focus:ring-4 focus:ring-gray-200 dark:focus:ring-gray-700 rounded-lg text-sm"
                        ) {
                            lightIcon()
                            darkIcon()
                        }
                    }
                }


            }

        }
