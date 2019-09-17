using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Converters
{
    public class GenericToPngImageConverter:IConverter
    {
        public ConvertedImage Convert(byte[] imageData)
        {
            var stream = new MemoryStream();
            var model = new ConvertedImage();

            var image = Image.FromStream(imageData.ToMemoryStream());

            model.Width = image.Width;
            model.Height = image.Height;

            image.Save(stream, ImageFormat.Png);

            model.ImageData = new byte[stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(model.ImageData, 0, model.ImageData.Length);
            stream.Dispose();

            return model;
        }
    }
}