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


    let public view (model: Model) dispatch = 
        View.Grid(rowdefs = [ Stars 2.0; Star ],
                  coldefs = [ Star; Stars 2.0; Star],
                  children = 
                      [ View.BoxView().BackgroundColor(Color.LightCoral)
                                      .Row(0).Column(0)
                        View.Viewport().Row(0).Column(1)
                        View.BoxView().BackgroundColor(Color.LightGreen)
                                      .Row(0).Column(2)
                        View.BoxView().BackgroundColor(Color.Coral)
                                      .Row(1).Column(0)
                        View.BoxView().BackgroundColor(Color.DarkOrange)
                                      .Row(1).Column(1)
                        View.BoxView().BackgroundColor(Color.LimeGreen)
                                      .Row(1).Column(2)
                      ])
            .RowSpacing(2.0)
            .ColumnSpacing(2.0)




