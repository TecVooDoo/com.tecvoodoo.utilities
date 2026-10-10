// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using NUnit.Framework;
using TecVooDoo.Utilities;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class HeightmapFileNameTests
    {
        [Test]
        public void TryParse_PackFileName_ReadsBothValues()
        {
            Assert.IsTrue(HeightmapFileName.TryParse("USA_24_WL_8192_Height_763.png", out float worldLength, out float heightRange));
            Assert.AreEqual(8192f, worldLength);
            Assert.AreEqual(763f, heightRange);
        }

        [Test]
        public void TryParse_FullPath_IgnoresDirectories()
        {
            Assert.IsTrue(HeightmapFileName.TryParse("Assets/USA Real-World Heightmaps - Vol.1/California/USA_24_WL_8192_Height_763.png", out float worldLength, out float heightRange));
            Assert.AreEqual(8192f, worldLength);
            Assert.AreEqual(763f, heightRange);
        }

        [Test]
        public void TryParse_DecimalValues_UseInvariantCulture()
        {
            Assert.IsTrue(HeightmapFileName.TryParse("Tile_WL_1024.5_Height_12.25", out float worldLength, out float heightRange));
            Assert.AreEqual(1024.5f, worldLength);
            Assert.AreEqual(12.25f, heightRange);
        }

        [Test]
        public void TryParse_DecimalWithExtension_ReadsBothValues()
        {
            Assert.IsTrue(HeightmapFileName.TryParse("Tile_WL_1024.5_Height_12.25.png", out float worldLength, out float heightRange));
            Assert.AreEqual(1024.5f, worldLength);
            Assert.AreEqual(12.25f, heightRange);
        }

        [Test]
        public void TryParse_MissingHeight_ReturnsFalse()
        {
            Assert.IsFalse(HeightmapFileName.TryParse("USA_24_WL_8192.png", out _, out _));
        }

        [Test]
        public void TryParse_MissingWorldLength_ReturnsFalse()
        {
            Assert.IsFalse(HeightmapFileName.TryParse("USA_24_Height_763.png", out _, out _));
        }

        [Test]
        public void TryParse_WlInsideAnotherToken_IsNotMatched()
        {
            // "BOWL_500" must not read as WL_500.
            Assert.IsFalse(HeightmapFileName.TryParse("BOWL_500_Height_20.png", out _, out _));
        }

        [Test]
        public void TryParse_ZeroValue_ReturnsFalse()
        {
            Assert.IsFalse(HeightmapFileName.TryParse("WL_0_Height_763.png", out _, out _));
        }

        [Test]
        public void TryParse_NullOrEmpty_ReturnsFalse()
        {
            Assert.IsFalse(HeightmapFileName.TryParse(null, out _, out _));
            Assert.IsFalse(HeightmapFileName.TryParse("", out _, out _));
        }
    }
}
