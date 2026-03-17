// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Timer that counts up from zero indefinitely. Useful for measuring durations.
    /// </summary>
    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0f) { }

        public override void Tick()
        {
            if (IsRunning)
            {
                CurrentTime += Time.deltaTime;
            }
        }

        public override bool IsFinished
        {
            get { return false; }
        }
    }
}
