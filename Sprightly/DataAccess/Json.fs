namespace Sprightly.DataAccess

open Sprightly.Domain

module Json =
    open Newtonsoft.Json

    type public JsonString = string

    let serialize (obj: 'T): JsonString =
        JsonConvert.SerializeObject obj

    let writeJsonString (path: Path.T) (content: JsonString): unit =
        let parentDirectory = Path.parentDirectory path
        System.IO.Directory.CreateDirectory ( parentDirectory |> Path.toString ) |> ignore

        use stream = new System.IO.StreamWriter (path |> Path.toString)
        stream.WriteAsync(content) |> Async.AwaitTask |> ignore

