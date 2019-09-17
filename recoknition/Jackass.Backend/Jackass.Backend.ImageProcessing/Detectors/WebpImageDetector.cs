using System;
using System.Linq;

namespace Jackass.Backend.ImageProcessing.Detectors
{
    internal sealed class WebpImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Webp;

        public bool Match(byte[] data)
        {
            const string webpSigPrefix = "52-49-46-46";
            const string webpSigSuffix = "57-45-42-50";

            if (data.Length < 12) return false;

            var header = BitConverter.ToString(data.Take(12).ToArray(), 0, 12);

            return header.StartsWith(webpSigPrefix, StringComparison.OrdinalIgnoreCase) && header.EndsWith(webpSigSuffix, StringComparison.OrdinalIgnoreCase);
        }
    }
}