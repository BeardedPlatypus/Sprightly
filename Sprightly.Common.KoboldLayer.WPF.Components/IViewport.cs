﻿using System;

namespace Sprightly.Common.KoboldLayer.WPF.Components
{
    /// <summary>
    /// <see cref="IViewport"/> defines the interface with which components
    /// can interact with the viewport.
    /// </summary>
    public interface IViewport
    {
        /// <summary>
        /// Initialises the specified p window.
        /// </summary>
        /// <param name="pWindow">The p window.</param>
        void Initialise(IntPtr pWindow);

        // TODO: refactor this
        void Update();

        /// <summary>
        /// Begins the render.
        /// </summary>
        void BeginRender();

        /// <summary>
        /// Finalises the render.
        /// </summary>
        void FinaliseRender();

        bool HasTexture(string id);

        void LoadTexture(string id, string path);
    }
}