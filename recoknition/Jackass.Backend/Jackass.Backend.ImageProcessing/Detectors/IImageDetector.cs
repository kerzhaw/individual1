namespace Jackass.Backend.ImageProcessing.Detectors
{
    internal interface IImageDetector
    {
        ImageType ImageType { get; }
        bool Match(byte[] data);
    }
}