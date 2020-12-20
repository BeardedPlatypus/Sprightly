namespace Sprightly.Presentation.WPF.Dialogs

open Sprightly.Common.Path
open Sprightly.Presentation.Components.Common.Dialogs

[<Sealed>]
/// <summary>
/// <see cref="SaveFileDialogImpl"/> implements the <see cref="SaveFileDialog"/> interface
/// to retrieve a file path.
/// </summary>
type public SaveFileDialogImpl() =
    let mutable configuration : FileDialogConfiguration Option = None

    interface SaveFileDialog with 
        member this.Pick () : Sprightly.Common.Path.T Option =
            let dialog = Microsoft.Win32.SaveFileDialog()

            let config = (this :> SaveFileDialog).Configuration
            if config.IsSome then
                Utils.configureDialogWith dialog config.Value

            let isOk = dialog.ShowDialog()
            if isOk.HasValue && isOk.Value then
                dialog.FileName |> fromString |> Some
            else 
                None

        override this.Configuration with get() = configuration and 
                                         set(v) = configuration <- v
