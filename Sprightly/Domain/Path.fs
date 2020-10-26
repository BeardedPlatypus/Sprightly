namespace Sprightly.Domain

/// <summary>
/// A Path module that adds some convenience over the <see cref="System.IO.Path"/>
/// class.
/// </summary>
module public Path = 
    /// <summary>
    /// The base path object.
    /// </summary>
    type T = T of string

    /// <summary>
    /// Construct a new <see cref="Path.T"/> from the provided string.
    /// </summary>
    /// <param name="s">The string from which to create a <see cref="Path.T"/>.</param>
    /// <returns>
    /// A new <see cref="Path.T"/>.
    /// </returns>
    let fromString (s: string) : T = T s

    /// <summary>
    /// Combine <paramref name="parent"/> and <paramref name="child"/> into a 
    /// single <see cref="Path.T"/>.
    /// </summary>
    /// <param name="parent">The parent path.</param>
    /// <param name="child">The relative child path. </param>
    /// <returns>
    /// The combined <see cref="Path.T"/>.
    /// </returns>
    let combine (parent: T) (child: T) : T = 
        match parent, child with 
        | T parentStr, T childStr -> System.IO.Path.Combine [| parentStr; childStr|] |> T

    /// <summary>
    /// Combine <paramref name="parent"/> and <paramref name="child"/> into a 
    /// single <see cref="Path.T"/>.
    /// </summary>
    /// <param name="parent">The parent path.</param>
    /// <param name="child">The relative child path. </param>
    /// <returns>
    /// The combined <see cref="Path.T"/>.
    /// </returns>
    let (/) (parent: T) (child: T) : T = combine parent child

    /// <summary>
    /// Get the name of provided <paramref name="path"/> as a string.
    /// </summary>
    /// <param name="path">The path from which to obtain the name. </param>
    /// <returns>
    /// The name of the directory or file to which <paramref name="path"/> points.
    /// </returns>
    let name (path: T) : string = 
        match path with | T pathStr -> System.IO.Path.GetFileName pathStr

    /// <summary>
    /// Get the parent directory of the provided <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path from which to obtain the name. </param>
    /// <returns>
    /// The parent directory of the directory or file to which <paramref name="path"/> points.
    /// </returns>
    let parentDirectory (path: T) : T = 
        match path with | T pathStr -> System.IO.Path.GetDirectoryName pathStr |> fromString
       
