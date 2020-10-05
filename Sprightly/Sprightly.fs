namespace Sprightly

open System.Diagnostics
open Fabulous
open Fabulous.XamarinForms
open Fabulous.XamarinForms.LiveUpdate
open Xamarin.Forms

/// <summary>
/// <see cref="App"/> is the main application module of our Sprightly application.
/// </summary>
/// <remarks
/// This code is based on the template provided by Fabulous for Xamarin.Forms.
/// See: https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/index.html#getting-started
/// </remarks>
module App = 
    type Model = unit
    type Msg = unit

    let initModel = ()

    let init () = initModel, Cmd.none

    let update (msg: Msg) (model: Model) =
        model, Cmd.none

    let view (model: Model) dispatch =
        let content = View.Grid( rowdefs = [ Stars 2.0; Star ],
                                 coldefs = [ Star; Stars 2.0; Star],
                                 children = 
                                  [ View.BoxView().BackgroundColor(Color.LightCoral)
                                                  .Row(0).Column(0)
                                    View.Viewport().Row(0).Column(1)
                                    View.BoxView().BackgroundColor(Color.LightGreen)
                                                  .Row(0).Column(2)
                                    View.BoxView().BackgroundColor(Color.Coral)
                                                  .Row(1).Column(0)
                                    View.BoxView().BackgroundColor(Color.DarkOrange)
                                                  .Row(1).Column(1)
                                    View.BoxView().BackgroundColor(Color.LimeGreen)
                                                  .Row(1).Column(2)
                                  ]
                               ).RowSpacing(2.0).ColumnSpacing(2.0)

        View.ContentPage(content = content,
                         backgroundColor = Color.Black,
                         hasNavigationBar = false)

    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgram init update view

type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> XamarinFormsProgram.run app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/tools.html#live-update for further  instructions.
    //
    //do runner.EnableLiveUpdate()
#endif    

    // Uncomment this code to save the application state to app.Properties using Newtonsoft.Json
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/models.html#saving-application-state for further  instructions.
#if APPSAVE
    let modelId = "model"
    override __.OnSleep() = 

        let json = Newtonsoft.Json.JsonConvert.SerializeObject(runner.CurrentModel)
        Console.WriteLine("OnSleep: saving model into app.Properties, json = {0}", json)

        app.Properties.[modelId] <- json

    override __.OnResume() = 
        Console.WriteLine "OnResume: checking for model in app.Properties"
        try 
            match app.Properties.TryGetValue modelId with
            | true, (:? string as json) -> 

                Console.WriteLine("OnResume: restoring model from app.Properties, json = {0}", json)
                let model = Newtonsoft.Json.JsonConvert.DeserializeObject<App.Model>(json)

                Console.WriteLine("OnResume: restoring model from app.Properties, model = {0}", (sprintf "%0A" model))
                runner.SetCurrentModel (model, Cmd.none)

            | _ -> ()
        with ex -> 
            App.program.onError("Error while restoring model found in app.Properties", ex)

    override this.OnStart() = 
        Console.WriteLine "OnStart: using same logic as OnResume()"
        this.OnResume()
#endif


