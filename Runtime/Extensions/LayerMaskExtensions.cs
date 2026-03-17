// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for LayerMask.
    /// </summary>
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Returns true if the LayerMask contains the specified layer number.
        /// </summary>
        /// <param name="mask">The LayerMask to check.</param>
        /// <param name="layerNumber">The layer number (0-31) to check for.</param>
        /// <returns>True if the layer is included in the mask.</returns>
        public static bool Contains(this LayerMask mask, int layerNumber)
        {
            return mask == (mask | (1 << layerNumber));
        }
    }
}
