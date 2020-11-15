namespace Sprightly.WPF.DataAccess

open Sprightly
open Sprightly.DataAccess

open Xamarin.Forms

[<Sealed>]
type public TextureInspector() = 
    interface ITextureInspector with 
        member this.ReadMetaData (path: Domain.Path.T) : Domain.Texture.MetaData option =
            try
                let fileInfo = System.IO.FileInfo(path |> Domain.Path.toString)
                use stream = fileInfo.OpenRead()
                use image = System.Drawing.Image.FromStream(stream, false, false)

                Some { Width = Domain.Texture.Pixel image.Width
                       Height = Domain.Texture.Pixel image.Height
                       DiskSize = Domain.Texture.Size (System.Math.Round((((float) fileInfo.Length) / 1024.0), 2))
                     }
            with 
            | _ -> None


module Dummy_TextureInspector =
    [<assembly: Dependency(typeof<TextureInspector>)>]
    do ()
