namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms
open Sprightly.Components

open Sprightly.Components.StartPage.FabulousRecentProjectButton

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
        { RecentProjects : Sprightly.DataAccess.RecentProject list
        }


    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="StartPage"/>.
    /// </summary>
    type public Msg  =
        | SetRecentProjects of Sprightly.DataAccess.RecentProject list
        | RequestNewProject 


    /// <summary>
    /// <see cref="InternalCmdMsg"/> defines the internal command messages for 
    /// the <see cref="StartPage"/>, which can be mapped through the 
    /// <see cref="mapInternalCmdMsg"/> method.
    /// </summary>
    type public InternalCmdMsg =
        | LoadRecentProjects
        | SaveRecentProjects of Sprightly.DataAccess.RecentProject list


    /// <summary>
    /// <see cref="ExternalCmdMsg"/> defines the external command messages for 
    /// the <see cref="StartPage"/>, which need to be mapped in the application
    /// level.
    /// </summary>
    type public ExternalCmdMsg =
        | StartNewProject


    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="StartPage"/>.
    /// </summary>
    type public CmdMsg =
        | Internal of InternalCmdMsg
        | External of ExternalCmdMsg

    let private recentProjectsKey = "recent_projects"

    let private loadRecentProjectsCmd () =
        async {
            do! Async.SwitchToThreadPool ()
            let app = Xamarin.Forms.Application.Current

            try 
                match app.Properties.TryGetValue recentProjectsKey with
                | true, (:? string as json) -> 
                    let recentProjects = Newtonsoft.Json.JsonConvert.DeserializeObject<Sprightly.DataAccess.RecentProject list>(json)
                    return Some <| SetRecentProjects recentProjects
                | _ -> 
                    return None
             with ex -> 
                 return None
        } |> Cmd.ofAsyncMsgOption


    let private saveRecentProjectsCmd (recentProjects: Sprightly.DataAccess.RecentProject list) = 
        async {
            do! Async.SwitchToThreadPool ()
            let app = Xamarin.Forms.Application.Current

            let json = Newtonsoft.Json.JsonConvert.SerializeObject(recentProjects)
            app.Properties.[recentProjectsKey] <- json
            app.SavePropertiesAsync () |> ignore

            return None
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
        | LoadRecentProjects -> 
            loadRecentProjectsCmd ()
        | SaveRecentProjects recentProjects -> 
            saveRecentProjectsCmd recentProjects


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
       
       
    let private projectButtonsView dispatch = 
        View.StackLayout(children = [ Common.Components.textButton "New Project"  (fun () -> dispatch RequestNewProject)
                                      Common.Components.textButton "Open Project" (fun () -> ())
                                    ])


    let private projectButtonsColumnView dispatch = 
        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 2.0 ],
                  children = [ Common.Components.sprightlyIcon.Row(0)
                               (projectButtonsView dispatch).Row(1)
                             ])
            .Margin(Thickness 20.0)
            .RowSpacing(25.0)



    let private recentProjectsView (recentProjects: Sprightly.DataAccess.RecentProject list ) dispatch = 
        let recentProjectsListView = 
            match recentProjects with 
            | [] ->
                View.Label(text = "No recent projects",
                           padding = Thickness (25.0, 0.0, 25.0, 0.0),
                           textColor = Color.Gray)
            | _ ->
                let recentProjectViewElements = 
                    List.map (fun rp -> View.RecentProjectButton(recentProjectValue=rp)) recentProjects
                View.ScrollView(View.StackLayout(children = recentProjectViewElements,
                                                 padding  = Thickness (0.0, 0.0, 25.0, 0.0)))

        View.Grid(coldefs = [ Star ],
                  rowdefs = [ Star; Stars 7.0 ],
                  children = [ View.Label(text = "Recent Projects:", 
                                          fontSize = FontSize.fromValue 28.0)
                                   .Row(0)
                               recentProjectsListView.Row(1)
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

