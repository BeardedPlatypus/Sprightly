namespace Sprightly.Components.ProjectPage.ToolBoxes

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

open Sprightly.Domain
open Sprightly.Components.Common

module public SpriteToolBox = 
    type public Model = 
        { Textures : Texture.T list
          ActiveTextureId : Texture.Id

          ProjectTreeIsOpen : bool
        }

    type public Pane =
        | ProjectTree

    type public Msg = 
        | SetIsOpen of Pane * bool
    
    type public CmdMsg = unit

    let public update (msg: Msg) (model: Model) : Model * CmdMsg list =
        match msg with 
        | SetIsOpen (pane, newState) -> 
            match pane with 
            | ProjectTree -> { model with ProjectTreeIsOpen = newState }, []

    let private projectTreeView (model: Model) dispatch = 
        View.StackLayout(orientation = StackOrientation.Vertical,
                         children = List.map (fun (x: Texture.T) -> Components.textButton (match x.name with Texture.Name n -> n) (fun () -> ()))
                                             model.Textures
                        )

    let private projectTreePaneView (model: Model) dispatch = 
        let editButtons = View.Grid(rowdefs = [Star],
                                    coldefs = [ Star; Star; Star; ],
                                    children = [ (Components.fontAwesomeIconButton FontAwesome.Icons.plus   (fun () -> ()) Color.White).Column(0)
                                                 (Components.fontAwesomeIconButton FontAwesome.Icons.pen    (fun () -> ()) Color.White).Column(1)
                                                 (Components.fontAwesomeIconButton FontAwesome.Icons.thrash (fun () -> ()) Color.White).Column(2)
                                               ])
                              .HorizontalItemSpacing(4.0)
        CollapsiblePane.view "Project Tree" 
                             model.ProjectTreeIsOpen 
                             [ projectTreeView model dispatch
                               editButtons
                             ]  
                             (dispatch << (fun x -> SetIsOpen (ProjectTree, x)))

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
        View.Grid(rowdefs = [ Stars 2.0; Stars 3.0 ],
                  coldefs = [ Star ],
                  children = [ (projectTreePaneView model dispatch).Column(0).Row(0)
                             ]
                 )

