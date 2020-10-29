namespace Sprightly.WPF.Components.Common.Dialogs

open Sprightly.Domain
open Sprightly.Components.Common.Dialogs

module public Utils = 
    let public configureProperty (optionalValue: 'T option) (propAssign: 'T -> unit) : unit =
        if optionalValue |> Option.isSome then propAssign optionalValue.Value

    let public configureDialogWith (dialog: Microsoft.Win32.FileDialog) 
                                   (config: FileDialogConfiguration): unit =

        configureProperty config.AddExtension 
                          (fun p -> dialog.AddExtension <- p)
        configureProperty config.CheckIfFileExists 
                          (fun p -> dialog.CheckFileExists <- p)
        configureProperty config.DereferenceLinks 
                          (fun p -> dialog.DereferenceLinks <- p)
        configureProperty config.Filter
                          (fun p -> dialog.Filter <- p)
        configureProperty config.FilterIndex
                          (fun p -> dialog.FilterIndex <- p)
        configureProperty config.InitialDirectory
                          (fun p -> dialog.InitialDirectory <- (match p with | Path.T v -> v))
        configureProperty config.RestoreDirectory
                          (fun p -> dialog.RestoreDirectory <- p)
        configureProperty config.Title
                          (fun p -> dialog.Title <- p)
        configureProperty config.ValidateNames
                          (fun p -> dialog.ValidateNames <- p)
                        
