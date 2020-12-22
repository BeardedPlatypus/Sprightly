namespace Sprightly.Presentation.Components.ProjectPage.ToolBoxes

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly.Common
open Sprightly.Domain.Textures
open Sprightly.Presentation.Components.Common

// TODO: Rename this to TextureToolbox
module public SpriteToolBox = 
    type public Model = 
        { Textures : Texture.Store
          ActiveTextureId : Texture.Id option

          ProjectTreeIsOpen : bool
          DetailIsOpen : bool

          SolutionDirectoryPath: Path.T
        }

    /// <summary>
    /// The type of Pane.
    /// </summary>
    type public Pane =
        | ProjectTree
        | Detail

    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="SpriteToolBox"/>.
    /// </summary>
    type public Msg = 
        | SetIsOpen of Pane * bool
        | SetActiveTextureId of Texture.Id
        | RequestOpenTexturePicker
        | RequestImportTexture of Path.T
        | AddTexture of Texture.T

    /// <summary>
    /// <see cref="InternalCmdMsg"/> defines the internal command messages for 
    /// the <see cref="NewProject"/>, which can be mapped through the 
    /// <see cref="mapInternalCmdMsg"/> method.
    /// </summary>
    type public InternalCmdMsg =
        | OpenTexturePicker
        | ImportTexture of Path.T * Path.T
    
    /// <summary>
    /// <see cref="CmdMsg"/> defines the command messages for the <see cref="SpriteToolbox"/>.
    /// </summary>
    type public CmdMsg = 
        | Internal of InternalCmdMsg

    let private openTexturePickerCmd () =
        let config = Dialogs.FileDialogConfiguration(addExtension = true,
                                                     checkIfFileExists = true,
                                                     dereferenceLinks = true,
                                                     filter = "Texture files (*.png)|*.png",
                                                     filterIndex = 1, 
                                                     multiSelect = false,
                                                     restoreDirectory = false, 
                                                     title = "Load a new texture")
        Dialogs.Cmds.openFileDialogCmd config RequestImportTexture

    // TODO: This should be moved to the application project / Sprightly
    let private importTextureCmd (solutionDirectoryPath: Path.T) 
                                 (texPath: Path.T) : Cmd<Msg> =
        async {
            do! Async.SwitchToThreadPool ()

            let inspector = DependencyService.Get<Sprightly.Domain.Textures.Inspector>()
            let metaData = inspector.ReadMetaData texPath

            if metaData.IsNone then
                return None
            else
                // TODO: Move this to a separate place
                let name = 
                    Path.name texPath
                let destinationPath = 
                    Path.combine
                        (Path.combine solutionDirectoryPath (Path.fromString "Textures"))
                        (Path.fromString name)
                System.IO.File.Copy(Path.toString texPath, Path.toString destinationPath)

                return Some <| AddTexture { Id = Texture.Id (name, uint 0)
                                            Data = { Name = Texture.Name name
                                                     Path = destinationPath
                                                     MetaData = metaData.Value
                                                   }
                                          }
        } |> Cmd.ofAsyncMsgOption

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
        | OpenTexturePicker ->
            openTexturePickerCmd ()
        | ImportTexture (solutionPath, texturePath) -> 
            importTextureCmd solutionPath texturePath

    let public update (msg: Msg) (model: Model) : Model * CmdMsg list =
        match msg with 
        | SetIsOpen (pane, newState) -> 
            match pane with 
            | ProjectTree -> { model with ProjectTreeIsOpen = newState }, []
            | Detail      -> { model with DetailIsOpen = newState }, []
        | SetActiveTextureId textureId ->
            { model with ActiveTextureId = Some textureId }, []
        | RequestOpenTexturePicker ->
            model, [ Internal OpenTexturePicker ]
        | RequestImportTexture path ->
            model, [ Internal (ImportTexture (model.SolutionDirectoryPath, path)) ]
        | AddTexture tex ->
            { model with Textures = List.sortBy (fun (t: Texture.T) -> (match t.Id with | Texture.Id (v, _) -> v)) (tex :: model.Textures) 
                         ActiveTextureId = Some tex.Id
            }, []

    let private projectTreeView (model: Model) dispatch = 
        let fClickListItem id () = dispatch ( SetActiveTextureId id )
        let toElement (texture: Texture.T) = 
            Components.listIconElement FontAwesome.Icons.textureIcon 
                                       (match texture.Data.Name with Texture.Name n -> n) 
                                       (fClickListItem texture.Id)
                                       (if model.ActiveTextureId.IsSome && model.ActiveTextureId.Value = texture.Id then MaterialDesign.PrimaryColors.blue else Color.White)

        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = List.map toElement model.Textures)
            .RowSpacing(0.0)
            .Padding(Thickness (4.0, 0.0, 4.0, 0.0))

    let private projectTreePaneView (model: Model) dispatch = 
        let editButtons = View.Grid(rowdefs = [Star],
                                    coldefs = [ Star; Star; Star; ],
                                    children = [ (Components.iconTextButton FontAwesome.Icons.plus   "Add"    (fun () -> dispatch RequestOpenTexturePicker) Color.White)
                                                     .Column(0)
                                                 (Components.iconTextButton FontAwesome.Icons.pen    "Edit"   (fun () -> ()) Color.White)
                                                     .Column(1)
                                                     .CommandCanExecute(false)
                                                 (Components.iconTextButton FontAwesome.Icons.thrash "Remove" (fun () -> ()) Color.White)
                                                     .Column(2)
                                               ])
                              .HorizontalItemSpacing(4.0)
                              .VerticalOptions(LayoutOptions.End)
        CollapsiblePane.view "Project Tree" 
                             model.ProjectTreeIsOpen 
                             [ projectTreeView model dispatch
                               editButtons
                             ]  
                             (dispatch << (fun b -> SetIsOpen (ProjectTree, b)))

    let private textureDetailsView (texture: Texture.T) =
        let rowName = View.Grid(coldefs = [ Star; Stars 2.0 ],
                                children = [ View.Label(text = "Name:",
                                                        textColor = Color.White,
                                                        fontSize = FontSize.fromValue 12.0,
                                                        fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular)
                                                 .Column(0)
                                                 .VerticalOptions(LayoutOptions.Center)
                                             View.Entry(text = (match texture.Data.Name with | Texture.Name n -> n),
                                                        textColor = Color.White,
                                                        fontSize = FontSize.fromValue 12.0,
                                                        fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular,
                                                        backgroundColor = Color.FromRgba(1.0, 1.0, 1.0, 0.04),
                                                        horizontalTextAlignment = TextAlignment.End)
                                                 .Column(1)
                                                 .BorderColor(Color.White)
                                                 .VerticalOptions(LayoutOptions.Center)])

        let rowFileName = View.Grid(coldefs = [ Star; Stars 2.0 ],
                                    children = [ View.Label(text = "Filename:",
                                                            textColor = Color.White,
                                                            fontSize = FontSize.fromValue 12.0,
                                                            fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular)
                                                     .Column(0)
                                                     .VerticalOptions(LayoutOptions.Center)
                                                 View.Entry(text = (match texture.Data.Path with | Path.T n -> n),
                                                            textColor = Color.FromRgba(1.0, 1.0, 1.0, 0.38),
                                                            fontSize = FontSize.fromValue 12.0,
                                                            fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular,
                                                            backgroundColor = Color.FromRgba(1.0, 1.0, 1.0, 0.0),
                                                            horizontalTextAlignment = TextAlignment.End)
                                                     .Column(1)
                                                     .BorderColor(Color.FromRgba(1.0, 1.0, 1.0, 0.12))
                                                     .IsEnabled(false)
                                                     .VerticalOptions(LayoutOptions.Center)])

        let width = match texture.Data.MetaData.Width with | MetaData.Pixel v -> v.ToString()
        let height = match texture.Data.MetaData.Height with | MetaData.Pixel v -> v.ToString()
        let dimensions = width + " x " + height
        let rowDimension = View.Grid(coldefs = [ Star; Stars 2.0 ],
                                     children = [ View.Label(text = "Dimensions:",
                                                             textColor = Color.White,
                                                             fontSize = FontSize.fromValue 12.0,
                                                             fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular)
                                                      .Column(0)
                                                      .VerticalOptions(LayoutOptions.Center)
                                                  View.Entry(text =  dimensions,
                                                             textColor = Color.FromRgba(1.0, 1.0, 1.0, 0.38),
                                                             fontSize = FontSize.fromValue 12.0,
                                                             fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular,
                                                             backgroundColor = Color.FromRgba(1.0, 1.0, 1.0, 0.0),
                                                             horizontalTextAlignment = TextAlignment.End)
                                                      .Column(1)
                                                      .BorderColor(Color.FromRgba(1.0, 1.0, 1.0, 0.12))
                                                      .IsEnabled(false)
                                                      .VerticalOptions(LayoutOptions.Center)])

        let diskSize = (match texture.Data.MetaData.DiskSize with | MetaData.Size v -> v.ToString()) + " KB"
        let rowSize = View.Grid(coldefs = [ Star; Stars 2.0 ],
                                children = [ View.Label(text = "Size:",
                                                        textColor = Color.White,
                                                        fontSize = FontSize.fromValue 12.0,
                                                        fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular)
                                                 .Column(0)
                                                 .VerticalOptions(LayoutOptions.Center)
                                             View.Entry(text = diskSize,
                                                        textColor = Color.FromRgba(1.0, 1.0, 1.0, 0.38),
                                                        fontSize = FontSize.fromValue 12.0,
                                                        fontFamily = MaterialDesign.Fonts.RobotoCondensedRegular,
                                                        backgroundColor = Color.FromRgba(1.0, 1.0, 1.0, 0.0),
                                                        horizontalTextAlignment = TextAlignment.End)
                                                 .Column(1)
                                                 .BorderColor(Color.FromRgba(1.0, 1.0, 1.0, 0.12))
                                                 .IsEnabled(false)
                                                 .VerticalOptions(LayoutOptions.Center)])
        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = [ rowName 
                                      rowFileName
                                      rowDimension
                                      rowSize
                                    ])

    let private textureSpecificView (model: Model) dispatch = 
        let textureFromId id = List.tryFind (fun (tex: Texture.T) -> tex.Id = id) model.Textures
        let texture = if Option.isSome model.ActiveTextureId then textureFromId model.ActiveTextureId.Value else None

        CollapsiblePane.view "Texture Details" 
                             model.DetailIsOpen
                             [ if Option.isSome texture then yield textureDetailsView texture.Value
                             ]  
                             (dispatch << (fun b -> SetIsOpen (Detail, b)))

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
        View.StackLayout(children = [ yield projectTreePaneView model dispatch
                                      if Option.isSome model.ActiveTextureId then yield textureSpecificView model dispatch
                                    ])
