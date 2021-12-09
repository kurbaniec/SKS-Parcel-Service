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
    public class ExportHierarchy
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
        public async Task SubmitNewParcelToTheLogisticsService()
        {
            var response = await client.GetAsync($"{ServerUrl}/warehouse");
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            var json = await response.Content.ReadAsStringAsync();
            
            Assert.IsTrue(json.Contains("Truck in Siebenhirten"));
            Assert.IsTrue(json.Contains("W-795293"));
            Assert.IsTrue(json.Contains("Neustift am Walde"));
            Assert.IsTrue(json.Contains("Truck in Sechshaus"));
            Assert.IsTrue(json.Contains("Root Warehouse - Österreich"));
        }
    }
}