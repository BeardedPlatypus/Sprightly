namespace Sprightly.Presentation.WPF

open Sprightly.Presentation.Components.StartPage
open Sprightly.Presentation.WPF.Components

open Xamarin.Forms.Platform.WPF

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

            let buttonResourceSource = System.Uri("pack://application:,,,/Sprightly.Presentation.WPF.Components;component/SprightlyButton.xaml")
            let resourceDictionary = System.Windows.ResourceDictionary()
            resourceDictionary.Source <- buttonResourceSource
            this.Control.Style <- resourceDictionary.["SprightlyButton"] :?> System.Windows.Style
