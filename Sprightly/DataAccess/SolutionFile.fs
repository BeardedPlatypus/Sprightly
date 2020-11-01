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
    /// <see cref="T"/> defines a single solution file.
    /// </summary>
    type public Description = 
        { FileName: string
          DirectoryPath: Path.T
        }

    /// <summary>
    /// Initialise a new <see cref="Description"/>.
    /// </summary>
    /// <param name="fileName">The solution file name.</param>
    /// <param name="directoryPath">The path to the parent directory.</param>
    /// <returns>
    /// A new <see cref="Description"/>
    /// </returns>
    let public init (fileName: string) (directoryPath: Path.T) : Description =
        { FileName = fileName
          DirectoryPath = directoryPath
        }

    /// <summary>
    /// Write the specified <paramref name="solutionFile"/> to the specified path.
    /// </summary>
    /// <param name="solutionFile">The solution file to write to disk.</param>
    /// <param name="path">The path to write to. </param>
    let write (solutionFile: T) (path: Path.T) : Async<unit> = 
        Json.serialize solutionFile 
        |> ( Json.writeJsonString path )
        

