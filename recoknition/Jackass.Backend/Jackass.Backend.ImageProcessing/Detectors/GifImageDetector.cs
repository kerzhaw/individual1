using System;
using System.Linq;

namespace Jackass.Backend.ImageProcessing.Detectors
{
    internal sealed class GifImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Gif;

        public bool Match(byte[] data)
        {
            const string gif87ASig = "47-49-46-38-37-61";
            const string gif89ASig = "47-49-46-38-39-61";

            if (data.Length < 6) return false;

            var header = BitConverter.ToString(data.Take(6).ToArray(), 0, 6);
            return header == gif87ASig || header == gif89ASig;
        }
    }
}