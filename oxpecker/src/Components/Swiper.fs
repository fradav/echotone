namespace Oxpecker.Solid.Imports

open Oxpecker.Solid
open Fable.Core

[<CompiledName("swiper-container")>]
type SwiperContainer() =
    inherit RegularNode()

[<CompiledName("swiper-slide")>]
type SwiperSlide() =
    inherit RegularNode()

module Swiper =
    [<Import("register", "swiper/element/bundle")>]
    let register () : unit = jsNative
