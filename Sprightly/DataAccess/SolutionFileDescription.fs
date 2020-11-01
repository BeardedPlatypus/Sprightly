namespace Sprightly.DataAccess

open Sprightly.Domain


/// <summary>
/// <see cref="ModelFileDescription"/> describes the methods and types related
/// to defining a solution file.
/// </summary>
module public SolutionFileDescription = 
    /// <summary>
    /// <see cref="T"/> defines a single solution file.
    /// </summary>
    type public T = 
        { FileName: string
          DirectoryPath: Path.T
        }

    /// <summary>
    /// Initialise a new <see cref="T"/>.
    /// </summary>
    /// <param name="fileName">The solution file name.</param>
    /// <param name="directoryPath">The path to the parent directory.</param>
    /// <returns>
    /// A new <see cref="T"/>
    /// </returns>
    let public init (fileName: string) (directoryPath: Path.T) : T =
        { FileName = fileName
          DirectoryPath = directoryPath
        }

