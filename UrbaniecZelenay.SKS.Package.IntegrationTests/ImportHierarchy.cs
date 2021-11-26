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
    //[TestFixture, Ignore("Integration Tests")]
    [ExcludeFromCodeCoverage]
    public class ImportHierarchy
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

        [Test]
        public async Task ImportAHierarchyOfHops()
        {
            var response = await client.PostAsync($"{ServerUrl}/warehouse", new StringContent(
                dataset, Encoding.UTF8, "application/json"
            ));
            
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}