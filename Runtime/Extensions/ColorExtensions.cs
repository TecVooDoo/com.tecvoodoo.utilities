// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for UnityEngine.Color.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns a copy of the color with the specified alpha value.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="alpha">The new alpha value (0-1).</param>
        /// <returns>A new color with the specified alpha.</returns>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Adds the RGBA components of two colors and clamps each to 0-1.
        /// </summary>
        /// <param name="color">The first color.</param>
        /// <param name="other">The color to add.</param>
        /// <returns>The clamped sum of both colors.</returns>
        public static Color Add(this Color color, Color other)
        {
            return ClampChannels(color + other);
        }

        /// <summary>
        /// Subtracts the RGBA components of another color and clamps each to 0-1.
        /// </summary>
        /// <param name="color">The first color.</param>
        /// <param name="other">The color to subtract.</param>
        /// <returns>The clamped difference.</returns>
        public static Color Subtract(this Color color, Color other)
        {
            return ClampChannels(color - other);
        }

        /// <summary>
        /// Linearly blends two colors by the given ratio.
        /// </summary>
        /// <param name="color">The starting color (ratio 0).</param>
        /// <param name="target">The target color (ratio 1).</param>
        /// <param name="ratio">Blend ratio clamped to 0-1.</param>
        /// <returns>The blended color.</returns>
        public static Color Blend(this Color color, Color target, float ratio)
        {
            float t = Mathf.Clamp01(ratio);
            float oneMinusT = 1f - t;
            return new Color(
                color.r * oneMinusT + target.r * t,
                color.g * oneMinusT + target.g * t,
                color.b * oneMinusT + target.b * t,
                color.a * oneMinusT + target.a * t
            );
        }

        /// <summary>
        /// Returns the inverted color (1 minus each RGB channel). Alpha is preserved.
        /// </summary>
        /// <param name="color">The color to invert.</param>
        /// <returns>The inverted color.</returns>
        public static Color Invert(this Color color)
        {
            return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
        }

        /// <summary>
        /// Converts the color to a hexadecimal string (e.g. "#FF0000FF").
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A hex string prefixed with #.</returns>
        public static string ToHex(this Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color);
        }

        /// <summary>
        /// Parses a hexadecimal string into a Color.
        /// Supports formats: "#RRGGBB", "#RRGGBBAA", "RRGGBB", "RRGGBBAA".
        /// </summary>
        /// <param name="hex">The hex string to parse.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="ArgumentException">Thrown if the hex string is invalid.</exception>
        public static Color FromHex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                return color;
            }

            throw new ArgumentException("Invalid hex color string: " + hex, nameof(hex));
        }

        static Color ClampChannels(Color color)
        {
            return new Color(
                Mathf.Clamp01(color.r),
                Mathf.Clamp01(color.g),
                Mathf.Clamp01(color.b),
                Mathf.Clamp01(color.a)
            );
        }
    }
}
