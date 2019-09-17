using Jackass.Backend.Models;

namespace Jackass.Backend.Services.Converters
{
    public interface IConverter
    {
        ConvertedImage Convert(byte[] imageData);
    }
}