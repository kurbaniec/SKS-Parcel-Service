using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Entities;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;

namespace UrbaniecZelenay.SKS.WebhookManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class WebhookRepositoryTests
    {
        // Create SqlException
        // See: https://stackoverflow.com/a/1387030/12347616
        private class SqlExceptionCreator
        {
            private static T Construct<T>(params object[] p)
            {
                var ctors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                return (T)ctors.First(ctor => ctor.GetParameters().Length == p.Length).Invoke(p);
            }

            internal static SqlException NewSqlException(int number = 1)
            {
                SqlErrorCollection collection = Construct<SqlErrorCollection>();
                SqlError error = Construct<SqlError>(number, (byte)2, (byte)3, "server name", "error message", "proc", 100, null);

                typeof(SqlErrorCollection)
                    .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .Invoke(collection, new object[] { error });


                return (typeof(SqlException)
                    .GetMethod("CreateException", BindingFlags.NonPublic | BindingFlags.Static,
                        null,
                        CallingConventions.ExplicitThis,
                        new[] { typeof(SqlErrorCollection), typeof(string) },
                        new ParameterModifier[] { })!
                    .Invoke(null, new object[] { collection, "7.0.0" }) as SqlException)!;
            }
        } 
        
        [Test]
        public void Create_ValidWebhook_WebhookReturned()
        {
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel(),
                Url = url,
                CreatedAt = DateTime.Now
            };
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            myDbMoq.Setup(m => m.Webhooks)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Webhook>(
                new List<Webhook>()
                ));
            Mock<DatabaseFacade> dbFacadeMock = new(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WebhookRepository>>();
            var webhookRepository = new WebhookRepository(mockLogger.Object, myDbMoq.Object);

            var w = webhookRepository.Create(validWebhook);
            
            Assert.NotNull(w);
            Assert.AreEqual(url, w.Url);
        }
        
        [Test]
        public void Create_DbProblemDbUpdateException_ThrowsDalSaveException()
        {
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel(),
                Url = url,
                CreatedAt = DateTime.Now
            };
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            myDbMoq.Setup(m => m.Webhooks).Throws(new DbUpdateException());
            Mock<DatabaseFacade> dbFacadeMock = new(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WebhookRepository>>();
            var webhookRepository = new WebhookRepository(mockLogger.Object, myDbMoq.Object);

            Assert.Throws<DalSaveException>(() => webhookRepository.Create(validWebhook));
        }
        
        [Test]
        public void Create_DbProblemSqlException_ThrowsDalConnectionException()
        {
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel(),
                Url = url,
                CreatedAt = DateTime.Now
            };
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            myDbMoq.Setup(m => m.Webhooks).Throws(SqlExceptionCreator.NewSqlException());
            Mock<DatabaseFacade> dbFacadeMock = new(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WebhookRepository>>();
            var webhookRepository = new WebhookRepository(mockLogger.Object, myDbMoq.Object);

            Assert.Throws<DalConnectionException>(() => webhookRepository.Create(validWebhook));
        }
        
        [Test]
        public void GetAllByTrackingId_ValidTrackingId_WebhooksReturned()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel { TrackingId = trackingId },
                Url = url,
                CreatedAt = DateTime.Now
            };
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            myDbMoq.Setup(m => m.Webhooks)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Webhook>(
                    new List<Webhook> { validWebhook }
                ));
            Mock<DatabaseFacade> dbFacadeMock = new(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WebhookRepository>>();
            var webhookRepository = new WebhookRepository(mockLogger.Object, myDbMoq.Object);

            var w = webhookRepository.GetAllByTrackingId(trackingId);
            
            Assert.NotNull(w);
            Assert.AreEqual(1, w.Count());
        }
        
        [Test]
        public void GetAllByTrackingId_DbProblemSqlException_ThrowsDalConnectionException()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel { TrackingId = trackingId },
                Url = url,
                CreatedAt = DateTime.Now
            };
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            myDbMoq.Setup(m => m.Webhooks).Throws(SqlExceptionCreator.NewSqlException());
            Mock<DatabaseFacade> dbFacadeMock = new(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WebhookRepository>>();
            var webhookRepository = new WebhookRepository(mockLogger.Object, myDbMoq.Object);

            Assert.Throws<DalConnectionException>(() => webhookRepository.GetAllByTrackingId(trackingId));
        }
        
        [Test]
        public void GetAllByTrackingId_DbProblemInvalidOperationException_ThrowsDalConnectionException()
        {
            const string trackingId = "000000001";
            const string url = "https://webhook.site/5a1d1232-ee26-418e-96cb-072a0add7804";
            var validWebhook = new Webhook
            {
                Parcel = new Parcel { TrackingId = trackingId },
                Url = url,
                CreatedAt = DateTime.Now
            };
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            myDbMoq.Setup(m => m.Webhooks).Throws(new InvalidOperationException());
            Mock<DatabaseFacade> dbFacadeMock = new(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WebhookRepository>>();
            var webhookRepository = new WebhookRepository(mockLogger.Object, myDbMoq.Object);

            Assert.Throws<DalConnectionException>(() => webhookRepository.GetAllByTrackingId(trackingId));
        }

        
    }
}