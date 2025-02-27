// ts2fable 0.9.0-build.738
module rec Flowbite

#nowarn "3390" // disable warnings for invalid XML comments

open System
open Fable.Core
open Fable.Core.JS
open Browser.Types


type [<AllowNullLiteral>] InstanceOptions =
    abstract id: string option with get, set
    abstract ``override``: bool option with get, set

type [<AllowNullLiteral>] EventListenerInstance =
    abstract element: HTMLElement with get, set
    abstract ``type``: string with get, set
    abstract handler: EventListenerOrEventListenerObject with get, set



type [<AllowNullLiteral>] Collapse =
    inherit CollapseInterface
    abstract _instanceId: string with get, set
    abstract _targetEl: HTMLElement option with get, set
    abstract _triggerEl: HTMLElement option with get, set
    abstract _options: CollapseOptions with get, set
    abstract _visible: bool with get, set
    abstract _initialized: bool with get, set
    abstract _clickHandler: EventListenerOrEventListenerObject with get, set
    abstract init: unit -> unit
    abstract destroy: unit -> unit
    abstract removeInstance: unit -> unit
    abstract destroyAndRemoveInstance: unit -> unit
    abstract collapse: unit -> unit
    abstract expand: unit -> unit
    abstract toggle: unit -> unit
    abstract updateOnCollapse: callback: (unit -> unit) -> unit
    abstract updateOnExpand: callback: (unit -> unit) -> unit
    abstract updateOnToggle: callback: (unit -> unit) -> unit

type [<AllowNullLiteral>] CollapseStatic =
    [<EmitConstructor>] abstract Create: ?targetEl: HTMLElement * ?triggerEl: HTMLElement * ?options: CollapseOptions * ?instanceOptions: InstanceOptions -> Collapse

type [<AllowNullLiteral>] CollapseOptions =
    abstract onCollapse: (CollapseInterface -> unit) option with get, set
    abstract onExpand: (CollapseInterface -> unit) option with get, set
    abstract onToggle: (CollapseInterface -> unit) option with get, set

type [<AllowNullLiteral>] CollapseInterface =
    abstract _targetEl: HTMLElement option with get, set
    abstract _triggerEl: HTMLElement option with get, set
    abstract _options: CollapseOptions with get, set
    abstract _visible: bool with get, set
    abstract init: unit -> unit
    abstract collapse: unit -> unit
    abstract expand: unit -> unit
    abstract toggle: unit -> unit
    abstract destroy: unit -> unit
    abstract removeInstance: unit -> unit
    abstract destroyAndRemoveInstance: unit -> unit


type [<AllowNullLiteral>] Carousel =
    inherit CarouselInterface
    abstract _instanceId: string with get, set
    abstract _carouselEl: HTMLElement with get, set
    abstract _items: ResizeArray<CarouselItem> with get, set
    abstract _indicators: ResizeArray<IndicatorItem> with get, set
    abstract _activeItem: CarouselItem with get, set
    abstract _intervalDuration: float with get, set
    abstract _intervalInstance: float with get, set
    abstract _options: CarouselOptions with get, set
    abstract _initialized: bool with get, set
    /// initialize carousel and items based on active one
    abstract init: unit -> unit
    abstract destroy: unit -> unit
    abstract removeInstance: unit -> unit
    abstract destroyAndRemoveInstance: unit -> unit
    abstract getItem: position: float -> CarouselItem
    /// <summary>Slide to the element based on id</summary>
    /// <param name="position" />
    abstract slideTo: position: float -> unit
    /// Based on the currently active item it will go to the next position
    abstract next: unit -> unit
    /// Based on the currently active item it will go to the previous position
    abstract prev: unit -> unit
    /// <summary>This method applies the transform classes based on the left, middle, and right rotation carousel items</summary>
    /// <param name="rotationItems" />
    abstract _rotate: rotationItems: RotationItems -> unit
    /// Set an interval to cycle through the carousel items
    abstract cycle: unit -> unit
    /// Clears the cycling interval
    abstract pause: unit -> unit
    /// Get the currently active item
    abstract getActiveItem: unit -> CarouselItem
    /// <summary>Set the currently active item and data attribute</summary>
    /// <param name="position" />
    abstract _setActiveItem: item: CarouselItem -> unit
    abstract updateOnNext: callback: (unit -> unit) -> unit
    abstract updateOnPrev: callback: (unit -> unit) -> unit
    abstract updateOnChange: callback: (unit -> unit) -> unit

type [<AllowNullLiteral>] CarouselStatic =
    [<EmitConstructor>] abstract Create: ?carouselEl: HTMLElement * ?items: ResizeArray<CarouselItem> * ?options: CarouselOptions * ?instanceOptions: InstanceOptions -> Carousel

type [<AllowNullLiteral>] CarouselItem =
    abstract position: float with get, set
    abstract el: HTMLElement with get, set

type [<AllowNullLiteral>] IndicatorItem =
    abstract position: float with get, set
    abstract el: HTMLElement with get, set

type [<AllowNullLiteral>] RotationItems =
    abstract left: CarouselItem with get, set
    abstract middle: CarouselItem with get, set
    abstract right: CarouselItem with get, set

type [<AllowNullLiteral>] CarouselOptions =
    abstract defaultPosition: float option with get, set
    abstract indicators: {| items: ResizeArray<IndicatorItem> option; activeClasses: string option; inactiveClasses: string option |} option with get, set
    abstract interval: float option with get, set
    abstract onNext: (CarouselInterface -> unit) option with get, set
    abstract onPrev: (CarouselInterface -> unit) option with get, set
    abstract onChange: (CarouselInterface -> unit) option with get, set

