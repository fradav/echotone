#r "nuget: Fli"

open System.IO
open System.Text.RegularExpressions
open Fli

let components = [ "collapse"; "carousel"; "modal" ]
let dtsNames = [ "index"; "types"; "interface" ]

let flowbiteLibdir =
    Path.Combine(__SOURCE_DIRECTORY__, "node_modules", "flowbite", "lib", "esm")

let addedFiles = [ Path.Combine(flowbiteLibdir, "dom", "types.d.ts") ]

let outputFile = Path.Combine(__SOURCE_DIRECTORY__, "src", "Imports", "Flowbite.fs")

let getDtsFile dir comp dtsName =
    Path.Combine(dir, "components", comp, $"{dtsName}.d.ts")

let dtsfiles =
    components
    |> List.collect (fun comp -> dtsNames |> List.map (getDtsFile flowbiteLibdir comp))

let ts2fableArgs =
    [ [ "ts2fable" ]; addedFiles; dtsfiles; [ outputFile ] ] |> List.concat

cli {
    Exec "bunx"
    Arguments ts2fableArgs
    WorkingDirectory __SOURCE_DIRECTORY__
}
|> Command.execute

let reIExports =
    Regex(@"type \[<AllowNullLiteral>\] IExports =\n(([^\n]+\n)+)\n", RegexOptions.Multiline)

let getIEexports =
    reIExports.Matches
    >> Seq.cast<Match>
    >> Seq.map _.Groups.[1].Value
    >> Seq.append [ "\n\ntype [<AllowNullLiteral>] IExports =\n" ]
    >> String.concat ""

// Remove all lines matching "^type\s+\w+\s+=\s+__.*$" from outputFile
let removedImport =
    File.ReadAllLines outputFile
    |> Array.filter (fun line -> not (Regex.IsMatch(line, @"^type\s+\w+\s+=\s+__.*$")))
    |> Array.toList
    |> String.concat "\n"

let codeIExports = getIEexports removedImport

let removedImportandIExports = reIExports.Replace(removedImport, "")

File.WriteAllText(outputFile, removedImportandIExports + codeIExports)
