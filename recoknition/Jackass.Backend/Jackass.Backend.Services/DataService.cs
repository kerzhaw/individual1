using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Jackass.Backend.Models;

namespace Jackass.Backend.Services
{
    public class DataService
    {
        private const string AccessKey = "AKIAIADUQPGXKH4FXIXA";
        private const string SecretKey = "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";
        private static readonly RegionEndpoint Region = RegionEndpoint.APSoutheast2;

        public async Task<RekognizedImage> GetImage(string hash)
        {
            using (var client = new AmazonDynamoDBClient(GetCredentials(), Region))
            {
                var table = Table.LoadTable(client, "Image");
                var config = new GetItemOperationConfig
                {
                    AttributesToGet = new List<string>
                    {
                        "Hash",
                        "Width",
                        "Height",
                        "FaceHeight",
                        "FaceWidth",
                        "FaceLeft",
                        "FaceTop"
                    },
                    ConsistentRead = true
                };

                var document = await table.GetItemAsync(hash, config);
                if (document == null) return null;

                return new RekognizedImage
                {
                    Hash = document["Hash"],
                    Width = document["Width"].AsInt(),
                    Height = document["Height"].AsInt(),
                    Faces = new List<Face>
                    {
                        new Face
                        {
                            FaceInfo = new FaceInfo
                            {
                                BoundingBox = new BoundingBox
                                {
                                    Width = document["FaceWidth"].AsInt(),
                                    Height = document["FaceHeight"].AsInt(),
                                    Left = document["FaceLeft"].AsInt(),
                                    Top = document["FaceTop"].AsInt(),
                                }
                            }
                        }
                    }
                };

            }
        }

        public async Task SaveImage(RekognizedImage image)
        {
            using (var client = new AmazonDynamoDBClient(GetCredentials(), Region))
            {
                var table = Table.LoadTable(client, "Image");

                var document = new Document
                {
                    ["Hash"] = image.Hash,
                    ["Width"] = image.Width,
                    ["Height"] = image.Height
                };

                if (image.Faces.Count > 0)
                {
                    var face = image.Faces.First();
                    document["FaceHeight"] = face.FaceInfo.BoundingBox.Height;
                    document["FaceWidth"] = face.FaceInfo.BoundingBox.Width;
                    document["FaceLeft"] = face.FaceInfo.BoundingBox.Left;
                    document["FaceTop"] = face.FaceInfo.BoundingBox.Top;
                }

                await table.PutItemAsync(document);
            }
        }
        private static AWSCredentials GetCredentials()
        {
            return new BasicAWSCredentials(AccessKey, SecretKey);
        }

    }
}