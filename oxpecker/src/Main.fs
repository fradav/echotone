module Main

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Meta

open Fable.Core.JsInterop
open Browser


open Data
open App

// importSideEffects "../index.css"

[<SolidComponent>]
let taggedRoute (tag: Tag) =
    Route(path = navItems.[tag].slug, component' = App(taggedPages tag))

[<SolidComponent>]
let NotFound () : HtmlElement =

    Fragment() {
        Title() { "404 - Not Found" }

        main() {

            div() {
                h1() { "404 - Not Found" }
                p() { "Sorry, the page you are looking for does not exist." }
            }
        }
    }

let slugToTag (slug: string) =
    let rewriteSlug =
        match slug with
        | s when s = baseR -> "/"
        | s when baseR <> "/" && s.StartsWith(baseR) -> s.Substring(baseR.Length)
        | s when s <> "/" -> s
        | _ -> "/"

    try
        let path = rewriteSlug.Split("/")
        printfn "path: %s" path[1]
        navItems |> Map.pick(fun k v -> if v.slug = "/" + path[1] then Some k else None)
    with _ ->
        printfn "failed to get %s" slug
        Tag.Accueil

let tagToTitle (tag: Tag) = navItems[tag].title

[<SolidComponent>]
let Layout (props: RootProps) : HtmlElement =
    let location = useLocation()
    let navigate = useNavigate()
    let currentTag, setCurrentTag = createSignal Tag.Accueil

    createEffect(fun () ->
        let path = location.pathname

        if path = (baseR + "/") then
            navigate.Invoke("/")
        else
            path |> slugToTag |> setCurrentTag)

    Fragment() {
        Base(href = addedTrailingSlash)
        Title() { $"Échotone / {currentTag() |> tagToTitle}" }
        Suspense(fallback = (p() { "Loading…" })) { props.children }
    }


// HMR doesn't work in Root for some reason
[<SolidComponent>]
let appRouter () =

    MetaProvider() {
        Router(base' = baseR, root = Layout) {
            taggedRoute Tag.Accueil
            taggedRoute Tag.Programmation
            taggedRoute Tag.Atelier
            taggedRoute Tag.Boutique
            Route(path = "/about", component' = App AboutP)
            Route(path = "/contact", component' = App ContactP)
            Route(path = "/:tag/:slug", component' = App getPage)
            // Add a catch-all route
            Route(path = "*", component' = NotFound)
        }
    }

render(appRouter, document.getElementById "root")
