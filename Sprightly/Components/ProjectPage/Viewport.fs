namespace Sprightly.Components.ProjectPage

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// The <see cref="Viewport"/> provides the viewport view element 
/// which we will substitute with a custom renderer in the platform 
/// specific code to render the SDL2 code.
/// </summary>
type Viewport() =
    inherit View()

// Code required to add the Viewport to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousViewport = 
    type Fabulous.XamarinForms.View with
        static member inline Viewport() =
            let attribs = ViewBuilders.BuildView(0)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: Viewport) =
                         ViewBuilders.UpdateView(registry, prevOpt, source, target)

            ViewElement.Create(Viewport, update, attribs)
