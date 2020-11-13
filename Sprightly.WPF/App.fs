namespace Sprightly.WPF

open System

open Xamarin.Forms
open Xamarin.Forms.Platform.WPF

type MainWindow() = 
    inherit FormsApplicationPage()

module Main = 
    [<EntryPoint>]
    [<STAThread>]
    let main(_args) =

        let app = new Windows.Application()
        Forms.Init()

        let window = MainWindow() 
        window.Title <- "Sprightly"

        window.LoadApplication(new Sprightly.App())

        app.Run(window)
