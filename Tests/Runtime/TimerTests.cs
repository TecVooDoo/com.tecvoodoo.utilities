// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using NUnit.Framework;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class CountdownTimerTests
    {
        [Test]
        public void Constructor_SetsInitialTime()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();

            Assert.That(timer.CurrentTime, Is.EqualTo(5f).Within(0.001f));
            Assert.IsTrue(timer.IsRunning);

            timer.Dispose();
        }

        [Test]
        public void Stop_SetsIsRunningFalse()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();
            timer.Stop();

            Assert.IsFalse(timer.IsRunning);

            timer.Dispose();
        }

        [Test]
        public void Pause_Resume_PreservesTime()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();
            timer.Pause();

            Assert.IsFalse(timer.IsRunning);
            Assert.That(timer.CurrentTime, Is.EqualTo(5f).Within(0.001f));

            timer.Resume();
            Assert.IsTrue(timer.IsRunning);

            timer.Dispose();
        }

        [Test]
        public void Reset_RestoresInitialTime()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();
            timer.Reset();

            Assert.That(timer.CurrentTime, Is.EqualTo(5f).Within(0.001f));

            timer.Dispose();
        }

        [Test]
        public void Reset_WithNewTime_ChangesInitialTime()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();
            timer.Reset(10f);

            Assert.That(timer.CurrentTime, Is.EqualTo(10f).Within(0.001f));

            timer.Dispose();
        }

        [Test]
        public void IsFinished_InitiallyFalse()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            Assert.IsFalse(timer.IsFinished);

            timer.Dispose();
        }

        [Test]
        public void OnTimerStart_FiresOnStart()
        {
            bool fired = false;
            CountdownTimer timer = new CountdownTimer(5f);
            timer.OnTimerStart += () => fired = true;
            timer.Start();

            Assert.IsTrue(fired);

            timer.Dispose();
        }

        [Test]
        public void OnTimerStop_FiresOnStop()
        {
            bool fired = false;
            CountdownTimer timer = new CountdownTimer(5f);
            timer.OnTimerStop += () => fired = true;
            timer.Start();
            timer.Stop();

            Assert.IsTrue(fired);

            timer.Dispose();
        }

        [Test]
        public void Progress_InitiallyOne()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();

            Assert.That(timer.Progress, Is.EqualTo(1f).Within(0.001f));

            timer.Dispose();
        }

        [Test]
        public void Dispose_DeregistersTimer()
        {
            CountdownTimer timer = new CountdownTimer(5f);
            timer.Start();
            Assert.IsTrue(timer.IsRunning);

            timer.Dispose();
            // After dispose, calling Dispose again should not throw
            timer.Dispose();
        }
    }

    [TestFixture]
    public class StopwatchTimerTests
    {
        [Test]
        public void Constructor_StartsAtZero()
        {
            StopwatchTimer timer = new StopwatchTimer();
            Assert.That(timer.CurrentTime, Is.EqualTo(0f).Within(0.001f));

            timer.Dispose();
        }

        [Test]
        public void IsFinished_AlwaysFalse()
        {
            StopwatchTimer timer = new StopwatchTimer();
            Assert.IsFalse(timer.IsFinished);

            timer.Start();
            Assert.IsFalse(timer.IsFinished);

            timer.Dispose();
        }

        [Test]
        public void Start_SetsRunning()
        {
            StopwatchTimer timer = new StopwatchTimer();
            timer.Start();

            Assert.IsTrue(timer.IsRunning);

            timer.Dispose();
        }
    }

    [TestFixture]
    public class FrequencyTimerTests
    {
        [Test]
        public void Constructor_SetsTickRate()
        {
            FrequencyTimer timer = new FrequencyTimer(10);
            Assert.AreEqual(10, timer.TicksPerSecond);

            timer.Dispose();
        }

        [Test]
        public void IsFinished_WhenNotRunning()
        {
            FrequencyTimer timer = new FrequencyTimer(10);
            Assert.IsTrue(timer.IsFinished);

            timer.Dispose();
        }

        [Test]
        public void Reset_WithNewTickRate()
        {
            FrequencyTimer timer = new FrequencyTimer(10);
            timer.Reset(20);

            Assert.AreEqual(20, timer.TicksPerSecond);
            Assert.That(timer.CurrentTime, Is.EqualTo(0f).Within(0.001f));

            timer.Dispose();
        }
    }

    [TestFixture]
    public class IntervalTimerTests
    {
        [Test]
        public void Constructor_SetsInitialTime()
        {
            IntervalTimer timer = new IntervalTimer(10f, 2f);
            timer.Start();

            Assert.That(timer.CurrentTime, Is.EqualTo(10f).Within(0.001f));

            timer.Dispose();
        }

        [Test]
        public void IsFinished_InitiallyFalse()
        {
            IntervalTimer timer = new IntervalTimer(10f, 2f);
            Assert.IsFalse(timer.IsFinished);

            timer.Dispose();
        }
    }
}
