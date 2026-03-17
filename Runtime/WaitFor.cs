// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Cached yield instruction instances to avoid per-frame allocations in coroutines.
    /// Usage: yield return WaitFor.FixedUpdate; yield return WaitFor.Seconds(0.5f);
    /// </summary>
    public static class WaitFor
    {
        static readonly WaitForFixedUpdate CachedFixedUpdate = new WaitForFixedUpdate();
        static readonly WaitForEndOfFrame CachedEndOfFrame = new WaitForEndOfFrame();

        static readonly Dictionary<float, WaitForSeconds> SecondsDictionary =
            new Dictionary<float, WaitForSeconds>(64, new FloatComparer());

        /// <summary>
        /// A cached WaitForFixedUpdate instance.
        /// </summary>
        public static WaitForFixedUpdate FixedUpdate
        {
            get { return CachedFixedUpdate; }
        }

        /// <summary>
        /// A cached WaitForEndOfFrame instance.
        /// </summary>
        public static WaitForEndOfFrame EndOfFrame
        {
            get { return CachedEndOfFrame; }
        }

        /// <summary>
        /// Returns a cached WaitForSeconds for the given duration.
        /// Durations smaller than one frame at the target frame rate return null (no wait).
        /// </summary>
        /// <param name="seconds">The wait duration in seconds.</param>
        /// <returns>A cached WaitForSeconds instance, or null for sub-frame durations.</returns>
        public static WaitForSeconds Seconds(float seconds)
        {
            int targetFrameRate = Application.targetFrameRate;
            if (targetFrameRate > 0 && seconds < 1f / targetFrameRate)
            {
                return null;
            }

            if (!SecondsDictionary.TryGetValue(seconds, out WaitForSeconds cached))
            {
                cached = new WaitForSeconds(seconds);
                SecondsDictionary[seconds] = cached;
            }

            return cached;
        }

        /// <summary>
        /// Float equality comparer that treats values within Mathf.Epsilon as equal.
        /// Used by the seconds cache to avoid near-duplicate entries.
        /// </summary>
        sealed class FloatComparer : IEqualityComparer<float>
        {
            public bool Equals(float x, float y)
            {
                return Mathf.Abs(x - y) <= Mathf.Epsilon;
            }

            public int GetHashCode(float value)
            {
                return value.GetHashCode();
            }
        }
    }
}
