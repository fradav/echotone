open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.DotNet.DotNet.Options
open Fake.JavaScript
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

    Target.create "dev" (fun _ -> Npm.run "dev" (fun o -> { o with WorkingDirectory = "oxpecker" }))

    Target.create "fcm" (fun _ -> Npm.run "fcm" (fun o -> { o with WorkingDirectory = "oxpecker" }))


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

    "refreshImages" ==> "fcm" ==> "build" |> ignore

    "refreshImages" ==> "fcm" ==> "buildDev" |> ignore


[<EntryPoint>]
let main args =
    runOrDefault initTargets "validate" args
