namespace Sprightly.Persistence

open Sprightly
open Xamarin.Forms

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

    /// <summary>
    /// <see cref="readMetaData"/> obtains the metadata of the 
    /// texture file at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path of the image file.</param>
    /// <returns>
    /// Some <see cref="MetaData"/> if the file can be read correctly;
    /// otherwise None.
    /// </returns>
    let public readMetaData (path: Common.Path.T) : Domain.Texture.MetaData option =
        let inspector = DependencyService.Get<ITextureInspector>()
        inspector.ReadMetaData path

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
    let public loadDomainTexture (solutionDirectoryPath: Common.Path.T) 
                                 (dao: DataAccessRecord) : Domain.Texture.T option =
        let fullPath = (textureFolder solutionDirectoryPath) / (Common.Path.fromString dao.FileName)
        let metaData = readMetaData fullPath

        // TODO: verify whether we can use name here for id
        metaData |> Option.map (fun md -> { Id = Domain.Texture.Id dao.Name
                                            Data = { Name = dao.Name |> Domain.Texture.Name
                                                     Path = fullPath
                                                     MetaData= md
                                                   }
                                          })
