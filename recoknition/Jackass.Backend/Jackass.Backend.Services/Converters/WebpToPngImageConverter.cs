using System.IO;
using ImageMagick;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Converters
{
    public class WebpToPngImageConverter : IConverter
    {
        public ConvertedImage Convert(byte[] imageData)
        {
            var stream = new MemoryStream();
            var model = new ConvertedImage();

            var magickImage = new MagickImage(imageData)
            {
                Format = MagickFormat.Png
            };

            model.Width = magickImage.Width;
            model.Height = magickImage.Height;

            magickImage.Write(stream);

            model.ImageData = new byte[stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(model.ImageData, 0, model.ImageData.Length);
            stream.Dispose();

            return model;
        }
    }
}