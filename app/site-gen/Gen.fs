namespace Site

open System
open System.Text
open System.Text.Json
open System.Security.Cryptography
open System.IO

open FsHttp
open FSharp.Data
open Amazon.S3.Model
open Amazon.S3

open Site

type UnitOrSome =
    | Unit of unit
    | Some of string

module Gen =

    let sha256 = SHA256.Create()

    let toSha256Base64 (hashAlgo: SHA256) : string -> string =
        Encoding.UTF8.GetBytes >> hashAlgo.ComputeHash >> Convert.ToBase64String

    let hashAndConvertToBase64 = sha256 |> toSha256Base64

    let options = JsonSerializerOptions(WriteIndented = true)
    options.Encoder <- System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

    let http = Conf.fsReadyHttp()
    let config = Conf.getConfig()
    let appName = config["APP_NAME"]
    let s3Bucket = config["S3_BUCKET"]

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

    let checkHash (asset: AssetsT.Item) hash =
        hashAndConvertToBase64 $"{hash}{asset.FileName}{asset.FileSize}" = asset.FileHash

    let getHashStream: Stream -> string = sha256.ComputeHash >> Convert.ToBase64String

    let getS3Client () =
        let secret = Conf.loadSecret()
        let region = config["S3_REGION"]

        new AmazonS3Client(
            secret.s3AccessKeyId,
            secret.s3SecretAccessKey,
            Amazon.RegionEndpoint.GetBySystemName(region)
        )

    let getS3AssetUrl (s3: AmazonS3Client) (slug: string) =
        try
            GetPreSignedUrlRequest(BucketName = s3Bucket, Key = slug, Expires = DateTime.Now.AddDays(7.0))
            |> s3.GetPreSignedURL
            |> Ok
        with :? System.AggregateException ->
            Error $"getS3AssetUrl : Not found {slug}"

    let getS3ObjectHash (s3: AmazonS3Client) (slug: string) =
        let s3ObjectRequest =
            new GetObjectRequest(BucketName = s3Bucket, Key = slug, ChecksumMode = ChecksumMode.ENABLED)

        try
            let hash = s3.GetObjectAsync(s3ObjectRequest).Result |> _.ChecksumSHA256

            match hash with
            | null -> Error $"getS3ObjectHash : no hash found {slug}"
            | "" -> Error $"getS3ObjectHash : empty hash {slug}"
            | _ -> Ok hash
        with :? System.AggregateException ->
            Error $"getS3ObjectHash : Not found {slug}"

    let uploadToS3 (s3: AmazonS3Client) (asset: AssetsT.Item) =
        printfn "Uploading %s" asset.Slug

        let href = asset.Links.ContentSlug.Href
        use stream = http { GET href } |> Request.send |> Response.toStream

        let putRequest =
            new PutObjectRequest(
                BucketName = s3Bucket,
                Key = asset.Slug,
                InputStream = stream,
                ContentType = asset.MimeType,
                ChecksumAlgorithm = ChecksumAlgorithm.SHA256
            )

        putRequest.Headers.ContentLength <- asset.FileSize

        putRequest
        |> s3.PutObjectAsync
        |> fun task -> task.Result.HttpStatusCode
        |> function
            | System.Net.HttpStatusCode.OK -> Ok asset.Slug
            | _ -> Error "upload failed"

    let refreshOrUploadToS3 (s3: AmazonS3Client) (asset: AssetsT.Item) =
        let slug = asset.Slug

        match getS3ObjectHash s3 slug with
        | Ok hash ->
            if checkHash asset hash then
                Ok slug
            else
                Error "hash mismatch"
        | Error _ -> Error "hash not found"
        |> function
            | Ok _ -> Ok slug
            | Error _ -> uploadToS3 s3 asset
        |> function
            | Ok _ -> getS3AssetUrl s3 slug
            | Error _ -> Error "upload failed"

    let refreshS3links () =
        let s3 = getS3Client()

        Json.JsonSerializer.Deserialize<Map<string, string>>(Path.Combine(dataDir, "s3.json"))
        |> Map.map(fun slug url ->
            match getS3AssetUrl s3 slug with
            | Ok newUrl -> slug, newUrl
            | Error _ -> slug, url)
        |> Conf.Serialize Conf.jsonOptions
        |> Conf.writeTextToFile(Path.Combine(dataDir, "s3.json"))

    let checkFile (asset: AssetsT.Item) =
        if not(Directory.Exists(assetsDir)) then
            Directory.CreateDirectory(assetsDir) |> ignore

        let path = Path.Combine(assetsDir, asset.Slug)

        if File.Exists(path) then
            use fileStream = File.OpenRead(path)
            fileStream |> getHashStream |> checkHash asset
        else
            false

    let downloadAsset (asset: AssetsT.Item) =
        printfn "Downloading %s" asset.Slug
        let path = Path.Combine(assetsDir, asset.Slug)
        let href = asset.Links.ContentSlug.Href
        http { GET href } |> Request.send |> Response.saveFile path

        if asset.IsImage then
            let thumbnailPath = Path.Combine(thumbDir, asset.Slug)

            if not(Directory.Exists(thumbDir)) then
                Directory.CreateDirectory(thumbDir) |> ignore

            http {
                GET href
                query [ "width", "400"; "mode", "Max" ]
            }
            |> Request.send
            |> Response.saveFile thumbnailPath


    let refreshAsset (mapS3: Map<string, string>) (asset: AssetsT.Item) =

        printfn "Checking %s" asset.Slug
        let path = Path.Combine(assetsDir, asset.Slug)

        // if asset exceeds 50MB, upload to S3
        if asset.FileSize > 50 * 1024 * 1024 then
            let s3 = getS3Client()

            match refreshOrUploadToS3 s3 asset with
            | Ok url ->
                printfn "Uploaded %s to %s" asset.Slug url
                Some url
            | Error msg -> printfn "Failed to upload %s: %s" asset.Slug msg |> Unit
        else if not(checkFile asset) then
            downloadAsset asset |> Unit
        else
            printfn "Already up-to-date %s" asset.Slug |> Unit
        |> function
            | Unit _ -> mapS3
            | Some url -> mapS3.Add(asset.Slug, url)

    let refreshAssets () =
        let mapS3 = Map.empty

        AssetsT.GetSample().Items
        |> Seq.fold refreshAsset mapS3
        |> Conf.Serialize Conf.jsonOptions
        |> Conf.writeTextToFile(Path.Combine(dataDir, "s3.json"))

        let fromSite = AssetsT.GetSample().Items |> Seq.map _.Slug |> Set.ofSeq

        let fromDisk =
            Directory.GetFiles(assetsDir) |> Array.map Path.GetFileName |> Set.ofArray

        fromDisk - fromSite
        |> Set.iter(fun slug ->
            printfn "Deleting obsolete asset %s" slug
            File.Delete(Path.Combine(assetsDir, slug)))

        let fromSiteImages =
            AssetsT.GetSample().Items |> Seq.filter _.IsImage |> Seq.map _.Slug |> Set.ofSeq

        let fromDiskImages =
            Directory.GetFiles(thumbDir) |> Array.map Path.GetFileName |> Set.ofArray

        fromDiskImages - fromSiteImages
        |> Set.iter(fun slug ->
            printfn "Deleting obsolete thumbnail %s" slug
            File.Delete(Path.Combine(thumbDir, slug)))
