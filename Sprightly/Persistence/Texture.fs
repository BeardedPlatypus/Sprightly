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

    open Domain.Texture
    /// <summary>
    /// Load the <see cref="Domain.Texture.T"/> from the specified <paramref name="dao"/>
    /// with the given new <paramref name="id"/>.
    /// </summary>
    /// <param name="dao">The data-access record.</param>
    /// <returns>
    /// The <see cref="Domain.Texture.T"/> if one can be read correctly from the specified
    /// <paramref name="dao"/>, else <see cref="Option.None"/>.
    /// </returns>
    let public loadDomainTexture (inspector: Domain.ITextureInspector)
                                 (solutionDirectoryPath: Common.Path.T) 
                                 (dao: DataAccessRecord) : Domain.Texture.T option =
        let fullPath = (textureFolder solutionDirectoryPath) / (Common.Path.fromString dao.FileName)
        let metaData = inspector.ReadMetaData fullPath

        // TODO: verify whether we can use name here for id
        metaData |> Option.map (fun md -> { Id = Domain.Texture.Id (dao.Name, uint 0)
                                            Data = { Name = dao.Name |> Domain.Texture.Name
                                                     Path = fullPath
                                                     MetaData= md
                                                   }
                                          })
