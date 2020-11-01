namespace Sprightly.DataAccess

open Sprightly.Domain

module SolutionFile =
    open FSharp.Data
    
    /// <summary>
    /// <see cref="T"/> defines the sprightly solution file.
    /// </summary>
    type public T = 
        JsonProvider<"./DataAccess/sprightly_project.json", 
                     EmbeddedResource="Sprightly, sprightly_project.json">

    /// <summary>
    /// Write the specified <paramref name="solutionFile"/> to the specified path.
    /// </summary>
    /// <param name="solutionFile">The solution file to write to disk.</param>
    /// <param name="path">The path to write to. </param>
    let write (solutionFile: T) (path: Path.T) : Async<unit> = 
        Json.serialize solutionFile 
        |> ( Json.writeJsonString path )
        

