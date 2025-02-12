namespace Site

open System
open System.Text
open System.Text.Json
open System.Security.Cryptography
open System.IO

open FsHttp
open FSharp.Data

open Site

module Gen =

    let sha256 = SHA256.Create()

    let toSha256Base64 (hashAlgo: SHA256) : string -> string =
        Encoding.UTF8.GetBytes >> hashAlgo.ComputeHash >> Convert.ToBase64String

    let hashAndConvertToBase64 = sha256 |> toSha256Base64

    let options = JsonSerializerOptions(WriteIndented = true)
    options.Encoder <- System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

    let http = Conf.fsReadyHttp ()
    let config = Conf.getConfig ()
    let appName = config["APP_NAME"]

    [<Literal>]
    let sourceDir = __SOURCE_DIRECTORY__ + "/../.."

    [<Literal>]
    let dataDir = sourceDir + "/data"

    [<Literal>]
    let assetsJson = dataDir + "/assets.json"

    [<Literal>]
    let aboutJson = dataDir + "/about.json"

    [<Literal>]
    let contactJson = dataDir + "/contact.json"

    [<Literal>]
    let pagesJson = dataDir + "/pages.json"

    type AssetsT = JsonProvider<assetsJson>
    type AboutT = JsonProvider<aboutJson>
    type ContactT = JsonProvider<contactJson>
    type PagesT = JsonProvider<pagesJson>

    let assetsDir = Path.Combine(sourceDir, config["ASSETS_DIR"])
    let thumbDir = Path.Combine(assetsDir, "thumbnails")

    let checkFile (asset: AssetsT.Item) =
        if not (Directory.Exists(assetsDir)) then
            Directory.CreateDirectory(assetsDir) |> ignore

        let path = Path.Combine(assetsDir, asset.Slug)

        if File.Exists(path) then
            use fileStream = File.OpenRead(path)
            let hashStream = Convert.ToBase64String(sha256.ComputeHash(fileStream))

            asset.FileHash = hashAndConvertToBase64 $"{hashStream}{asset.FileName}{asset.FileSize}"
        else
            false

    let refreshAsset (asset: AssetsT.Item) =

        printfn "Checking %s" asset.Slug
        let path = Path.Combine(assetsDir, asset.Slug)

        if not (checkFile asset) then
            printfn "Downloading %s" asset.Slug
            let href = asset.Links.ContentSlug.Href
            http { GET href } |> Request.send |> Response.saveFile path
            // Download thumbnail if present
            if asset.IsImage then
                let thumbnailPath = Path.Combine(thumbDir, asset.Slug)

                if not (Directory.Exists(thumbDir)) then
                    Directory.CreateDirectory(thumbDir) |> ignore

                http {
                    GET href
                    query [ "width", "100"; "mode", "Max" ]
                }
                |> Request.send
                |> Response.saveFile thumbnailPath

    let refreshAssets () =
        AssetsT.GetSample().Items |> Seq.iter refreshAsset
        let fromSite = AssetsT.GetSample().Items |> Seq.map _.Slug |> Set.ofSeq

        let fromDisk =
            Directory.GetFiles(assetsDir) |> Array.map Path.GetFileName |> Set.ofArray

        fromDisk - fromSite
        |> Set.iter (fun slug ->
            printfn "Deleting obsolete asset %s" slug
            File.Delete(Path.Combine(assetsDir, slug)))

        let fromSiteImages =
            AssetsT.GetSample().Items |> Seq.filter _.IsImage |> Seq.map _.Slug |> Set.ofSeq

        let fromDiskImages =
            Directory.GetFiles(thumbDir) |> Array.map Path.GetFileName |> Set.ofArray

        fromDiskImages - fromSiteImages
        |> Set.iter (fun slug ->
            printfn "Deleting obsolete thumbnail %s" slug
            File.Delete(Path.Combine(thumbDir, slug)))
