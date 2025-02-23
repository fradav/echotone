namespace Oxpecker.Solid.Meta

open Oxpecker.Solid
open Fable.Core

[<Erase; Import("Title", "@solidjs/meta")>]
type Title() =
    inherit RegularNode()

[<Erase; Import("MetaProvider", "@solidjs/meta")>]
type MetaProvider() =
    inherit RegularNode()
