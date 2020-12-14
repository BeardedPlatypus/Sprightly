namespace Sprightly.Presentation.Components.Common.Dialogs

open Sprightly

/// <summary>
/// <see cref="ISaveFileDialog"/> defines the interface with which a 
/// save file dialog can be created.
/// </summary>
type public ISaveFileDialog = 
    /// <summary>
    /// Pick a file through an save file dialog and return the result as a path.
    /// </summary>
    /// <returns>
    /// Upon success the path will be returned; otherwise None.
    /// </returns>
    abstract Pick : unit -> Common.Path.T Option

    /// <summary>
    /// Configure this <see cref="ISaveFileDialog"/> with the provided 
    /// <see cref="FileBrowserDialogConfiguration"/>.
    /// </summary>
    abstract Configuration : FileDialogConfiguration option with get, set

