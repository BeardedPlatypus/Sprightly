namespace Sprightly.Presentation.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly
open Sprightly.Presentation.Components
open Sprightly.Presentation.Components.StartPage.FabulousRecentProjectButton


/// <summary>
/// <see cref="StartPage"/> defines the introduction page shown when
/// sprightly is started.
/// </summary>
module public StartPage =
    /// <summary>
    /// <see cref="Model"/> defines the model for the <see cref="StartPage"/>.
    /// This consists of a list of recent projects.
    /// </summary>
    type public Model = 
        { RecentProjects : Persistence.RecentProject list
        }

    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="StartPage"/>.
    /// </summary>
    type public Msg  =
        | SetRecentProjects of Persistence.RecentProject list
        | RequestNewProject 
        | RequestOpenProjectPicker
        | RequestOpenProject of Persistence.SolutionFile.Description

    /// <summary>
    /// <see cref="InternalCmdMsg"/> defines the internal command messages for 
    /// the <see cref="StartPage"/>, which can be mapped through the 
    /// <see cref="mapInternalCmdMsg"/> method.
    /// </summary>
    type public InternalCmdMsg =
        | LoadRecentProjects
        | SaveRecentProjects of Persistence.RecentProject list
        | OpenLoadProjectPicker

    /// <summary>
    /// <see cref="ExternalCmdMsg"/> defines the external command messages for 
    /// the <see cref="StartPage"/>, which need to be mapped in the application
    /// level.
    /// </summary>
    type public ExternalCmdMsg =
        | StartNewProject
        | OpenLoadingPage
        | OpenProject of Persistence.SolutionFile.Description

    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="StartPage"/>.
    /// </summary>
    type public CmdMsg =
        | Internal of InternalCmdMsg
        | External of ExternalCmdMsg

    let private loadRecentProjectsCmd () =
        async {
            do! Async.SwitchToThreadPool ()

            return Persistence.RecentProject.loadRecentProjects ()
                   |> Option.map SetRecentProjects
        } |> Cmd.ofAsyncMsgOption

    let private saveRecentProjectsCmd (recentProjects: Persistence.RecentProject list) = 
        async {
            do! Async.SwitchToThreadPool ()
            Persistence.RecentProject.saveRecentProjects recentProjects

            return None
        } |> Cmd.ofAsyncMsgOption

    let private openLoadProjectPickerCmd () =
        let config = Common.Dialogs.FileDialogConfiguration(addExtension = true,
                                                            checkIfFileExists = true,
                                                            dereferenceLinks = true,
                                                            filter = "Sprightly solution files (*.sprightly)|*.sprightly|All files (*.*)|*.*",
                                                            filterIndex = 1, 
                                                            multiSelect = false,
                                                            restoreDirectory = false, 
                                                            title = "Load a sprightly solution")
        Common.Dialogs.Cmds.openFileDialogCmd config ( RequestOpenProject << Persistence.SolutionFile.pathToDescription )

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
        | LoadRecentProjects -> 
            loadRecentProjectsCmd ()
        | SaveRecentProjects recentProjects -> 
            saveRecentProjectsCmd recentProjects
        | OpenLoadProjectPicker ->
            openLoadProjectPickerCmd ()

    /// <summary>
    /// Initialise a model and CmdMsg for the <see cref="StartPage"/>.
    /// </summary>
    let public init : Model * CmdMsg list = 
        { RecentProjects = [] }, [ Internal LoadRecentProjects ]

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
        | SetRecentProjects recentProjects -> 
            { model with RecentProjects = recentProjects }, []
        | RequestNewProject ->
            model, [ External StartNewProject ]
        | RequestOpenProjectPicker ->
            model, [ Internal OpenLoadProjectPicker ]
        | RequestOpenProject description -> 
            model, [ External <| OpenLoadingPage
                     External <| OpenProject description 
                   ]
       
    let private projectButtonsView dispatch = 
        let newProjectButton = 
            Common.Components.textButton "New Project"  
                                         (fun () -> dispatch RequestNewProject) 
        let openProjectButton = 
            Common.Components.textButton "Open Project" 
                                         (fun () -> dispatch RequestOpenProjectPicker) 

        View.StackLayout(children = [ newProjectButton; openProjectButton ])

    let private projectButtonsColumnView dispatch = 
        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 2.0 ],
                  children = [ Common.Components.sprightlyIcon
                                   .Row(0)
                                   .VerticalOptions(LayoutOptions.Center)
                                   .HorizontalOptions(LayoutOptions.Center)
                                   .Margin(Thickness 16.0)
                               (projectButtonsView dispatch)
                                   .Row(1)
                             ])
            .RowSpacing(24.0) 
            |> Common.MaterialDesign.withElevation (Common.MaterialDesign.Elevation 4)


    let private recentProjectsView (recentProjects: Persistence.RecentProject list ) dispatch = 
        let recentProjectsListView = 
            match recentProjects with 
            | [] ->
                View.Label(text = "No recent projects",
                           padding = Thickness (24.0, 0.0, 24.0, 0.0),
                           textColor = Color.Gray,
                           fontSize = FontSize.fromValue 14.0,
                           fontFamily = Common.MaterialDesign.Fonts.RobotoCondensedRegular)
            | _ ->
                let recentProjectButtonCmd (rp: Persistence.RecentProject) = 
                    fun () -> dispatch (RequestOpenProject <| Persistence.SolutionFile.pathToDescription rp.Path )

                let recentProjectButtonView (rp: Persistence.RecentProject) = 
                    View.RecentProjectButton(recentProjectValue = rp,
                                             command = recentProjectButtonCmd rp)
                        .With(textColor = Color.White, 
                              fontFamily = Common.MaterialDesign.Fonts.EczarRegular,
                              fontSize = FontSize.fromValue 14.0)

                let recentProjectViewElements = 
                    List.map recentProjectButtonView recentProjects
                View.ScrollView(View.StackLayout(children = recentProjectViewElements))

        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = [ (Common.Components.header "Recent Projects:")
                                          .Margin(Thickness 16.0)
                                      recentProjectsListView
                                          .VerticalOptions(LayoutOptions.FillAndExpand)
                                    ])
            |> Common.MaterialDesign.withElevation (Common.MaterialDesign.Elevation 4)

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
        let recentProjects = recentProjectsView model.RecentProjects dispatch
        let projectButtons = projectButtonsColumnView dispatch
     
        View.Grid(rowdefs = [ Star ],
                  coldefs = [ Stars 4.0; Star ],
                  children = 
                    [ recentProjects
                          .Column(0)
                    ; projectButtons
                          .Column(1);
                    ])
              .ColumnSpacing(16.0)
              .Margin(Thickness 16.0)    
            |> Common.MaterialDesign.withElevation (Common.MaterialDesign.Elevation 0)
