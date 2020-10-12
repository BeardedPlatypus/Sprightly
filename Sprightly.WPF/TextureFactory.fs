namespace Sprightly.WPF

open Sprightly.Domain.Texture
open Sprightly.Domain.Path

open Xamarin.Forms

/// <summary>
/// <see cref="TextureFactory"/> provides a platform specific implementation 
/// of the <see cref="Sprightly.ITextureFactory"/> interface.
/// </summary>
[<Sealed>]
type public TextureFactory() =
    let viewport = ViewportFactory.Create()

    interface Sprightly.ITextureFactory with
        member this.HasTexture (id: Sprightly.Domain.Texture.Id) :  bool = 
            match id with | Id idVal -> viewport.HasTexture <| idVal.ToString()

        member this.RequestTextureLoad (id: Sprightly.Domain.Texture.Id) 
                                       (path: Sprightly.Domain.Path.T) = 
            match id, path with 
            | (Id idVal, T pathVal) -> viewport.LoadTexture (idVal.ToString(), pathVal)


module Dummy_TextureFactory =
    [<assembly: Dependency(typeof<TextureFactory>)>]
    do ()
