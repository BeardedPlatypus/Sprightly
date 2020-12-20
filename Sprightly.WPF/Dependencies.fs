namespace Sprightly.WPF

open Xamarin.Forms
open Sprightly.Presentation.WPF.Dialogs

[<assembly: Dependency(typeof<SaveFileDialogImpl>)>]
do ()

[<assembly: Dependency(typeof<OpenFileDialogImpl>)>]
do ()
