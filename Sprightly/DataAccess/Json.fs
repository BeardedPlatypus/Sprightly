namespace Sprightly.DataAccess

open Sprightly.Domain

/// <summary>
/// <see cref="Json"/> wraps the methods to serialize, deserialize, read and write
/// json records.
/// </summary>
module public Json =
    open Newtonsoft.Json

    /// <summary>
    /// <see cref="JsonString"/> defines a json string to use in this module.
    /// </summary>
    type public JsonString = string

    /// <summary>
    /// Serialize the provided <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>
    /// A serialized <see cref="JsonString"/> describing the provided
    /// <paramref name="obj"/>.
    /// </returns>
    let public serialize (obj: 'T) : JsonString =
        JsonConvert.SerializeObject obj

    /// <summary>
    /// Write the specified <paramref name="content"/> to the specified
    /// <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path to write to.</param>
    /// <param name="content">The json string to write.</param>
    let public writeJsonString (path: Path.T) (content: JsonString) : unit =
        let parentDirectory = Path.parentDirectory path
        System.IO.Directory.CreateDirectory ( parentDirectory |> Path.toString ) |> ignore

        use stream = new System.IO.StreamWriter (path |> Path.toString)
        stream.WriteAsync(content) |> Async.AwaitTask |> ignore

