namespace Sprightly.Components.ProjectPage.ToolBoxes

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly.Domain
open Sprightly.Components.Common

module public SpriteToolBox = 
    type public Model = 
        { Textures : Texture.T list
          ActiveTextureId : Texture.Id option

          ProjectTreeIsOpen : bool
          DetailIsOpen : bool
        }

    type public Pane =
        | ProjectTree
        | Detail

    type public Msg = 
        | SetIsOpen of Pane * bool
        | SetActiveTextureId of Texture.Id
    
    type public CmdMsg = unit

    let public update (msg: Msg) (model: Model) : Model * CmdMsg list =
        match msg with 
        | SetIsOpen (pane, newState) -> 
            match pane with 
            | ProjectTree -> { model with ProjectTreeIsOpen = newState }, []
            | Detail      -> { model with DetailIsOpen = newState }, []
        | SetActiveTextureId textureId ->
            { model with ActiveTextureId = Some textureId }, []

    let private projectTreeView (model: Model) dispatch = 
        let fClickListItem id () = dispatch ( SetActiveTextureId id )
        let toElement (texture: Texture.T) = 
            Components.listIconElement FontAwesome.Icons.textureIcon 
                                       (match texture.name with Texture.Name n -> n) 
                                       (fClickListItem texture.id)
                                       (if model.ActiveTextureId.IsSome && model.ActiveTextureId.Value = texture.id then MaterialDesign.PrimaryColors.blue else Color.White)

        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = List.map toElement model.Textures)
            .RowSpacing(0.0)
            .Padding(Thickness (4.0, 0.0, 4.0, 0.0))

    let private projectTreePaneView (model: Model) dispatch = 
        let editButtons = View.Grid(rowdefs = [Star],
                                    coldefs = [ Star; Star; Star; ],
                                    children = [ (Components.iconTextButton FontAwesome.Icons.plus   "Add"    (fun () -> ()) Color.White)
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

    let private textureSpecificView (model: Model) dispatch = 
        CollapsiblePane.view "Texture Details" 
                             model.DetailIsOpen
                             [ 
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
        View.StackLayout(children = [ projectTreePaneView model dispatch
                                      textureSpecificView model dispatch
                                    ])

