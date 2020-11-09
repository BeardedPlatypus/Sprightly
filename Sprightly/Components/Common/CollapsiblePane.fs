namespace Sprightly.Components.Common

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

module CollapsiblePane =
    let view (headerText: string) (isOpen: bool) elements (collapseCommand: bool -> unit) =
        let childrenContainer = 
            View.StackLayout(orientation = StackOrientation.Vertical,
                             children = elements)
                .Padding(Thickness(5.0))

        let header = View.CollapsiblePaneHeader(isOpen = isOpen, 
                                                headerText = headerText,
                                                command = fun () -> collapseCommand (not isOpen))

        View.StackLayout(orientation = StackOrientation.Vertical, 
                         children = [ yield header  
                                      if isOpen then yield childrenContainer
                                    ])
            .Spacing(0.0)

