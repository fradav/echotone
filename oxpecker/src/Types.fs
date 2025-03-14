module Types

open Fable.Core
open Fable.JsonProvider

[<Erase>]
type TaggedTopic =
    | Accueil
    | Programmation
    | Atelier
    | Boutique

[<Erase>]
type StaticTopic =
    | Apropos
    | Contact

type Topic =
    | StaticTopic of StaticTopic
    | TaggedTopic of TaggedTopic

[<Erase>]
type Navigation = {
    currentTopic: Topic
    currentSlug: string option
}

type BaseNavItem = { title: string; slug: string }

type NavItem =
    | BaseItem of BaseNavItem
    | TaggedItem of BaseNavItem * string

type Sponsor = {
    name: string
    url: string
    src: string
}

[<Erase>]
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
let jsonFolder = "./../data/"

[<Literal>]
let assetsJsonPath = jsonFolder + "assets.json"

[<Literal>]
let contactJsonPath = jsonFolder + "contact.json"

[<Literal>]
let aboutJsonPath = jsonFolder + "about.json"

[<Literal>]
let pagesJsonPath = jsonFolder + "pages.json"

type PagesT = Generator<pagesJsonPath>
type Unit = PagesT.Items.Data.Unit.Fr
type Medias = PagesT.Items.Data.Medias

type AboutContactT = Generator<aboutJsonPath>
type AboutContact = AboutContactT.Items.Data.Content.Fr

type AssetsT = Generator<assetsJsonPath>
type Assets = AssetsT.Items

[<Erase>]
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

[<Erase>]
type MenuState =
    | Closed
    | Opening
    | Open
    | Closing
    | TransitioningActive

[<Erase>]
type LinkState =
    | NoTransition
    | TransitionFrom
    | TransitionTo

type TransitionLinkState = { state: LinkState; link: Link }

type TransitionLinkFromToState = {
    from: TransitionLinkState
    to': TransitionLinkState
}

[<Erase>]
type GlobalLinkState =
    | Current of Link
    | Transitioning of TransitionLinkFromToState

[<Erase>]
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

[<Erase>]
type Theme =
    | Light
    | Dark
    | Auto

[<Erase>]
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
