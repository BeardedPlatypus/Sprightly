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

    /// <summary>
    /// Copy the specified <paramref name="texturePath"/> into the texture folder of
    /// the solution at <paramref name="slnDirectoryPath"/> and return the new path.
    /// </summary>
    /// <param name="slnDirectoryPath">The path to the directory containing the solution.</param>
    /// <param name="texturePath">The path to the texture external of the solution.</param>
    /// <returns>
    /// The path to the copied texture if one was copied.
    /// </returns>
    let public copyTextureIntoTextureFolder (slnDirectoryPath: Common.Path.T)
                                            (texturePath: Common.Path.T) : Common.Path.T option =
        if Common.Path.exists texturePath &&
           Common.Path.exists slnDirectoryPath then 
            let texFolder = textureFolder slnDirectoryPath
            let name = Common.Path.name texturePath
            let newPath = 
                Common.Path.combine texFolder ((Common.Path.generateUniqueName texFolder name) |> Common.Path.fromString)

            System.IO.File.Copy(texturePath |> Common.Path.toString,
                                newPath |> Common.Path.toString)

            Some newPath
        else 
            None
