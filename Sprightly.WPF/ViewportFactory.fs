namespace Sprightly.WPF

/// <summary>
/// <see cref="ViewportFactory"/> module provides the methods to construct a
/// <see cref="IViewport"/>.
/// </summary>
module internal ViewportFactory = 
    // Currently this is a singleton that creates a single Viewport.
    let private viewport : Sprightly.WPF.Components.IViewport =
        Sprightly.WPF.Components.Viewport() :> Sprightly.WPF.Components.IViewport

    /// <summary>
    /// Create a reference to the singleton <see cref="IViewport"/>.
    /// </summary>
    let internal Create () : Sprightly.WPF.Components.IViewport = viewport

