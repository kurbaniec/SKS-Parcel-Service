using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Entities;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;

namespace UrbaniecZelenay.SKS.WebhookManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class WebhookManagerTests
    {
        [Test]
        public void ListParcelWebhooks_ValidTrackingId_WebhooksReturned()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel { TrackingId = trackingId },
                Url = url,
            };
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            mockWebhookRepository
                .Setup(m => m.GetAllByTrackingId(It.IsAny<string>()))
                .Returns(new List<Webhook> { validWebhook });
            var mockParcelRepository = new Mock<IParcelRepository>();
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            var webhooks = webhookManager.ListParcelWebhooks(trackingId);
            
            Assert.NotNull(webhooks);
            Assert.AreEqual(1, webhooks.Count());
        }
        
        [Test]
        public void SubscribeParcelWebhook_ValidTrackingIdAndUrl_WebhookReturned()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            var mockParcelRepository = new Mock<IParcelRepository>();
            mockParcelRepository
                .Setup(m => m.GetByTrackingId(It.IsAny<string>()))
                .Returns(new Parcel { TrackingId = trackingId });
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            var webhook = webhookManager.SubscribeParcelWebhook(trackingId, url);
            
            Assert.NotNull(webhook);
            Assert.AreEqual(trackingId, webhook.TrackingId);
            Assert.AreEqual(url, webhook.Url);
        }
        
        [Test]
        public void SubscribeParcelWebhook_ParcelDoesNotExist_ThrowsDalException()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            var mockParcelRepository = new Mock<IParcelRepository>();
            mockParcelRepository
                .Setup(m => m.GetByTrackingId(It.IsAny<string>()))
                .Returns((Parcel)null!);
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            Assert.Throws<DalException>(() => webhookManager.SubscribeParcelWebhook(trackingId, url));
        }
        
        [Test]
        public void UnsubscribeParcelWebhook_ValidId_VoidReturned()
        {
            const long id = 1;
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            mockWebhookRepository.Setup(m => m.Delete(It.IsAny<long>()));
            var mockParcelRepository = new Mock<IParcelRepository>();
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            Assert.DoesNotThrow(() => webhookManager.UnsubscribeParcelWebhook(id));
        }
        
        [Test]
        public void NotifyParcelWebhooks_ValidParcel_VoidReturned()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validParcel = new Parcel { TrackingId = trackingId };
            var validWebhook = new Webhook
            {
                Id = 1,
                Parcel = validParcel,
                Url = url
            };
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
            };
            // See: https://stackoverflow.com/a/59724629/12347616
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), 
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            mockWebhookRepository
                .Setup(m => m.GetAllByTrackingId(It.IsAny<string>()))
                .Returns(new List<Webhook> { validWebhook });
            var mockParcelRepository = new Mock<IParcelRepository>();
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                httpClient,
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            Assert.DoesNotThrow(() => webhookManager.NotifyParcelWebhooks(validParcel));
        }
        
        [Test]
        public void NotifyParcelWebhooks_InternalHttpRequestException_VoidReturned()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validParcel = new Parcel { TrackingId = trackingId };
            var validWebhook = new Webhook
            {
                Id = 1,
                Parcel = validParcel,
                Url = url
            };
            var handlerMock = new Mock<HttpMessageHandler>();
            // See: https://stackoverflow.com/a/59724629/12347616
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException());
            var httpClient = new HttpClient(handlerMock.Object);
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            mockWebhookRepository
                .Setup(m => m.GetAllByTrackingId(It.IsAny<string>()))
                .Returns(new List<Webhook> { validWebhook });
            var mockParcelRepository = new Mock<IParcelRepository>();
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                httpClient,
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            Assert.DoesNotThrow(() => webhookManager.NotifyParcelWebhooks(validParcel));
        }
        
        [Test]
        public void NotifyParcelWebhooks_InternalOtherException_VoidReturned()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validParcel = new Parcel { TrackingId = trackingId };
            var validWebhook = new Webhook
            {
                Id = 1,
                Parcel = validParcel,
                Url = url
            };
            var handlerMock = new Mock<HttpMessageHandler>();
            // See: https://stackoverflow.com/a/59724629/12347616
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws(new Exception());
            var httpClient = new HttpClient(handlerMock.Object);
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            mockWebhookRepository
                .Setup(m => m.GetAllByTrackingId(It.IsAny<string>()))
                .Returns(new List<Webhook> { validWebhook });
            var mockParcelRepository = new Mock<IParcelRepository>();
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                httpClient,
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            Assert.DoesNotThrow(() => webhookManager.NotifyParcelWebhooks(validParcel));
        }
        
        [Test]
        public void UnsubscribeAllParcelWebhooks_ValidTrackingId_VoidReturned()
        {
            const string trackingId = "000000001";
            var mockWebhookRepository = new Mock<IWebhookRepository>();
            mockWebhookRepository.Setup(m => m.Delete(It.IsAny<long>()));
            var mockParcelRepository = new Mock<IParcelRepository>();
            var mapperConfig = new MapperConfiguration(
                mc => mc.AddProfile(new MappingsProfileBlDal()));
            var mockLogger = new Mock<ILogger<WebhookManager>>();
            var webhookManager = new WebhookManager(
                mockWebhookRepository.Object, mockParcelRepository.Object,
                mapperConfig.CreateMapper(), mockLogger.Object
            );

            Assert.DoesNotThrow(() => webhookManager.UnsubscribeAllParcelWebhooks(trackingId));
        }
    }
}