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
    let public textButton (text: string) (command: unit -> unit) (elevation: MaterialDesign.Elevation Option) = 
        let button = 
            View.SprightlyButton(text = text.ToUpperInvariant(), 
                                 command = command)
                .FontSize(FontSize.fromValue 14.0)
                .FontFamily(MaterialDesign.Fonts.RobotoCondensedBold)
                .TextColor(textColor)

        match elevation with 
        | Some elevationValue -> button |> ( MaterialDesign.withElevation elevationValue )
        | None                -> button

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
    let public fontAwesomeIconButton (icon: string) (command: unit -> unit) =
        let backgroundColor = Color.FromHex "#3498db"

        View.Button(text = icon, 
                    fontFamily = FontAwesome.Fonts.Solid900,
                    fontSize = FontSize.fromValue 28.0,
                    width = 54.0,
                    height = 54.0,
                    command = command,
                    textColor = Color.Black,
                    backgroundColor = Color.White,
                    borderColor = backgroundColor)
