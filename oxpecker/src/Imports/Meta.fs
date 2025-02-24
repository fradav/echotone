namespace Oxpecker.Solid.Meta

open Oxpecker.Solid
open Fable.Core

[<AutoOpen>]
module Bindings =
    [<Erase; Import("MetaProvider", "@solidjs/meta")>]
    type MetaProvider() =
        inherit RegularNode()

    [<Erase; Import("Title", "@solidjs/meta")>]
    type Title() =
        inherit RegularNode()

    [<Erase; Import("Base", "@solidjs/meta")>]
    type Base() =
        inherit RegularNode()

        [<Erase>]
        member this.href
            with set (value: string) = ()

        [<Erase>]
        member this.target
            with set (value: string) = ()


    [<Erase; Import("Link", "@solidjs/meta")>]
    type Link() =
        inherit RegularNode()

        [<Erase>]
        member this.href
            with set (value: string) = ()

        [<Erase>]
        member this.rel
            with set (value: string) = ()

    [<Erase; Import("Meta", "@solidjs/meta")>]
    type Meta() =
        inherit RegularNode()

        [<Erase>]
        member this.name
            with set (value: string) = ()

        [<Erase>]
        member this.content
            with set (value: string) = ()
