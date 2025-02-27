module Data

open Fable.JsonProvider
open Fable.Core

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

[<Import("default", "../../data/assets.json")>]
let assetsJson: AssetsT = jsNative

type AssetType =
    | Image
    | Video
    | Audio

type Asset = {
    slug: string
    height: float
    width: float
    ``type``: AssetType
}

let fileTypeToAssetType (s: string) =
    s
    |> _.Split("/")
    |> Array.head
    |> function
        | "image" -> AssetType.Image
        | "video" -> AssetType.Video
        | "audio" -> AssetType.Audio
        | _ -> failwith "Unknown asset type"

let mapSlugs =
    assetsJson.items
    |> Seq.map(fun item ->
        item.id,
        {
            slug = item.slug
            width = item.metadata.pixelWidth
            height = item.metadata.pixelHeight
            ``type`` = fileTypeToAssetType item.mimeType
        })
    |> Map.ofSeq

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
type navItem = {
    title: string
    slug: string
    cmstag: string
}

let navItems =
    Map [
        Tag.Accueil,
        {
            title = "Accueil"
            slug = "/"
            cmstag = "accueil"
        }
        Tag.Programmation,
        {
            title = "Programmation"
            slug = "/programmation"
            cmstag = "programmation"
        }
        Tag.Boutique,
        {
            title = "Boutique"
            slug = "/shop"
            cmstag = "boutique"
        }
        Tag.Atelier,
        {
            title = "Atelier"
            slug = "/workshop"
            cmstag = "atelier"
        }
        Tag.Apropos,
        {
            title = "À propos"
            slug = "/about"
            cmstag = "a-propos"
        }
        Tag.Contact,
        {
            title = "Informations pratiques"
            slug = "/contact"
            cmstag = "contact"
        }
    ]
