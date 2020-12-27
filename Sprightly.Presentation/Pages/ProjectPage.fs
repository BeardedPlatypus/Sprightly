namespace Sprightly.Presentation.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly
open Sprightly.Common
open Sprightly.Presentation.Components.Common
open Sprightly.Presentation.Components.ProjectPage
open Sprightly.Presentation.Components.ProjectPage.ToolBoxes


/// <summary>
/// <see cref="ProjectPage"/> defines the project page which allows the 
/// user to load textures, define sprites, and define sprite animations.
/// </summary>
module ProjectPage =
    /// <summary>
    /// <see cref="Model"/> defines the model for the <see cref="ProjectPage"/>.
    /// </summary>
    type public Model = 
        { IsOpen : bool 
          TextureToolBox : TextureToolBox.Model
          SolutionDirectoryPath : Path.T
        }

    /// <summary>
    /// <see cref="ToolBoxMsg"/> defines all possible toolbox messages.
    /// </summary>
    type public ToolBoxMsg = 
        | SpriteToolBoxMsg of TextureToolBox.Msg

    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="ProjectPage"/>.
    /// </summary>
    type public Msg =
        | Initialise of Domain.Project
        | SetIsOpen of bool
        | ToolBoxMsg of ToolBoxMsg
        | UpdateTextureStore of (Domain.Textures.Texture.Id option * Domain.Textures.Texture.Store)

    /// <summary>
    /// <see cref="InternalCmdMsg"/> defines the internal command messages for 
    /// the <see cref="NewProject"/>, which can be mapped through the 
    /// <see cref="mapInternalCmdMsg"/> method.
    /// </summary>
    type public InternalCmdMsg =
        | InternalSpriteToolBoxCmdMsg of TextureToolBox.InternalCmdMsg


    type public AddTextureDescription = 
        { TexturePath: Path.T
          Store: Domain.Textures.Texture.Store
          SolutionDirectoryPath: Path.T
        }

    /// <summary>
    /// <see cref="ExternalCmdMsg"/> defines the external command messages for 
    /// the <see cref="NewProject"/>.
    /// </summary>
    type public ExternalCmdMsg =
        | InitialiseFromPath of Path.T
        | StartLoading
        | StopLoading
        | AddTextureToStore of AddTextureDescription

    /// <summary>
    /// <see cref="CmdMsg"/> defines the command messages for the <see cref="ProjectPage"/>.
    /// </summary>
    type public CmdMsg =
        | Internal of InternalCmdMsg
        | External of ExternalCmdMsg

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
        | InternalSpriteToolBoxCmdMsg cmdMsg ->
             TextureToolBox.mapInternalCmdMsg cmdMsg |> ( Cmd.map (ToolBoxMsg << SpriteToolBoxMsg ))

    /// <summary>
    /// Initialise a new project with the given solution path.
    /// </summary>
    let public init (solutionPath : Path.T) : Model * CmdMsg list = 
        { IsOpen = true 
          TextureToolBox = TextureToolBox.initEmpty 
          SolutionDirectoryPath = (Path.parentDirectory solutionPath)
        }, [ External StartLoading; External (InitialiseFromPath solutionPath) ]

    let private mapTextureToolBoxCmdMsg (model: Model) (cmdMsg: TextureToolBox.CmdMsg) : CmdMsg  =
        match cmdMsg with 
        | TextureToolBox.Internal internalMsg -> 
            Internal (InternalSpriteToolBoxCmdMsg internalMsg)
        | TextureToolBox.External (TextureToolBox.AddTexture path) -> 
            External (AddTextureToStore { TexturePath = path
                                          Store = model.TextureToolBox.Textures
                                          SolutionDirectoryPath = model.SolutionDirectoryPath
                                        })

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
        | SetIsOpen v -> 
            { model with IsOpen = v }, []
        | ToolBoxMsg tbMsg -> 
            match tbMsg with 
            | SpriteToolBoxMsg m ->
                let newToolboxModel, cmdMsgs = TextureToolBox.update m model.TextureToolBox
                let newModel = { model with TextureToolBox = newToolboxModel } 
                newModel, List.map (mapTextureToolBoxCmdMsg newModel) cmdMsgs
        | Initialise project -> 
            { model with TextureToolBox = TextureToolBox.initFromProject project }, [ External StopLoading]
        | UpdateTextureStore (id, store) ->
            let textureToolBox = { model.TextureToolBox with Textures = store 
                                                             ActiveTextureId = id
                                 }
            { model with TextureToolBox = textureToolBox }, []

    let private placeHolderIcons = 
        [ (FontAwesome.Icons.home, Color.White)
          (FontAwesome.Icons.image, MaterialDesign.PrimaryColors.blue)
          (FontAwesome.Icons.photoVideo, Color.White)
        ]

    let private ToolBoxNavigationRailView (model: Model) dispatch = 
        View.StackLayout(children = List.map (fun (icon, color) -> Components.fontAwesomeIconButton icon (fun () -> ()) color) placeHolderIcons,
                         orientation = StackOrientation.Vertical)
            .Spacing(0.0)

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
        let viewport = View.Viewport()

        let editorContent = 
            View.Grid(rowdefs = [ Star ],
                      coldefs = [ Stars 5.0; Star ],
                      children = [ viewport.VerticalOptions(LayoutOptions.FillAndExpand)
                                           .Column(0)
                                   (TextureToolBox.view model.TextureToolBox (dispatch << ( ToolBoxMsg << SpriteToolBoxMsg)))
                                       .VerticalOptions(LayoutOptions.FillAndExpand)
                                       .Column(1)
                                 ])
                .RowSpacing(0.0)
                .ColumnSpacing(0.0)

        View.StackLayout(orientation = StackOrientation.Horizontal,
                         children = [ editorContent.HorizontalOptions(LayoutOptions.FillAndExpand)
                                                   .VerticalOptions(LayoutOptions.FillAndExpand)
                                      (ToolBoxNavigationRailView model dispatch)
                                          .VerticalOptions(LayoutOptions.FillAndExpand)
                             ])
            .Spacing(0.0)
