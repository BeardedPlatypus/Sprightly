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
        | OpenProject of Common.Path.T

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

    /// <summary>
    /// Create a new model and corresponding <see cref="CmdMsg"/> list.
    /// </summary>
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
        | PresentationMsg presentationMsg -> 
            updatePresentation presentationMsg model
        | StartNewProject ->
            { model with PageModel = Pages.NewProjectPage.init () |> NewProjectPageModel}, []
        | OpenProject solutionFilePath ->
            let initModel, cmdMsgs = Pages.ProjectPage.init solutionFilePath

            { model with PageModel = ProjectPageModel initModel; IsLoading = false }, 
            [ MoveProjectToTopOfRecentProjects { Path = solutionFilePath; LastOpened = System.DateTime.Now } ] @
            List.map ProjectPageCmdMsg cmdMsgs
        | ReturnToStartPage ->
            init ()
        | OpenLoadingPage ->
            { model with IsLoading = true }, []
        | CloseLoadingPage ->
            { model with IsLoading = false }, []

    /// <summary>
    /// <see cref="view"/> transforms the <paramref name="model"/> into
    /// its corresponding view.
    /// </summary>
    /// <param name="model">The model to display.</param>
    /// <param name="dispatch">The function to dispatch messages with.</param>
    /// <remarks>
    /// <see cref='view"/> is executed on the ui thread.
    /// </remarks>
    let public view (model: Model) dispatch =
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

    let private createSolutionFileCmd (path: Common.Path.T) : Cmd<Msg> =
        async {
            do! Async.SwitchToThreadPool ()
            
            let fWriteEmptySolution (p: Common.Path.T) = 
                let solutionFileDescription : Sprightly.Persistence.SolutionFile.Description = 
                    { FileName = Common.Path.name p
                      DirectoryPath = Common.Path.parentDirectory p
                    }

                Persistence.SolutionFile.writeEmpty (solutionFileDescription |> Sprightly.Persistence.SolutionFile.descriptionToPath)
                Persistence.Texture.createTextureFolder solutionFileDescription.DirectoryPath

            Application.Project.createNewProject fWriteEmptySolution path

            return PresentationMsg ( NewProjectPageMsg ( Pages.NewProjectPage.RequestOpenNewProject path ))
        } |> Cmd.ofAsyncMsg

    let private mapExternalNewProjectPageCmdMsg (cmdMsg: Pages.NewProjectPage.ExternalCmdMsg) =
        match cmdMsg with 
        | Pages.NewProjectPage.ReturnToStartPage ->
            Cmd.ofMsg ReturnToStartPage
        | Pages.NewProjectPage.OpenNewProject description ->
            Cmd.ofMsg ( OpenProject description )
        | Pages.NewProjectPage.OpenLoadingPage -> 
            Cmd.ofMsg OpenLoadingPage
        | Pages.NewProjectPage.CreateSolutionFile path ->
            createSolutionFileCmd path
            
    let private mapNewProjectPageCmdMsg (cmdMsg: Pages.NewProjectPage.CmdMsg) =
        match cmdMsg with 
        | Pages.NewProjectPage.Internal internalCmdMsg -> 
            Pages.NewProjectPage.mapInternalCmdMsg internalCmdMsg 
            |> ( Cmd.map ( NewProjectPageMsg >> PresentationMsg ))
        | Pages.NewProjectPage.External externalCmdMsg -> 
            mapExternalNewProjectPageCmdMsg externalCmdMsg

    let private initialiseFromPathCmd (path: Common.Path.T) : Cmd<Msg> =
        async {
            do! Async.SwitchToThreadPool ()

            let textureFolder = Persistence.Texture.textureFolder (Common.Path.parentDirectory path)
            let toTextureDescription (t: Persistence.Texture.DataAccessRecord) : (Application.Texture.TextureDescription) =
                { Name = t.Name 
                  Id = Domain.Textures.Texture.Id (t.idString, t.idIndex) 
                  Path = Common.Path.combine textureFolder (Common.Path.fromString t.FileName)
                }

            let fRetrieveTexturePathsFromSolution (p: Common.Path.T) : (Application.Texture.TextureDescription list) option =
                Persistence.SolutionFile.read p
                |> Option.map (fun dao -> dao.Textures |> List.map toTextureDescription)


            let inspector = DependencyService.Get<Domain.Textures.Inspector>()
            let fRetrieveTextureData (texDescr: Application.Texture.TextureDescription) : Domain.Textures.Texture.T option =
                Persistence.Texture.loadDomainTexture inspector texDescr.Name texDescr.Id texDescr.Path

            let textureFactory = DependencyService.Get<Infrastructure.TextureFactory>()
            let fLoadTexture (tex: Domain.Textures.Texture.T) : unit =
                textureFactory.RequestTextureLoad tex.Id tex.Data.Path
            
            return Application.Project.loadProject fRetrieveTexturePathsFromSolution
                                                   fRetrieveTextureData
                                                   fLoadTexture
                                                   path
                   |> Option.map (PresentationMsg << ProjectPageMsg << Pages.ProjectPage.Msg.Initialise)
        } |> Cmd.ofAsyncMsgOption

    let private addTextureToStoreCmd (description: Pages.ProjectPage.AddTextureDescription) : Cmd<Msg> =
        async {
            do! Async.SwitchToThreadPool ()

            let solutionDirectoryPath = Common.Path.parentDirectory description.SolutionPath
            let fCopyTextureIntoSolution : Application.Texture.CopyTextureIntoSolutionFunc = 
                Persistence.Texture.copyTextureIntoTextureFolder solutionDirectoryPath

            let inspector = DependencyService.Get<Domain.Textures.Inspector>()
            let fRetrieveTextureMetaData = inspector.ReadMetaData

            let textureFactory = DependencyService.Get<Infrastructure.TextureFactory>()
            let fLoadTexture (tex: Domain.Textures.Texture.T) : unit =
                textureFactory.RequestTextureLoad tex.Id tex.Data.Path

            let fSaveStore (store: Domain.Textures.Texture.Store) : unit =
                let textureDARs = store |> List.map Persistence.Texture.toDataAccessRecord
                Persistence.SolutionFile.updateTexturesOnDisk textureDARs description.SolutionPath

            let res = Application.Texture.addNewTextureToStore fCopyTextureIntoSolution
                                                               fRetrieveTextureMetaData
                                                               fLoadTexture
                                                               fSaveStore
                                                               description.TexturePath
                                                               description.Store

            return Option.map (fun (newId, newStore) -> Pages.ProjectPage.UpdateTextureStore ((Some newId), newStore)) res
                |> Option.map (PresentationMsg << ProjectPageMsg)
        } |> Cmd.ofAsyncMsgOption
        
    let private mapExternalProjectPageCmdMsg (cmdMsg: Pages.ProjectPage.ExternalCmdMsg) =
        match cmdMsg with 
        | Pages.ProjectPage.StartLoading -> 
            Cmd.ofMsg OpenLoadingPage
        | Pages.ProjectPage.StopLoading  -> 
            Cmd.ofMsg CloseLoadingPage
        | Pages.ProjectPage.InitialiseFromPath path ->
            initialiseFromPathCmd path
        | Pages.ProjectPage.AddTextureToStore description ->
            addTextureToStoreCmd description

    let private mapProjectPageCmdMsg (cmdMsg: Pages.ProjectPage.CmdMsg) =
        match cmdMsg with 
        | Pages.ProjectPage.Internal internalCmdMsg -> 
            Pages.ProjectPage.mapInternalCmdMsg internalCmdMsg 
            |> ( Cmd.map ( ProjectPageMsg >> PresentationMsg ))
        | Pages.ProjectPage.External externalCmdMsg -> 
            mapExternalProjectPageCmdMsg externalCmdMsg

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


