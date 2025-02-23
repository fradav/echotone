open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.DotNet.DotNet.Options
open Site


let runOrDefault initTargets defaultTarget args =
    let execContext = Context.FakeExecutionContext.Create false "build.fsx" []
    Context.setExecutionContext (Context.RuntimeContext.Fake execContext)
    initTargets () |> ignore

    try
        match args with
        | [| target |] -> Target.runOrDefault target
        | _ -> Target.runOrDefault defaultTarget

        0
    with e ->
        printfn "%A" e
        1

let initTargets () =
    Target.create "validate" (fun _ -> Conf.validateOrRefresh ())

    Target.create "refreshToken" (fun _ -> Conf.refreshToken ())

    Target.create "refreshJsons" (fun _ -> Conf.refreshJsons ())

    Target.create "refreshImages" (fun _ -> Gen.refreshAssets ())

    Target.create "refreshS3Links" (fun _ -> Gen.refreshS3links ())

    Target.create "dev" (fun _ -> Shell.Exec("bun", "run dev", "oxpecker") |> ignore)

    Target.create "fcm" (fun _ -> Shell.Exec("bunx", "fcm .", "oxpecker") |> ignore)

    Target.create "bunInstall" (fun _ -> Shell.Exec("bun", "install", "oxpecker") |> ignore)

    Target.create "build" (fun _ ->
        let app_root = Conf.getConfig()["APP_ROOT"]

        let result =
            DotNet.exec
                (withWorkingDirectory "oxpecker")
                "fable"
                $"--noCache --extension .jsx --run vite build --base={app_root}"

        if not result.OK then
            failwith "fable build failed")

    Target.create "buildDev" (fun _ ->
        let app_root = Conf.getConfig()["APP_ROOT"]

        let result =
            DotNet.exec (withWorkingDirectory "oxpecker") "fable" $"--noCache --extension .jsx --run vite build"

        if not result.OK then
            failwith "fable build failed")


    "validate" ==> "refreshJsons" |> ignore

    "refreshJsons" ==> "refreshImages" |> ignore

    "refreshJsons" ==> "refreshImages" |> ignore

    "refreshImages" ==> "dev" |> ignore

    "refreshImages" ==> "bunInstall" ==> "fcm" ==> "build" |> ignore

    "refreshImages" ==> "fcm" ==> "buildDev" |> ignore


[<EntryPoint>]
let main args = runOrDefault initTargets "build" args
