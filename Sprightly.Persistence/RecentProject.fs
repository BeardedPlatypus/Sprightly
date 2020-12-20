namespace Sprightly.Persistence

open Sprightly.Domain

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
