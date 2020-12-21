namespace Sprightly.Presentation.WPF

open Xamarin.Forms.Platform.WPF

open Sprightly.Common.KoboldLayer.WPF
open Sprightly.Presentation.WPF.Components

/// <summary>
/// <see cref="ViewportRenderer"/> provides the custom renderer implementation
/// to render the <see cref="Viewport"/> as a <see cref="ViewportControl"/>.
/// </summary>
type ViewportRenderer() =
    inherit ViewRenderer<Sprightly.Presentation.Components.ProjectPage.Viewport, ViewportControl>()

    override this.OnElementChanged(e: ElementChangedEventArgs<Sprightly.Presentation.Components.ProjectPage.Viewport>) = 
        base.OnElementChanged(e)

        match this.Control with
        | null -> 
            let viewport = ViewportFactory.Create ()
            this.SetNativeControl(ViewportControl(viewport))
        | _ ->
            do ()
