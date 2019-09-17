using System;
using System.Linq;

namespace Jackass.Backend.ImageProcessing.Detectors
{
    internal sealed class TifImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Tif;

        public bool Match(byte[] data)
        {
            const string tifLittleEndianSig = "49-49-2A-00";
            const string tifBigEndianSig = "4D-4D-00-2A";

            if (data.Length < 4) return false;
            var header = BitConverter.ToString(data.Take(4).ToArray(), 0, 4);
            return header == tifLittleEndianSig || header == tifBigEndianSig;
        }
    }
}