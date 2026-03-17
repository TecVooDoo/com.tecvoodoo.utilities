// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Countdown timer that fires an OnInterval event at regular intervals during its countdown.
    /// Counts down from totalTime to zero, invoking OnInterval every intervalSeconds.
    /// </summary>
    public class IntervalTimer : Timer
    {
        readonly float interval;
        float nextInterval;

        /// <summary>
        /// Invoked each time an interval threshold is crossed during countdown.
        /// </summary>
        public Action OnInterval = delegate { };

        /// <summary>
        /// Creates an interval timer.
        /// </summary>
        /// <param name="totalTime">Total countdown duration in seconds.</param>
        /// <param name="intervalSeconds">Time between interval events in seconds.</param>
        public IntervalTimer(float totalTime, float intervalSeconds) : base(totalTime)
        {
            interval = intervalSeconds;
            nextInterval = totalTime - interval;
        }

        public override void Tick()
        {
            if (IsRunning && CurrentTime > 0f)
            {
                CurrentTime -= Time.deltaTime;

                while (CurrentTime <= nextInterval && nextInterval >= 0f)
                {
                    OnInterval.Invoke();
                    nextInterval -= interval;
                }
            }

            if (IsRunning && CurrentTime <= 0f)
            {
                CurrentTime = 0f;
                Stop();
            }
        }

        public override bool IsFinished
        {
            get { return CurrentTime <= 0f; }
        }

        public override void Reset()
        {
            base.Reset();
            nextInterval = initialTime - interval;
        }

        public override void Reset(float newTime)
        {
            base.Reset(newTime);
            nextInterval = initialTime - interval;
        }
    }
}
