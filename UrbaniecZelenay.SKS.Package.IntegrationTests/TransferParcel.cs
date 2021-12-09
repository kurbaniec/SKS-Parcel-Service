using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UrbaniecZelenay.SKS.Package.IntegrationTests
{
    // Ignore Tests for now
    // See: https://stackoverflow.com/a/1217089/12347616
    // [TestFixture, Ignore("Integration Tests")]
    [ExcludeFromCodeCoverage]
    public class TransferParcel
    {
        private const string ServerUrl = "http://localhost:5000";
        private readonly HttpClient client = new();
        private string dataset;
    
        [OneTimeSetUp]
        public void LoadDataSet()
        {
            // Load test dataset
            // See: https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;
            var datasetPath =
                $"{projectDirectory}{Path.DirectorySeparatorChar}datasets{Path.DirectorySeparatorChar}light.json";
            dataset = File.ReadAllText(datasetPath);
        }
    
        [SetUp]
        public void ImportWarehouseHierarchy()
        {
            // Add dataset
            // Send JSON payload
            // See: https://stackoverflow.com/a/8199814/12347616
            client.PostAsync($"{ServerUrl}/warehouse", new StringContent(
                dataset, Encoding.UTF8, "application/json"
            )).Wait();
        }
    
        [Test]
        public async Task TransferAnExistingParcelFromTheServiceOfALogisticsPartner()
        {
            const string trackingId = "PYJRB4HZ6";
            const string body = @"
            {
                ""weight"": 2,
                ""recipient"": {
                    ""name"": ""Max Mustermann"",
                    ""street"": ""Höchstädtplatz 6"",
                    ""postalCode"": ""1200"",
                    ""city"": ""Vienna"",
                    ""country"": ""Austria""
                },
                ""sender"": {
                    ""name"": ""Maxine Mustermann"",
                    ""street"": ""Urban-Loritz-Platz 2A"",
                    ""postalCode"": ""1070"",
                    ""city"": ""Vienna"",
                    ""country"": ""Austria""
                }
            }";
            // const string body = @"
            // {
            //     ""weight"": 2,
            //     ""recipient"": {
            //         ""name"": ""Max Mustermann"",
            //         ""street"": ""Weißenburger Pl. 8"",
            //         ""postalCode"": ""81667"",
            //         ""city"": ""München"",
            //         ""country"": ""Germany""
            //     },
            //     ""sender"": {
            //         ""name"": ""Maxine Mustermann"",
            //         ""street"": ""Urban-Loritz-Platz 2A"",
            //         ""postalCode"": ""1070"",
            //         ""city"": ""Vienna"",
            //         ""country"": ""Austria""
            //     }
            // }";
            var response = await client.PostAsync($"{ServerUrl}/parcel/{trackingId}", new StringContent(
                body, Encoding.UTF8, "application/json"
            ));
    
            Assert.IsTrue(response.IsSuccessStatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = responseBody.Replace(" ", "");
            Assert.IsTrue(json.Contains("{\"trackingId\":\"PYJRB4HZ6\"}"));
        }
    }
}