using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;


namespace UrbaniecZelenay.SKS.ServiceAgents.Tests
{
    [ExcludeFromCodeCoverage]
    public class Tests
    {

        // Test class with http client:
        // https://dev.to/gautemeekolsen/how-to-test-httpclient-with-moq-in-c-2ldp
        [Test]
        public void EncodeAddress_ValidAddress_GeoCoordinateReturned()
        {
            var mockLogger = new Mock<ILogger<OpenStreetMapEncodingAgent>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            double lon = 16.3279026131193;
            double lat = 48.3418609;
            string content = "[{\"place_id\":111607674,\"licence\":\"Data © OpenStreetMap contributors, ODbL 1.0. " +
                             "https://osm.org/copyright\",\"osm_type\":\"way\",\"osm_id\":36856339,\"boundingbox\":" +
                             "[\"48.3417309\",\"48.341991\",\"16.3275901\",\"16.3282152\"],\"lat\":\"" +
                             lat.ToString(CultureInfo.InvariantCulture) +
                             "\",\"lon\":\"" + lon.ToString(CultureInfo.InvariantCulture) +
                             "\",\"display_name\":\"6, Brückenstraße, Korneuburg, Gemeinde Korneuburg, " +
                             "Bezirk Korneuburg, Lower Austria, 2100, Austria\",\"place_rank\":30,\"" +
                             "category\":\"building\",\"type\":\"yes\",\"importance\":0.33100000000000007}]";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content),
            };
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var geoEncodingAgent = new OpenStreetMapEncodingAgent(httpClient, mockLogger.Object);

            var geoCoordinate =
                geoEncodingAgent.EncodeAddress("Brückenstraße 6", "2100", "Korneuburg", "Austria");
            Assert.AreEqual(lon, geoCoordinate.X);
            Assert.AreEqual(lat, geoCoordinate.Y);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }
        
        [Test]
        public void EncodeAddress_StatusCode400_NullReturned()
        {
            var mockLogger = new Mock<ILogger<OpenStreetMapEncodingAgent>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            double lon = 16.3279026131193;
            double lat = 48.3418609;
            string content = "[{\"place_id\":111607674,\"licence\":\"Data © OpenStreetMap contributors, ODbL 1.0. " +
                             "https://osm.org/copyright\",\"osm_type\":\"way\",\"osm_id\":36856339,\"boundingbox\":" +
                             "[\"48.3417309\",\"48.341991\",\"16.3275901\",\"16.3282152\"],\"lat\":\"" +
                             lat.ToString(CultureInfo.InvariantCulture) +
                             "\",\"lon\":\"" + lon.ToString(CultureInfo.InvariantCulture) +
                             "\",\"display_name\":\"6, Brückenstraße, Korneuburg, Gemeinde Korneuburg, " +
                             "Bezirk Korneuburg, Lower Austria, 2100, Austria\",\"place_rank\":30,\"" +
                             "category\":\"building\",\"type\":\"yes\",\"importance\":0.33100000000000007}]";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(content),
            };
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var geoEncodingAgent = new OpenStreetMapEncodingAgent(httpClient, mockLogger.Object);

            var geoCoordinate =
                geoEncodingAgent.EncodeAddress("Brückenstraße 6", "2100", "Korneuburg", "Austria");
            Assert.IsNull(geoCoordinate);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }
        
        [Test]
        public void EncodeAddress_ExceptionThrown_NullReturned()
        {
            var mockLogger = new Mock<ILogger<OpenStreetMapEncodingAgent>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException("No such host is known."));
            var httpClient = new HttpClient(handlerMock.Object);
            var geoEncodingAgent = new OpenStreetMapEncodingAgent(httpClient, mockLogger.Object);

            var geoCoordinate =
                geoEncodingAgent.EncodeAddress("Brückenstraße 6", "2100", "Korneuburg", "Austria");
            Assert.IsNull(geoCoordinate);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}