// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

#if TVU_TERRAIN_MODULE
using NUnit.Framework;
using TecVooDoo.Utilities;
using UnityEngine;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class HeightmapTerrainTests
    {
        const int Size = 64;

        Texture2D _texture;
        TerrainData _data;

        [SetUp]
        public void SetUp()
        {
            // Horizontal ramp from 0 to full scale.
            ushort[] pixels = new ushort[Size * Size];
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    pixels[y * Size + x] = (ushort)(x * 65535 / (Size - 1));
                }
            }

            _texture = new Texture2D(Size, Size, TextureFormat.R16, false, true);
            _texture.SetPixelData(pixels, 0);
            _texture.Apply(false);
            _data = new TerrainData();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_texture);
            Object.DestroyImmediate(_data);
        }

        [TestCase(10, 33)]
        [TestCase(33, 33)]
        [TestCase(100, 129)]
        [TestCase(4096, 4097)]
        [TestCase(4097, 4097)]
        [TestCase(8192, 4097)]
        public void ResolutionFor_ReturnsCoveringTerrainResolution(int textureSize, int expected)
        {
            Assert.AreEqual(expected, HeightmapTerrain.ResolutionFor(textureSize));
        }

        [Test]
        public void Apply_SetsResolutionAndSize()
        {
            HeightmapTerrain.Apply(_data, _texture, 1000f, 250f);

            Assert.AreEqual(65, _data.heightmapResolution);
            Assert.AreEqual(new Vector3(1000f, 250f, 1000f), _data.size);
        }

        [Test]
        public void Apply_MapsFullScaleToHeightRange()
        {
            HeightmapTerrain.Apply(_data, _texture, 1000f, 250f);

            Assert.AreEqual(0f, _data.GetHeight(0, 32), 0.05f);
            Assert.AreEqual(250f, _data.GetHeight(64, 32), 0.05f);
        }

        [Test]
        public void TryApply_ScaleFromFileName()
        {
            Assert.IsTrue(HeightmapTerrain.TryApply(_data, _texture, "USA_24_WL_8192_Height_763.png"));

            Assert.AreEqual(new Vector3(8192f, 763f, 8192f), _data.size);
        }

        [Test]
        public void TryApply_NoScaleInFileName_LeavesDataUntouched()
        {
            Vector3 before = _data.size;
            int resolutionBefore = _data.heightmapResolution;

            Assert.IsFalse(HeightmapTerrain.TryApply(_data, _texture, "heightmap.png"));

            Assert.AreEqual(before, _data.size);
            Assert.AreEqual(resolutionBefore, _data.heightmapResolution);
        }
    }
}
#endif
