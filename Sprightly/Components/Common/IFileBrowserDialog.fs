namespace IFileBrowserDialog

open Fabulous
open Xamarin.Forms

open Sprightly


/// <summary>
/// <see cref="FileBrowserDialogConfiguration"/> defines the optional configuration
/// options of a <see cref="IFileBrowserDialog"/>.
/// </summary>
type public FileBrowserDialogConfiguration (?addExtension: bool,
                                            ?checkIfFileExists: bool,
                                            ?dereferenceLinks: bool,
                                            ?filter: string,
                                            ?filterIndex: int,
                                            ?initialDirectory: Domain.Path.T,
                                            ?multiSelect: bool,
                                            ?readOnlyChecked: bool,
                                            ?restoreDirectory: bool,
                                            ?showReadOnly: bool,
                                            ?supportMultiDottedExtensions: bool,
                                            ?title: string,
                                            ?validateNames: bool) =
    /// <summary>
    /// Value indicating whether the dialog box automatically adds an 
    /// extension to a file name if the user omits the extension.
    /// </summary>
    member val AddExtension: bool option = addExtension

    /// <summary>
    /// Value indicating whether the dialog box displays a warning if the user 
    /// specifies a file name that does not exist.
    /// </summary>
    member val CheckIfFileExists: bool option = checkIfFileExists

    /// <summary>
    /// Value indicating whether the dialog box returns the location of the 
    /// file referenced by the shortcut or whether it returns the location of 
    /// the shortcut (.lnk).
    /// </summary>
    member val DereferenceLinks: bool option = dereferenceLinks

    /// <summary>
    /// The current file name filter string, which determines the choices that 
    /// appear in the "Save as file type" or "Files of type" box in the dialog box.
    /// </summary>
    member val Filter: string option = filter

    /// <summary>
    ///  The index of the filter currently selected in the file dialog box.
    /// </summary>
    member val FilterIndex: int option = filterIndex

    /// <summary>
    /// The initial directory displayed by the file dialog box.
    /// </summary>
    member val InitialDirectory: Domain.Path.T option = initialDirectory

    /// <summary>
    /// Value indicating whether the dialog box allows multiple files to be selected.
    /// </summary>
    member val MultiSelect: bool option = multiSelect

    /// <summary>
    /// Value indicating whether the read-only check box is selected.
    /// </summary>
    member val ReadOnlyChecked: bool option = readOnlyChecked

    /// <summary>
    /// value indicating whether the dialog box restores the directory to the 
    /// previously selected directory before closing.
    /// </summary>
    member val restoreDirectory: bool option = restoreDirectory

    /// <summary>
    /// Gets or sets a value indicating whether the dialog box contains a read-only check box.
    /// </summary>
    member val ShowReadOnly: bool option = showReadOnly

    /// <summary>
    /// Value indicating whether the dialog box supports displaying and saving 
    /// files that have multiple file name extensions.
    /// </summary>
    member val SupportMultiDottedExtensions = supportMultiDottedExtensions

    /// <summary>
    /// The file dialog box title.
    /// </summary>
    member val Title: string option = title

    /// <summary>
    /// Value indicating whether the dialog box accepts only valid Win32 file names.
    /// </summary>
    member val ValidateNames: bool option = validateNames


/// <summary>
/// <see cref="IFilePicker"/> defines the interface with which a file
/// picker interface can be created.
/// </summary>
type public IFileBrowserDialog = 
    /// <summary>
    /// Pick a file through a file dialog and return the result as a path.
    /// </summary>
    /// <returns>
    /// Upon success the path will be returned; otherwise None.
    /// </returns>
    abstract Pick : unit -> Domain.Path.T Option

    /// <summary>
    /// Configure this <see cref="IFilePicker"/> with the provided 
    /// <see cref="FileBrowserDialogConfiguration"/>.
    /// </summary>
    abstract ConfigureWith : FileBrowserDialogConfiguration -> unit
    

/// <summary>
/// <see cref="Cmds"/> exposes the commonly used commands related to the 
/// <see cref="IFileBrowserDialog"/>.
/// </summary>
module public Cmds =
    /// <summary>
    /// Construct a new async command to open a file browser dialog.
    /// </summary>
    /// <param name="config">The configuration for the file browser dialog.</param>
    /// <param name="toMsg"/>Function to generate the relevant message object returned if a path is obtained successfully.</param>
    /// <returns>
    /// An async command to open a file browser dialog.
    /// </returns>
    let public openFileBrowserDialogCmd (config: FileBrowserDialogConfiguration) (toMsg: Domain.Path.T -> 'Msg) =
        async {
            do! Async.SwitchToThreadPool ()

            let picker = DependencyService.Get<IFileBrowserDialog>()
            picker.ConfigureWith config

            return Option.map toMsg <| picker.Pick ()
        } |> Cmd.ofAsyncMsgOption
