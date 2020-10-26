namespace IFilePicker

open Sprightly

/// <summary>
/// <see cref="IFilePicker"/> defines the interface with which a file
/// picker interface can be created.
/// </summary>
type public IFilePicker = 
    /// <summary>
    /// Pick a file through a file dialog and return the result as a path.
    /// </summary>
    /// <returns>
    /// Upon success the path will be returned; otherwise None.
    /// </returns>
    abstract Pick : string -> Domain.Path.T Option
