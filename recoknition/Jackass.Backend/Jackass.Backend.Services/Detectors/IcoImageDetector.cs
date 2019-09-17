using System;
using System.Linq;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Detectors
{
    internal sealed class IcoImageDetector : IImageDetector
    {
        public ImageType ImageType => ImageType.Ico;

        public bool Match(byte[] data)
        {
            const string sig = "00-00-01-00";
            if (data.Length < 4) return false;
            var header = BitConverter.ToString(data.Take(4).ToArray(), 0);
            return header == sig;
        }
    }
}