namespace Sprightly.Domain

/// <summary>
/// <see cref="Texture"/> defines the <see cref="Texture.T"/> and related
/// functions.
/// </summary>
module public Texture = 
    /// <summary>
    /// <see cref="Id"/> defines a texture id.
    /// </summary>
    type Id = | Id of int32

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
    }

    /// <summary>
    /// Construct a new <see cref="T"/>
    /// </summary>
    /// <param name="id">The id of the texture. </param>
    /// <param name="name">The human-readable name of this texture. </param>
    /// <returns>
    /// A new texture.
    /// </returns>
    let construct (id: int32) (name: string) (path: Path.T) : T =
        { id = Id id
          name = Name name
          path = path
        }

