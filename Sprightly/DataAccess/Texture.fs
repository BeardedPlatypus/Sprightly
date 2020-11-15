namespace Sprightly.DataAccess

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
    let public readMetaData (path: Domain.Path.T) : Domain.Texture.MetaData option =
        let inspector = DependencyService.Get<ITextureInspector>()
        inspector.ReadMetaData path

    open Domain.Path
    let public textureFolder (solutionDirectoryPath: Domain.Path.T) : Domain.Path.T =
        solutionDirectoryPath / (fromString "Textures")

