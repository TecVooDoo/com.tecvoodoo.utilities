// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

// TerrainData lives in the com.unity.modules.terrain built-in module, which some fleet projects
// (BloodMiner, HideNReap) disable; the define comes from the asmdef's versionDefines.
#if TVU_TERRAIN_MODULE
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Writes a heightmap texture into a TerrainData at a given real-world scale.
    /// </summary>
    public static class HeightmapTerrain
    {
        /// <summary>Smallest heightmap resolution Unity terrain accepts.</summary>
        public const int MinResolution = 33;

        /// <summary>Largest heightmap resolution Unity terrain accepts.</summary>
        public const int MaxResolution = 4097;

        /// <summary>
        /// Returns the terrain heightmap resolution (2^n + 1, clamped to Unity's range) that covers a texture edge.
        /// </summary>
        /// <param name="textureSize">Texture edge length in texels; 4096 and 4097 both map to 4097.</param>
        /// <returns>A valid TerrainData.heightmapResolution.</returns>
        public static int ResolutionFor(int textureSize)
        {
            int resolution = MinResolution;
            while (resolution < textureSize && resolution < MaxResolution)
            {
                resolution = (resolution - 1) * 2 + 1;
            }
            return resolution;
        }

        /// <summary>
        /// Resizes <paramref name="data"/> to (worldLength, heightRange, worldLength) and fills it from <paramref name="heightmap"/>.
        /// </summary>
        /// <param name="data">Target terrain data; its heightmap resolution is set from the texture size.</param>
        /// <param name="heightmap">Single-channel heightmap texture (see <see cref="HeightmapSampler"/> for import settings).</param>
        /// <param name="worldLength">Edge length of the square tile in metres.</param>
        /// <param name="heightRange">Vertical range of the tile in metres.</param>
        public static void Apply(TerrainData data, Texture heightmap, float worldLength, float heightRange)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            if (heightmap == null) throw new System.ArgumentNullException(nameof(heightmap));
            if (worldLength <= 0f) throw new System.ArgumentOutOfRangeException(nameof(worldLength), worldLength, "World length must be positive.");
            if (heightRange <= 0f) throw new System.ArgumentOutOfRangeException(nameof(heightRange), heightRange, "Height range must be positive.");

            int resolution = ResolutionFor(Mathf.Max(heightmap.width, heightmap.height));
            float[,] heights = HeightmapSampler.Sample(heightmap, resolution);

            // Resolution first: changing heightmapResolution rescales size, so size must be set after it.
            data.heightmapResolution = resolution;
            data.size = new Vector3(worldLength, heightRange, worldLength);
            data.SetHeights(0, 0, heights);
        }

        /// <summary>
        /// As <see cref="Apply(TerrainData, Texture, float, float)"/>, reading the scale from the heightmap's filename.
        /// </summary>
        /// <param name="data">Target terrain data.</param>
        /// <param name="heightmap">Heightmap texture.</param>
        /// <param name="fileName">Filename carrying <c>WL_</c> and <c>Height_</c> (see <see cref="HeightmapFileName"/>).</param>
        /// <returns>False, leaving <paramref name="data"/> untouched, if the filename carries no scale.</returns>
        public static bool TryApply(TerrainData data, Texture heightmap, string fileName)
        {
            if (!HeightmapFileName.TryParse(fileName, out float worldLength, out float heightRange)) return false;

            Apply(data, heightmap, worldLength, heightRange);
            return true;
        }
    }
}
#endif
