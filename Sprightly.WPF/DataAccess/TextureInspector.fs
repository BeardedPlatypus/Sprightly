namespace Sprightly.WPF.DataAccess

open Sprightly
open Sprightly.DataAccess


type public TextureInspector() = 
    interface ITextureInspector with 
        member this.ReadMetaData (path: Domain.Path.T) : Domain.Texture.MetaData option =
            try
                use stream = System.IO.File.OpenRead(path |> Domain.Path.toString)
         
                None
            with 
            | _ -> None
