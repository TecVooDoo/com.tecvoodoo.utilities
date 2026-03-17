// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods specific to List and IList types.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns true if the list is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">The list to check.</param>
        /// <returns>True if null or empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Swaps two elements in the list at the specified indices.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">The list to modify.</param>
        /// <param name="indexA">The first index.</param>
        /// <param name="indexB">The second index.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        /// <summary>
        /// Clears the list and repopulates it from the given source.
        /// Used for sweep-list patterns where safe iteration during callbacks is needed.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">The list to refresh.</param>
        /// <param name="source">The source to populate from.</param>
        public static void RefreshWith<T>(this List<T> list, IEnumerable<T> source)
        {
            list.Clear();
            list.AddRange(source);
        }
    }
}
