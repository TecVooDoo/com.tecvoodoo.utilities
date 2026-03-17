// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Abstract base class for all timer types.
    /// Timers are non-MonoBehaviour objects that register with the TimerManager
    /// and tick via the PlayerLoop. Dispose when no longer needed.
    /// </summary>
    public abstract class Timer : IDisposable
    {
        /// <summary>
        /// The current elapsed or remaining time depending on timer type.
        /// </summary>
        public float CurrentTime { get; protected set; }

        /// <summary>
        /// Whether the timer is currently running and receiving ticks.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// The initial duration or value the timer was created with.
        /// </summary>
        protected float initialTime;

        /// <summary>
        /// Normalized progress from 0 to 1 based on CurrentTime / initialTime.
        /// </summary>
        public float Progress
        {
            get { return Mathf.Clamp01(CurrentTime / initialTime); }
        }

        /// <summary>
        /// Invoked when the timer starts via Start().
        /// </summary>
        public Action OnTimerStart = delegate { };

        /// <summary>
        /// Invoked when the timer stops via Stop() or when it finishes.
        /// </summary>
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            CurrentTime = value;
        }

        /// <summary>
        /// Starts the timer, resetting CurrentTime to initialTime and registering with TimerManager.
        /// </summary>
        public void Start()
        {
            CurrentTime = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                OnTimerStart.Invoke();
            }
        }

        /// <summary>
        /// Stops the timer and deregisters from TimerManager.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnTimerStop.Invoke();
            }
        }

        /// <summary>
        /// Called each frame by TimerManager. Subclasses implement the tick logic.
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Whether the timer has completed its cycle.
        /// </summary>
        public abstract bool IsFinished { get; }

        /// <summary>
        /// Resumes a paused timer without resetting CurrentTime.
        /// </summary>
        public void Resume()
        {
            IsRunning = true;
        }

        /// <summary>
        /// Pauses the timer without deregistering it from TimerManager.
        /// </summary>
        public void Pause()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Resets CurrentTime back to the original initialTime.
        /// </summary>
        public virtual void Reset()
        {
            CurrentTime = initialTime;
        }

        /// <summary>
        /// Resets the timer with a new duration.
        /// </summary>
        /// <param name="newTime">The new initial time.</param>
        public virtual void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }

        bool disposed;

        ~Timer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the timer, deregistering it from TimerManager.
        /// Call this when the owning object is destroyed.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                TimerManager.DeregisterTimer(this);
            }

            disposed = true;
        }
    }
}
