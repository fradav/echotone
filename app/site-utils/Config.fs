namespace Site

open System.IO
open System.Net
open FsHttp
open dotenv.net

module Conf =
    open System.Text.Json

    let writeTextToFile (path: string) (content: string) = File.WriteAllText(path, content)

    let sourceDir = Path.Combine(__SOURCE_DIRECTORY__, "..", "..")

    let getConfig () =
        DotEnv.Read(DotEnvOptions(envFilePaths = [ Path.Combine(sourceDir, ".env-app") ]))

    let loadSecret () =
        if File.Exists(__SOURCE_DIRECTORY__ + "/../../.env-secret.local") then
            DotEnv.Load(DotEnvOptions(envFilePaths = [ __SOURCE_DIRECTORY__ + "/../../.env-secret.local" ]))

        System.Environment.GetEnvironmentVariable("CLIENT_SECRET")

    let refreshToken () =
        let envVars = getConfig ()
        let secret = loadSecret ()

        http {
            config_useBaseUrl envVars["BASE_URL"]
            POST "/identity-server/connect/token"
            body

            formUrlEncoded
                [ "grant_type", "client_credentials"
                  "client_id", envVars["CLIENT_ID"]
                  "client_secret", secret ]
        }
        |> Request.send
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
        let token = getToken ()

        http {
            config_useBaseUrl envVars["BASE_URL"]
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
