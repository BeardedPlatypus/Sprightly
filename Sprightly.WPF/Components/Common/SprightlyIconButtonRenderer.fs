namespace Sprightly.WPF

open Xamarin.Forms.Platform.WPF
open Sprightly.WPF.Components

open Sprightly.Components.Common

type SprightlyIconButtonRenderer() =
    inherit ButtonRenderer()

    override this.OnElementChanged(e: ElementChangedEventArgs<Xamarin.Forms.Button>) = 
        base.OnElementChanged(e)

        match (this.Control, e.NewElement) with 
        | null, _ -> do ()
        | _ , null -> do ()
        | _ ->
            let element = (e.NewElement :?> SprightlyIconButton)
            
            let elementContent = 
                match element.IconOrientation with 
                | Orientation.Horizontal -> SprightlyIconButtonHorizontalContent(element.Icon, element.Text, element.IconSize) :> obj
                | Orientation.Vertical   -> SprightlyIconButtonVerticalContent(element.Icon, element.Text, element.IconSize) :> obj

            let color: System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(36uy, 255uy, 255uy, 255uy)
            let rippleContainer = RippleContainer(elementContent, color)

            this.Control.Content <- rippleContainer
            this.Control.HorizontalContentAlignment <- System.Windows.HorizontalAlignment.Stretch

            let buttonResourceSource = System.Uri("pack://application:,,,/Sprightly.WPF.Components;component/SprightlyButton.xaml")
            let resourceDictionary = System.Windows.ResourceDictionary()
            resourceDictionary.Source <- buttonResourceSource
            this.Control.Style <- resourceDictionary.["SprightlyButton"] :?> System.Windows.Style


// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_SprightlyListElementRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Components.Common.SprightlyIconButton>, 
                               typeof<SprightlyIconButtonRenderer>)>]
    do ()
