namespace Sprightly.Components.Common

open System.ComponentModel

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

type Orientation = 
    | Horizontal
    | Vertical

type SprightlyIconButton() =
    inherit Button()
    
    let event = new Event<_,_>()

    let mutable icon = ""
    let mutable iconSize = base.FontSize

    member public this.Icon
        with get() = icon 
        and set value =
            icon <- value
            this.OnPropertyChanged "Icon"

    member public this.IconSize 
        with get() = iconSize
        and set value = 
            iconSize <- value 
            this.OnPropertyChanged "IconSize"

    member val public IconOrientation = Orientation.Horizontal with get, set

    interface INotifyPropertyChanged with 
        [<CLIEvent>] member __.PropertyChanged = event.Publish


// Code required to add the RecentProjectButton to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousSprightlyListElement= 
    let iconValueAttribKey = 
        AttributeKey<string> "SprightlyListElement_IconValue"
    let iconSizeValueAttribKey = 
        AttributeKey<float> "SprightlyListElement_IconSizeValue"
    let iconOrientationValueAttribKey = 
        AttributeKey<Orientation> "SprightlyListElement_IconOrientationValue"

    type Fabulous.XamarinForms.View with 
        static member inline SprightlyIconButton(?icon: string, ?iconSize: float, ?iconOrientation: Orientation,
                                                 // Inherited attributes
                                                 ?text: string, ?command: (unit -> unit),
                                                 ?horizontalOptions, ?verticalOptions, ?margin, ?gestureRecognizers, ?anchorX, ?anchorY, ?backgroundColor, 
                                                 ?inputTransparent, ?isEnabled, ?isVisible, ?opacity, ?rotation, ?rotationX, ?rotationY, ?scale, ?style, 
                                                 ?translationX, ?translationY, ?resources, ?styles, ?styleSheets, ?classId, ?styleId, ?automationId) =

            let attribCount = 0
            let attribCount = match icon with Some _ -> attribCount + 1 | None -> attribCount
            let attribCount = match iconSize with Some _ -> attribCount + 1 | None -> attribCount
            let attribCount = match iconOrientation with Some _ -> attribCount + 1 | None -> attribCount

            let attribs = ViewBuilders.BuildButton(attribCount, 
                                                   ?text = text,
                                                   ?command=command, ?horizontalOptions=horizontalOptions, ?verticalOptions=verticalOptions, ?margin=margin,
                                                   ?gestureRecognizers=gestureRecognizers, ?anchorX=anchorX, ?anchorY=anchorY, ?backgroundColor=backgroundColor, 
                                                   ?inputTransparent=inputTransparent, ?isEnabled=isEnabled, ?isVisible=isVisible, ?opacity=opacity,
                                                   ?rotation=rotation, ?rotationX=rotationX, ?rotationY=rotationY, ?scale=scale, ?style=style, ?translationX=translationX, ?translationY=translationY, 
                                                   ?resources=resources, ?styles=styles, ?styleSheets=styleSheets, ?classId=classId, ?styleId=styleId, ?automationId=automationId)

            match icon with | None -> () | Some (v: string) -> attribs.Add (iconValueAttribKey, v)
            match iconSize with | None -> () | Some (v: float) -> attribs.Add (iconSizeValueAttribKey, v)
            match iconOrientation with | None -> () | Some (v: Orientation) -> attribs.Add (iconOrientationValueAttribKey, v)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: SprightlyIconButton) =
                ViewBuilders.UpdateButton(registry, prevOpt, source, target)
                source.UpdatePrimitive (prevOpt, target, iconValueAttribKey, (fun target v -> target.Icon <- v))
                source.UpdatePrimitive (prevOpt, target, iconSizeValueAttribKey, (fun target v -> target.IconSize <- v))
                source.UpdatePrimitive (prevOpt, target, iconOrientationValueAttribKey, (fun target v -> target.IconOrientation <- v))

            ViewElement.Create(SprightlyIconButton, update, attribs)

