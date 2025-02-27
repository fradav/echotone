namespace Site

open Fable.Core

module Imports =
    open Flowbite

    [<ImportAll("flowbite")>]
    let flowbite: IExports = jsNative
