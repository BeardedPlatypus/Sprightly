namespace Sprightly.Components.Common

open Fabulous
open Plugin.FilePicker
open Plugin.FilePicker.Abstractions


/// <summary>
/// <see cref="FilePicker"/> provides the commands to open the file picker.
/// </summary>
module public FilePicker = 
  /// <summary>
  /// Open the file picker asynchronously and return the message created with 
  /// <paramref name="toMsg"/> and the resulting <see cref="FileData"/> if a 
  /// file was selected.
  /// </summary>
  /// <param name="toMsg">Function to transform the obtained file data to a msg.</param>
  /// <param name="allowedTypes">The allowed types.</param>
  let public openFilePickerAsync (toMsg: FileData -> 'msg) 
                                 (allowedTypes: string array) : Async<'msg option> =
    async { 
      let! fileData = CrossFilePicker.Current.PickFile(allowedTypes) |> Async.AwaitTask

      if fileData <> null then 
        return Some ( fileData |> toMsg )
      else 
        return None
    }

  /// <summary>
  /// Open file picker command.
  /// </summary>
  /// <param name="toMsg">Function to transform the obtained file data to a msg.</param>
  /// <param name="allowedTypes">The allowed types.</param>
  let public openFilePickerCmd (toMsg: FileData -> 'msg) 
                               (allowedTypes: string array) = 
    openFilePickerAsync toMsg allowedTypes |> Cmd.ofAsyncMsgOption
