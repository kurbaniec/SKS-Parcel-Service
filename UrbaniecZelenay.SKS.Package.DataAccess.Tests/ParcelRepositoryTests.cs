using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Sql;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParcelRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void GetByTrackingId_ValidTrackingId_ParcelReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string trackingId = "PYJRB4HZ6";
            var validParcel = new Parcel
            {
                TrackingId = trackingId,
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
            myDbMoq.Setup(m => m.Parcels)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Parcel>(new List<Parcel> { validParcel }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<ParcelRepository>>();
            IParcelRepository parcelRepository = new ParcelRepository(mockLogger.Object, myDbMoq.Object);
            Parcel? p = parcelRepository.GetByTrackingId(trackingId);
            Assert.NotNull(p);
            Assert.AreEqual(p.TrackingId, trackingId);
        }

        [Test]
        public void Update_ValidParcel_UpdatedParcelReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string trackingId = "PYJRB4HZ6";
            var validParcel = new Parcel
            {
                TrackingId = trackingId,
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
            myDbMoq.Setup(m => m.Parcels)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Parcel>(new List<Parcel> { validParcel }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<ParcelRepository>>();
            IParcelRepository parcelRepository = new ParcelRepository(mockLogger.Object, myDbMoq.Object);
            validParcel.Weight = 100;
            Parcel? p = parcelRepository.Update(validParcel);
            Assert.NotNull(p);
            Assert.AreEqual(p.TrackingId, trackingId);

            Assert.AreEqual(p.Weight, 100);
        }

        [Test]
        public void Delete_ValidParcel_ParcelRemoved()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string trackingId = "PYJRB4HZ6";
            var validParcel = new Parcel
            {
                TrackingId = trackingId,
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
            myDbMoq.Setup(m => m.Parcels)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Parcel>(new List<Parcel>()));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<ParcelRepository>>();
            IParcelRepository parcelRepository = new ParcelRepository(mockLogger.Object, myDbMoq.Object);
            parcelRepository.Create(validParcel);
            parcelRepository.Delete(validParcel);
            Parcel? p = parcelRepository.GetByTrackingId(trackingId);
            Assert.IsNull(p);
        }

        [Test]
        public void GetByTrackingId_InvalidTrackingId_NullReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string trackingId = "PYJRB4HZ6";
            var validParcel = new Parcel
            {
                TrackingId = trackingId,
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
            myDbMoq.Setup(m => m.Parcels)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Parcel>(new List<Parcel> { validParcel }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<ParcelRepository>>();
            IParcelRepository parcelRepository = new ParcelRepository(mockLogger.Object, myDbMoq.Object);
            Parcel? p = parcelRepository.GetByTrackingId("");
            Assert.IsNull(p);
        }

        [Test]
        public void Create_ValidParcel_NoException()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
            // myDbMoq.Setup(m => m.Hops).Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop>()));
            myDbMoq.Setup(m => m.Parcels)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Parcel>(new List<Parcel>()));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<ParcelRepository>>();
            IParcelRepository parcelRepository = new ParcelRepository(mockLogger.Object, myDbMoq.Object);
            parcelRepository.Create(validParcel);
        }
    }
}