namespace Sprightly.WPF

open System.ComponentModel
open Xamarin.Forms.Platform.WPF
open Sprightly.WPF.Components

open Sprightly.Components.Common

type CollapsiblePaneHeaderRenderer() =
    inherit ButtonRenderer()

    override this.OnElementChanged(e: ElementChangedEventArgs<Xamarin.Forms.Button>) = 
        base.OnElementChanged(e)

        match (this.Control, e.NewElement) with 
        | null, _ -> do ()
        | _ , null -> do ()
        | _ ->
            let pane = (e.NewElement :?> CollapsiblePaneHeader)

            if (not (this.Control.Content :? CollapsiblePaneHeaderContent)) then
                this.Control.Content <- CollapsiblePaneHeaderContent()

            let content = this.Control.Content :?> CollapsiblePaneHeaderContent
            content.SetIsOpen(pane.IsOpen)
            content.HeaderText <- pane.HeaderText

            this.Control.HorizontalContentAlignment <- System.Windows.HorizontalAlignment.Stretch


    override this.OnElementPropertyChanged((sender: obj), (e: PropertyChangedEventArgs)) =
        if (sender :? CollapsiblePaneHeader) && (this.Control.Content :? CollapsiblePaneHeaderContent) then
            let content = this.Control.Content :?> CollapsiblePaneHeaderContent
            let paneHeader = sender :?> CollapsiblePaneHeader
       
            if (e.PropertyName = "IsOpen") then
                content.SetIsOpen(paneHeader.IsOpen)
            else if e.PropertyName = "HeaderText"  then
                content.HeaderText <- paneHeader.HeaderText
        else 
            do ()


// Dummy module to ensure this renderer is exported and picked up by Xamarin.Forms
module Dummy_CollapsiblePaneHeaderRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Components.Common.CollapsiblePaneHeader>, 
                               typeof<CollapsiblePaneHeaderRenderer>)>]
    do ()
