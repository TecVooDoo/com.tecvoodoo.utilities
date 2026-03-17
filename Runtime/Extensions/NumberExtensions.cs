// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for numeric types.
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Maps a value from one range to another.
        /// </summary>
        /// <param name="value">The value to remap.</param>
        /// <param name="fromMin">Source range minimum.</param>
        /// <param name="fromMax">Source range maximum.</param>
        /// <param name="toMin">Target range minimum.</param>
        /// <param name="toMax">Target range maximum.</param>
        /// <returns>The remapped value.</returns>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float fromRange = fromMax - fromMin;
            if (Mathf.Approximately(fromRange, 0f)) return toMin;

            float normalized = (value - fromMin) / fromRange;
            return toMin + normalized * (toMax - toMin);
        }

        /// <summary>
        /// Returns true if the two floats are approximately equal (within Mathf.Epsilon).
        /// </summary>
        /// <param name="value">The first value.</param>
        /// <param name="other">The second value.</param>
        /// <returns>True if approximately equal.</returns>
        public static bool Approximately(this float value, float other)
        {
            return Mathf.Approximately(value, other);
        }

        /// <summary>
        /// Returns true if the integer is odd.
        /// </summary>
        /// <param name="value">The integer to check.</param>
        /// <returns>True if odd.</returns>
        public static bool IsOdd(this int value)
        {
            return (value & 1) == 1;
        }

        /// <summary>
        /// Returns true if the integer is even.
        /// </summary>
        /// <param name="value">The integer to check.</param>
        /// <returns>True if even.</returns>
        public static bool IsEven(this int value)
        {
            return (value & 1) == 0;
        }
    }
}
