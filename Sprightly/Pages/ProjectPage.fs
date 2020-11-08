namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly.Components.ProjectPage

module ProjectPage =
    type public Model = unit

    type public Msg = unit

    type public InternalCmdMsg = unit
    type public ExternalCmdMsg = unit

    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="ProjectPage"/>.
    /// </summary>
    type public CmdMsg =
        | Internal of InternalCmdMsg
        | External of ExternalCmdMsg


    /// <summary>
    /// Update the provided <paramref name="model"/> to its new state given the
    /// provided <paramref name="msg"/>.
    /// </summary>
    /// <param name="msg">The message for which the new state should be returned.</param>
    /// </param name="model">The model from which the new state is calculated.</param>
    /// <returns>
    /// The new state as a <see cref="Model"/> and a set of command messages to be 
    /// executed.
    /// </returns>
    let public update (msg: Msg) (model: Model) : Model * CmdMsg list =
        model, []


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
        let viewport = View.Viewport()

        View.StackLayout(orientation = StackOrientation.Horizontal,
                         children = [ viewport.VerticalOptions(LayoutOptions.FillAndExpand)
                                              .HorizontalOptions(LayoutOptions.FillAndExpand)
                                    ])




