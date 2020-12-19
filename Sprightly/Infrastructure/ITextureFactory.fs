namespace Sprightly.Infrastructure

open Sprightly

/// <summary>
/// <see cref="ITextureFactory"/> exposes the relevant methods to generate new 
/// underlying texture objects to be used in the <see cref="Viewport"/>.
/// </summary>
type ITextureFactory = 
    /// <summary>
    /// Check if the texture with the specified <paramref name="label"/> exists.
    /// </summary>
    /// <param name="label">The label of the texture to check.</param>
    /// <returns>
    /// True if there exists a texture with the associated <param name="label"/>;
    /// False otherwise.
    /// </returns>
    abstract HasTexture : id:Domain.Texture.Id -> bool

    /// <summary>
    /// Load the the texture at the specified <paramref name="texturePath"/> and
    /// associate it with the specified <paramref name="label"/>.
    /// </summary>
    /// <param name="label">The new label for the texture.</param>
    /// <param name="texturePath">The path at which the texture is located.</param>
    abstract RequestTextureLoad : id:Domain.Texture.Id -> texturePath:Common.Path.T -> unit
