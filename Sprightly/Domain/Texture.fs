namespace Sprightly.Domain

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
    type Id = | Id of string

    /// <summary>
    /// <see cref="Name"/> defines a texture name.
    /// </summary>
    type Name = | Name of string

    /// <summary>
    /// <see cref="T"/> defines the texture type.
    /// </summary>
    type public T = {
        id: Id
        name: Name
        path: Path.T
        metaData: MetaData
    }

    /// <summary>
    /// Construct a new <see cref="T"/>
    /// </summary>
    /// <param name="id">The id of the texture. </param>
    /// <param name="name">The human-readable name of this texture. </param>
    /// <returns>
    /// A new texture.
    /// </returns>
    let construct (id: string) (name: string) (path: Path.T) (metaData: MetaData) : T =
        { id = Id id
          name = Name name
          path = path
          metaData = metaData
        }

