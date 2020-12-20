namespace Sprightly

open Sprightly.Presentation

open System.Diagnostics
open Fabulous
open Fabulous.XamarinForms
open Fabulous.XamarinForms.LiveUpdate
open Xamarin.Forms


/// <summary>
/// <see cref="App"/> is the main application module of our Sprightly application.
/// </summary>
/// <remarks
/// This code is based on the template provided by Fabulous for Xamarin.Forms.
/// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/index.html#getting-started
/// </remarks>
module App = 
    /// <summary>
    /// The model of the currently active page.
    /// </summary>
    type public PageModel = 
        | StartPageModel      of Pages.StartPage.Model
        | ProjectPageModel    of Pages.ProjectPage.Model
        | NewProjectPageModel of Pages.NewProjectPage.Model

    /// <summary>
    /// The model of the Sprightly application.
    /// </summary>
    type public Model = 
        { PageModel : PageModel
          IsLoading : bool
        }

    type public PresentationMsg = 
        | StartPageMsg      of Pages.StartPage.Msg
        | ProjectPageMsg    of Pages.ProjectPage.Msg
        | NewProjectPageMsg of Pages.NewProjectPage.Msg

    /// <summary>
    /// The msg of the Sprightly App consisting of page msgs
    /// and global sprightly msgs.
    /// </summary>
    type public Msg = 
        | PresentationMsg of PresentationMsg

        | StartNewProject
        | ReturnToStartPage
        | OpenProject of Persistence.SolutionFile.Description

        | OpenLoadingPage
        | CloseLoadingPage

    /// <summary>
    /// The cmd msg of the Sprightly App consisting of page cmd msgs
    /// and global sprightly cmd mssgs.
    /// </summary>
    type public CmdMsg = 
        | StartPageCmdMsg      of Pages.StartPage.CmdMsg
        | ProjectPageCmdMsg    of Pages.ProjectPage.CmdMsg
        | NewProjectPageCmdMsg of Pages.NewProjectPage.CmdMsg
        
        | MoveProjectToTopOfRecentProjects of Domain.RecentProject

    let private toCmdMsg (mapFunc: 'a -> CmdMsg) (cmdMsgList: 'a list) : CmdMsg list =
        List.map mapFunc cmdMsgList

    let public init () =
        let model, cmdMsgs = Pages.StartPage.init

        { PageModel = model |> StartPageModel; IsLoading = false }, cmdMsgs |> ( toCmdMsg StartPageCmdMsg )

    let private updatePresentation (msg: PresentationMsg) (model: Model) : Model * CmdMsg list =
        match model.PageModel, msg with 
        | (StartPageModel startPageModel, StartPageMsg startPageMsg) ->
            let updatedModel, cmdMsgs = Pages.StartPage.update startPageMsg startPageModel
            
            { model with PageModel = updatedModel |> StartPageModel }, 
            cmdMsgs |> ( toCmdMsg StartPageCmdMsg )
        | (ProjectPageModel projectPageModel, ProjectPageMsg projectPageMsg) ->
            let updatedModel, cmdMsgs = Pages.ProjectPage.update projectPageMsg projectPageModel
            
            { model with PageModel = updatedModel |> ProjectPageModel }, 
            cmdMsgs |> ( toCmdMsg ProjectPageCmdMsg )
        | (NewProjectPageModel newProjectPageModel, NewProjectPageMsg newProjectPageMsg) ->
            let updatedModel, cmdMsgs = Pages.NewProjectPage.update newProjectPageMsg newProjectPageModel
            
            { model with PageModel = updatedModel |> NewProjectPageModel }, 
            cmdMsgs |> ( toCmdMsg NewProjectPageCmdMsg )
        | _ ->
            model, []

    let update (msg: Msg) (model: Model) : Model * CmdMsg list =
        match msg with 
        | PresentationMsg presentationMsg -> 
            updatePresentation presentationMsg model
        | StartNewProject ->
            { model with PageModel = Pages.NewProjectPage.init () |> NewProjectPageModel}, []
        | OpenProject description ->
            // TODO: Move this code into a separate function.
            let solutionFilePath = 
                Common.Path.combine description.DirectoryPath (Common.Path.fromString description.FileName)

            match Persistence.SolutionFile.read solutionFilePath with
            | None -> init ()
            | Some solutionFile -> 
                let inspector = DependencyService.Get<Sprightly.Domain.ITextureInspector>()
                let textures = List.map ( Persistence.Texture.loadDomainTexture inspector description.DirectoryPath ) solutionFile.Textures
                               |> List.choose id
                
                
                let initModel, cmdMsgs = Pages.ProjectPage.init description.DirectoryPath textures
                { model with PageModel = ProjectPageModel initModel
                             IsLoading = false }, 
                [ MoveProjectToTopOfRecentProjects { Path = description |> Persistence.SolutionFile.descriptionToPath; LastOpened = System.DateTime.Now } ] @
                  List.map ProjectPageCmdMsg cmdMsgs
        | ReturnToStartPage ->
            init ()
        | OpenLoadingPage ->
            { model with IsLoading = true }, []
        | CloseLoadingPage ->
            { model with IsLoading = false }, []

    let view (model: Model) dispatch =
        let pageDispatch = dispatch << PresentationMsg
        let pageContent = 
            match model.PageModel with 
            | StartPageModel startPageModel ->
                Pages.StartPage.view startPageModel ( pageDispatch << StartPageMsg )
            | ProjectPageModel projectPageModel ->
                Pages.ProjectPage.view projectPageModel ( pageDispatch << ProjectPageMsg )
            | NewProjectPageModel newProjectPageModel ->
                Pages.NewProjectPage.view newProjectPageModel ( pageDispatch << NewProjectPageMsg )

        let content = 
            if model.IsLoading then 
                View.Grid(children = [ pageContent.IsEnabled(false) 
                                                  .Row(0).Column(0)
                                       View.BoxView(color = Color.FromRgba(0.0, 0.0, 0.0, 0.5))
                                                  .Row(0).Column(0)
                                     ])
            else 
                pageContent

        View.ContentPage(content = content,
                         hasNavigationBar = false,
                         backgroundColor = Components.Common.MaterialDesign.ElevationColors.dp00)

    let private loadRecentProjectsCmd () =
        async {
            do! Async.SwitchToThreadPool ()

            return Application.Project.loadRecentProjects 
                Persistence.RecentProject.loadRecentProjects ()
                   |> Option.map ( Pages.StartPage.SetRecentProjects 
                                   >> StartPageMsg 
                                   >> PresentationMsg )
        } |> Cmd.ofAsyncMsgOption
    
    let private mapExternalStartPageCmdMsg (cmdMsg: Pages.StartPage.ExternalCmdMsg) =
        match cmdMsg with 
        | Pages.StartPage.StartNewProject -> 
            Cmd.ofMsg StartNewProject
        | Pages.StartPage.OpenProject description ->
            Cmd.ofMsg ( OpenProject description )
        | Pages.StartPage.OpenLoadingPage ->
            Cmd.ofMsg OpenLoadingPage
        | Pages.StartPage.LoadRecentProjects ->
            loadRecentProjectsCmd ()

    let private mapStartPageCmdMsg (cmdMsg: Pages.StartPage.CmdMsg) = 
        match cmdMsg with 
        | Pages.StartPage.Internal internalCmdMsg -> 
            Pages.StartPage.mapInternalCmdMsg internalCmdMsg 
            |> ( Cmd.map ( StartPageMsg >> PresentationMsg ))
        | Pages.StartPage.External externalCmdMsg ->
            mapExternalStartPageCmdMsg externalCmdMsg

    let private mapExternalNewProjectPageCmdMsg (cmdMsg: Pages.NewProjectPage.ExternalCmdMsg) =
        match cmdMsg with 
        | Pages.NewProjectPage.ReturnToStartPage ->
            Cmd.ofMsg ReturnToStartPage
        | Pages.NewProjectPage.OpenNewProject description ->
            Cmd.ofMsg ( OpenProject description )
        | Pages.NewProjectPage.OpenLoadingPage -> 
            Cmd.ofMsg OpenLoadingPage

    let private mapNewProjectPageCmdMsg (cmdMsg: Pages.NewProjectPage.CmdMsg) =
        match cmdMsg with 
        | Pages.NewProjectPage.Internal internalCmdMsg -> 
            Pages.NewProjectPage.mapInternalCmdMsg internalCmdMsg 
            |> ( Cmd.map ( NewProjectPageMsg >> PresentationMsg ))
        | Pages.NewProjectPage.External externalCmdMsg -> 
            mapExternalNewProjectPageCmdMsg externalCmdMsg

    let private mapProjectPageCmdMsg (cmdMsg: Pages.ProjectPage.CmdMsg) =
        match cmdMsg with 
        | Pages.ProjectPage.Internal internalCmdMsg -> 
            Pages.ProjectPage.mapInternalCmdMsg internalCmdMsg 
            |> ( Cmd.map ( ProjectPageMsg >> PresentationMsg ))
        | Pages.ProjectPage.External externalCmdMsg -> 
            Cmd.none

    let private moveProjectToTopOfRecentProjectsCmd (recentProject: Domain.RecentProject) =
        async {
            do! Async.SwitchToThreadPool ()

            Application.Project.moveProjectToTopOfRecentProjects 
                Persistence.RecentProject.loadRecentProjects
                Persistence.RecentProject.saveRecentProjects
                recentProject
            return None
        } |> Cmd.ofAsyncMsgOption

    let private mapCmdMsg (cmdMsg: CmdMsg) =
        match cmdMsg with 
        | StartPageCmdMsg startPageCmdMsg -> 
            mapStartPageCmdMsg startPageCmdMsg
        | ProjectPageCmdMsg projectCmdMsg -> 
            mapProjectPageCmdMsg projectCmdMsg
        | NewProjectPageCmdMsg newProjectCmdMsg -> 
            mapNewProjectPageCmdMsg newProjectCmdMsg
        | MoveProjectToTopOfRecentProjects recentProject ->
            moveProjectToTopOfRecentProjectsCmd recentProject

    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgramWithCmdMsg init update view mapCmdMsg