type [<AllowNullLiteral>] CarouselInterface =
    abstract _items: ResizeArray<CarouselItem> with get, set
    abstract _indicators: ResizeArray<IndicatorItem> with get, set
    abstract _activeItem: CarouselItem with get, set
    abstract _intervalDuration: float with get, set
    abstract _intervalInstance: float with get, set
    abstract _options: CarouselOptions with get, set
    abstract init: unit -> unit
    abstract getItem: position: float -> CarouselItem
    abstract getActiveItem: unit -> CarouselItem
    abstract _setActiveItem: item: CarouselItem -> unit
    abstract slideTo: position: float -> unit
    abstract next: unit -> unit
    abstract prev: unit -> unit
    abstract _rotate: rotationItems: RotationItems -> unit
    abstract cycle: unit -> unit
    abstract pause: unit -> unit
    abstract destroy: unit -> unit
    abstract removeInstance: unit -> unit
    abstract destroyAndRemoveInstance: unit -> unit


type [<AllowNullLiteral>] Modal =
    inherit ModalInterface
    abstract _instanceId: string with get, set
    abstract _targetEl: HTMLElement option with get, set
    abstract _options: ModalOptions with get, set
    abstract _isHidden: bool with get, set
    abstract _backdropEl: HTMLElement option with get, set
    abstract _clickOutsideEventListener: EventListenerOrEventListenerObject with get, set
    abstract _keydownEventListener: EventListenerOrEventListenerObject with get, set
    abstract _eventListenerInstances: ResizeArray<EventListenerInstance> with get, set
    abstract _initialized: bool with get, set
    abstract init: unit -> unit
    abstract destroy: unit -> unit
    abstract removeInstance: unit -> unit
    abstract destroyAndRemoveInstance: unit -> unit
    abstract _createBackdrop: unit -> unit
    abstract _destroyBackdropEl: unit -> unit
    abstract _setupModalCloseEventListeners: unit -> unit
    abstract _removeModalCloseEventListeners: unit -> unit
    abstract _handleOutsideClick: target: EventTarget -> unit
    abstract _getPlacementClasses: unit -> ResizeArray<string>
    abstract toggle: unit -> unit
    abstract show: unit -> unit
    abstract hide: unit -> unit
    abstract isVisible: unit -> bool
    abstract isHidden: unit -> bool
    abstract addEventListenerInstance: element: HTMLElement * ``type``: string * handler: EventListenerOrEventListenerObject -> unit
    abstract removeAllEventListenerInstances: unit -> unit
    abstract getAllEventListenerInstances: unit -> ResizeArray<EventListenerInstance>
    abstract updateOnShow: callback: (unit -> unit) -> unit
    abstract updateOnHide: callback: (unit -> unit) -> unit
    abstract updateOnToggle: callback: (unit -> unit) -> unit

type [<AllowNullLiteral>] ModalStatic =
    [<EmitConstructor>] abstract Create: ?targetEl: HTMLElement * ?options: ModalOptions * ?instanceOptions: InstanceOptions -> Modal

type [<StringEnum>] [<RequireQualifiedAccess>] modalBackdrop =
    | Static
    | Dynamic

type [<StringEnum>] [<RequireQualifiedAccess>] modalPlacement =
    | [<CompiledName("top-left")>] TopLeft
    | [<CompiledName("top-center")>] TopCenter
    | [<CompiledName("top-right")>] TopRight
    | [<CompiledName("center-left")>] CenterLeft
    | Center
    | [<CompiledName("center-right")>] CenterRight
    | [<CompiledName("bottom-left")>] BottomLeft
    | [<CompiledName("bottom-center")>] BottomCenter
    | [<CompiledName("bottom-right")>] BottomRight

type [<AllowNullLiteral>] ModalOptions =
    abstract placement: modalPlacement option with get, set
    abstract backdropClasses: string option with get, set
    abstract backdrop: modalBackdrop option with get, set
    abstract closable: bool option with get, set
    abstract onShow: (ModalInterface -> unit) option with get, set
    abstract onHide: (ModalInterface -> unit) option with get, set
    abstract onToggle: (ModalInterface -> unit) option with get, set

type [<AllowNullLiteral>] ModalInterface =
    abstract _targetEl: HTMLElement option with get, set
    abstract _options: ModalOptions with get, set
    abstract _isHidden: bool with get, set
    abstract _backdropEl: HTMLElement option with get, set
    abstract _clickOutsideEventListener: EventListenerOrEventListenerObject with get, set
    abstract _keydownEventListener: EventListenerOrEventListenerObject with get, set
    abstract init: unit -> unit
    abstract _createBackdrop: unit -> unit
    abstract _destroyBackdropEl: unit -> unit
    abstract _setupModalCloseEventListeners: unit -> unit
    abstract _handleOutsideClick: target: EventTarget -> unit
    abstract _getPlacementClasses: unit -> ResizeArray<string>
    abstract toggle: unit -> unit
    abstract show: unit -> unit
    abstract hide: unit -> unit
    abstract isHidden: unit -> bool
    abstract isVisible: unit -> bool
    abstract destroy: unit -> unit
    abstract removeInstance: unit -> unit
    abstract destroyAndRemoveInstance: unit -> unit
    abstract addEventListenerInstance: element: HTMLElement * ``type``: string * handler: EventListenerOrEventListenerObject -> unit
    abstract removeAllEventListenerInstances: unit -> unit
    abstract getAllEventListenerInstances: unit -> unit

type [<AllowNullLiteral>] IExports =
    abstract initFlowbite: unit -> unit
    abstract Collapse: CollapseStatic
    abstract initCollapses: unit -> unit
    abstract Carousel: CarouselStatic
    abstract initCarousels: unit -> unit
    abstract Modal: ModalStatic
    abstract initModals: unit -> unit
