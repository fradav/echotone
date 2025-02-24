module Data

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

[<Import("default", "../../data/about.json")>]
let aboutJson: AboutContactT = jsNative

[<Import("default", "../../data/contact.json")>]
let contactJson: AboutContactT = jsNative

[<Import("default", "../../data/pages.json")>]
let pages: PagesT = jsNative

// import import.meta.env.MODE from vite config
[<Global("import.meta.env.BASE_URL")>]
let baseR: string = jsNative

let addedTrailingSlash =
    match baseR with
    | "/" -> baseR
    | _ -> baseR + "/"

printfn "baseR: %s" baseR

let about: AboutContact = aboutJson.items |> Seq.head |> _.data.content.fr
let contact: AboutContact = contactJson.items |> Seq.head |> _.data.content.fr

type Tag =
    | Atelier
    | Programmation
    | Boutique
    | Accueil
    | Apropos
    | Contact

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
            cmstag = "atelier" }
          Tag.Apropos,
          { title = "À propos"
            slug = "/about"
            cmstag = "a-propos" }
          Tag.Contact,
          { title = "Informations pratiques"
            slug = "/contact"
            cmstag = "contact" } ]

let slugToTag (slug: string) =
    try
        navItems |> Map.pick (fun k v -> if v.slug = slug then Some k else None)
    with e ->
        printf "failed to get %s" slug
        Tag.Accueil

let tagToTitle (tag: Tag) = navItems[tag].title
