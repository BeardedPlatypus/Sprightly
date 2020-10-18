namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms
open Sprightly.Components

/// <summary>
/// <see cref="NewProjectPage"/> defines the page shown to create a new project
/// </summary>
module NewProjectPage =
    /// <summary>
    /// <see cref="Model"/> defines the model for the <see cref="NewProjectPage"/>.
    /// This consists of a project name and path to a directory.
    /// </summary>
    type public Model =
        { ProjectName : string option
          DirectoryPath : Sprightly.Domain.Path.T option
        }


    /// <summary>
    /// Initialise a new default <see cref="Model"/>.
    /// </summary>
    let public init () : Model = { ProjectName = None; DirectoryPath = None }


    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="NewProjectPage"/>.
    /// </summary>
    type public Msg = 
        | SetProjectName of string
        | SetDirectoryPath of string
        | RequestNewProject
        | RequestStartPage


    /// <summary>
    /// <see cref="ExternalCmdMsg"/> defines the external command messages for 
    /// the <see cref="NewProjectPage"/>, which need to be mapped in the application
    /// level.
    /// </summary>
    type public ExternalCmdMsg =
        | CreateNewProject
        | ReturnToStartPage


    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="NewProjectPage"/>.
    /// </summary>
    type public CmdMsg =
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
        match msg with 
        | SetProjectName newName   -> 
            { model with ProjectName = Some newName }, []
        | SetDirectoryPath newPath -> 
            { model with DirectoryPath = Some (Sprightly.Domain.Path.fromString newPath) }, []
        | RequestNewProject ->
            model, [ External CreateNewProject ]
        | RequestStartPage -> 
            model, [ External ReturnToStartPage ]


    let private navigationButtonsView dispatch = 
        let createProjectButton =
            (Common.Components.textButton "Create Project"  (fun () -> dispatch RequestNewProject))
                .HorizontalOptions(LayoutOptions.Fill)

        let returnButton = 
            (Common.Components.textButton "Back" (fun () -> dispatch RequestStartPage))
                .HorizontalOptions(LayoutOptions.Fill)

        let topButtons = 
            View.StackLayout(children = [ createProjectButton ])
                .StackLayoutOrientation(StackOrientation.Vertical)
                .VerticalOptions(LayoutOptions.Start)
        let middleButtons = 
            View.StackLayout(children = [])
                .StackLayoutOrientation(StackOrientation.Vertical)
                .VerticalOptions(LayoutOptions.CenterAndExpand)
        let bottomButtons = 
            View.StackLayout(children = [ returnButton ])
                .StackLayoutOrientation(StackOrientation.Vertical)
                .VerticalOptions(LayoutOptions.End)

        View.StackLayout(children = [ topButtons; middleButtons; bottomButtons ])
            .VerticalOptions(LayoutOptions.FillAndExpand)
            .HorizontalOptions(LayoutOptions.FillAndExpand)


    let private navigationButtonsColumnView dispatch = 
        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 2.0 ],
                  children = [ Common.Components.sprightlyIcon.Row(0)
                               (navigationButtonsView dispatch).Row(1)
                             ])
            .Margin(Thickness 20.0)
            .RowSpacing(25.0)


    let private nameEntryView (i: int) (name: string) dispatch =
        [ View.Label(text="Project name:")
              .Row(0).Column(0)
          View.Entry(text = name)
              .Row(0).Column(1)
        ]


    let private directoryEntryView (i: int) (name: string) dispatch =
        [ View.Label(text="Project directory:")
              .Row(i).Column(0)
          View.Entry(text = name)
              .Row(i).Column(1)
          View.Button(text = "...")
              .Row(i).Column(2)
              .Padding(Thickness (10.0, 0.0))
        ]


    let private newProjectDataFieldsView (model : Model) dispatch =
        let projectName = match model.ProjectName with | None -> "" | Some v -> v
        let projectDirectory = match model.DirectoryPath with | None -> "" | Some ( Sprightly.Domain.Path.T v) -> v

        View.Grid(coldefs = [ Star; Stars 6.0; Auto ],
                  rowdefs = [ Star; Star ],
                  children = nameEntryView 0 projectName dispatch @
                             directoryEntryView 1 projectDirectory dispatch)
            .VerticalOptions(LayoutOptions.Start)


    let private newProjectDataEntryView (model: Model) dispatch = 
        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 7.0 ],
                  children = [ View.Label(text = "Create new project:", 
                                          fontSize = FontSize.fromValue 28.0)
                                   .Row(0)
                               (newProjectDataFieldsView model dispatch)
                                   .Row(1)
                             ])
            .Margin(Thickness 20.0)


    /// <summary>
    /// <see cref="view"/> transforms the <paramref name="model"/> onto
    /// its corresponding view.
    /// </summary>
    /// <param name="model">The model to display.</param>
    /// <param name="dispatch">The function to dispatch messages with.</param>
    /// <remarks>
    /// <see cref='update"/> is executed on the ui thread.
    /// </remarks>
    let public view (model: Model) dispatch = 
        let dataEntryView = newProjectDataEntryView model dispatch
        let navigationButtons = navigationButtonsColumnView dispatch
     
        let divider = 
          View.BoxView(color = Color.Gray,
                       width = 2.5)
              .Padding(Thickness 20.0)
              .BoxViewCornerRadius(CornerRadius 1.25)
     
        View.Grid(rowdefs = [ Star ],
                  coldefs = [ Stars 4.0; Auto; Star ],
                  children = 
                    [ dataEntryView
                          .VerticalOptions(LayoutOptions.FillAndExpand)
                          .HorizontalOptions(LayoutOptions.FillAndExpand)
                          .Column(0)
                    ; divider
                          .VerticalOptions(LayoutOptions.FillAndExpand)
                          .Column(1)
                    ; navigationButtons
                          .VerticalOptions(LayoutOptions.FillAndExpand)
                          .Column(2);
                    ])
              .Spacing(10.0)
              .Margin(Thickness 20.0)    

