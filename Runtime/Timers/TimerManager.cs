// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Centralized manager that ticks all registered timers each frame.
    /// Uses a sweep-list pattern for safe iteration during callbacks.
    /// Automatically injected into the PlayerLoop by TimerBootstrapper.
    /// </summary>
    public static class TimerManager
    {
        static readonly List<Timer> timers = new List<Timer>();
        static readonly List<Timer> sweep = new List<Timer>();

        /// <summary>
        /// Registers a timer to receive Tick calls each frame.
        /// </summary>
        /// <param name="timer">The timer to register.</param>
        public static void RegisterTimer(Timer timer)
        {
            timers.Add(timer);
        }

        /// <summary>
        /// Deregisters a timer so it no longer receives Tick calls.
        /// </summary>
        /// <param name="timer">The timer to deregister.</param>
        public static void DeregisterTimer(Timer timer)
        {
            timers.Remove(timer);
        }

        /// <summary>
        /// Ticks all registered timers. Called automatically via the PlayerLoop.
        /// Uses a sweep copy to handle timers that deregister during their Tick.
        /// </summary>
        public static void UpdateTimers()
        {
            if (timers.Count == 0) return;

            sweep.RefreshWith(timers);

            for (int i = 0; i < sweep.Count; i++)
            {
                sweep[i].Tick();
            }
        }

        /// <summary>
        /// Disposes and clears all registered timers. Called when exiting play mode.
        /// </summary>
        public static void Clear()
        {
            sweep.RefreshWith(timers);

            for (int i = 0; i < sweep.Count; i++)
            {
                sweep[i].Dispose();
            }

            timers.Clear();
            sweep.Clear();
        }
    }
}
