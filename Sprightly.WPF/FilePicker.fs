namespace FilePicker

open Sprightly.Domain.Path
open Microsoft.Win32

open Xamarin.Forms

[<Sealed>]
/// <summary>
/// <see cref="FilePicker"/> implements the <see cref="IFilePicker"/> interface
/// to retrieve a file path.
/// </summary>
type public FilePicker() =
    interface IFilePicker.IFilePicker with 
        member this.Pick (fileFilter: string) : Sprightly.Domain.Path.T Option =
            let dialog = SaveFileDialog()
            dialog.Filter <- fileFilter 
            dialog.FilterIndex <- 2
            dialog.RestoreDirectory <- true

            let isOk = dialog.ShowDialog()
            if isOk.HasValue && isOk.Value then
                dialog.FileName |> fromString |> Some
            else 
                None


[<assembly: Dependency(typeof<FilePicker>)>]
do ()
