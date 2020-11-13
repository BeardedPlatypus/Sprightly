﻿namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms
open Sprightly.Components
open Sprightly.Domain.Path


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
          CreateNewDirectory: bool
        }


    /// <summary>
    /// Initialise a new default <see cref="Model"/>.
    /// </summary>
    let public init () : Model = 
        { ProjectName = None
          DirectoryPath = None
          CreateNewDirectory = true
        }


    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="NewProjectPage"/>.
    /// </summary>
    type public Msg = 
        | SetProjectName of string
        | SetDirectoryPath of Sprightly.Domain.Path.T
        | SetCreateNewDirectory of bool
        | RequestNewProject
        | RequestStartPage
        | RequestOpenFilePicker
        | RequestOpenNewProject of Sprightly.DataAccess.SolutionFile.Description


    /// <summary>
    /// <see cref="InternalCmdMsg"/> defines the internal command messages for 
    /// the <see cref="NewProject"/>, which can be mapped through the 
    /// <see cref="mapInternalCmdMsg"/> method.
    /// </summary>
    type public InternalCmdMsg =
        | OpenFilePicker
        | CreateSolutionFile of Sprightly.DataAccess.SolutionFile.Description


    /// <summary>
    /// <see cref="ExternalCmdMsg"/> defines the external command messages for 
    /// the <see cref="NewProjectPage"/>, which need to be mapped in the application
    /// level.
    /// </summary>
    type public ExternalCmdMsg =
        | OpenNewProject of Sprightly.DataAccess.SolutionFile.Description
        | OpenLoadingPage
        | ReturnToStartPage


    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="NewProjectPage"/>.
    /// </summary>
    type public CmdMsg =
        | Internal of InternalCmdMsg
        | External of ExternalCmdMsg


    let private openProjectFolderSelectionCmd () =
        let config = Common.Dialogs.FileDialogConfiguration(addExtension = true,
                                                            checkIfFileExists = false,
                                                            dereferenceLinks = true,
                                                            filter = "Sprightly solution files (*.sprightly)|*.sprightly|All files (*.*)|*.*",
                                                            filterIndex = 1, 
                                                            multiSelect = false,
                                                            restoreDirectory = false, 
                                                            title = "Select new sprightly solution location")
        Common.Dialogs.Cmds.openFileDialogCmd config SetDirectoryPath

   
    let private createSolutionFileCmd (solutionFileDescription: Sprightly.DataAccess.SolutionFile.Description) : Cmd<Msg> =
        async {
            do! Async.SwitchToThreadPool ()
            Sprightly.DataAccess.SolutionFile.writeEmpty (solutionFileDescription |> Sprightly.DataAccess.SolutionFile.descriptionToPath)
            return RequestOpenNewProject solutionFileDescription
        } |> Cmd.ofAsyncMsg


    /// <summary>
    /// <see cref="mapInternalCmdMsg> maps the provided <paramref name="cmd"/>
    /// to a corresponding cmd.
    /// </summary>
    /// <param name="cmd">The command message to convert to a command.</param>
    /// <returns>
    /// The command corresponding with the provided <paramref name="cmd"/>.
    /// </returns>
    let public mapInternalCmdMsg (cmd: InternalCmdMsg) =
        match cmd with 
        | OpenFilePicker -> 
            openProjectFolderSelectionCmd ()
        | CreateSolutionFile description -> 
            createSolutionFileCmd description
       
    let private getCreateNewProjectCmdMsg (model: Model) : CmdMsg list =
        match model.DirectoryPath, model.ProjectName with 
        | Some directoryPath, Some projectName -> 
            let nameWithoutExtension = 
                System.IO.Path.GetFileNameWithoutExtension projectName

            let directoryPath = 
                if model.CreateNewDirectory then
                    directoryPath / ( fromString nameWithoutExtension)
                else 
                    directoryPath

            let fileDescription = Sprightly.DataAccess.SolutionFile.description projectName  directoryPath
            [ Internal <| CreateSolutionFile fileDescription
              External <| OpenLoadingPage
            ]
        | _ ->
            []


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
            { model with ProjectName   = Some ( Sprightly.Domain.Path.name newPath )
                         DirectoryPath = Some ( Sprightly.Domain.Path.parentDirectory newPath ) 
            }, []
        | SetCreateNewDirectory newCreateDirectoryFlag ->
            { model with CreateNewDirectory = newCreateDirectoryFlag }, []
        | RequestNewProject ->
            model, getCreateNewProjectCmdMsg model
        | RequestStartPage -> 
            model, [ External ReturnToStartPage ]
        | RequestOpenFilePicker -> 
            model, [ Internal OpenFilePicker ]
        | RequestOpenNewProject description ->
            model, [ External <| OpenNewProject description ]


    let private IsValidNewSolution (model: Model): bool =
        match model.DirectoryPath, model.ProjectName with
        | Some directoryPath, Some projectName ->
            Sprightly.Domain.Path.isValid directoryPath &&
            Sprightly.Domain.Path.isRooted directoryPath && 
            projectName.Length > 0
        | _ -> 
            false


    let private navigationButtonsView (model: Model) dispatch = 
        let createProjectButton =
            (Common.Components.textButton "Create Project"  (fun () -> dispatch RequestNewProject) (Some (Common.MaterialDesign.Elevation 2)))
                .HorizontalOptions(LayoutOptions.Fill)
                .CommandCanExecute(IsValidNewSolution model)

        let returnButton = 
            (Common.Components.textButton "Back" (fun () -> dispatch RequestStartPage) None)
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


    let private navigationButtonsColumnView (model: Model) dispatch = 
        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 2.0 ],
                  children = [ Common.Components.sprightlyIcon.Row(0)
                               (navigationButtonsView model dispatch).Row(1)
                             ])
            .Margin(Thickness 20.0)
            .RowSpacing(25.0)


    let private entryBoxColumnDef = [ Stars 21.0; Star ]


    let private nameEntryView (name: string) dispatch =
        let fTextChanged (args: TextChangedEventArgs) = 
            dispatch (SetProjectName args.NewTextValue)

        let label = View.Label(text="Project name:")
        let entry = View.Grid(rowdefs = [ Star ],
                              coldefs = entryBoxColumnDef,
                              children = [ View.Entry(text = name, textChanged=fTextChanged)
                                               .Column(0)])

        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = [ label; entry ])
            .Spacing(12.0)


    let private directoryEntryView (directoryPath: string) (isChecked: bool) dispatch =
        let fTextChanged (args: TextChangedEventArgs) = 
            dispatch (SetDirectoryPath (Sprightly.Domain.Path.fromString args.NewTextValue))

        let label = View.Label(text="Project directory:")
        let entry = View.Grid(rowdefs = [ Star ],
                              coldefs = entryBoxColumnDef,
                              children = [ View.Entry(text = directoryPath, textChanged=fTextChanged)
                                               .Column(0)
                                           View.Button(text = "...", command = (fun () -> dispatch RequestOpenFilePicker))
                                               .Padding(Thickness (10.0, 0.0))
                                               .Column(1)])

        let fCheckBoxChanged (args: CheckedChangedEventArgs) = 
             dispatch (SetCreateNewDirectory args.Value)
        let checkbox = View.StackLayout(orientation = StackOrientation.Horizontal,
                                        children = [ View.CheckBox(isChecked = isChecked, 
                                                                   checkedChanged = fCheckBoxChanged) 
                                                     View.Label(text="Create the new project in a new directory.")])
        
        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = [ label; entry; checkbox])
            .Spacing(12.0)


    let private newProjectDataFieldsView (model : Model) dispatch =
        let projectName = match model.ProjectName with | None -> "" | Some v -> v
        let directoryPath = match model.DirectoryPath with | None -> "" | Some ( Sprightly.Domain.Path.T v) -> v

        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = [ nameEntryView projectName dispatch
                                      directoryEntryView directoryPath model.CreateNewDirectory dispatch 
                                    ])
            .VerticalOptions(LayoutOptions.Start)
            .Spacing(24.0)


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
        let navigationButtons = navigationButtonsColumnView model dispatch
     
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

