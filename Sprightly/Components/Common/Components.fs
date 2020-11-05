namespace Sprightly.Components.Common

open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// The common <see cref="Components"/> module defines several components 
/// commonly used within Sprightly, like simple styled buttons, labels etc.
/// </summary>
module public Components =
    /// <summary>
    /// Common text button for the Sprightly application.
    /// </summary>
    /// <param name="text">The text of the text button.</param>
    /// <param name="command">The command to execute.</param>
    /// <returns>
    /// A styled text button with the given <paramref name="text"/>
    /// and <paramref name="command"/>.
    /// </returns>
    let public textButton text command = 
        View.Button(text = text, command = command)
            .FontSize(FontSize.fromValue 12.0)
            .Padding(Thickness 10.0)

    /// <summary>
    /// The Sprightly icon
    /// </summary>
    let public sprightlyIcon =
        View.Image(source = Image.fromPath "Assets/icon.png")
            .VerticalOptions(LayoutOptions.Center)
            .HorizontalOptions(LayoutOptions.Center)
            .Padding(Thickness 5.0)

