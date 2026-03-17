// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for GameObject and UnityEngine.Object.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns the object itself if it exists, or C# null if it has been destroyed.
        /// Solves the Unity fake-null problem where destroyed objects pass C# null-conditional
        /// checks but fail Unity's == null check.
        /// </summary>
        /// <typeparam name="T">The Unity Object type.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <returns>The object if it exists, or null if it has been destroyed.</returns>
        public static T OrNull<T>(this T obj) where T : Object
        {
            return obj != null ? obj : null;
        }

        /// <summary>
        /// Gets an existing component of type T, or adds one if it does not exist.
        /// </summary>
        /// <typeparam name="T">The component type to get or add.</typeparam>
        /// <param name="gameObject">The GameObject to search on.</param>
        /// <returns>The existing or newly added component.</returns>
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Checks whether the GameObject has a component of type T without allocating.
        /// </summary>
        /// <typeparam name="T">The component type to check for.</typeparam>
        /// <param name="gameObject">The GameObject to check.</param>
        /// <returns>True if the component exists, false otherwise.</returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out _);
        }
    }
}
