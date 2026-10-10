// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using NUnit.Framework;
using UnityEngine;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class VectorExtensionsTests
    {
        [Test]
        public void With_ReplacesX()
        {
            Vector3 original = new Vector3(1f, 2f, 3f);
            Vector3 result = original.With(x: 10f);
            Assert.AreEqual(new Vector3(10f, 2f, 3f), result);
        }

        [Test]
        public void With_ReplacesY()
        {
            Vector3 original = new Vector3(1f, 2f, 3f);
            Vector3 result = original.With(y: 20f);
            Assert.AreEqual(new Vector3(1f, 20f, 3f), result);
        }

        [Test]
        public void With_ReplacesZ()
        {
            Vector3 original = new Vector3(1f, 2f, 3f);
            Vector3 result = original.With(z: 30f);
            Assert.AreEqual(new Vector3(1f, 2f, 30f), result);
        }

        [Test]
        public void With_ReplacesMultiple()
        {
            Vector3 original = new Vector3(1f, 2f, 3f);
            Vector3 result = original.With(x: 10f, z: 30f);
            Assert.AreEqual(new Vector3(10f, 2f, 30f), result);
        }

        [Test]
        public void With_NoArgs_ReturnsSameValue()
        {
            Vector3 original = new Vector3(1f, 2f, 3f);
            Vector3 result = original.With();
            Assert.AreEqual(original, result);
        }

        [Test]
        public void Flat_ZerosY()
        {
            Vector3 original = new Vector3(5f, 10f, 15f);
            Vector3 result = original.Flat();
            Assert.AreEqual(new Vector3(5f, 0f, 15f), result);
        }

        [Test]
        public void Flat_AlreadyFlat_Unchanged()
        {
            Vector3 original = new Vector3(5f, 0f, 15f);
            Vector3 result = original.Flat();
            Assert.AreEqual(original, result);
        }

        [Test]
        public void DirectionTo_ReturnsNormalized()
        {
            Vector3 from = Vector3.zero;
            Vector3 to = new Vector3(3f, 0f, 4f);
            Vector3 direction = from.DirectionTo(to);

            Assert.That(direction.magnitude, Is.EqualTo(1f).Within(0.001f));
            Assert.That(direction.x, Is.EqualTo(0.6f).Within(0.001f));
            Assert.That(direction.z, Is.EqualTo(0.8f).Within(0.001f));
        }

        [Test]
        public void DirectionTo_SamePoint_ReturnsZero()
        {
            Vector3 point = new Vector3(5f, 5f, 5f);
            Vector3 direction = point.DirectionTo(point);
            Assert.AreEqual(Vector3.zero, direction);
        }

        [Test]
        public void Vector2_With_ReplacesX()
        {
            Vector2 original = new Vector2(1f, 2f);
            Vector2 result = original.With(x: 10f);
            Assert.AreEqual(new Vector2(10f, 2f), result);
        }

        [Test]
        public void Vector2_With_ReplacesY()
        {
            Vector2 original = new Vector2(1f, 2f);
            Vector2 result = original.With(y: 20f);
            Assert.AreEqual(new Vector2(1f, 20f), result);
        }
    }
}
