module Data

open Fable.Core
open Fable.Core.JsInterop

open Browser

open Types
open System

[<Import("default", "../../data/about.json")>]
let aboutJson: AboutContactT = jsNative

[<Import("default", "../../data/contact.json")>]
let contactJson: AboutContactT = jsNative

[<Import("default", "../../data/pages.json")>]
let pages: PagesT = jsNative


[<Import("default", "../../data/assets.json")>]
let assetsJson: AssetsT = jsNative

let fileTypeToAssetType (s: string) =
    s
    |> _.Split("/")
    |> Array.head
    |> function
        | "image" -> AssetType.Image
        | "video" -> AssetType.Video
        | "audio" -> AssetType.Audio
        | _ -> failwith "Unknown asset type"

let s3slugs: obj = importDefault "../../data/s3.json"

let mapSlugAssets =
    assetsJson.items
    |> Seq.map(fun item ->
        item.id,
        {
            slug =
                if s3slugs?(item.slug) then
                    s3slugs?(item.slug)
                else
                    "medias/" + item.slug
            width = item.metadata.pixelWidth
            height = item.metadata.pixelHeight
            thumbnail = "medias/thumbnails/" + item.slug |> Some
            ``type`` = fileTypeToAssetType item.mimeType
        })
    |> Map.ofSeq

let sponsorsLogos =
    assetsJson.items
    |> Seq.filter(fun x -> x.tags |> Seq.contains "sponsor")
    |> Seq.sortBy(fun x -> x.metadata?ordre)
    |> Seq.map(fun x -> {
        name = x.metadata?nom
        src = "medias/" + x.slug
        url = x.metadata?url
    })

let about: AboutContact = aboutJson.items |> Seq.head |> _.data.content.fr
let contact: AboutContact = contactJson.items |> Seq.head |> _.data.content.fr

let navTaggedItems: Map<TaggedTopic, BaseNavItem * string> =
    Map [
        Accueil, ({ title = "Accueil"; slug = "/" }, "accueil")
        Programmation,
        ({
            title = "Programmation"
            slug = "/programmation"
         },
         "programmation")
        Atelier,
        ({
            title = "Atelier"
            slug = "/workshop"
         },
         "atelier")
        Boutique, ({ title = "Boutique"; slug = "/shop" }, "boutique")
    ]

let navStaticItems: Map<StaticTopic, BaseNavItem> =
    Map [
        Apropos, { title = "À propos"; slug = "/about" }
        Contact,
        {
            title = "Informations pratiques"
            slug = "/contact"
        }
    ]

let navItems =
    let tagged =
        navTaggedItems
        |> Map.toList
        |> List.map(fun (k, v) -> TaggedTopic k, TaggedItem v)
    let static' =
        navStaticItems
        |> Map.toList
        |> List.map(fun (k, v) -> StaticTopic k, BaseItem v)
    List.append tagged static' |> Map.ofList

let getBaseItemFromTopic (topic: Topic) =
    match topic with
    | StaticTopic t -> navStaticItems.[t]
    | TaggedTopic t -> navTaggedItems.[t] |> fst

// import import.meta.env.MODE from vite config
[<Global("import.meta.env.BASE_URL")>]
let baseR: string = jsNative

let addedTrailingSlash =
    match baseR with
    | "/" -> baseR
    | _ -> baseR + "/"

let pageTaggedTopics =
    navTaggedItems.Keys |> Seq.toList |> Seq.filter(fun x -> x <> Accueil)

let tryCmstagToTaggedTopic s =
    navTaggedItems
    |> Map.tryPick(fun k v ->
        match v with
        | (_, t: string) when s = t -> Some k
        | _ -> None)

let trySlugToTopic s =
    navItems
    |> Map.tryPick(fun k v ->
        let t =
            (match v with
             | BaseItem t -> t
             | TaggedItem(t, _) -> t)
            : BaseNavItem
        match t.slug = s with
        | true -> Some k
        | false -> None)


let mapPageSlugToTag =
    let cmsTags =
        pageTaggedTopics |> Seq.map(fun x -> navTaggedItems[x] |> snd) |> Set.ofSeq
    pages.items
    |> Seq.map(fun x ->
        x.data.unit.fr.tags
        |> Set
        |> Set.intersect cmsTags
        |> Seq.head
        |> (fun y -> x.data.id.iv, y |> tryCmstagToTaggedTopic |> Option.defaultValue Accueil))
    |> Map.ofSeq

let realNonEmptyTopics =
    let temp =
        pageTaggedTopics
        |> Seq.filter(fun x -> mapPageSlugToTag |> Map.exists(fun k v -> v = x))
        |> Array.ofSeq
    // Use temp as TaggedTopic sequence and cast the final result to Tag array
    [| Programmation; Atelier; Boutique |]
    |> Array.filter(fun x -> Array.contains x temp)
    |> Array.map TaggedTopic
    |> (fun x -> Array.append x [| StaticTopic Apropos; StaticTopic Contact |])


let breakWidth = [| 640; 768; 1024; 1280; 1536 |]

let breakColumnsList = [ Xs, 1; Sm, 1; Md, 2; Lg, 2; Xl, 3; Xxl, 4 ]

let breakColumns = Map breakColumnsList
let breakQueries =
    seq {
        yield Xs, $"(max-width: {breakWidth[0] - 1}px)"
        for i in 0 .. (breakWidth.Length - 2) do
            let brekppoint = breakColumnsList[i + 1] |> fst
            yield brekppoint, $"(min-width: {breakWidth[i]}px) and (max-width: {breakWidth.[i + 1] - 1}px)"
        yield Xxl, $"(min-width: {breakWidth.[breakWidth.Length - 1]}px)"
    }
