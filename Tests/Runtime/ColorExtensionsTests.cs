// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using NUnit.Framework;
using UnityEngine;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class ColorExtensionsTests
    {
        [Test]
        public void WithAlpha_ChangesAlpha()
        {
            Color color = Color.red;
            Color result = color.WithAlpha(0.5f);

            Assert.That(result.r, Is.EqualTo(1f).Within(0.001f));
            Assert.That(result.g, Is.EqualTo(0f).Within(0.001f));
            Assert.That(result.b, Is.EqualTo(0f).Within(0.001f));
            Assert.That(result.a, Is.EqualTo(0.5f).Within(0.001f));
        }

        [Test]
        public void Add_ClampsToOne()
        {
            Color result = Color.red.Add(Color.red);
            Assert.That(result.r, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void Add_CombinesChannels()
        {
            Color result = Color.red.Add(Color.blue);
            Assert.That(result.r, Is.EqualTo(1f).Within(0.001f));
            Assert.That(result.b, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void Subtract_ClampsToZero()
        {
            Color result = Color.black.Subtract(Color.red);
            Assert.That(result.r, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void Blend_HalfwayBlend()
        {
            Color result = Color.red.Blend(Color.blue, 0.5f);
            Assert.That(result.r, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(result.b, Is.EqualTo(0.5f).Within(0.001f));
        }

        [Test]
        public void Blend_ZeroRatio_ReturnsFirst()
        {
            Color result = Color.red.Blend(Color.blue, 0f);
            Assert.That(result.r, Is.EqualTo(1f).Within(0.001f));
            Assert.That(result.b, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void Blend_OneRatio_ReturnsSecond()
        {
            Color result = Color.red.Blend(Color.blue, 1f);
            Assert.That(result.r, Is.EqualTo(0f).Within(0.001f));
            Assert.That(result.b, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void Invert_InvertsRGB_PreservesAlpha()
        {
            Color color = new Color(0.2f, 0.3f, 0.4f, 0.8f);
            Color result = color.Invert();

            Assert.That(result.r, Is.EqualTo(0.8f).Within(0.001f));
            Assert.That(result.g, Is.EqualTo(0.7f).Within(0.001f));
            Assert.That(result.b, Is.EqualTo(0.6f).Within(0.001f));
            Assert.That(result.a, Is.EqualTo(0.8f).Within(0.001f));
        }

        [Test]
        public void ToHex_ReturnsCorrectFormat()
        {
            string hex = Color.red.ToHex();
            Assert.AreEqual("#FF0000FF", hex);
        }

        [Test]
        public void FromHex_ParsesValid()
        {
            Color result = ColorExtensions.FromHex("#FF0000FF");
            Assert.That(result.r, Is.EqualTo(1f).Within(0.01f));
            Assert.That(result.g, Is.EqualTo(0f).Within(0.01f));
            Assert.That(result.b, Is.EqualTo(0f).Within(0.01f));
        }

        [Test]
        public void FromHex_InvalidThrows()
        {
            Assert.Throws<ArgumentException>(() => ColorExtensions.FromHex("not_a_color"));
        }
    }
}
