// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for System.String.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns true if the string is null, empty, or consists only of whitespace.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if null, empty, or whitespace-only.</returns>
        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Returns the string itself, or an empty string if it is null.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>The original string or string.Empty.</returns>
        public static string OrEmpty(this string value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Truncates the string to the specified maximum length.
        /// Returns the original string if it is shorter than maxLength.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Returns a substring from startIndex up to (but not including) endIndex.
        /// Negative endIndex counts from the end of the string.
        /// </summary>
        /// <param name="value">The string to slice.</param>
        /// <param name="startIndex">The inclusive start index.</param>
        /// <param name="endIndex">The exclusive end index. Negative values count from the end.</param>
        /// <returns>The sliced substring.</returns>
        public static string Slice(this string value, int startIndex, int endIndex)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            int length = value.Length;

            if (endIndex < 0)
            {
                endIndex = length + endIndex;
            }

            if (startIndex < 0) startIndex = 0;
            if (endIndex > length) endIndex = length;
            if (endIndex <= startIndex) return string.Empty;

            return value.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Strips all non-alphanumeric characters except underscores (and optionally periods).
        /// Ensures the result does not start with a digit or period.
        /// Trailing periods are removed.
        /// </summary>
        /// <param name="input">The input string to sanitize.</param>
        /// <param name="allowPeriods">Whether to allow period characters in the output.</param>
        /// <returns>A sanitized alphanumeric string, or empty if input is null/empty.</returns>
        public static string ToAlphanumeric(this string input, bool allowPeriods = false)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            List<char> filtered = new List<char>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                bool isValid = char.IsLetterOrDigit(c) || c == '_' || (allowPeriods && c == '.');
                if (!isValid) continue;

                if (filtered.Count == 0 && (char.IsDigit(c) || c == '.')) continue;

                filtered.Add(c);
            }

            int lastValid = filtered.Count - 1;
            while (lastValid >= 0 && filtered[lastValid] == '.')
            {
                lastValid--;
            }

            if (lastValid < 0) return string.Empty;

            return new string(filtered.ToArray(), 0, lastValid + 1);
        }

        /// <summary>
        /// Wraps the string in a rich text color tag.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <param name="color">The color name or hex value.</param>
        /// <returns>The color-tagged string.</returns>
        public static string RichColor(this string text, string color)
        {
            return "<color=" + color + ">" + text + "</color>";
        }

        /// <summary>
        /// Wraps the string in a rich text size tag.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <param name="size">The font size.</param>
        /// <returns>The size-tagged string.</returns>
        public static string RichSize(this string text, int size)
        {
            return "<size=" + size + ">" + text + "</size>";
        }

        /// <summary>
        /// Wraps the string in rich text bold tags.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <returns>The bold-tagged string.</returns>
        public static string RichBold(this string text)
        {
            return "<b>" + text + "</b>";
        }

        /// <summary>
        /// Wraps the string in rich text italic tags.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <returns>The italic-tagged string.</returns>
        public static string RichItalic(this string text)
        {
            return "<i>" + text + "</i>";
        }
    }
}
