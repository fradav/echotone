namespace Components

open Oxpecker.Solid

[<AutoOpen>]
module RawNode =
    [<SolidComponent>]
    let html (t: string) () : HtmlElement =
        div (class' = "contents", innerHTML = t)
