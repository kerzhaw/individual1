using System;
using System.IO;
using System.Security.Cryptography;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services
{
    internal static class Extensions
    {
        public static string GetSha1Hash(this byte[] data)
        {
            using (var hasher = new SHA1CryptoServiceProvider())
            {
                var hashBytes = hasher.ComputeHash(data);
                return BitConverter.ToString(hashBytes);
            }
        }

        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static bool IsSizeOk(this ConvertedImage image)
        {
            // 5 Mb
            return image.ImageData.Length <= 5242880;
        }

        public static bool AreDimensionsOk(this ConvertedImage image)
        {
            return image.Width >= 320 && image.Height >= 240;
        }

    }
}