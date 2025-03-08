module Main

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Oxpecker.Solid.Meta

open Fable.Core.JsInterop
open Browser

open Types
open Data
open State
open App

// importSideEffects "../index.css"

[<SolidComponent>]
let taggedRoute (taggedtopic: TaggedTopic) =
    Route(path = (navTaggedItems[taggedtopic] |> fst |> _.slug), component' = App(taggedPages taggedtopic))

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

let routerSlugToTag (slug: string) =
    let rewriteSlug =
        match slug with
        | s when s = baseR -> "/"
        | s when baseR <> "/" && s.StartsWith(baseR) -> s.Substring(baseR.Length)
        | s when s <> "/" -> s
        | _ -> "/"

    let path = rewriteSlug.Split("/")
    trySlugToTopic("/" + path[1]) |> Option.defaultValue(TaggedTopic Accueil)

let getPage () =
    let tag = (useParams())?tag
    let slug: string = (useParams())?slug

    let sTag = "/" + tag |> trySlugToTopic
    Map.tryFind slug mapPageSlugToTag
    |> Option.map(fun t ->
        match sTag with
        | Some(TaggedTopic t') when t = t' -> true
        | _ -> false)
    |> Option.contains true
    |> function
        | false -> None
        | true ->
            pages.items
            |> Seq.tryFind(fun x -> x.data.id.iv = slug)
            |> Option.map(fun page -> page |> makePage)
    |> Option.defaultWith(fun p -> NotFound())


let topicToTitle = getBaseItemFromTopic >> _.title

let currentTitle () =
    match store.globalLinkState with
    | Current(TopicLink t) -> t |> topicToTitle
    | Current(PageLink { topic = t; slug = _ }) -> t |> TaggedTopic |> topicToTitle
    | _ -> "…"

[<SolidComponent>]
let Layout (props: RootProps) : HtmlElement =
    let location = useLocation()
    let navigate = useNavigate()

    createEffect(fun () ->
        let path = location.pathname

        if path = (baseR + "/") then
            navigate.Invoke("/", jsOptions(fun o -> o.replace <- true))
    // else
    //     let newTag = path |> routerSlugToTag
    //     setStore.Path.Map(_.currentTag).Update newTag
    )

    Fragment() {
        Base(href = addedTrailingSlash)
        Title() { $"Échotone / {currentTitle()}" }
        Suspense(fallback = Loading()) { props.children }
    }


// HMR doesn't work in Root for some reason
[<SolidComponent>]
let appRouter () =
    MetaProvider() {
        Router(base' = baseR, root = Layout) {
            // Replace For with explicit route declarations
            // yield! pageTaggedTopics |> Seq.map taggedRoute |> Seq.toArray
            taggedRoute Accueil
            taggedRoute Programmation
            taggedRoute Atelier
            taggedRoute Boutique
            Route(path = "/about", component' = App AboutP)
            Route(path = "/contact", component' = App ContactP)
            Route(path = "/:tag/:slug", component' = App getPage)
            // Add a catch-all route
            Route(path = "*", component' = NotFound)
        }
    }

render(appRouter, document.getElementById "root")
