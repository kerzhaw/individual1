using Jackass.Backend.Models;

namespace Jackass.Backend.Rekognition
{
    internal static class Extensions
    {
        public static bool IsSizeOk(this ProcessedImage image)
        {
            // 5 Mb
            return image.ImageData.Length <= 5242880;
        }

        public static bool AreDimensionsOk(this ProcessedImage image)
        {
            return image.Width >= 320 && image.Height >= 240;
        }
    }
}