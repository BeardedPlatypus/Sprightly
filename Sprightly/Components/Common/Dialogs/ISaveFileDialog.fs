namespace Sprightly.Components.Common.Dialogs

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
    abstract Pick : unit -> Domain.Path.T Option

    /// <summary>
    /// Configure this <see cref="ISaveFileDialog"/> with the provided 
    /// <see cref="FileBrowserDialogConfiguration"/>.
    /// </summary>
    abstract ConfigureWith : FileDialogConfiguration -> unit

