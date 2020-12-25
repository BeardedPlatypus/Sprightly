namespace Sprightly.Persistence

open Sprightly

/// <summary>
/// The <see cref="DataAccess.Texture"/> module provides all methods related to
/// to accessing texture information on disk.
/// </summary>
module Texture =
    /// <summary>
    /// <see cref='DataAccessRecord"/> defines the data acces information of a 
    /// single texture.
    /// </summary>
    type public DataAccessRecord =
        { Name : string 
          FileName : string
          idString : string 
          idIndex : uint
        }

    open Common.Path
    /// <summary>
    /// Get the texture folder based on the provided <paramref name="solutionDirectoryPath"/>.
    /// </summary>
    /// <param name="solutionDirectoryPath">Path to to the solution directory.</param>
    /// <returns>
    /// The folder containing all texture files.
    /// <returns>
    let public textureFolder (solutionDirectoryPath: Common.Path.T) : Common.Path.T =
        solutionDirectoryPath / (fromString "Textures")

    open Domain.Textures.Texture
    /// <summary>
    /// Load the <see cref="Domain.Texture.T"/> from the specified <paramref name="texturePath"/>.
    /// </summary>
    /// <param name="inspector">The texture inspector to retrieve the metadata.</param>
    /// <param name="name">The name of the new texture.</param>
    /// <param name="id">The id of the new texture.</param>
    /// <param name="texturePath">The texture path.</param>
    /// <returns>
    /// The <see cref="Domain.Texture.T"/> if one can be read correctly from the specified
    /// <paramref name="texturePath"/>, else <see cref="Option.None"/>.
    /// </returns>
    let public loadDomainTexture (inspector: Domain.Textures.Inspector)
                                 (name: string)
                                 (id: Id)
                                 (texturePath: Common.Path.T) : T option =
        let metaData = inspector.ReadMetaData texturePath

        metaData |> Option.map (fun md -> { Id = id
                                            Data = { Name = name |> Name
                                                     Path = texturePath
                                                     MetaData= md
                                                   }
                                          })
