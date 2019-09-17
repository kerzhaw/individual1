using System;
using System.Linq;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Detectors
{
    internal sealed class PngImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Png;

        public bool Match(byte[] data)
        {
            const string pngSig = "89-50-4E-47-0D-0A-1A-0A";

            if (data.Length < pngSig.Length) return false;

            var header = BitConverter.ToString(data.Take(8).ToArray(), 0, 8);

            return header == pngSig;
        }
    }
}