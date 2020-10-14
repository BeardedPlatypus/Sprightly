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


// Code required to add the RecentProjectButton to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousRecentProjectButton = 
    type Fabulous.XamarinForms.View with 
        static member inline RecentProjectButton() = 
            let attribs = ViewBuilders.BuildView(0)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: RecentProjectButton) =
                ViewBuilders.UpdateView(registry, prevOpt, source, target)

            ViewElement.Create(RecentProjectButton, update, attribs)
