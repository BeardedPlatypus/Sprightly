namespace Sprightly.Components.Common

open System.ComponentModel
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// The <see cref="CollapsiblePaneHeader"/> defines a custom 
/// pane header button to open and collapse the pane with.
/// </summary>
type CollapsiblePaneHeader() = 
    inherit Button() 

    let event = new Event<_,_>()

    let mutable isOpen: bool = false
    let mutable headerText = ""

    /// <summary>
    /// Value indicating whether this pane is open or collapsed.
    /// </summary>
    member public this.IsOpen
        with get() = isOpen
        and set value = 
            isOpen <- value
            this.OnPropertyChanged "IsOpen"

    /// <summary>
    /// The header text of this pane header.
    /// </summary>
    member public this.HeaderText
        with get() = headerText
        and set value = 
            headerText <- value
            this.OnPropertyChanged "HeaderText"

    interface INotifyPropertyChanged with 
        [<CLIEvent>] member __.PropertyChanged = event.Publish


// Code required to add the CollapsiblePaneHeader to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousCollapsiblePaneHeader = 
    let isOpenValueAttribKey = 
        AttributeKey<bool> "CollapsiblePaneHeader_IsOpenValue"
    let headerTextValueAttribKey = 
        AttributeKey<string> "CollapsiblePaneHeader_HeaderTextValue"

    type Fabulous.XamarinForms.View with 
        static member inline CollapsiblePaneHeader(?isOpen: bool, ?headerText: string,
                                                   // Inherited attributes
                                                   ?command: (unit -> unit),
                                                   ?horizontalOptions, ?verticalOptions, ?margin, ?gestureRecognizers, ?anchorX, ?anchorY, ?backgroundColor, 
                                                   ?inputTransparent, ?isEnabled, ?isVisible, ?opacity, ?rotation, ?rotationX, ?rotationY, ?scale, ?style, 
                                                   ?translationX, ?translationY, ?resources, ?styles, ?styleSheets, ?classId, ?styleId, ?automationId) =

            let attribCount = 0
            let attribCount = match isOpen with Some _ -> attribCount + 1 | None -> attribCount
            let attribCount = match headerText with Some _ -> attribCount + 1 | None -> attribCount
            
            let attribs = ViewBuilders.BuildButton(attribCount, 
                                                   ?command=command, ?horizontalOptions=horizontalOptions, ?verticalOptions=verticalOptions, ?margin=margin,
                                                   ?gestureRecognizers=gestureRecognizers, ?anchorX=anchorX, ?anchorY=anchorY, ?backgroundColor=backgroundColor, 
                                                   ?inputTransparent=inputTransparent, ?isEnabled=isEnabled, ?isVisible=isVisible, ?opacity=opacity,
                                                   ?rotation=rotation, ?rotationX=rotationX, ?rotationY=rotationY, ?scale=scale, ?style=style, ?translationX=translationX, ?translationY=translationY, 
                                                   ?resources=resources, ?styles=styles, ?styleSheets=styleSheets, ?classId=classId, ?styleId=styleId, ?automationId=automationId)

            match isOpen with | None -> () | Some (v: bool) -> attribs.Add (isOpenValueAttribKey, v)
            match headerText with | None -> () | Some (v: string) -> attribs.Add (headerTextValueAttribKey, v)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: CollapsiblePaneHeader) =
                ViewBuilders.UpdateButton(registry, prevOpt, source, target)
                source.UpdatePrimitive (prevOpt, target, isOpenValueAttribKey, (fun target v -> target.IsOpen <- v))
                source.UpdatePrimitive (prevOpt, target, headerTextValueAttribKey, (fun target v -> target.HeaderText <- v))

            ViewElement.Create(CollapsiblePaneHeader, update, attribs)
