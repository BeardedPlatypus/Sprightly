namespace Sprightly.Components.Common

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

module CollapsiblePane =
    let view (headerText: string) (isOpen: bool) elements (collapseCommand: bool -> unit) =
        let childrenContainer = 
            View.StackLayout(orientation = StackOrientation.Vertical,
                             children = elements)
                .Padding(Thickness(4.0, 4.0, 4.0, 8.0))

        let header = View.CollapsiblePaneHeader(isOpen = isOpen, 
                                                headerText = headerText,
                                                command = fun () -> collapseCommand (not isOpen))
                         .FontFamily(MaterialDesign.Fonts.RobotoCondensedRegular)
                         .FontSize(FontSize.fromValue 16.0)
                         .TextColor(Color.White)
                         |> MaterialDesign.withElevation (MaterialDesign.Elevation 6)

        View.StackLayout(orientation = StackOrientation.Vertical, 
                         children = [ yield header  
                                      if isOpen then yield childrenContainer
                                    ])
            .Spacing(0.0)
            |> MaterialDesign.withElevation (MaterialDesign.Elevation 4)

