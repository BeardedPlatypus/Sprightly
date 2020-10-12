namespace Sprightly.Pages

open Fabulous.XamarinForms
open Xamarin.Forms

/// <summary>
/// <see cref="ProjectPage"/> defines the introduct project page shown when
/// sprightly is started.
/// </summary>
module public StartPage =
    /// <summary>
    /// <see cref="RecentProject"/> defines a single recent project with a 
    /// path and a date when it was last opened.
    /// </summary>
    type public RecentProject = 
        { Path : Sprightly.Domain.Path.T 
          LastOpened : System.DateTime
        }

    /// <summary>
    /// <see cref="Model"/> defines the model for the <see cref="StartPage"/>.
    /// This consists of a list of recent projects.
    /// </summary>
    type public Model = 
        { RecentProjects : RecentProject list
        }

    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="StartPage"/>.
    /// </summary>
    type public Msg  =
        | SetRecentProjects of RecentProject list
        | RequestNewProject 

    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="StartPage"/>.
    /// </summary>
    type public CmdMsg =
        | LoadRecentProjects
        | StartNewProject

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
            model, [ StartNewProject ]
        
    let private projectButtonsView dispatch = 
        View.StackLayout(children = [ Common.Components.textButton "New Project"  (fun () -> dispatch RequestNewProject)
                                      Common.Components.textButton "Open Project" (fun () -> ())
                                    ])

    let private projectButtonsColumnView dispatch = 
        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 2.0 ],
                  children = [ View.Image(source = Image.fromPath "Assets/icon.png")
                                   .VerticalOptions(LayoutOptions.Center)
                                   .HorizontalOptions(LayoutOptions.Center)
                                   .Padding(Thickness 5.0)
                                   .Row(0)
                               (projectButtonsView dispatch)
                                   .Row(1)
                             ])
            .Margin(Thickness 20.0)
            .RowSpacing(35.0)
    
    let private recentProjectsView (recentProjects: RecentProject list ) dispatch = 
        View.Grid(coldefs = [ Star],
                  rowdefs = [ Star; Stars 5.0 ],
                  children = [ 
                      View.BoxView()
                          .BoxViewCornerRadius(CornerRadius 7.5)
                          .Row(0)
                      View.BoxView()
                          .BoxViewCornerRadius(CornerRadius 7.5)
                          .Row(1)])
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
        let recentProjects = recentProjectsView model.RecentProjects dispatch
        let projectButtons = projectButtonsColumnView dispatch
     
        let divider = 
          View.BoxView(color = Color.Gray,
                       width = 2.5)
              .Padding(Thickness 20.0)
              .BoxViewCornerRadius(CornerRadius 1.25)
     
        View.Grid(rowdefs = [ Star ],
                  coldefs = [ Stars 4.0; Auto; Star ],
                  children = 
                    [ recentProjects
                          .VerticalOptions(LayoutOptions.FillAndExpand)
                          .HorizontalOptions(LayoutOptions.FillAndExpand)
                          .Column(0)
                    ; divider
                          .VerticalOptions(LayoutOptions.FillAndExpand)
                          .Column(1)
                    ; projectButtons
                          .VerticalOptions(LayoutOptions.FillAndExpand)
                          .Column(2);
                    ])
              .Spacing(10.0)
              .Margin(Thickness 20.0)    

