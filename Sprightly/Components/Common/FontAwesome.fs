namespace Sprightly.Components.Common

open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// <see cref="FontAwesome"/> describes all helper methods to handle 
//// Font Awesome icons.
/// </summary>
module public FontAwesome =
    /// <summary>
    /// <see cref="Fonts"/> describes the font families used within the Sprightly
    /// application.
    /// </summary>
    module public Fonts = 
        let public Regular400 = 
            match Device.RuntimePlatform with 
            | Device.WPF -> "fa-regular-400.ttf#Font Awesome 5 Free Regular"
            | _          -> null
        
        let public Solid900 = 
            match Device.RuntimePlatform with 
            | Device.WPF -> "fa-solid-900.ttf#Font Awesome 5 Free Solid"
            | _          -> null

    /// <summary>
    /// <see cref="Icons"/> describes the icons used within the Sprightly
    /// application.
    /// </summary>
    module Icons =
        let public image = "\uf03e"
        let public home = "\uf015"
        let public photoVideo = "\uf87c"
 

