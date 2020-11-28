namespace Sprightly.Components.Common

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// The common <see cref="Components"/> module defines several components 
/// commonly used within Sprightly, like simple styled buttons, labels etc.
/// </summary>
module public Components =
    let private textColor = Color.White

    /// <summary>
    /// Common text button for the Sprightly application.
    /// </summary>
    /// <param name="text">The text of the text button.</param>
    /// <param name="command">The command to execute.</param>
    /// <returns>
    /// A styled text button with the given <paramref name="text"/>
    /// and <paramref name="command"/>.
    /// </returns>
    let public textButton (text: string) (command: unit -> unit) = 
        View.SprightlyButton(text = text.ToUpperInvariant(), 
                             command = command)
            .FontSize(FontSize.fromValue 14.0)
            .FontFamily(MaterialDesign.Fonts.RobotoCondensedBold)
            .TextColor(textColor)

    /// <summary>
    /// Common header for the Sprightly application.
    /// </summary>
    /// <param name="text">The text of the text button.</param>
    /// <returns>
    /// A styled label with the given <paramref name="text"/>.
    /// </returns>
    let public header (text: string) : ViewElement = 
        View.Label(text = text, 
                   fontSize = FontSize.fromValue 60.0,
                   textColor = textColor,
                   fontFamily = MaterialDesign.Fonts.RobotoCondensedLight)


    /// <summary>
    /// The Sprightly icon
    /// </summary>
    let public sprightlyIcon =
        View.Image(source = Image.fromPath "Assets/icon.png")
            .VerticalOptions(LayoutOptions.Center)
            .HorizontalOptions(LayoutOptions.Center)
            .Padding(Thickness 8.0)

    /// <summary>
    /// Create a new button with the specified <paramref name="icon"/> and 
    /// <paramref name="command"/>.
    /// </summary>
    /// <param name="icon">The icon to be used within the button.</param>
    /// <param name="command">The command to be executed when the button is pressed.</param>
    /// <returns>
    /// A new icon button
    /// </returns>
    let public fontAwesomeIconButton (icon: string) (command: unit -> unit) (textColor: Color)=
        View.SprightlyButton(text = icon, 
                             command = command)
            .FontSize(FontSize.fromValue 24.0)
            .FontFamily(FontAwesome.Fonts.Solid900)
            .TextColor(textColor)
            .WidthRequest(54.0)
            .HeightRequest(54.0)

    /// <summary>
    /// Create a new styled list element with the specified <paramref name="icon"/>, 
    /// <paramref name="text"/>, and <paramref name="textColor"/>.
    /// </summary>
    /// <param name="icon">The unicode icon.</param>
    /// <param name="text">The text of the list icon element.</param>
    /// <param name="command">The command to execute when it is clicked.</param>
    /// <param name="textColor">The color of the icon.</param>
    /// <returns>
    /// A new list icon element.
    /// </returns>
    let public listIconElement (icon: string) (text: string) (command: unit -> unit) (textColor: Color) =
        View.SprightlyIconButton(icon = icon, text = text, command = command, iconSize = 14.0)
            .FontSize(FontSize.fromValue 14.0)
            .FontFamily(MaterialDesign.Fonts.RobotoCondensedRegular)
            .TextColor(textColor)

    /// <summary>
    /// Create a new styled icon text button with the specified 
    /// <paramref name="icon"/>, <paramref name="text"/>, 
    /// and <paramref name="textColor"/>.
    /// </summary>
    /// <param name="icon">The unicode icon.</param>
    /// <param name="text">The text of the list icon element.</param>
    /// <param name="command">The command to execute when it is clicked.</param>
    /// <param name="textColor">The color of the icon.</param>
    /// <returns>
    /// A new icon text button.
    /// </returns>
    let public iconTextButton (icon: string) (text: string) (command: unit -> unit) (textColor: Color) =
        View.SprightlyIconButton(icon = icon, 
                                 text = text.ToUpperInvariant(), 
                                 command = command, 
                                 iconSize = 14.0, 
                                 iconOrientation = Orientation.Vertical)
            .FontSize(FontSize.fromValue 12.0)
            .FontFamily(MaterialDesign.Fonts.RobotoCondensedBold)
            .TextColor(textColor)
