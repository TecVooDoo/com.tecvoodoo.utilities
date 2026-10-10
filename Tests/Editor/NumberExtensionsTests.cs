// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using NUnit.Framework;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class NumberExtensionsTests
    {
        [Test]
        public void Remap_MapsCorrectly()
        {
            float result = 5f.Remap(0f, 10f, 0f, 100f);
            Assert.That(result, Is.EqualTo(50f).Within(0.001f));
        }

        [Test]
        public void Remap_MinToMin()
        {
            float result = 0f.Remap(0f, 10f, 100f, 200f);
            Assert.That(result, Is.EqualTo(100f).Within(0.001f));
        }

        [Test]
        public void Remap_MaxToMax()
        {
            float result = 10f.Remap(0f, 10f, 100f, 200f);
            Assert.That(result, Is.EqualTo(200f).Within(0.001f));
        }

        [Test]
        public void Remap_InvertedRange()
        {
            float result = 0f.Remap(0f, 10f, 100f, 0f);
            Assert.That(result, Is.EqualTo(100f).Within(0.001f));
        }

        [Test]
        public void Remap_ZeroRange_ReturnsToMin()
        {
            float result = 5f.Remap(5f, 5f, 0f, 100f);
            Assert.That(result, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void Approximately_EqualValues_ReturnsTrue()
        {
            Assert.IsTrue(1.0f.Approximately(1.0f));
        }

        [Test]
        public void Approximately_DifferentValues_ReturnsFalse()
        {
            Assert.IsFalse(1.0f.Approximately(2.0f));
        }

        [Test]
        public void IsOdd_OddNumber_ReturnsTrue()
        {
            Assert.IsTrue(1.IsOdd());
            Assert.IsTrue(3.IsOdd());
            Assert.IsTrue(99.IsOdd());
            Assert.IsTrue((-1).IsOdd());
        }

        [Test]
        public void IsOdd_EvenNumber_ReturnsFalse()
        {
            Assert.IsFalse(0.IsOdd());
            Assert.IsFalse(2.IsOdd());
            Assert.IsFalse(100.IsOdd());
        }

        [Test]
        public void IsEven_EvenNumber_ReturnsTrue()
        {
            Assert.IsTrue(0.IsEven());
            Assert.IsTrue(2.IsEven());
            Assert.IsTrue(100.IsEven());
            Assert.IsTrue((-2).IsEven());
        }

        [Test]
        public void IsEven_OddNumber_ReturnsFalse()
        {
            Assert.IsFalse(1.IsEven());
            Assert.IsFalse(3.IsEven());
            Assert.IsFalse(99.IsEven());
        }
    }
}
