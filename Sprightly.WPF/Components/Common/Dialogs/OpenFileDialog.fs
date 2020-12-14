namespace Sprightly.WPF.Components.Common.Dialogs

open Sprightly.Presentation.Components.Common.Dialogs
open Sprightly.Common.Path

open Xamarin.Forms

[<Sealed>]
/// <summary>
/// <see cref="OpenFileDialog"/> implements the <see cref="IOpenFileDialog"/> interface
/// to retrieve a file path through an open file dialog.
/// </summary>
type public OpenFileDialog() =
    let mutable configuration : FileDialogConfiguration Option = None

    interface IOpenFileDialog with 
        member this.Pick () : Sprightly.Common.Path.T Option =
            let dialog = Microsoft.Win32.OpenFileDialog()

            let config = (this :> IOpenFileDialog).Configuration

            if config.IsSome then
                Utils.configureDialogWith dialog config.Value

                Utils.configureProperty config.Value.MultiSelect
                                        (fun p -> dialog.Multiselect <- p)
                Utils.configureProperty config.Value.ReadOnlyChecked
                                        (fun p -> dialog.ReadOnlyChecked <- p)
                Utils.configureProperty config.Value.ReadOnlyChecked
                                        (fun p -> dialog.ShowReadOnly <- p)

            let isOk = dialog.ShowDialog()
            if isOk.HasValue && isOk.Value then
                dialog.FileName |> fromString |> Some
            else 
                None

        override this.Configuration with get() = configuration and 
                                         set(v) = configuration <- v


[<assembly: Dependency(typeof<OpenFileDialog>)>]
do ()
