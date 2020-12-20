namespace Sprightly.WPF.DataAccess

open Sprightly
open Sprightly.Persistence

open Xamarin.Forms

[<Sealed>]
type public TextureInspector() = 
    interface Domain.ITextureInspector with 
        member this.ReadMetaData (path: Common.Path.T) : Domain.MetaData.T option =
            try
                let fileInfo = System.IO.FileInfo(path |> Common.Path.toString)
                use stream = fileInfo.OpenRead()
                use image = System.Drawing.Image.FromStream(stream, false, false)

                Some { Width = Domain.MetaData.Pixel image.Width
                       Height = Domain.MetaData.Pixel image.Height
                       DiskSize = Domain.MetaData.Size (System.Math.Round((((float) fileInfo.Length) / 1024.0), 2))
                     }
            with 
            | _ -> None


module Dummy_TextureInspector =
    [<assembly: Dependency(typeof<TextureInspector>)>]
    do ()
