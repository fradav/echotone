module Types

open Fable.JsonProvider

type TaggedTopic =
    | Accueil
    | Atelier
    | Programmation
    | Boutique

type StaticTopic =
    | Apropos
    | Contact

type Topic =
    | StaticTopic of StaticTopic
    | TaggedTopic of TaggedTopic

// Structure de navigation
type Navigation = {
    currentTopic: Topic
    currentSlug: string option
}

// cmstag is the tag used in the CMS (the same as the slug but in french)
type BaseNavItem = { title: string; slug: string }

type NavItem =
    | BaseItem of BaseNavItem
    | TaggedItem of BaseNavItem * string

type Sponsor = {
    name: string
    url: string
    src: string
}

type AssetType =
    | Image
    | Video
    | Audio

type Asset = {
    slug: string
    thumbnail: string option
    height: float
    width: float
    ``type``: AssetType
}

[<Literal>]
let assetsJsonPath = __SOURCE_DIRECTORY__ + "/../../data/assets.json"

[<Literal>]
let aboutJsonPath = __SOURCE_DIRECTORY__ + "/../../data/about.json"

[<Literal>]
let pagesJsonPath = __SOURCE_DIRECTORY__ + "/../../data/pages.json"

type PagesT = Generator<pagesJsonPath>
type Unit = PagesT.Items.Data.Unit.Fr
type Medias = PagesT.Items.Data.Medias

type AboutContactT = Generator<aboutJsonPath>
type AboutContact = AboutContactT.Items.Data.Content.Fr

type AssetsT = Generator<assetsJsonPath>
type Assets = AssetsT.Items

type Breakpoint =
    | Xs
    | Sm
    | Md
    | Lg
    | Xl
    | Xxl

type PageLink = { topic: TaggedTopic; slug: string }

type Link =
    | TopicLink of Topic
    | PageLink of PageLink

// États pour le menu mobile
type MenuState =
    | Closed
    | Opening
    | Open
    | Closing
    | TransitioningActive

// États pour les transitions de page
type LinkState =
    | NoTransition
    | TransitionFrom
    | TransitionTo

type TransitionLinkState = { state: LinkState; link: Link }

type TransitionLinkFromToState = {
    from: TransitionLinkState
    to': TransitionLinkState
}

type GlobalLinkState =
    | Current of Link
    | Transitioning of TransitionLinkFromToState

type MenuItemState =
    | Active
    | Inactive
    | TransitioningFromActiveToInactive
    | TransitioningFromInactiveToActive

type MenuItem = { item: NavItem; state: MenuItemState }

type Menu = {
    items: MenuItem list
    state: MenuState
}

type Theme =
    | Light
    | Dark
    | Auto

type ScreenType =
    | Mobile
    | Desktop

type Model = {
    globalLinkState: GlobalLinkState
    menu: Menu
    breakpoint: Breakpoint
    screenType: ScreenType
    theme: Theme
    scrolled: float
}