type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> XamarinFormsProgram.run app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/tools.html#live-update for further  instructions.
    //
    //do runner.EnableLiveUpdate()
#endif    

    // Uncomment this code to save the application state to app.Properties using Newtonsoft.Json
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/models.html#saving-application-state for further  instructions.

#if APPSAVE
    let modelId = "model"
    override __.OnSleep() = 

        let json = Newtonsoft.Json.JsonConvert.SerializeObject(runner.CurrentModel)
        Console.WriteLine("OnSleep: saving model into app.Properties, json = {0}", json)

        app.Properties.[modelId] <- json

    override __.OnResume() = 
        Console.WriteLine "OnResume: checking for model in app.Properties"
        try 
            match app.Properties.TryGetValue modelId with
            | true, (:? string as json) -> 

                Console.WriteLine("OnResume: restoring model from app.Properties, json = {0}", json)
                let model = Newtonsoft.Json.JsonConvert.DeserializeObject<App.Model>(json)

                Console.WriteLine("OnResume: restoring model from app.Properties, model = {0}", (sprintf "%0A" model))
                runner.SetCurrentModel (model, Cmd.none)

            | _ -> ()
        with ex -> 
            App.program.onError("Error while restoring model found in app.Properties", ex)

    override this.OnStart() = 
        Console.WriteLine "OnStart: using same logic as OnResume()"
        this.OnResume()
#endif


