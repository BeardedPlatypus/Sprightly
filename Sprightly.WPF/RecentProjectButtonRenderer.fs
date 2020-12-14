namespace Sprightly.WPF

open Xamarin.Forms.Platform.WPF
open Sprightly.WPF.Components

open Sprightly.Presentation.Components.StartPage

type RecentProjectButtonRenderer() =
    inherit ButtonRenderer()

    override this.OnElementChanged(e: ElementChangedEventArgs<Xamarin.Forms.Button>) = 
        base.OnElementChanged(e)

        match (this.Control, e.NewElement) with 
        | null, _ -> do ()
        | _ , null -> do ()
        | _ ->
            let recentProjectButton = (e.NewElement :?> RecentProjectButton)
            let recentProject = recentProjectButton.RecentProjectValue

            let path = match recentProject.Path with | Sprightly.Common.Path.T v -> v
            let color: System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(36uy, 255uy, 255uy, 255uy)

            this.Control.Content <- 
                RippleContainer(RecentProjectContent(path, recentProject.LastOpened), color)
            this.Control.HorizontalContentAlignment <- System.Windows.HorizontalAlignment.Stretch

            let buttonResourceSource = System.Uri("pack://application:,,,/Sprightly.WPF.Components;component/SprightlyButton.xaml")
            let resourceDictionary = System.Windows.ResourceDictionary()
            resourceDictionary.Source <- buttonResourceSource
            this.Control.Style <- resourceDictionary.["SprightlyButton"] :?> System.Windows.Style
            


// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_RecentProjectButtonRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.StartPage.RecentProjectButton>, 
                               typeof<RecentProjectButtonRenderer>)>]
    do ()


