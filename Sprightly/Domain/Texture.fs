namespace Sprightly.Domain

open Sprightly.Common

/// <summary>
/// <see cref="Texture"/> defines the <see cref="Texture.T"/> and related
/// functions.
/// </summary>
module public Texture = 
    /// <summary>
    /// <see cref="Pixel"/> defines a number of pixels of a texture.
    /// </summary>
    type public Pixel = Pixel of int

    /// <summary>
    /// <see cref="Size"/> defines the size of a texture in kilo bytes (KB)
    /// </summary>
    type public Size = Size of float

    /// <summary>
    /// <see cref="MetaData"/> defines the metadata information of a texture 
    /// file.
    /// </summary>
    type public MetaData =
        { Width: Pixel
          Height: Pixel
          DiskSize: Size
        }

    /// <summary>
    /// <see cref="Id"/> defines a texture id.
    /// </summary>
    type public Id = | Id of string * uint

    /// <summary>
    /// Convert the specified <paramref name="id"/> to its corresponding key
    /// string value.
    /// </summary>
    /// <param name="id">The <see cref='Id"/> to convert.</param>
    /// <returns>
    /// The corresponding key string.
    /// </returns>
    let public toKeyString (id: Id): string = 
        match id with | Id (idVal, idInt) -> idVal + "#" + idInt.ToString()

    /// <summary>
    /// <see cref="Name"/> defines a texture name.
    /// </summary>
    type public Name = | Name of string

    /// <summary>
    /// The data of a single texture.
    /// </summary>
    type public Data = {
        Name: Name
        Path: Path.T
        MetaData: MetaData
    }

    /// <summary>
    /// <see cref="T"/> defines the texture type.
    /// </summary>
    type public T = {
        Id: Id
        Data: Data
    }

    /// <summary>
    /// Construct a new <see cref="T"/>
    /// </summary>
    /// <param name="id">The id of the texture. </param>
    /// <param name="name">The human-readable name of this texture. </param>
    /// <returns>
    /// A new texture.
    /// </returns>
    let construct (id: (string * uint)) (name: string) (path: Path.T) (metaData: MetaData) : T =
        { Id = Id id
          Data = { Name = Name name
                   Path = path
                   MetaData = metaData
                 }
        }

