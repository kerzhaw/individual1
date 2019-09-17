using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime;
using Jackass.Backend.Models;
using Face = Jackass.Backend.Models.Face;
using Landmark = Jackass.Backend.Models.Landmark;
using LandmarkType = Jackass.Backend.Models.LandmarkType;

namespace Jackass.Backend.Services
{
    public class RekognizerService
    {
        private const string AccessKey = "AKIAIADUQPGXKH4FXIXA";
        private const string SecretKey = "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";
        private static readonly RegionEndpoint Region = RegionEndpoint.USEast1;

        private readonly ConverterService _converterService;

        public RekognizerService(ConverterService converterService)
        {
            _converterService = converterService;
        }

        public async Task<RekognizedImage> RekognizeImage(byte[] imageData)
        {
            var dataService = new DataService();
            var hash = imageData.GetSha1Hash();
            var existingImage = await dataService.GetImage(hash);

            if (existingImage != null) return existingImage;

            var convertedImage = _converterService.GetRekognitionCompatibleImage(imageData);

            if (!convertedImage.IsSizeOk())
                throw new SourceImageContentTooLargeException();

            if (!convertedImage.AreDimensionsOk())
                throw new SourceImageDimensionsTooSmallException();

            var client = new AmazonRekognitionClient(GetCredentials(), Region);

            var stream = new MemoryStream(convertedImage.ImageData);
            await stream.WriteAsync(convertedImage.ImageData, 0, convertedImage.ImageData.Length);
            stream.Seek(0, SeekOrigin.Begin);

            var image = new Image { Bytes = stream };
            var request = new RecognizeCelebritiesRequest { Image = image };
            var response = await client.RecognizeCelebritiesAsync(request);

            var recoknizedImage = new RekognizedImage
            {
                Width = convertedImage.Width,
                Height = convertedImage.Height,
                Hash = hash,
                Faces = MapResponse(convertedImage, response)
            };

            await dataService.SaveImage(recoknizedImage);

            return recoknizedImage;
        }

        private static AWSCredentials GetCredentials()
        {
            return new BasicAWSCredentials(AccessKey, SecretKey);
        }

        private static List<Face> MapResponse(ConvertedImage image, RecognizeCelebritiesResponse response)
        {
            var faces = new List<Face>();

            foreach (var celebrityFace in response.CelebrityFaces)
            {
                var face = new Face
                {
                    MatchConfidence = celebrityFace.MatchConfidence,
                    Name = celebrityFace.Name,
                    FaceInfo = new FaceInfo
                    {
                        Confidence = celebrityFace.Face.Confidence,
                        ImageQuality = new Models.ImageQuality
                        {
                            Brightness = celebrityFace.Face.Quality.Brightness,
                            Sharpness = celebrityFace.Face.Quality.Sharpness
                        },
                        Pose = new Models.Pose
                        {
                            Pitch = celebrityFace.Face.Pose.Pitch,
                            Roll = celebrityFace.Face.Pose.Roll,
                            Yaw = celebrityFace.Face.Pose.Yaw
                        },
                        BoundingBox = new Models.BoundingBox
                        {
                            Width =Convert.ToInt32(celebrityFace.Face.BoundingBox.Width * image.Width),
                            Height = Convert.ToInt32(celebrityFace.Face.BoundingBox.Height * image.Height),
                            Left = Convert.ToInt32(celebrityFace.Face.BoundingBox.Left * image.Width),
                            Top = Convert.ToInt32(celebrityFace.Face.BoundingBox.Top * image.Height)
                        }
                    }
                };

                var landmarks = new List<Landmark>();

                foreach (var faceLandmark in celebrityFace.Face.Landmarks)
                {
                    Enum.TryParse(faceLandmark.Type.Value, true, out LandmarkType type);

                    var landmark = new Landmark
                    {
                        Type = type,
                        X = faceLandmark.X * image.Width,
                        Y = faceLandmark.Y * image.Height
                    };

                    landmarks.Add(landmark);
                }

                face.FaceInfo.Landmarks = landmarks;

                faces.Add(face);
            }

            return faces;
        }
    }
}