namespace Sprightly.Presentation.WPF.Dialogs

open Sprightly.Common.Path
open Sprightly.Presentation.Components.Common.Dialogs

[<Sealed>]
/// <summary>
/// <see cref="OpenFileDialogImpl"/> implements the <see cref="OpenFileDialog"/> interface
/// to retrieve a file path through an open file dialog.
/// </summary>
type public OpenFileDialogImpl() =
    let mutable configuration : FileDialogConfiguration Option = None

    interface OpenFileDialog with 
        member this.Pick () : Sprightly.Common.Path.T Option =
            let dialog = Microsoft.Win32.OpenFileDialog()

            let config = (this :> OpenFileDialog).Configuration

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
