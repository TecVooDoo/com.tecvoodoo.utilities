// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using NUnit.Framework;
using TecVooDoo.Utilities;
using UnityEngine;

namespace TecVooDoo.Utilities.Tests
{
    [TestFixture]
    public class HeightmapSamplerTests
    {
        const int Size = 64;
        const float OneStep16 = 1f / 65535f;

        Texture2D _texture;

        [TearDown]
        public void TearDown()
        {
            if (_texture != null) Object.DestroyImmediate(_texture);
        }

        // Horizontal ramp: value = baseValue + x * step, identical on every row.
        static Texture2D CreateRamp(ushort baseValue, ushort step)
        {
            ushort[] pixels = new ushort[Size * Size];
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    pixels[y * Size + x] = (ushort)(baseValue + x * step);
                }
            }

            Texture2D texture = new Texture2D(Size, Size, TextureFormat.R16, false, true);
            texture.SetPixelData(pixels, 0);
            texture.Apply(false);
            return texture;
        }

        [Test]
        public void Sample_SameResolution_ReproducesTexels()
        {
            _texture = CreateRamp(0, 1000);

            float[,] heights = HeightmapSampler.Sample(_texture, Size);

            for (int x = 0; x < Size; x++)
            {
                Assert.AreEqual(x * 1000f / 65535f, heights[10, x], 2f * OneStep16, $"x={x}");
            }
        }

        [Test]
        public void Sample_KeepsSixteenBitPrecision()
        {
            // A step of 7 is far below one 8-bit level (257); an 8-bit decode would flatten it to 0.
            _texture = CreateRamp(30000, 7);

            float[,] heights = HeightmapSampler.Sample(_texture, Size);

            for (int x = 1; x < Size; x++)
            {
                Assert.AreEqual(7f * OneStep16, heights[5, x] - heights[5, x - 1], OneStep16, $"x={x}");
            }
        }

        [Test]
        public void Sample_UpsampleByOne_PinsCornersToSourceCorners()
        {
            // The 4096 -> 4097 case from ENTRY-437, in miniature.
            _texture = CreateRamp(0, 1000);

            float[,] heights = HeightmapSampler.Sample(_texture, Size + 1);

            Assert.AreEqual(0f, heights[0, 0], 2f * OneStep16);
            Assert.AreEqual((Size - 1) * 1000f / 65535f, heights[0, Size], 2f * OneStep16);
            Assert.AreEqual((Size - 1) * 1000f / 65535f, heights[Size, Size], 2f * OneStep16);
        }

        [Test]
        public void Sample_UpsampleByOne_IsMonotonicAlongRamp()
        {
            _texture = CreateRamp(0, 1000);

            float[,] heights = HeightmapSampler.Sample(_texture, Size + 1);

            for (int x = 1; x <= Size; x++)
            {
                Assert.Greater(heights[Size / 2, x], heights[Size / 2, x - 1], $"x={x}");
            }
        }

        [Test]
        public void Sample_RestoresSourceSamplerState()
        {
            _texture = CreateRamp(0, 1000);
            _texture.wrapMode = TextureWrapMode.Repeat;
            _texture.filterMode = FilterMode.Point;

            HeightmapSampler.Sample(_texture, Size);

            Assert.AreEqual(TextureWrapMode.Repeat, _texture.wrapMode);
            Assert.AreEqual(FilterMode.Point, _texture.filterMode);
        }

        [Test]
        public void Sample_NullSource_Throws()
        {
            Assert.Throws<System.ArgumentNullException>(() => HeightmapSampler.Sample(null, Size));
        }
    }
}
