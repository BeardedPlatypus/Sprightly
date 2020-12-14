namespace Sprightly.Presentation.Components.Common

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// <see cref="CollapsiblePane"/> module provides the methods related to 
/// viewing and updating collapsible panes.
/// </summary>
module CollapsiblePane =
    /// <summary>
    /// View the collapsible pane defined with the provided arguments.
    /// </summary>
    /// <param name="headerText">The text displays on the header of the pane </param>
    /// <param name="isOpen">Whether the pane is opened or collapsed.</param>
    /// <param name="elements">The view elements displayed in the pane.</param>
    /// <param name="collapseCommand">The command to open or collapse the pane</param>
    /// <returns>
    /// The styled collapsible pane view element.
    /// </returns>
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
                         |> MaterialDesign.withElevation (MaterialDesign.Elevation 16)

        View.StackLayout(orientation = StackOrientation.Vertical, 
                         children = [ yield header  
                                      if isOpen then yield childrenContainer
                                    ])
            .Spacing(0.0)
            |> MaterialDesign.withElevation (MaterialDesign.Elevation 4)

