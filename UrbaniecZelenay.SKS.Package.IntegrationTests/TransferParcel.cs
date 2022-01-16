using System;
using System.Collections.Generic;
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
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TransferParcel
    {
        private readonly string ServerUrl =
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SKSURL"))
                ? "https://sks-team-x-test.azurewebsites.net"
                : Environment.GetEnvironmentVariable("SKSURL");


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
            // client.PostAsync($"{ServerUrl}/warehouse", new StringContent(
                // dataset, Encoding.UTF8, "application/json"
            // )).Wait();
            var task = Task.Run(() => client.PostAsync($"{ServerUrl}/warehouse", new StringContent(
                dataset, Encoding.UTF8, "application/json"
            )));
            task.Wait();
            var response = task.Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

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
                    ""street"": ""Am Europlatz 3"",
                    ""postalCode"": ""1120"",
                    ""city"": ""Wien"",
                    ""country"": ""Austria""
                },
                ""sender"": {
                    ""name"": ""Maxine Mustermann"",
                    ""street"": ""Franklinstrasse 21"",
                    ""postalCode"": ""1210"",
                    ""city"": ""Wien"",
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
            
                        // Track parcel
            response = await client.GetAsync($"{ServerUrl}/parcel/{trackingId}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            responseBody = await response.Content.ReadAsStringAsync();
            json = responseBody.Replace("\r", "");
            json = responseBody.Replace("\n", "");
            Assert.IsTrue(json.Contains("\"state\":\"Pickup\""));
            Assert.IsTrue(json.Contains("\"visitedHops\":[]"));
            Assert.IsTrue(json.Contains("\"futureHops\":[{"));
            Queue<string> futureHops = new Queue<string>();
            futureHops.Enqueue("WTTA070");
            futureHops.Enqueue("WENB01");
            futureHops.Enqueue("WTTA014");
            
            // report hop
            response = await client.PostAsync($"{ServerUrl}/parcel/{trackingId}/reportHop/{futureHops.Dequeue()}", new StringContent("")); 
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            // track parcel
            response = await client.GetAsync($"{ServerUrl}/parcel/{trackingId}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            responseBody = await response.Content.ReadAsStringAsync();
            json = responseBody.Replace("\r", "");
            json = responseBody.Replace("\n", "");
            Assert.IsTrue(json.Contains("\"state\":\"InTruckDelivery\""));
            Assert.IsTrue(json.Contains("\"visitedHops\":[{"));
            Assert.IsTrue(json.Contains("\"futureHops\":[{"));
 
            // report hop
            response = await client.PostAsync($"{ServerUrl}/parcel/{trackingId}/reportHop/{futureHops.Dequeue()}", new StringContent("")); 
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            // track parcel
            response = await client.GetAsync($"{ServerUrl}/parcel/{trackingId}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            responseBody = await response.Content.ReadAsStringAsync();
            json = responseBody.Replace("\r", "");
            json = responseBody.Replace("\n", "");
            Assert.IsTrue(json.Contains("\"state\":\"InTransport\""));
            Assert.IsTrue(json.Contains("\"visitedHops\":[{"));
            Assert.IsTrue(json.Contains("\"futureHops\":[{"));
            
            // report hop
            response = await client.PostAsync($"{ServerUrl}/parcel/{trackingId}/reportHop/{futureHops.Dequeue()}", new StringContent("")); 
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            // track parcel
            response = await client.GetAsync($"{ServerUrl}/parcel/{trackingId}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            responseBody = await response.Content.ReadAsStringAsync();
            json = responseBody.Replace("\r", "");
            json = responseBody.Replace("\n", "");
            Assert.IsTrue(json.Contains("\"state\":\"InTruckDelivery\""));
            Assert.IsTrue(json.Contains("\"visitedHops\":[{"));
            Assert.IsTrue(json.Contains("\"futureHops\":[]"));
            
            // report delivery
            response = await client.PostAsync($"{ServerUrl}/parcel/{trackingId}/reportDelivery", new StringContent("")); 
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            // track parcel
            response = await client.GetAsync($"{ServerUrl}/parcel/{trackingId}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            responseBody = await response.Content.ReadAsStringAsync();
            json = responseBody.Replace("\r", "");
            json = responseBody.Replace("\n", "");
            Assert.IsTrue(json.Contains("\"state\":\"Delivered\""));
            Assert.IsTrue(json.Contains("\"visitedHops\":[{"));
            Assert.IsTrue(json.Contains("\"futureHops\":[]"));
        }
    }
}