﻿namespace Sprightly.WPF.Components.Common

open Xamarin.Forms.Platform.WPF
open Sprightly.WPF.Components

type SprightlyButtonRenderer() =
    inherit ButtonRenderer()

    override this.OnElementChanged(e: ElementChangedEventArgs<Xamarin.Forms.Button>) = 
        base.OnElementChanged(e)

        match (this.Control, e.NewElement) with 
        | null, _ -> do ()
        | _ , null -> do ()
        | _ ->
            let color: System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(36uy, 255uy, 255uy, 255uy)

            this.Control.Content <- 
                RippleContainer(SprightlyButtonContent(e.NewElement.Text), 
                                color)
            this.Control.HorizontalContentAlignment <- System.Windows.HorizontalAlignment.Stretch
            this.Control.VerticalContentAlignment <- System.Windows.VerticalAlignment.Stretch

            let buttonResourceSource = System.Uri("pack://application:,,,/Sprightly.WPF.Components;component/SprightlyButton.xaml")
            let resourceDictionary = System.Windows.ResourceDictionary()
            resourceDictionary.Source <- buttonResourceSource
            this.Control.Style <- resourceDictionary.["SprightlyButton"] :?> System.Windows.Style


// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_SprightlyButtonRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.Common.SprightlyButton>, 
                               typeof<SprightlyButtonRenderer>)>]
    do ()


