using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Jackass.Backend.Models;
using Jackass.Backend.Services.Converters;
using Jackass.Backend.Services.Detectors;

namespace Jackass.Backend.Services
{
    public class ConverterService
    {
        public ConvertedImage GetRekognitionCompatibleImage(byte[] originalImageData)
        {
            var imageType = GetImageType(originalImageData);

            if (imageType == ImageType.Unknown)
                throw new InvalidDataException("Unknown image type");

            if (imageType == ImageType.Png || imageType == ImageType.Jpg)
            {
                var stream = originalImageData.ToMemoryStream();
                var image = Image.FromStream(stream);

                return new ConvertedImage
                {
                    ImageData = originalImageData,
                    Width = image.Width,
                    Height = image.Height
                };

            }

            IConverter converter;

            switch (imageType)
            {
                case ImageType.Webp:
                    converter = new WebpToPngImageConverter();
                    break;
                default:
                    converter = new GenericToPngImageConverter();
                    break;
            }

            return converter.Convert(originalImageData);

        }

        private static ImageType GetImageType(byte[] imageData)
        {
            var detectors = new List<IImageDetector>
            {
                new WebpImageDetector(),
                new GifImageDetector(),
                new TifImageDetector(),
                new JpgImageDetector(),
                new PngImageDetector(),
                new BmpImageDetector(),
                new IcoImageDetector()
            };

            return (
                from detector in detectors
                where detector.Match(imageData)
                select detector.ImageType
            ).FirstOrDefault();
        }

    }
}