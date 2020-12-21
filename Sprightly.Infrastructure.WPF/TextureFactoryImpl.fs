namespace Sprightly.Infrastructure.WPF

open Sprightly.Common.Path
open Sprightly.Common.KoboldLayer.WPF

open Sprightly.Domain.Textures.Texture

/// <summary>
/// <see cref="TextureFactory"/> provides a platform specific implementation 
/// of the <see cref="Sprightly.ITextureFactory"/> interface.
/// </summary>
[<Sealed>]
type public TextureFactoryImpl() =
    let viewport = ViewportFactory.Create()
    
    interface Sprightly.Infrastructure.TextureFactory with
        member this.HasTexture (id: Id) :  bool = 
            match id with | Id (idVal, idInt) -> viewport.HasTexture <| idVal.ToString()

        member this.RequestTextureLoad (id: Id) 
                                       (path: Sprightly.Common.Path.T) = 
            viewport.LoadTexture (toKeyString id, toString path)
