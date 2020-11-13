namespace Sprightly.Components.Common

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms


/// <summary>
/// The <see cref="RecentProjectButton"/> provides the recent project view 
/// element which will be substituted with a custom render to enable
/// hover behaviour.
/// </summary>
type SprightlyButton() =
    inherit Button()


// Code required to add the RecentProjectButton to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousSprightlyButton = 
    type Fabulous.XamarinForms.View with 
        static member inline SprightlyButton(// Inherited attributes
                                             ?text: string, ?command: (unit -> unit),
                                             ?horizontalOptions, ?verticalOptions, ?margin, ?gestureRecognizers, ?anchorX, ?anchorY, ?backgroundColor, 
                                             ?inputTransparent, ?isEnabled, ?isVisible, ?opacity, ?rotation, ?rotationX, ?rotationY, ?scale, ?style, 
                                             ?translationX, ?translationY, ?resources, ?styles, ?styleSheets, ?classId, ?styleId, ?automationId) =

            let attribs = ViewBuilders.BuildButton(0, 
                                                   ?text = text,
                                                   ?command=command, ?horizontalOptions=horizontalOptions, ?verticalOptions=verticalOptions, ?margin=margin,
                                                   ?gestureRecognizers=gestureRecognizers, ?anchorX=anchorX, ?anchorY=anchorY, ?backgroundColor=backgroundColor, 
                                                   ?inputTransparent=inputTransparent, ?isEnabled=isEnabled, ?isVisible=isVisible, ?opacity=opacity,
                                                   ?rotation=rotation, ?rotationX=rotationX, ?rotationY=rotationY, ?scale=scale, ?style=style, ?translationX=translationX, ?translationY=translationY, 
                                                   ?resources=resources, ?styles=styles, ?styleSheets=styleSheets, ?classId=classId, ?styleId=styleId, ?automationId=automationId)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: SprightlyButton) =
                ViewBuilders.UpdateButton(registry, prevOpt, source, target)

            ViewElement.Create(SprightlyButton, update, attribs)

