namespace Sprightly.Common

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

    let toString (path: T) : string = match path with | T s -> s

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
        System.IO.Path.Combine [| (parent |> toString) ; (child |> toString) |] |> T

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
        System.IO.Path.GetFileName (toString path)

    /// <summary>
    /// Get the parent directory of the provided <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path from which to obtain the name. </param>
    /// <returns>
    /// The parent directory of the directory or file to which <paramref name="path"/> points.
    /// </returns>
    let parentDirectory (path: T) : T = 
        ( System.IO.Path.GetDirectoryName ( path |> toString )) |> fromString

    /// <summary>
    /// Get whether <paramref name="path"/> is valid or not.
    /// </summary>
    /// <param name="path">The path to verify.</param>
    /// <returns>
    /// True if <paramref name="path"/> is valid; false otherwise.
    /// </returns>
    let isValid (path: T) : bool =
        let pathStr = toString path

        try 
            System.IO.Path.GetFullPath pathStr |> ignore
            true
        with 
        | _ -> false

    /// <summary>
    /// Get whether <paramref name="path"/> is root or not.
    /// </summary>
    /// <param name="path">The path to verify.</param>
    /// <returns>
    /// True if <paramref name="path"/> is rooted; false otherwise.
    /// </returns>
    let isRooted (path: T) : bool = 
        System.IO.Path.IsPathRooted (path |> toString)

    /// <summary>
    /// <see cref="OpenMode"/> defines whether a file should be opened to read
    /// or to write.
    /// </summary>
    type OpenMode = 
        | Read
        | Write

    /// <summary>
    /// Open the file at the specified <paramref name="path"/> with the given 
    /// <paramref name="mode"/>.
    /// </summary>
    let openFile (path: T) (mode: OpenMode) : System.IO.FileStream = 
        match mode with 
        | Read  -> System.IO.File.OpenRead (path |> toString)
        | Write -> System.IO.File.OpenWrite (path |> toString)



