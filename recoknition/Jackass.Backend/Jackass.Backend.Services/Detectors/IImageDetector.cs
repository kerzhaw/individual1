using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Detectors
{
    internal interface IImageDetector
    {
        ImageType ImageType { get; }
        bool Match(byte[] data);
    }
}