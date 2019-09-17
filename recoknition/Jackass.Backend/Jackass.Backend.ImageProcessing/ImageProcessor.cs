using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ImageMagick;
using Jackass.Backend.ImageProcessing.Detectors;
using Jackass.Backend.Models;

namespace Jackass.Backend.ImageProcessing
{
    public sealed class ImageProcessor
    {

        public static ProcessedImage GetImageAsPng(byte[] imageData)
        {
            var imageType = GetImageType(imageData);
            var stream = new MemoryStream();
            var processedImage = new ProcessedImage();

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (imageType)
            {
                case ImageType.Unknown:
                    throw new InvalidDataException("Unknown image type");

                case ImageType.Webp:
                    var magickImage = new MagickImage(imageData) { Format = MagickFormat.Png };

                    processedImage.Width = magickImage.Width;
                    processedImage.Height = magickImage.Height;

                    magickImage.Write(stream);
                    break;

                default:
                    var image = Image.FromStream(imageData.ToMemoryStream());

                    processedImage.Width = image.Width;
                    processedImage.Height = image.Height;

                    image.Save(stream, ImageFormat.Png);
                    break;
            }


            stream.Seek(0, SeekOrigin.Begin);

            processedImage.ImageData = new byte[stream.Length];
            stream.Read(processedImage.ImageData, 0, processedImage.ImageData.Length);
            stream.Dispose();

            return processedImage;
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

        private static string Gethash(byte[] data)
        {
            using (var hasher = new SHA1CryptoServiceProvider())
            {
                var hashBytes = hasher.ComputeHash(data);
                return BitConverter.ToString(hashBytes);
            }
        }
    }
}