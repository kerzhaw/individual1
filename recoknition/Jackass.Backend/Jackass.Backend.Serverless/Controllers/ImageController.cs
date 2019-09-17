using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Jackass.Backend.Models;
using Jackass.Backend.Services;
using Microsoft.AspNetCore.Cors;

namespace Jackass.Backend.Serverless.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public sealed class ImageController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get(Uri imageUri)
        {
            byte[] imageData;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(imageUri);
                imageData = await response.Content.ReadAsByteArrayAsync();
            }

            RekognizedImage image;

            try
            {
                var rekognizer = new RekognizerService(new ConverterService());
                image = await rekognizer.RekognizeImage(imageData);
            }
            catch (SourceImageContentTooLargeException)
            {
                return BadRequest("Source image content size is too large");
            }
            catch (SourceImageDimensionsTooSmallException)
            {
                return BadRequest("Source image dimensions are too small");
            }

            if (image.Faces.Count == 0)
            {
                return BadRequest("No results");
            }

            var viewModels = image.Faces.Select(face => new FaceViewModel
                {
                    Confidence = face.MatchConfidence,
                    Left = face.FaceInfo.BoundingBox.Left,
                    Top = face.FaceInfo.BoundingBox.Top,
                    Width = face.FaceInfo.BoundingBox.Width,
                    Height = face.FaceInfo.BoundingBox.Height
                })
                .ToList();

            return Ok(viewModels);
        }
    }
}