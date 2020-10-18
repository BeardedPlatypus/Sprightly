namespace Sprightly.Pages

open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms
open Sprightly.Components

/// <summary>
/// <see cref="NewProjectPage"/> defines the page shown to create a new project
/// </summary>
module NewProjectPage =
    /// <summary>
    /// <see cref="Model"/> defines the model for the <see cref="NewProjectPage"/>.
    /// This consists of a project name and path to a directory.
    /// </summary>
    type public Model =
        { ProjectName : string option
          DirectoryPath : Sprightly.Domain.Path.T option
        }


    /// <summary>
    /// <see cref="Msg"/> defines the messages for the <see cref="NewProjectPage"/>.
    /// </summary>
    type public Msg = 
        | SetProjectName of string
        | SetDirectoryPath of string
        | RequestNewProject


    /// <summary>
    /// <see cref="ExternalCmdMsg"/> defines the external command messages for 
    /// the <see cref="NewProjectPage"/>, which need to be mapped in the application
    /// level.
    /// </summary>
    type public ExternalCmdMsg =
        | CreateNewProject


    /// <summary>
    /// <see cref="Msg"/> defines the command messages for the <see cref="NewProjectPage"/>.
    /// </summary>
    type public CmdMsg =
        | External of ExternalCmdMsg


    /// <summary>
    /// Update the provided <paramref name="model"/> to its new state given the
    /// provided <paramref name="msg"/>.
    /// </summary>
    /// <param name="msg">The message for which the new state should be returned.</param>
    /// </param name="model">The model from which the new state is calculated.</param>
    /// <returns>
    /// The new state as a <see cref="Model"/> and a set of command messages to be 
    /// executed.
    /// </returns>
    let public update (msg: Msg) (model: Model) : Model * CmdMsg list =
        match msg with 
        | SetProjectName newName   -> 
            { model with ProjectName = Some newName }, []
        | SetDirectoryPath newPath -> 
            { model with DirectoryPath = Some (Sprightly.Domain.Path.fromString newPath) }, []
        | RequestNewProject ->
            model, [ External CreateNewProject ]


    /// <summary>
    /// <see cref="view"/> transforms the <paramref name="model"/> onto
    /// its corresponding view.
    /// </summary>
    /// <param name="model">The model to display.</param>
    /// <param name="dispatch">The function to dispatch messages with.</param>
    /// <remarks>
    /// <see cref='update"/> is executed on the ui thread.
    /// </remarks>
    let public view (model: Model) dispatch = 
        View.Label(text = "Create new project:", 
                   fontSize = FontSize.fromValue 28.0)
            .Margin(Thickness 40.0)





