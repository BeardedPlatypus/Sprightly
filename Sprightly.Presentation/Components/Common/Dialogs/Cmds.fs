namespace Sprightly.Presentation.Components.Common.Dialogs

open Sprightly
open Fabulous
open Xamarin.Forms

/// <summary>
/// <see cref="Cmds"/> exposes the commonly used commands related to the 
/// <see cref="IFileBrowserDialog"/>.
/// </summary>
module public Cmds =
    /// <summary>
    /// Construct a new async command to open a open file dialog.
    /// </summary>
    /// <param name="config">The configuration for the file dialog.</param>
    /// <param name="toMsg"/>Function to generate the relevant message object returned if a path is obtained successfully.</param>
    /// <returns>
    /// An async command to open an open file dialog.
    /// </returns>
    let public openFileDialogCmd (config: FileDialogConfiguration) (toMsg: Common.Path.T -> 'Msg) =
        async {
            do! Async.SwitchToThreadPool ()

            let picker = DependencyService.Get<OpenFileDialog>()
            picker.Configuration <- Some config

            return Option.map toMsg <| picker.Pick ()
        } |> Cmd.ofAsyncMsgOption

    /// <summary>
    /// Construct a new async command to open a save file dialog.
    /// </summary>
    /// <param name="config">The configuration for the file dialog.</param>
    /// <param name="toMsg"/>Function to generate the relevant message object returned if a path is obtained successfully.</param>
    /// <returns>
    /// An async command to open a save file dialog.
    /// </returns>
    let public saveFileDialogCmd (config: FileDialogConfiguration) (toMsg: Common.Path.T -> 'Msg) =
        async {
            do! Async.SwitchToThreadPool ()

            let picker = DependencyService.Get<SaveFileDialog>()
            picker.Configuration <- Some config

            return Option.map toMsg <| picker.Pick ()
        } |> Cmd.ofAsyncMsgOption
