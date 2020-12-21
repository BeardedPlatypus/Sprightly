namespace Sprightly.WPF

open Xamarin.Forms
open Sprightly.Presentation.WPF.Dialogs
open Sprightly.Persistence.WPF

[<assembly: Dependency(typeof<SaveFileDialogImpl>)>]
do ()

[<assembly: Dependency(typeof<OpenFileDialogImpl>)>]
do ()

[<assembly: Dependency(typeof<InspectorImpl>)>]
do ()
