﻿namespace Sprightly

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
    type public Model = 
        | StartPageModel   of Pages.StartPage.Model
        | ProjectPageModel of Pages.ProjectPage.Model


    type public Msg = 
        | StartPageMsg    of Pages.StartPage.Msg
        | ProjectPageMsg  of Pages.ProjectPage.Msg
        | StartNewProject


    type public CmdMsg = 
        | StartPageCmdMsg   of Pages.StartPage.CmdMsg
        | ProjectPageCmdMsg of Pages.ProjectPage.CmdMsg


    let private toCmdMsg (mapFunc: 'a -> CmdMsg) (cmdMsgList: 'a list) : CmdMsg list =
        List.map mapFunc cmdMsgList


    let init () =
        let model, cmdMsgs = Pages.StartPage.init
        model |> StartPageModel, cmdMsgs |> ( toCmdMsg StartPageCmdMsg )


    let update (msg: Msg) (model: Model) : Model * CmdMsg list =
        match model, msg with 
        | (StartPageModel startPageModel, StartPageMsg startPageMsg) ->
            let updatedModel, cmdMsgs = Pages.StartPage.update startPageMsg startPageModel
            updatedModel |> StartPageModel, cmdMsgs |> ( toCmdMsg StartPageCmdMsg )
        | (ProjectPageModel projectPageModel, ProjectPageMsg projectPageMsg) ->
            let updatedModel, cmdMsgs = Pages.ProjectPage.update projectPageMsg projectPageModel
            updatedModel |> ProjectPageModel, cmdMsgs |> ( toCmdMsg ProjectPageCmdMsg )
        | _, StartNewProject ->
            () |> ProjectPageModel, []
        | _ -> 
            model, []


    let view (model: Model) dispatch =
        let content = 
            match model with 
            | StartPageModel startPageModel ->
                Pages.StartPage.view startPageModel ( dispatch << StartPageMsg )
            | ProjectPageModel projectPageModel ->
                Pages.ProjectPage.view projectPageModel ( dispatch << ProjectPageMsg )

        View.ContentPage(content = content,
                         hasNavigationBar = false)

    
    let private startNewProjectCmd = 
         Cmd.ofMsg StartNewProject


    let mapExternalStartPageCmdMsg (cmdMsg: Pages.StartPage.ExternalCmdMsg) =
        match cmdMsg with 
        | Pages.StartPage.StartNewProject -> 
            startNewProjectCmd
        | _ ->
            Cmd.none


    let mapStartPageCmdMsg (cmdMsg: Pages.StartPage.CmdMsg) = 
        match cmdMsg with 
        | Pages.StartPage.Internal internalCmdMsg -> 
            Pages.StartPage.mapInternalCmdMsg internalCmdMsg |> ( Cmd.map StartPageMsg )
        | Pages.StartPage.External externalCmdMsg ->
            mapExternalStartPageCmdMsg externalCmdMsg


    let mapCmdMsg (cmdMsg: CmdMsg) =
        match cmdMsg with 
        | StartPageCmdMsg startPageCmdMsg -> mapStartPageCmdMsg startPageCmdMsg
        | ProjectPageCmdMsg _ -> Cmd.none


    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgramWithCmdMsg init update view mapCmdMsg


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


