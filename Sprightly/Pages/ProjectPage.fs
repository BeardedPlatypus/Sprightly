namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly.Domain
open Sprightly.Components.Common
open Sprightly.Components.ProjectPage
open Sprightly.Components.ProjectPage.ToolBoxes

module ProjectPage =
    type public Model = 
        { IsOpen : bool 
          SpriteToolBox : SpriteToolBox.Model
        }

    type public ToolBoxMsg = 
        | SpriteToolBoxMsg of SpriteToolBox.Msg

    type public Msg =
        | SetIsOpen of bool
        | ToolBoxMsg of ToolBoxMsg

    type public InternalCmdMsg =
        | InternalSpriteToolBoxCmdMsg of SpriteToolBox.InternalCmdMsg
    type public ExternalCmdMsg = unit

    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="ProjectPage"/>.
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
             SpriteToolBox.mapInternalCmdMsg cmdMsg |> ( Cmd.map (ToolBoxMsg << SpriteToolBoxMsg ))

    let public init (solutionDirectoryPath : Path.T) : Model * CmdMsg list = 
        let metaData: Texture.MetaData = { Width = Texture.Pixel 1000
                                           Height = Texture.Pixel 1500
                                           DiskSize = Texture.Size 56.3
                                         }

        { IsOpen = true 
          SpriteToolBox = { Textures = [ { id = Texture.Id "1" 
                                           name = Texture.Name "Texture 1"
                                           path = Path.fromString "Texture1.png"
                                           metaData = metaData
                                         }
                                         { id = Texture.Id "2" 
                                           name = Texture.Name "Texture 2"
                                           path = Path.fromString "Texture2.png"
                                           metaData = metaData
                                         }
                                         { id = Texture.Id "3" 
                                           name = Texture.Name "Texture 3"
                                           path = Path.fromString "Texture3.png"
                                           metaData = metaData
                                         }
                                         { id = Texture.Id "4" 
                                           name = Texture.Name "Texture 4"
                                           path = Path.fromString "Texture4.png"
                                           metaData = metaData
                                         }
                                       ] 
                            ActiveTextureId = Some (Texture.Id "1")
                            ProjectTreeIsOpen = true
                            DetailIsOpen = true
                            SolutionDirectoryPath = solutionDirectoryPath
                          }
        }, []

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
        | ToolBoxMsg tbMsg -> 
            match tbMsg with 
            | SpriteToolBoxMsg m ->
                let newToolboxModel, cmdMsgs = SpriteToolBox.update m model.SpriteToolBox

                let fMapCmdMsg (cmdMsg: SpriteToolBox.CmdMsg) = 
                    match cmdMsg with 
                    | SpriteToolBox.Internal internalMsg -> Internal (InternalSpriteToolBoxCmdMsg internalMsg)
                 

                { model with SpriteToolBox = newToolboxModel }, List.map fMapCmdMsg cmdMsgs
        | _           -> model, []

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
                                   (SpriteToolBox.view model.SpriteToolBox (dispatch << ( ToolBoxMsg << SpriteToolBoxMsg)))
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




