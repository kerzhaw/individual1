using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;

namespace Jackass.Backend.TestConsole
{
    internal static class Program
    {
        private const string AccessKey = "AKIAIADUQPGXKH4FXIXA";
        private const string SecretKey = "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";
        private static readonly RegionEndpoint Region = RegionEndpoint.APSoutheast2;

        public static async Task Main(string[] args)
        {
            await Get();
            Console.WriteLine();
        }

        private static async Task Get()
        {
            var creds = new BasicAWSCredentials(AccessKey, SecretKey);
            var client = new AmazonDynamoDBClient(creds, Region);

            var table = Table.LoadTable(client, "Image");

            var config = new GetItemOperationConfig
            {
                AttributesToGet = new List<string> {"Hash", "testing"},
                ConsistentRead = true
            };

            var image = await table.GetItemAsync("2a8d7291-78fe-44f6-9f4d-33178bad00d0", config);
        }

        private static async Task Bla()
        {
            var creds = new BasicAWSCredentials(AccessKey, SecretKey);
            var client = new AmazonDynamoDBClient(creds, Region);

            var table = Table.LoadTable(client, "Image");

            var image = new Document
            {
                ["testing"] = "Hello",
                ["Hash"] = Guid.NewGuid().ToString()
            };

            await table.PutItemAsync(image);

        }

    }
}