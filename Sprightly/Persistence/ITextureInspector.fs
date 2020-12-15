namespace Sprightly.Persistence

open Sprightly

/// <summary>
/// <see crfe="ITextureInspector"/> defines the interface with
/// which a texture inspector can be created.
/// </summary>
type public ITextureInspector =
    /// <summary>
    /// Read the metadata of the file at the specified path.
    /// </summary>
    /// <returns>
    /// Some <see cref="MetaData"/> if the file can be read correctly;
    /// otherwise None.
    /// </returns>
    abstract ReadMetaData : Common.Path.T -> Domain.Texture.MetaData option

