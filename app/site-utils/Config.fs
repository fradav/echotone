namespace Site

open System.IO
open System.Net
open FsHttp
open dotenv.net
open System

module Conf =
    open System.Text.Json

    let writeTextToFile (path: string) (content: string) = File.WriteAllText(path, content)

    let sourceDir = Path.Combine(__SOURCE_DIRECTORY__, "..", "..")
    let secretPath = Path.Combine(sourceDir, ".env-secret.local")
    let envPath = Path.Combine(sourceDir, ".env-app")

    let getConfig () =
        DotEnv.Read(DotEnvOptions(envFilePaths = [ envPath ]))

    let loadSecret () =
        if File.Exists(secretPath) then
            DotEnv.Load(DotEnvOptions(envFilePaths = [ secretPath ]))

        {| baseURL = Environment.GetEnvironmentVariable("BASE_URL")
           clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET")
           s3AccessKeyId = Environment.GetEnvironmentVariable("S3_ACCESS_KEY_ID")
           s3SecretAccessKey = Environment.GetEnvironmentVariable("S3_SECRET_ACCESS_KEY") |}

    let refreshToken () =
        let envVars = getConfig ()
        let secret = loadSecret ()
        // check if secret is empty
        if System.String.IsNullOrEmpty(secret.clientSecret) then
            failwith "CLIENT_SECRET not found"

        let response =
            http {
                config_useBaseUrl secret.baseURL
                POST "/identity-server/connect/token"
                body

                formUrlEncoded
                    [ "grant_type", "client_credentials"
                      "client_id", envVars["CLIENT_ID"]
                      "client_secret", secret.clientSecret ]
            }
            |> Request.send

        if response.statusCode <> HttpStatusCode.OK then
            failwith "Failed to get token"

        response
        |> Response.deserializeJson
        |> (fun x -> (x?access_token).GetString())
        |> (sprintf "TOKEN=%s\n")
        |> writeTextToFile (Path.Combine(sourceDir, ".env"))

    let getToken () =
        if not (File.Exists(Path.Combine(sourceDir, ".env"))) then
            refreshToken ()

        DotEnv.Read()["TOKEN"]


    let fsReadyHttp () =
        let envVars = getConfig ()
        let secret = loadSecret ()
        let token = getToken ()

        http {
            config_useBaseUrl secret.baseURL
            AuthorizationBearer token
        }

    let validateOrRefresh () =
        let appName = getConfig()["APP_NAME"]

        fsReadyHttp () { GET $"/api/apps/{appName}/assets" }
        |> Request.send
        |> function
            | r when r.statusCode = HttpStatusCode.OK ->
                printfn "Token OK"
                ()
            | _ ->
                printfn "Token invalid, refreshing..."
                refreshToken ()

    let jsonOptions = JsonSerializerOptions(WriteIndented = true)

    let Serialize (data: 'a) =
        JsonSerializer.Serialize(data, jsonOptions)

    let refreshJsons () =
        let config = getConfig ()
        let appName = config["APP_NAME"]
        let dataDir = config["DATA_DIR"]

        let siteParts =
            Map
                [ "assets", $"/api/apps/{appName}/assets"
                  "about", $"/api/content/{appName}/about"
                  "contact", $"/api/content/{appName}/contact"
                  "pages", $"/api/content/{appName}/page" ]

        if not (Directory.Exists(dataDir)) then
            Directory.CreateDirectory(dataDir) |> ignore

        siteParts
        |> Map.iter (fun key value ->
            fsReadyHttp () { GET value }
            |> Request.send
            |> Response.toJson
            |> Serialize
            |> writeTextToFile (Path.Combine(dataDir, key + ".json")))
