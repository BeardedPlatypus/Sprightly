namespace Sprightly.Pages

open Fabulous.XamarinForms
open Xamarin.Forms


/// <summary>
/// <see cref="LoadingPage"/> defines the loading page of the Sprightly
/// application.
/// </summary>
module LoadingPage = 
    /// <summary>
    /// <see cref="Model"/> defines the model for the <see cref="LoadingPage"/>.
    /// </summary>
    type public Model = unit

    /// <summary>
    /// <see cref="view"/> transforms the <paramref name="model"/> onto
    /// its corresponding view.
    /// </summary>
    /// <param name="model">The model to display.</param>
    /// <param name="dispatch">The function to dispatch messages with.</param>
    /// <remarks>
    /// <see cref='view"/> is executed on the ui thread.
    /// </remarks>
    let public view (model: Model) dispatch = 
        View.BoxView(color = Color.Black)





