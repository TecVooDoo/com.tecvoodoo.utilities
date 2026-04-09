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

        /// <summary>
        /// Resets local position to Vector3.zero.
        /// </summary>
        public static void ResetPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Resets local rotation to Quaternion.identity.
        /// </summary>
        public static void ResetRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Resets local scale to Vector3.one.
        /// </summary>
        public static void ResetScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Destroys all child GameObjects immediately (editor) or deferred (runtime).
        /// </summary>
        public static void DestroyChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Returns the full hierarchy path of the transform (e.g. "Root/Parent/Child").
        /// </summary>
        public static string HierarchyPath(this Transform transform)
        {
            string path = transform.name;
            Transform current = transform.parent;
            while (current != null)
            {
                path = current.name + "/" + path;
                current = current.parent;
            }
            return path;
        }
    }
}
