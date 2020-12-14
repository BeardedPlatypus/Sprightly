namespace Sprightly.WPF.Components.Common.Dialogs

open Sprightly.Common.Path
open Sprightly.Presentation.Components.Common.Dialogs

open Xamarin.Forms

[<Sealed>]
/// <summary>
/// <see cref="SaveFileDialog"/> implements the <see cref="ISaveFileDialog"/> interface
/// to retrieve a file path.
/// </summary>
type public SaveFileDialog() =
    let mutable configuration : FileDialogConfiguration Option = None

    interface ISaveFileDialog with 
        member this.Pick () : Sprightly.Common.Path.T Option =
            let dialog = Microsoft.Win32.SaveFileDialog()

            let config = (this :> ISaveFileDialog).Configuration
            if config.IsSome then
                Utils.configureDialogWith dialog config.Value

            let isOk = dialog.ShowDialog()
            if isOk.HasValue && isOk.Value then
                dialog.FileName |> fromString |> Some
            else 
                None

        override this.Configuration with get() = configuration and 
                                         set(v) = configuration <- v


[<assembly: Dependency(typeof<SaveFileDialog>)>]
do ()
