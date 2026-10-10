// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using NUnit.Framework;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void IsBlank_Null_ReturnsTrue()
        {
            string value = null;
            Assert.IsTrue(value.IsBlank());
        }

        [Test]
        public void IsBlank_Empty_ReturnsTrue()
        {
            Assert.IsTrue("".IsBlank());
        }

        [Test]
        public void IsBlank_Whitespace_ReturnsTrue()
        {
            Assert.IsTrue("   ".IsBlank());
        }

        [Test]
        public void IsBlank_Content_ReturnsFalse()
        {
            Assert.IsFalse("hello".IsBlank());
        }

        [Test]
        public void OrEmpty_Null_ReturnsEmpty()
        {
            string value = null;
            Assert.AreEqual(string.Empty, value.OrEmpty());
        }

        [Test]
        public void OrEmpty_NonNull_ReturnsSame()
        {
            Assert.AreEqual("hello", "hello".OrEmpty());
        }

        [Test]
        public void Truncate_ShorterThanMax_ReturnsSame()
        {
            Assert.AreEqual("hi", "hi".Truncate(10));
        }

        [Test]
        public void Truncate_LongerThanMax_Truncates()
        {
            Assert.AreEqual("hel", "hello".Truncate(3));
        }

        [Test]
        public void Truncate_ExactLength_ReturnsSame()
        {
            Assert.AreEqual("hello", "hello".Truncate(5));
        }

        [Test]
        public void Slice_BasicSlice()
        {
            Assert.AreEqual("ell", "hello".Slice(1, 4));
        }

        [Test]
        public void Slice_NegativeEnd()
        {
            Assert.AreEqual("ell", "hello".Slice(1, -1));
        }

        [Test]
        public void Slice_FullString()
        {
            Assert.AreEqual("hello", "hello".Slice(0, 5));
        }

        [Test]
        public void ToAlphanumeric_RemovesSpecialChars()
        {
            Assert.AreEqual("hello123", "he!llo@#1$2%3".ToAlphanumeric());
        }

        [Test]
        public void ToAlphanumeric_SkipsLeadingDigit()
        {
            Assert.AreEqual("abc", "123abc".ToAlphanumeric());
        }

        [Test]
        public void ToAlphanumeric_AllowsPeriods()
        {
            Assert.AreEqual("hello.world", "hello.world!".ToAlphanumeric(true));
        }

        [Test]
        public void ToAlphanumeric_RemovesTrailingPeriods()
        {
            Assert.AreEqual("hello", "hello...".ToAlphanumeric(true));
        }

        [Test]
        public void ToAlphanumeric_Empty_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, "".ToAlphanumeric());
        }

        [Test]
        public void RichBold_WrapsCorrectly()
        {
            Assert.AreEqual("<b>hello</b>", "hello".RichBold());
        }

        [Test]
        public void RichItalic_WrapsCorrectly()
        {
            Assert.AreEqual("<i>hello</i>", "hello".RichItalic());
        }

        [Test]
        public void RichColor_WrapsCorrectly()
        {
            Assert.AreEqual("<color=red>hello</color>", "hello".RichColor("red"));
        }

        [Test]
        public void RichSize_WrapsCorrectly()
        {
            Assert.AreEqual("<size=24>hello</size>", "hello".RichSize(24));
        }
    }
}
