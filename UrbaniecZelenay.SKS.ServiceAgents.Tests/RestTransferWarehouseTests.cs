using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace UrbaniecZelenay.SKS.ServiceAgents.Tests
{
    [ExcludeFromCodeCoverage]
    public class RestTransferWarehouseTests
    {
        // Test class with http client:
        // https://dev.to/gautemeekolsen/how-to-test-httpclient-with-moq-in-c-2ldp
        [Test]
        public void TransferParcel_ValidRequest_TrueReturned()
        {
            var mockLogger = new Mock<ILogger<RestTransferWarehouseAgent>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            string responseContent = "{\"trackingId\":\"string\"}";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent),
            };
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var restTransferWarehouseAgent = new RestTransferWarehouseAgent(mockLogger.Object, httpClient);
            // var geoEncodingAgent = new OpenStreetMapEncodingAgent(httpClient, mockLogger.Object);
            bool result =
                restTransferWarehouseAgent.TransferParcel("https://localhost/", "PYJRB4HZ6", 1, "", "", "", "", "", "", "", "", "",
                    "");
            Assert.IsTrue(result);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void TransferParcel_StatusCode400_FalseReturned()
        {
            var mockLogger = new Mock<ILogger<RestTransferWarehouseAgent>>();
            var handlerMock = new Mock<HttpMessageHandler>();
            string responseContent = "{\"trackingId\":\"string\"}";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(responseContent),
            };
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var restTransferWarehouseAgent = new RestTransferWarehouseAgent(mockLogger.Object, httpClient);
            // var geoEncodingAgent = new OpenStreetMapEncodingAgent(httpClient, mockLogger.Object);
            bool result =
                restTransferWarehouseAgent.TransferParcel("https://localhost/", "PYJRB4HZ6", 1, "", "", "", "", "", "", "", "", "",
                    "");

            Assert.IsFalse(result);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void TransferParcel_ExceptionThrown_FalseReturned()
        {
            var mockLogger = new Mock<ILogger<RestTransferWarehouseAgent>>();
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException("No such host is known."));
            var httpClient = new HttpClient(handlerMock.Object);
            var restTransferWarehouseAgent = new RestTransferWarehouseAgent(mockLogger.Object, httpClient);
            // var geoEncodingAgent = new OpenStreetMapEncodingAgent(httpClient, mockLogger.Object);
            bool result =
                restTransferWarehouseAgent.TransferParcel("https://localhost/", "PYJRB4HZ6", 1, "", "", "", "", "", "", "", "", "",
                    "");

            Assert.IsFalse(result);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}