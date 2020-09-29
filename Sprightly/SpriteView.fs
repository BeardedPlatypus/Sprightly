namespace Sprightly

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// The <see cref="SpriteView"/> provides the view element which we will 
/// substitute with a custom renderer in the platform specific code to render
/// the SDL2 code.
/// </summary>
type SpriteView() = 
    inherit View()

// Code required to add the SpriteView to the Fabulous view function
// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/views-extending.html
[<AutoOpen>]
module FabulousSpriteView =
    type Fabulous.XamarinForms.View with
        static member inline SpriteView() =
            let attribs = ViewBuilders.BuildView(0)

            let update registry (prevOpt: ViewElement voption) (source: ViewElement) (target: SpriteView) =
                ViewBuilders.UpdateView(registry, prevOpt, source, target)                

            ViewElement.Create(SpriteView, update, attribs)

