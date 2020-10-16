namespace Sprightly.Pages.StartPageComponents

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

    member val public RecentProjectValue : Sprightly.DataAccess.RecentProject Option = None with get, set


// Code required to add the RecentProjectButton to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousRecentProjectButton = 
    let recentProjectValueAttribKey = 
        AttributeKey<Sprightly.DataAccess.RecentProject Option> "RecentProjectButton_RecentProjectValue"

    type Fabulous.XamarinForms.View with 
        static member inline RecentProjectButton(recentProjectValue: Sprightly.DataAccess.RecentProject Option ) = 
            let attribs = ViewBuilders.BuildButton(1)

            attribs.Add (recentProjectValueAttribKey, recentProjectValue)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: RecentProjectButton) =
                ViewBuilders.UpdateView(registry, prevOpt, source, target)
                source.UpdatePrimitive (prevOpt, target, recentProjectValueAttribKey, (fun target v -> target.RecentProjectValue <- v))

            ViewElement.Create(RecentProjectButton, update, attribs)
