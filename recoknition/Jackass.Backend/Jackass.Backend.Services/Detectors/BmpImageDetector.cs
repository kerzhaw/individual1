using System;
using System.Linq;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Detectors
{
    internal sealed class BmpImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Bmp;

        public bool Match(byte[] data)
        {
            const string bmpSig = "42-4D";

            if (data.Length < 2) return false;

            var header = BitConverter.ToString(data.Take(2).ToArray(), 0, 2);

            return header == bmpSig;
        }
    }
}