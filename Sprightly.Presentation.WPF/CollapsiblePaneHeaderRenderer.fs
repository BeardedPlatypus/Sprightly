﻿namespace Sprightly.Presentation.WPF

open System.ComponentModel
open Xamarin.Forms.Platform.WPF
open Sprightly.Presentation.WPF.Components

open Sprightly.Presentation.Components.Common

/// <summary>
/// <see cref="CollapsiblePaneHeaderRenderer"/> provides the custom renderer implementation
/// to render the <see cref="CollapsiblePaneHeader"/> as a <see cref="Xamarin.Forms.Button"/>.
/// </summary>
type CollapsiblePaneHeaderRenderer() =
    inherit ButtonRenderer()

    override this.OnElementChanged(e: ElementChangedEventArgs<Xamarin.Forms.Button>) = 
        base.OnElementChanged(e)

        match (this.Control, e.NewElement) with 
        | null, _ -> do ()
        | _ , null -> do ()
        | _ ->
            let pane = (e.NewElement :?> CollapsiblePaneHeader)
            
            let headerContent = CollapsiblePaneHeaderContent()
            headerContent.SetIsOpen(pane.IsOpen)
            headerContent.HeaderText <- pane.HeaderText

            let color: System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(36uy, 255uy, 255uy, 255uy)
            let rippleContainer = RippleContainer(headerContent, color)

            this.Control.Content <- rippleContainer
            this.Control.HorizontalContentAlignment <- System.Windows.HorizontalAlignment.Stretch

            let buttonResourceSource = System.Uri("pack://application:,,,/Sprightly.Presentation.WPF.Components;component/SprightlyButton.xaml")
            let resourceDictionary = System.Windows.ResourceDictionary()
            resourceDictionary.Source <- buttonResourceSource
            this.Control.Style <- resourceDictionary.["SprightlyButton"] :?> System.Windows.Style

    override this.OnElementPropertyChanged((sender: obj), (e: PropertyChangedEventArgs)) =
        if (sender :? CollapsiblePaneHeader) then
            let contentContainer = this.Control.Content :?> RippleContainer
            let content = contentContainer.WrappedElement :?> CollapsiblePaneHeaderContent;

            let paneHeader = sender :?> CollapsiblePaneHeader
       
            if (e.PropertyName = "IsOpen") then
                content.SetIsOpen(paneHeader.IsOpen)
            else if e.PropertyName = "HeaderText"  then
                content.HeaderText <- paneHeader.HeaderText
        else 
            do ()
