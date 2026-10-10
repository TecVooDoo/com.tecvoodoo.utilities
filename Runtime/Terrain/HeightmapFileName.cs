// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Globalization;
using System.Text.RegularExpressions;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Reads the real-world scale that heightmap packs encode in their filenames.
    /// </summary>
    /// <remarks>
    /// Convention: <c>USA_24_WL_8192_Height_763.png</c> = world length 8192 m, height range 763 m.
    /// <c>Height_N</c> is the range (max - min), not altitude above sea level.
    /// </remarks>
    public static class HeightmapFileName
    {
        static readonly Regex WorldLengthPattern = new Regex(@"(?:^|[_\-\s])WL_(\d+(?:\.\d+)?)", RegexOptions.IgnoreCase);
        static readonly Regex HeightRangePattern = new Regex(@"(?:^|[_\-\s])Height_(\d+(?:\.\d+)?)", RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses world length and height range (both in metres) from a heightmap filename or path.
        /// </summary>
        /// <param name="fileName">Filename or full path; directories are ignored.</param>
        /// <param name="worldLength">Edge length of the square tile in metres.</param>
        /// <param name="heightRange">Vertical range of the tile in metres.</param>
        /// <returns>True if both values were found and are positive.</returns>
        public static bool TryParse(string fileName, out float worldLength, out float heightRange)
        {
            worldLength = 0f;
            heightRange = 0f;
            if (string.IsNullOrEmpty(fileName)) return false;

            // GetFileName, not GetFileNameWithoutExtension: an extensionless "Height_12.25" would lose ".25".
            // The extension never matches the patterns, so it is safe to keep.
            string name = System.IO.Path.GetFileName(fileName);
            return TryMatch(WorldLengthPattern, name, out worldLength)
                && TryMatch(HeightRangePattern, name, out heightRange);
        }

        static bool TryMatch(Regex pattern, string name, out float value)
        {
            value = 0f;
            Match match = pattern.Match(name);
            if (!match.Success) return false;

            return float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                && value > 0f;
        }
    }
}
