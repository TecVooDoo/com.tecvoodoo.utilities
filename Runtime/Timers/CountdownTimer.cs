// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Timer that counts down from a specified duration to zero, then stops.
    /// </summary>
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float duration) : base(duration) { }

        public override void Tick()
        {
            if (IsRunning && CurrentTime > 0f)
            {
                CurrentTime -= Time.deltaTime;
            }

            if (IsRunning && CurrentTime <= 0f)
            {
                Stop();
            }
        }

        public override bool IsFinished
        {
            get { return CurrentTime <= 0f; }
        }
    }
}
