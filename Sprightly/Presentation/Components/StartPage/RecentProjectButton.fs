namespace Sprightly.Presentation.Components.StartPage

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms


/// <summary>
/// The <see cref="RecentProjectButton"/> provides the recent project view 
/// element which will be substituted with a custom render to enable
/// hover behaviour.
/// </summary>
type RecentProjectButton() =
    inherit Button()

    /// <summary>
    /// The recent project value describing the path and last opened value.
    /// </summary>
    member val public RecentProjectValue : Sprightly.Persistence.RecentProject = 
        {  Path = Sprightly.Common.Path.T ""; LastOpened = System.DateTime.Today } with get, set


// Code required to add the RecentProjectButton to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousRecentProjectButton = 
    let recentProjectValueAttribKey = 
        AttributeKey<Sprightly.Persistence.RecentProject> "RecentProjectButton_RecentProjectValue"

    type Fabulous.XamarinForms.View with 
        static member inline RecentProjectButton(?recentProjectValue: Sprightly.Persistence.RecentProject,
                                                 // Inherited attributes
                                                 ?command: (unit -> unit),
                                                 ?horizontalOptions, ?verticalOptions, ?margin, ?gestureRecognizers, ?anchorX, ?anchorY, ?backgroundColor, 
                                                 ?inputTransparent, ?isEnabled, ?isVisible, ?opacity, ?rotation, ?rotationX, ?rotationY, ?scale, ?style, 
                                                 ?translationX, ?translationY, ?resources, ?styles, ?styleSheets, ?classId, ?styleId, ?automationId) =

            let attribCount = 0
            let attribCount = match recentProjectValue with Some _ -> attribCount + 1 | None -> attribCount
            
            let attribs = ViewBuilders.BuildButton(attribCount, 
                                                   ?command=command, ?horizontalOptions=horizontalOptions, ?verticalOptions=verticalOptions, ?margin=margin,
                                                   ?gestureRecognizers=gestureRecognizers, ?anchorX=anchorX, ?anchorY=anchorY, ?backgroundColor=backgroundColor, 
                                                   ?inputTransparent=inputTransparent, ?isEnabled=isEnabled, ?isVisible=isVisible, ?opacity=opacity,
                                                   ?rotation=rotation, ?rotationX=rotationX, ?rotationY=rotationY, ?scale=scale, ?style=style, ?translationX=translationX, ?translationY=translationY, 
                                                   ?resources=resources, ?styles=styles, ?styleSheets=styleSheets, ?classId=classId, ?styleId=styleId, ?automationId=automationId)

            match recentProjectValue with | None -> () | Some (v: Sprightly.Persistence.RecentProject) -> attribs.Add (recentProjectValueAttribKey, v)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: RecentProjectButton) =
                ViewBuilders.UpdateButton(registry, prevOpt, source, target)
                source.UpdatePrimitive (prevOpt, target, recentProjectValueAttribKey, (fun target v -> target.RecentProjectValue <- v))

            ViewElement.Create(RecentProjectButton, update, attribs)
