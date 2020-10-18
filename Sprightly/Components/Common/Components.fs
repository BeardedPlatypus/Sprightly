namespace Sprightly.Components.Common

open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// The common <see cref="Components"/> module defines several components 
/// commonly used within Sprightly, like simple styled buttons, labels etc.
/// </summary>
module public Components =
    let public textButton text command = 
        View.Button(text = text, command = command)
            .FontSize(FontSize.fromValue 12.0)
            .Padding(Thickness 10.0)

    let public sprightlyIcon =
        View.Image(source = Image.fromPath "Assets/icon.png")
            .VerticalOptions(LayoutOptions.Center)
            .HorizontalOptions(LayoutOptions.Center)
            .Padding(Thickness 5.0)

