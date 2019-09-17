using System;
using System.Linq;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Detectors
{
    internal sealed class JpgImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Jpg;

        public bool Match(byte[] data)
        {
            const string sig1 = "FF-D8-FF-DB";

            const string sig2Prefix = "FF-D8-FF-E0";
            const string sig2Suffix = "4A-46-49-46-00-01";

            const string sig3Prefix = "FF-D8-FF-E1";
            const string sig3Suffix = "45-78-69-66-00-00";

            if (data.Length < 12) return false;

            var headerBytes = data.Take(12).ToArray();
            var header = BitConverter.ToString(headerBytes, 0, headerBytes.Length);

            if (header.StartsWith(sig1,StringComparison.OrdinalIgnoreCase)) return true;
            if (header.StartsWith(sig2Prefix, StringComparison.OrdinalIgnoreCase) && header.EndsWith(sig2Suffix, StringComparison.OrdinalIgnoreCase)) return true;
            if (header.StartsWith(sig3Prefix, StringComparison.OrdinalIgnoreCase) && header.EndsWith(sig3Suffix, StringComparison.OrdinalIgnoreCase)) return true;

            return false;

        }
    }
}