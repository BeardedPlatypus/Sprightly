namespace Sprightly.WPF

open Xamarin.Forms.Platform.WPF

open Sprightly.WPF.Components

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

// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_ViewPortRenderer = 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.ProjectPage.Viewport>, 
                               typeof<ViewportRenderer>)>]
    do ()

