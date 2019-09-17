using System.IO;

namespace Jackass.Backend.ImageProcessing
{
    internal static class Extensions
    {
        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}