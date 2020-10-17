namespace Sprightly.WPF

open Xamarin.Forms.Platform.WPF
open Sprightly.WPF.Components

type RecentProjectButtonRenderer() =
    inherit ButtonRenderer()

    override this.OnElementChanged(e: ElementChangedEventArgs<Xamarin.Forms.Button>) = 
        base.OnElementChanged(e)

        match (this.Control, e.NewElement) with 
        | null, _ -> do ()
        | _ , null -> do ()
        | _ ->
            let recentProject = (e.NewElement :?> Sprightly.RecentProjectButton).RecentProjectValue

            let path = match recentProject.Path with | Sprightly.Domain.Path.T v -> v
            this.Control.Content <- RecentProjectContent(path, recentProject.LastOpened)
            this.Control.HorizontalContentAlignment <- System.Windows.HorizontalAlignment.Stretch


// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_RecentProjectButtonRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.RecentProjectButton>, typeof<RecentProjectButtonRenderer>)>]
    do ()


