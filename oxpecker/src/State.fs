module State

open Oxpecker.Solid
open Browser
open Types
open Data

let store, setStore =
    createStore {
        globalLinkState = Accueil |> TaggedTopic |> TopicLink |> Current
        menu = {
            items =
                navItems
                |> Map.values
                |> Seq.map(fun x -> { item = x; state = Inactive })
                |> List.ofSeq
            state = Closed
        }
        breakpoint = Lg
        screenType = Desktop
        theme = Auto
        scrolled = 0.
    }


let observer =
    IntersectionObserver.Create(fun entries _ ->
        for entry in entries do
            if entry.isIntersecting then
                (entry.target :?> HTMLElement).classList.remove("vignette-invisible")
            else
                (entry.target :?> HTMLElement).classList.add("vignette-invisible"))


// Détection si on est sur mobile
let isMobile =
    createMemo(fun () ->
        match store.breakpoint with
        | Xs
        | Sm -> true
        | _ -> false)

// Fonction de navigation avec animation
let navigateFromTo (from: Link) (to': Link) =
    // Commencer la transition de sortie
    setStore.Path
        .Map(_.globalLinkState)
        .Update(
            Transitioning {
                from = { state = TransitionFrom; link = from }
                to' = { state = TransitionTo; link = to' }
            }
        )


let toggleMenu () =
    match store.menu.state with
    | Closed
    | Closing -> setStore.Path.Map(_.menu.state).Update(Opening)
    | Open
    | Opening -> setStore.Path.Map(_.menu.state).Update(Closing)
    | _ -> ()

let getTopicFromLink (link: Link) =
    match link with
    | TopicLink t -> t
    | PageLink { topic = t; slug = _ } -> TaggedTopic t

let toggleActiveInactiveMenuItems (item: NavItem) =
    match store.menu.state with
    | Open
    | Closed ->
        let currentActive = store.menu.items |> List.tryFind(fun x -> x.state = Active)
        match currentActive with
        | Some x when x.item = item -> ()
        | _ ->
            store.menu.items
            |> List.map(fun x ->
                match x.item with
                | t when t = item -> {
                    x with
                        state = TransitioningFromInactiveToActive
                  }
                | t when x.state = Active -> {
                    x with
                        state = TransitioningFromActiveToInactive
                  }
                | _ -> x)
            |> (fun l ->
                setStore.Path
                    .Map(_.menu)
                    .Update(
                        {
                            items = l
                            state = TransitioningActive
                        }
                    ))
    | _ -> ()

let chooseTopic (topic: Topic) =
    match store.globalLinkState with
    | Current(TopicLink t) when t = topic -> ()
    | Transitioning _ -> ()
    | Current l ->
        let link = TopicLink topic
        navigateFromTo l link

let chooseLink (plink: PageLink) =
    match store.globalLinkState with
    | Current(PageLink pl) when pl = plink -> ()
    | Transitioning _ -> ()
    | Current l ->
        let link = PageLink plink
        navigateFromTo l link


// Fonctions utiles pour récupérer les pages
let getPagesForTopic (taggedtopic: TaggedTopic) =
    pages.items
    |> Seq.filter(fun page -> page.data.unit.fr.tags |> Seq.contains(snd navTaggedItems[taggedtopic]))
    |> Seq.toArray

// Récupérer une page par son slug
let getPageBySlug (slug: string) =
    pages.items |> Seq.tryFind(fun page -> page.data.id.iv = slug)
