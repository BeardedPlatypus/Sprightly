namespace Sprightly.DataAccess

    /// <summary>
    /// <see cref="RecentProject"/> defines a single recent project with a 
    /// path and a date when it was last opened.
    /// </summary>
    type public RecentProject = 
        { Path : Sprightly.Domain.Path.T 
          LastOpened : System.DateTime
        }

    /// <summary>
    /// <see cref="RecentProject"/> module defines the methods related
    /// to recent projects.
    /// </summary>
    module public RecentProject =
        let private recentProjectsKey = "recent_projects"

        let public loadRecentProjects () : (RecentProject list) option =
            let app = Xamarin.Forms.Application.Current

            try 
                match app.Properties.TryGetValue recentProjectsKey with
                | true, (:? string as json) -> 
                    match Json.deserialize<RecentProject list>(json) with
                    | Result.Ok recentProjects -> Some recentProjects
                    | Result.Error _   -> None
                | _ -> 
                    None
             with _ -> 
                 None

        let public saveRecentProjects (recentProjects: RecentProject list) : unit =
            let app = Xamarin.Forms.Application.Current

            try 
                let serializedRecentProjects = Json.serialize recentProjects
                app.Properties.[recentProjectsKey] <- serializedRecentProjects
                app.SavePropertiesAsync () |> ignore
             with _ -> 
                 do ()
            

        let moveProjectToTopOfRecentProjects (recentProject: RecentProject) : unit =
            loadRecentProjects () 
            |> Option.defaultValue [] 
            |> List.filter (fun x -> x <> recentProject)
            |> (fun l -> recentProject :: l)
            |> saveRecentProjects
