// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.
// Based on AllocCounter by Adam Myhre (adammyhre)

using System;
using UnityEngine.Profiling;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Counts GC allocation blocks between construction and Stop().
    /// Use in development builds to verify zero-alloc code paths.
    /// </summary>
    /// <example>
    /// AllocCounter counter = new AllocCounter();
    /// DoSomething();
    /// int allocs = counter.Stop();
    /// Debug.Log($"Allocations: {allocs}");
    /// </example>
    public class AllocCounter
    {
        Recorder rec;

        public AllocCounter()
        {
            rec = Recorder.Get("GC.Alloc");
            rec.enabled = false;
#if !UNITY_WEBGL
            rec.FilterToCurrentThread();
#endif
            rec.enabled = true;
        }

        /// <summary>
        /// Stops recording and returns the number of GC allocation blocks captured.
        /// </summary>
        public int Stop()
        {
            if (rec == null) throw new InvalidOperationException("AllocCounter has already been stopped.");
            rec.enabled = false;
#if !UNITY_WEBGL
            rec.CollectFromAllThreads();
#endif
            int result = rec.sampleBlockCount;
            rec = null;
            return result;
        }
    }
}
