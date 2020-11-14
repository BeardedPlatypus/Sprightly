namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly.Components.Common
open Sprightly.Components.ProjectPage

module ProjectPage =
    type public Model = 
        { IsOpen : bool 
        }

    type public Msg =
        | SetIsOpen of bool

    type public InternalCmdMsg = unit
    type public ExternalCmdMsg = unit

    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="ProjectPage"/>.
    /// </summary>
    type public CmdMsg =
        | Internal of InternalCmdMsg
        | External of ExternalCmdMsg

    let public init: Model * CmdMsg list = 
        { IsOpen = true }, []

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
        match msg with 
        | SetIsOpen v -> { model with IsOpen = v }, []
        | _           -> model, []

    let private placeHolderIcons = 
        [ FontAwesome.Icons.home
          FontAwesome.Icons.image 
          FontAwesome.Icons.photoVideo 
        ]

    let private ToolBoxSelectionButtonsView (model: Model) dispatch = 
        View.StackLayout(children = List.map (fun x -> Components.fontAwesomeIconButton x (fun () -> ()) Color.White) placeHolderIcons,
                         orientation = StackOrientation.Vertical)
            .Spacing(0.0)

    let private ToolboxView (model: Model) dispatch =
        let toolboxItem = CollapsiblePane.view "Some Pane" model.IsOpen [ View.Button(text = "option A"); 
                                                                          View.Button(text = "option B")
                                                                          View.Button(text = "option C") 
                                                                        ]  (dispatch << SetIsOpen)
        View.StackLayout(children = [ toolboxItem ],
                         orientation = StackOrientation.Vertical)
        

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

        let editorContent = 
            View.Grid(rowdefs = [ Star ],
                      coldefs = [ Stars 5.0; Star ],
                      children = [ viewport.VerticalOptions(LayoutOptions.FillAndExpand)
                                           .Column(0)
                                   (ToolboxView model dispatch).VerticalOptions(LayoutOptions.FillAndExpand)
                                                               .Column(1)
                                 ])
                .RowSpacing(0.0)
                .ColumnSpacing(0.0)

        View.StackLayout(orientation = StackOrientation.Horizontal,
                         children = [ editorContent.HorizontalOptions(LayoutOptions.FillAndExpand)
                                                   .VerticalOptions(LayoutOptions.FillAndExpand)
                                      (ToolBoxSelectionButtonsView model dispatch)
                                          .VerticalOptions(LayoutOptions.FillAndExpand)
                             ])
            .Spacing(0.0)




