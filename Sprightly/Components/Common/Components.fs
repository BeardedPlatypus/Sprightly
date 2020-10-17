namespace Sprightly.Pages.Common

open Fabulous
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

