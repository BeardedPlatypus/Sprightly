namespace Sprightly.Components.Common.Dialogs

open Sprightly

/// <summary>
/// <see cref="IOpenFileDialog"/> defines the interface with which a 
/// open file dialog can be created.
/// </summary>
type public IOpenFileDialog = 
    /// <summary>
    /// Pick a file through an open file dialog and return the result as a path.
    /// </summary>
    /// <returns>
    /// Upon success the path will be returned; otherwise None.
    /// </returns>
    abstract Pick : unit -> Domain.Path.T Option

    /// <summary>
    /// Configure this <see cref="ISaveFileDialog"/> with the provided 
    /// <see cref="FileBrowserDialogConfiguration"/>.
    /// </summary>
    abstract Configuration : FileDialogConfiguration option with get, set
