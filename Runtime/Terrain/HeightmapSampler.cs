// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Resamples a heightmap texture into a normalized height grid at full precision via a GPU blit.
    /// </summary>
    /// <remarks>
    /// Never decode a 16-bit PNG with <c>Texture2D.LoadImage</c>: it returns ARGB32 and silently drops
    /// to 256 levels (~3 m terraces on a 763 m tile, measured in Sandbox ENTRY-437). Import the PNG as a
    /// texture with format R16, sRGB off and Non-Power of 2 = None, then pass it here. Read/Write is not needed.
    /// </remarks>
    public static class HeightmapSampler
    {
        /// <summary>
        /// Samples <paramref name="source"/> bilinearly onto a square grid, indexed [y, x] as TerrainData.SetHeights expects.
        /// </summary>
        /// <param name="source">Single-channel heightmap texture; the red channel is read as height.</param>
        /// <param name="resolution">Grid edge length in samples (e.g. 4097). Corner samples land on the source's corner texels.</param>
        /// <returns>Heights in [0, 1].</returns>
        public static float[,] Sample(Texture source, int resolution)
        {
            if (source == null) throw new System.ArgumentNullException(nameof(source));
            if (resolution < 2) throw new System.ArgumentOutOfRangeException(nameof(resolution), resolution, "Resolution must be at least 2.");
            if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat))
                throw new System.NotSupportedException("RFloat render textures are not supported on this device.");

            RenderTexture target = RenderTexture.GetTemporary(resolution, resolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
            target.filterMode = FilterMode.Point;
            RenderTexture previousActive = RenderTexture.active;
            TextureWrapMode previousWrap = source.wrapMode;
            FilterMode previousFilter = source.filterMode;
            Texture2D readback = null;

            try
            {
                // Clamp keeps the edge rows from blending with the opposite edge; bilinear does the resample.
                source.wrapMode = TextureWrapMode.Clamp;
                source.filterMode = FilterMode.Bilinear;
                Vector2 scale = new Vector2(Scale(source.width, resolution), Scale(source.height, resolution));
                Vector2 offset = new Vector2(Offset(source.width, resolution), Offset(source.height, resolution));
                Graphics.Blit(source, target, scale, offset);

                RenderTexture.active = target;
                readback = new Texture2D(resolution, resolution, TextureFormat.RFloat, false, true);
                readback.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0, false);

                return ToGrid(readback.GetPixelData<float>(0), resolution);
            }
            finally
            {
                RenderTexture.active = previousActive;
                source.wrapMode = previousWrap;
                source.filterMode = previousFilter;
                RenderTexture.ReleaseTemporary(target);
                if (readback != null) DestroyObject(readback);
            }
        }

        // Maps destination texel centres so sample 0 hits source texel 0 and sample (resolution - 1)
        // hits the last source texel, instead of Blit's default edge-to-edge stretch.
        static float Scale(int sourceSize, int resolution)
        {
            return (sourceSize - 1f) / sourceSize * resolution / (resolution - 1f);
        }

        static float Offset(int sourceSize, int resolution)
        {
            return 0.5f / sourceSize - 0.5f * Scale(sourceSize, resolution) / resolution;
        }

        static float[,] ToGrid(Unity.Collections.NativeArray<float> pixels, int resolution)
        {
            float[,] heights = new float[resolution, resolution];
            for (int y = 0; y < resolution; y++)
            {
                int row = y * resolution;
                for (int x = 0; x < resolution; x++)
                {
                    heights[y, x] = Mathf.Clamp01(pixels[row + x]);
                }
            }
            return heights;
        }

        static void DestroyObject(Object obj)
        {
            if (Application.isPlaying) Object.Destroy(obj);
            else Object.DestroyImmediate(obj);
        }
    }
}
