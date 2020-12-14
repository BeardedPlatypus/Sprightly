namespace Sprightly.Presentation.Components.Common

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
        let public Regular400 = "fa-regular"
        let public Solid900 = "fa-solid"

    /// <summary>
    /// <see cref="Icons"/> describes the icons used within the Sprightly
    /// application.
    /// </summary>
    module Icons =
        let public image = "\uf03e"
        let public home = "\uf015"
        let public photoVideo = "\uf87c"
        let public plus = "\uf067"
        let public pen = "\uf304"
        let public thrash = "\uf2ed"
        let public textureIcon = "\uf1c5"
 

module Dummy_FontAwesome= 
    [<assembly: ExportFont("fa-regular-400.ttf", Alias = "fa-regular")>]
    do()

    [<assembly: ExportFont("fa-solid-900.ttf", Alias = "fa-solid")>]
    do()


