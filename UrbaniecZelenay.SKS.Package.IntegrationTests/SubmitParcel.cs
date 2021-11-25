using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace UrbaniecZelenay.SKS.Package.IntegrationTests
{
    // Ignore Tests for now
    // See: https://stackoverflow.com/a/1217089/12347616
    [TestFixture, Ignore("Integration Tests")]
    [ExcludeFromCodeCoverage]
    public class SubmitParcel
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void SubmitNewParcelToTheLogisticsService()
        {
            Assert.Pass();
        }
    }
}