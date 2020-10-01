namespace Sprightly.WPF

open Xamarin.Forms.Platform.WPF

open Sprightly
open Sprightly.WPF.Components

type SpriteViewRenderer() = 
    inherit ViewRenderer<SpriteView, SpriteViewControl>()

    override this.OnElementChanged(e: ElementChangedEventArgs<SpriteView>) = 
        base.OnElementChanged(e) 

        match this.Control with
        | null ->
            this.SetNativeControl(SpriteViewControl())
        | _ -> 
            do ()


// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_SpriteViewRenderer = 
    [<assembly: ExportRenderer(typeof<Sprightly.SpriteView>, typeof<SpriteViewControl>)>]
    do ()
