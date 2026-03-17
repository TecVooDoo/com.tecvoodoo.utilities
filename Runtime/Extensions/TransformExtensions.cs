// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for Transform.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Gets an existing component of type T, or adds one if it does not exist.
        /// </summary>
        /// <typeparam name="T">The component type to get or add.</typeparam>
        /// <param name="transform">The transform to search on.</param>
        /// <returns>The existing or newly added component.</returns>
        public static T GetOrAdd<T>(this Transform transform) where T : Component
        {
            T component = transform.GetComponent<T>();
            if (component == null)
            {
                component = transform.gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Enumerates all direct children of the transform.
        /// </summary>
        /// <param name="parent">The parent transform.</param>
        /// <returns>An enumerable of child transforms.</returns>
        public static IEnumerable<Transform> Children(this Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                yield return parent.GetChild(i);
            }
        }
    }
}
