// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Timer that fires an OnTick event at a fixed frequency (N times per second).
    /// Runs indefinitely until stopped.
    /// </summary>
    public class FrequencyTimer : Timer
    {
        /// <summary>
        /// The configured tick rate in ticks per second.
        /// </summary>
        public int TicksPerSecond { get; private set; }

        /// <summary>
        /// Invoked each time the frequency threshold is crossed.
        /// </summary>
        public Action OnTick = delegate { };

        float timeThreshold;

        public FrequencyTimer(int ticksPerSecond) : base(0f)
        {
            CalculateTimeThreshold(ticksPerSecond);
        }

        public override void Tick()
        {
            if (IsRunning && CurrentTime >= timeThreshold)
            {
                CurrentTime -= timeThreshold;
                OnTick.Invoke();
            }

            if (IsRunning && CurrentTime < timeThreshold)
            {
                CurrentTime += Time.deltaTime;
            }
        }

        public override bool IsFinished
        {
            get { return !IsRunning; }
        }

        public override void Reset()
        {
            CurrentTime = 0f;
        }

        /// <summary>
        /// Resets the timer with a new tick rate.
        /// </summary>
        /// <param name="newTicksPerSecond">The new tick rate.</param>
        public void Reset(int newTicksPerSecond)
        {
            CalculateTimeThreshold(newTicksPerSecond);
            Reset();
        }

        void CalculateTimeThreshold(int ticksPerSecond)
        {
            TicksPerSecond = ticksPerSecond;
            timeThreshold = 1f / ticksPerSecond;
        }
    }
}
