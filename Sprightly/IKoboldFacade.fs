namespace Sprightly 

/// <summary>
/// <see cref="IKoboldFacade"/> exposes the relevant update logic of the kobold
/// facade to the Sprightly application.
/// </summary>
type IKoboldUpdateFacade = 
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
    abstract RequestTextureLoad : id:Domain.Texture.Id -> texturePath:Domain.Path.T -> unit
