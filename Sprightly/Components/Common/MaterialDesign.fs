namespace Sprightly.Components.Common

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// <see cref="MaterialDesign"/> contains the values and methods related to
/// achieving a Material Design look within the Sprightly application.
/// </summary>
module public MaterialDesign = 
    /// <summary>
    /// <see cref="Elevation"/> describes the elevation of an element.
    /// </summary>
    type public Elevation = Elevation of int

    /// <summary>
    /// <see cref="Fonts"/> provides the values and methods related to
    /// the fonts.
    /// </summary>
    module public Fonts = 
        let public EczarRegular = "eczar"
        let public RobotoCondensedLight = "roboto-condensed-light"
        let public RobotoCondensedRegular = "roboto-condensed-regular"
        let public RobotoCondensedBold = "roboto-condensed-bold"

    /// <summary>
    /// <see cref="ElevationColors"/> provides the values and methods related to
    /// the colors of the elavation.
    /// </summary>
    module public ElevationColors =
        let public dp00 = Color.FromHex("#33333d")
        let public dp01 = Color.FromHex("#373742")
        let public dp02 = Color.FromHex("#383844")
        let public dp03 = Color.FromHex("#393944")
        let public dp04 = Color.FromHex("#3a3a45")
        let public dp06 = Color.FromHex("#3b3b47")
        let public dp08 = Color.FromHex("#3c3c48")
        let public dp12 = Color.FromHex("#3d3d49")
        let public dp16 = Color.FromHex("#3e3e4a")
        let public dp24 = Color.FromHex("#3f3f4b")

        /// <summary>
        /// Convert the provided <paramref name="elevation"/> to its 
        /// corresponding color.
        /// </summary>
        /// <param name="elevation">The elevation.</param>
        /// <returns>
        /// The color corresponding with the <paramref name="elevation"/>.
        /// </returns>
        let public fromElevation (elevation: Elevation) : Color =
            match elevation with 
            | Elevation v when v <= 0            -> dp00
            | Elevation 1                        -> dp01
            | Elevation 2                        -> dp02
            | Elevation 3                        -> dp03
            | Elevation v when v >=  4 && v <  6 -> dp04
            | Elevation v when v >=  6 && v <  8 -> dp06
            | Elevation v when v >=  8 && v < 12 -> dp08
            | Elevation v when v >= 12 && v < 16 -> dp12
            | Elevation v when v >= 16 && v < 24 -> dp16
            | _                                  -> dp24


    /// <summary>
    /// Style the provided <paramref name="viewElement"/> with the given 
    /// <paramref name="elevation"/>.
    /// </summary>
    /// <param name="elevation">The elevation.</param>
    /// <param name="viewElement">The view element to style.</param>
    /// <returns>
    /// The styled <paramref name="viewElement"/>.
    /// </returns>
    let public withElevation (elevation: Elevation) (viewElement: ViewElement) : ViewElement =
        let elevationColor = ElevationColors.fromElevation elevation
        viewElement.With(backgroundColor = elevationColor,
                         borderColor     = elevationColor)


// Dummy module to load the MaterialDesign fonts.
module Dummy_MaterialDesign= 
    [<assembly: ExportFont("Eczar-Regular.ttf", Alias = "eczar")>]
    do()

    [<assembly: ExportFont("RobotoCondensed-Bold.ttf", Alias = "roboto-condensed-bold")>]
    do()
    [<assembly: ExportFont("RobotoCondensed-Regular.ttf", Alias = "roboto-condensed-regular")>]
    do()
    [<assembly: ExportFont("RobotoCondensed-Light.ttf", Alias = "roboto-condensed-light")>]
    do()
