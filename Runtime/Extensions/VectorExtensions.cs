// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for Vector2 and Vector3.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns a new Vector3 with selectively replaced components.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="x">Optional replacement for X. Null keeps original.</param>
        /// <param name="y">Optional replacement for Y. Null keeps original.</param>
        /// <param name="z">Optional replacement for Z. Null keeps original.</param>
        /// <returns>A new Vector3 with the specified components replaced.</returns>
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        /// <summary>
        /// Returns the vector projected onto the XZ plane (Y zeroed).
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <returns>A new Vector3 with Y set to zero.</returns>
        public static Vector3 Flat(this Vector3 vector)
        {
            return new Vector3(vector.x, 0f, vector.z);
        }

        /// <summary>
        /// Returns the normalized direction from this vector to the target.
        /// </summary>
        /// <param name="from">The starting position.</param>
        /// <param name="to">The target position.</param>
        /// <returns>A normalized direction vector. Returns zero if positions are identical.</returns>
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        /// <summary>
        /// Returns a new Vector2 with selectively replaced components.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="x">Optional replacement for X. Null keeps original.</param>
        /// <param name="y">Optional replacement for Y. Null keeps original.</param>
        /// <returns>A new Vector2 with the specified components replaced.</returns>
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }

        /// <summary>
        /// Projects a Vector3 to Vector2 using X and Y components.
        /// </summary>
        public static Vector2 ToVector2XY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Projects a Vector3 to Vector2 using X and Z components.
        /// </summary>
        public static Vector2 ToVector2XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}
