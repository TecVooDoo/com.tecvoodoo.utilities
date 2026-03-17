// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for IList and IEnumerable collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Shuffles the list in-place using the Fisher-Yates (Durstenfeld) algorithm.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int index = UnityEngine.Random.Range(0, count + 1);
                T temp = list[index];
                list[index] = list[count];
                list[count] = temp;
            }
        }

        /// <summary>
        /// Returns a random element from the list.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">The list to pick from. Must not be null or empty.</param>
        /// <returns>A random element.</returns>
        /// <exception cref="ArgumentException">Thrown if the list is null or empty.</exception>
        public static T RandomElement<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("Cannot get a random element from a null or empty list.");
            }
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Returns true if the enumerable is null or contains no elements.
        /// Uses ICollection.Count when available to avoid enumeration overhead.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <returns>True if null or empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return true;

            if (enumerable is ICollection<T> collection)
            {
                return collection.Count == 0;
            }

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                return !enumerator.MoveNext();
            }
        }

        /// <summary>
        /// Executes an action on each element in the enumerable.
        /// Avoids the need for System.Linq just for iteration side-effects.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="enumerable">The enumerable to iterate.</param>
        /// <param name="action">The action to apply to each element.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null) return;

            foreach (T item in enumerable)
            {
                action(item);
            }
        }
    }
}
