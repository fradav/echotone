module App

open Oxpecker.Solid
open Oxpecker.Solid.Router
open Fable.JsonProvider
open Fable.Core

[<Literal>]
let aboutJsonPath = __SOURCE_DIRECTORY__ + "/../../data/about.json"

[<Literal>]
let pagesJsonPath = __SOURCE_DIRECTORY__ + "/../../data/pages.json"

type PagesT = Generator<pagesJsonPath>
type Unit = PagesT.Items.Data.Unit.Fr
type Medias = PagesT.Items.Data.Medias

type AboutContactT = Generator<aboutJsonPath>
type AboutContact = AboutContactT.Items.Data.Content.Fr

type Tag =
    | Atelier
    | Programmation
    | Boutique
    | Accueil

// cmstag is the tag used in the CMS (the same as the slug but in french)
type navItem =
    { title: string
      slug: string
      cmstag: string }

let navItems =
    Map
        [ Tag.Accueil,
          { title = "Accueil"
            slug = "/"
            cmstag = "accueil" }
          Tag.Programmation,
          { title = "Programmation"
            slug = "/programmation"
            cmstag = "programmation" }
          Tag.Boutique,
          { title = "Boutique"
            slug = "/shop"
            cmstag = "boutique" }
          Tag.Atelier,
          { title = "Atelier"
            slug = "/workshop"
            cmstag = "atelier" } ]

[<Import("default", "../../data/about.json")>]
let aboutJson: AboutContactT = jsNative

[<Import("default", "../../data/contact.json")>]
let contactJson: AboutContactT = jsNative

[<Import("default", "../../data/pages.json")>]
let pages: PagesT = jsNative


let about: AboutContact = aboutJson.items |> Seq.head |> _.data.content.fr
let contact: AboutContact = contactJson.items |> Seq.head |> _.data.content.fr

[<SolidComponent>]
let SolidAboutContact (unit: AboutContact) : HtmlElement =
    Fragment() {
        h1 () { unit.title }
        h2 () { unit.short }
        div (innerHTML = unit.text)
        A(href = "/", class' = "block text-right") { "Retour" }
    }

[<SolidComponent>]
let SolidUnit (unit: Unit) : HtmlElement =
    Fragment() {
        h1 () { unit.title }
        h2 () { unit.short }
        div (innerHTML = unit.text)
        A(href = "/", class' = "block text-right") { " Retour " }
    }

[<SolidComponent>]
let About () : HtmlElement = SolidAboutContact about

[<SolidComponent>]
let Contact () : HtmlElement = SolidAboutContact contact

let filterPages (tag: string) =
    pages.items |> Seq.filter (fun x -> x.data.unit.fr.tags |> Seq.contains tag)

let getPages (tag: string) =
    filterPages tag |> Seq.map (fun x -> x.data.unit.fr) |> Array.ofSeq

[<SolidComponent>]
let taggedPages (tag: Tag) () : HtmlElement =
    Fragment() {
        h1 () { navItems[tag].title }
        div () { For(each = getPages navItems[tag].cmstag) { yield fun (page: Unit) index -> SolidUnit page } }
    }

[<SolidComponent>]
let taggedNavItem (tag: Tag) () : HtmlElement =
    A(href = navItems[tag].slug, class' = "block text-right") { navItems[tag].title }

[<SolidComponent>]
let App () : HtmlElement =
    div () {
        br ()
        br ()
        A(href = "/", class' = "block text-right") { "Échotone" }
        For(each = [| Tag.Programmation; Tag.Atelier; Tag.Boutique |]) { yield fun tag index -> taggedNavItem tag () }
        A(href = "/about", class' = "block text-right") { about.title }
        A(href = "/contact", class' = "block text-right") { contact.title }
        div () { taggedPages Tag.Accueil () }
    }
