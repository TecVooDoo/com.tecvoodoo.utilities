// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;
using NUnit.Framework;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void IsNullOrEmpty_NullCollection_ReturnsTrue()
        {
            List<int> list = null;
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_EmptyCollection_ReturnsTrue()
        {
            List<int> list = new List<int>();
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_NonEmptyCollection_ReturnsFalse()
        {
            List<int> list = new List<int> { 1, 2, 3 };
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [Test]
        public void Shuffle_PreservesAllElements()
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };
            list.Shuffle();

            Assert.AreEqual(5, list.Count);
            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(2));
            Assert.IsTrue(list.Contains(3));
            Assert.IsTrue(list.Contains(4));
            Assert.IsTrue(list.Contains(5));
        }

        [Test]
        public void Shuffle_SingleElement_NoError()
        {
            List<int> list = new List<int> { 42 };
            list.Shuffle();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(42, list[0]);
        }

        [Test]
        public void RandomElement_ReturnsElementFromList()
        {
            List<int> list = new List<int> { 10, 20, 30 };
            int element = list.RandomElement();
            Assert.IsTrue(list.Contains(element));
        }

        [Test]
        public void ForEach_AppliesActionToAllElements()
        {
            List<int> source = new List<int> { 1, 2, 3 };
            List<int> results = new List<int>();

            source.ForEach(item => results.Add(item * 2));

            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(2, results[0]);
            Assert.AreEqual(4, results[1]);
            Assert.AreEqual(6, results[2]);
        }
    }

    [TestFixture]
    public class ListExtensionsTests
    {
        [Test]
        public void ListIsNullOrEmpty_NullList_ReturnsTrue()
        {
            List<int> list = null;
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void ListIsNullOrEmpty_EmptyList_ReturnsTrue()
        {
            List<int> list = new List<int>();
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void ListIsNullOrEmpty_NonEmpty_ReturnsFalse()
        {
            List<int> list = new List<int> { 1 };
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [Test]
        public void Swap_SwapsElements()
        {
            List<int> list = new List<int> { 10, 20, 30 };
            list.Swap(0, 2);

            Assert.AreEqual(30, list[0]);
            Assert.AreEqual(20, list[1]);
            Assert.AreEqual(10, list[2]);
        }

        [Test]
        public void RefreshWith_ReplacesContents()
        {
            List<int> target = new List<int> { 1, 2, 3 };
            List<int> source = new List<int> { 10, 20 };

            target.RefreshWith(source);

            Assert.AreEqual(2, target.Count);
            Assert.AreEqual(10, target[0]);
            Assert.AreEqual(20, target[1]);
        }

        [Test]
        public void RefreshWith_EmptySource_ClearsList()
        {
            List<int> target = new List<int> { 1, 2, 3 };
            List<int> source = new List<int>();

            target.RefreshWith(source);

            Assert.AreEqual(0, target.Count);
        }
    }
}
