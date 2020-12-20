namespace Sprightly.Application

open Sprightly.Domain

/// <summary>
/// The <see cref="Project"/> module contains all logic for interacting with
/// projects, including retrieving recent projects, saving and loading existing
/// projects and creating new projects.
/// </summary>
module public Project =
    /// <summary>
    /// The description of the LoadRecentProjects function.
    /// </summary>
    type public LoadRecentProjectsFunc = unit -> (RecentProject list) option

    /// <summary>
    /// The description of the SaveRecentProjects function.
    /// </summary>
    type public SaveRecentProjectsFunc = RecentProject list -> unit

    /// <summary>
    /// Load the recent projects.
    /// </summary>
    /// <param name="fLoadRecentProjects">
    /// Function to load the recent projects.
    /// </param>
    /// <returns>
    /// The list of <see cref="RecentProject"/> records describing the recent lists
    /// ordered by date.
    /// </returns>
    let public loadRecentProjects (fLoadRecentProjects: LoadRecentProjectsFunc) () : (RecentProject list) option =
        fLoadRecentProjects ()

    /// <summary>
    /// Update the stored recent project such that the specified 
    /// <paramref name="recentProject"/> is moved to the top of the list.
    /// </summary>
    /// <param name="fLoadRecentProjects">
    /// Function to load the recent projects.
    /// </param>
    /// <param name="fSaveRecentProjects">
    /// Function to save the recent projects.
    /// </param>
    /// <param name="recentProject">
    /// The recent project to move to the top of recent project.
    /// </param>
    /// <remarks>
    /// If <paramref name="recentProject"/> exists within the recent 
    /// projects, it is moved to the top. If it does not exist within
    /// the recent project it is added as a new element to the top of
    /// the recent projects.
    /// </remarks>
    let public moveProjectToTopOfRecentProjects (fLoadRecentProjects: LoadRecentProjectsFunc) 
                                                (fSaveRecentProjects: SaveRecentProjectsFunc)
                                                (recentProject: RecentProject) : unit =
        fLoadRecentProjects () 
        |> Option.defaultValue []
        |> List.filter (fun x -> x.Path <> recentProject.Path)
        |> (fun l -> recentProject :: l)
        |> fSaveRecentProjects
