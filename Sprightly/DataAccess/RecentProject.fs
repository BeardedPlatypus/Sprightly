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

        /// <summary>
        /// Load the recent projects from the app properties.
        /// </summary>
        /// <returns>
        /// The list of <see cref="RecentProject"/> records if they can
        /// be retrieved from the app properties.
        /// </returns>
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

        /// <summary>
        /// Save the specified <paramref name="recentProjects"/> to the app properties.
        /// </summary>
        /// <param name="recentProjects">The recent projects to save.</param>
        let public saveRecentProjects (recentProjects: RecentProject list) : unit =
            let app = Xamarin.Forms.Application.Current

            try 
                let serializedRecentProjects = Json.serialize recentProjects
                app.Properties.[recentProjectsKey] <- serializedRecentProjects
                app.SavePropertiesAsync () |> Async.AwaitTask |> ignore
             with _ -> 
                 do ()
            
        /// <summary>
        /// Update the stored recent project such that the specified 
        /// <paramref name="recentProject"/> is moved to the top of the list.
        /// </summary>
        /// <param name="recentProject">
        /// The recent project to move to the top of recent project.
        /// </param>
        /// <remarks>
        /// If <paramref name="recentProject"/> exists within the recent 
        /// projects, it is moved to the top. If it does not exist within
        /// the recent project it is added as a new element to the top of
        /// the recent projects.
        /// </remarks>
        let public moveProjectToTopOfRecentProjects (recentProject: RecentProject) : unit =
            loadRecentProjects () 
            |> Option.defaultValue [] 
            |> List.filter (fun x -> x.Path <> recentProject.Path)
            |> (fun l -> recentProject :: l)
            |> saveRecentProjects
