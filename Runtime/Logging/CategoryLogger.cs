// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Lightweight categorized logger that wraps UnityEngine.Debug.Log.
    /// Each category gets a color-coded prefix for easy filtering in the Console.
    /// All logging calls are stripped from release builds via [Conditional("UNITY_EDITOR")] and
    /// [Conditional("DEVELOPMENT_BUILD")].
    /// </summary>
    public static class CategoryLogger
    {
        /// <summary>
        /// Logs an informational message with a colored category prefix.
        /// Stripped from release builds.
        /// </summary>
        /// <param name="category">The category label (e.g. "Audio", "AI", "UI").</param>
        /// <param name="message">The log message.</param>
        /// <param name="color">The hex color for the category prefix (default: cyan).</param>
        /// <param name="context">Optional Unity Object context for Console ping.</param>
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string category, string message, string color = "#00FFFF", Object context = null)
        {
            Debug.Log(FormatMessage(category, message, color), context);
        }

        /// <summary>
        /// Logs a warning message with a colored category prefix.
        /// Stripped from release builds.
        /// </summary>
        /// <param name="category">The category label.</param>
        /// <param name="message">The warning message.</param>
        /// <param name="color">The hex color for the category prefix (default: yellow).</param>
        /// <param name="context">Optional Unity Object context for Console ping.</param>
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(string category, string message, string color = "#FFFF00", Object context = null)
        {
            Debug.LogWarning(FormatMessage(category, message, color), context);
        }

        /// <summary>
        /// Logs an error message with a colored category prefix.
        /// This method is NOT stripped from release builds since errors should always be visible.
        /// </summary>
        /// <param name="category">The category label.</param>
        /// <param name="message">The error message.</param>
        /// <param name="color">The hex color for the category prefix (default: red).</param>
        /// <param name="context">Optional Unity Object context for Console ping.</param>
        public static void LogError(string category, string message, string color = "#FF4444", Object context = null)
        {
            Debug.LogError(FormatMessage(category, message, color), context);
        }

        static string FormatMessage(string category, string message, string color)
        {
            return "<color=" + color + ">[" + category + "]</color> " + message;
        }
    }
}
