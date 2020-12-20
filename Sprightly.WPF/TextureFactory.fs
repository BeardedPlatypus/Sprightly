namespace Sprightly.WPF

open Sprightly.Common.Path
open Sprightly.Domain.Textures.Texture

open Xamarin.Forms

/// <summary>
/// <see cref="TextureFactory"/> provides a platform specific implementation 
/// of the <see cref="Sprightly.ITextureFactory"/> interface.
/// </summary>
[<Sealed>]
type public TextureFactory() =
    let viewport = ViewportFactory.Create()
    
    interface Sprightly.Infrastructure.ITextureFactory with
        member this.HasTexture (id: Id) :  bool = 
            match id with | Id (idVal, idInt) -> viewport.HasTexture <| idVal.ToString()

        member this.RequestTextureLoad (id: Id) 
                                       (path: Sprightly.Common.Path.T) = 
            viewport.LoadTexture (toKeyString id, toString path)


module Dummy_TextureFactory =
    [<assembly: Dependency(typeof<TextureFactory>)>]
    do ()
