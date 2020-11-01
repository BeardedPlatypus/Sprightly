namespace Sprightly.DataAccess

open Sprightly.Domain

module Json =
    open Newtonsoft.Json

    type public JsonString = string

    let serialize (obj: 'T): JsonString =
        JsonConvert.SerializeObject obj

    let writeJsonString (path: Path.T) (content: JsonString): Async<unit> =
        async {
            do! Async.SwitchToThreadPool ()

            use stream = new System.IO.StreamWriter (path |> Path.toString)
            return stream.WriteAsync(content) 
        } |> Async.Ignore

