namespace Sprightly.Presentation.Components.Common.Dialogs

open Sprightly

/// <summary>
/// <see cref="OpenFileDialog"/> defines the interface with which a 
/// open file dialog can be created.
/// </summary>
type public OpenFileDialog = 
    /// <summary>
    /// Pick a file through an open file dialog and return the result as a path.
    /// </summary>
    /// <returns>
    /// Upon success the path will be returned; otherwise None.
    /// </returns>
    abstract Pick : unit -> Common.Path.T Option

    /// <summary>
    /// Configure this <see cref="OpenFileDialog"/> with the provided 
    /// <see cref="FileBrowserDialogConfiguration"/>.
    /// </summary>
    abstract Configuration : FileDialogConfiguration option with get, set
