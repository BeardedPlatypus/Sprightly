﻿namespace Sprightly.DataAccess

open Sprightly.Domain
open Sprightly.Domain.Path

///<summary>
/// <see cref="SolutionFile"/> defines the types and functions related
/// to accessing a solution file on disk.
/// </summary>
module SolutionFile =
    /// <summary>
    /// <see cref="currentFileVersion"> defines the current file version.
    /// </summary>
    let private currentFileVersion : Version.T =
        { Major = 0
          Minor = 1
          Patch = 0
        }

    /// <summary>
    /// <see cref="DataAccessRecord"/> describes the Data Access version of 
    /// the solution file.
    /// </summary>
    type public DataAccessRecord = 
        { FileVersion: string 
        }

    /// <summary>
    /// Write the specified <paramref name="solutionFile"/> to the specified path.
    /// </summary>
    /// <param name="solutionFile">The solution file to write to disk.</param>
    /// <param name="path">The path to write to. </param>
    let write (solutionFile: DataAccessRecord ) (path: Path.T) : unit = 
        Json.serialize solutionFile 
        |> ( Json.writeJsonString path )

    /// <summary>
    /// Create an empty document.
    /// </summary>
    // TODO: refactor this.
    let private emptyRecord : DataAccessRecord = 
        { FileVersion = ( currentFileVersion |> Version.toString )
        }

    let public writeEmpty (path: Path.T) : unit = 
        write emptyRecord path

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
    let public description (fileName: string) (directoryPath: Path.T) : Description =
        { FileName = fileName
          DirectoryPath = directoryPath
        }

    /// <summary>
    /// Transform the provided <paramref name="description"/> to a 
    /// <see cref="Path.T"/>.
    /// </summary>
    /// <param name="description">The description to transform.</param>
    /// <returns>
    /// The path corresponding with the provided <paramref name="description"/>.
    /// </returns>
    let public descriptionToPath (description: Description): Path.T =
        description.DirectoryPath / (description.FileName |> Path.fromString)



