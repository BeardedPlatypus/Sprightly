namespace Sprightly.WPF.DataAccess

open Sprightly

open Xamarin.Forms

[<Sealed>]
type public InspectorImpl() = 
    interface Domain.Textures.Inspector with 
        member this.ReadMetaData (path: Common.Path.T) : Domain.Textures.MetaData.T option =
            try
                let fileInfo = System.IO.FileInfo(path |> Common.Path.toString)
                use stream = fileInfo.OpenRead()
                use image = System.Drawing.Image.FromStream(stream, false, false)

                Some { Width = Domain.Textures.MetaData.Pixel image.Width
                       Height = Domain.Textures.MetaData.Pixel image.Height
                       DiskSize = Domain.Textures.MetaData.Size (System.Math.Round((((float) fileInfo.Length) / 1024.0), 2))
                     }
            with 
            | _ -> None


module Dummy_TextureInspector =
    [<assembly: Dependency(typeof<InspectorImpl>)>]
    do ()
