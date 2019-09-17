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

namespace Jackass.Backend.Rekognition
{
    public sealed class Recoknizer
    {
        private const string AccessKey = "AKIAIADUQPGXKH4FXIXA";
        private const string SecretKey = "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";
        private static readonly RegionEndpoint Region = RegionEndpoint.USEast1;

        public static async Task<List<Face>> PerformRecognition(ProcessedImage processedImage)
        {
            if (!processedImage.IsSizeOk())
                throw new SourceImageContentTooLargeException();

            if (!processedImage.AreDimensionsOk())
                throw new SourceImageDimensionsTooSmallException();

            var client = new AmazonRekognitionClient(GetCredentials(), Region);

            var stream = new MemoryStream(processedImage.ImageData);
            await stream.WriteAsync(processedImage.ImageData, 0, processedImage.ImageData.Length);
            stream.Seek(0, SeekOrigin.Begin);

            var image = new Image { Bytes = stream };
            var request = new RecognizeCelebritiesRequest { Image = image };
            var response = await client.RecognizeCelebritiesAsync(request);
            return MapResponse(processedImage, response);

        }

        private static AWSCredentials GetCredentials()
        {
            return new BasicAWSCredentials(AccessKey, SecretKey);
        }

        private static List<Face> MapResponse(ProcessedImage image, RecognizeCelebritiesResponse response)
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
                            Width = celebrityFace.Face.BoundingBox.Width * image.Width,
                            Height = celebrityFace.Face.BoundingBox.Height * image.Height,
                            Left = celebrityFace.Face.BoundingBox.Left * image.Width,
                            Top = celebrityFace.Face.BoundingBox.Top * image.Height
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