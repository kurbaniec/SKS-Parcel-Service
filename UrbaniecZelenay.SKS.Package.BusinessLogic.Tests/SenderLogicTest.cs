using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.ServiceAgents;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalRecipient = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Recipient;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;
using DalTruck = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Truck;
using DalWarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Warehouse;
using DalWarehouseNextHops = UrbaniecZelenay.SKS.Package.DataAccess.Entities.WarehouseNextHops;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class SenderLogicTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SubmitParcel_ValidParcel_ParcelReturned()
        {
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
            var mockLogger = new Mock<ILogger<SenderLogic>>();
            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DalParcel>(), false)).Returns(new DalParcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = DalParcel.StateEnum.TransferredEnum,
                VisitedHops = new List<DalHopArrival>(),
                FutureHops = new List<DalHopArrival>()
            });
            var mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var rootWarehouse = new DalWarehouse()
            {
                NextHops = new List<DalWarehouseNextHops>()
            };
            var truckA = new DalTruck()
            {
                PreviousHop = rootWarehouse
            };
            var truckB = new DalTruck()
            {
                PreviousHop = rootWarehouse
            };
            mockWarehouseRepo.SetupSequence(m => m.GetTruckByPoint(It.IsAny<Point>())).Returns(truckA).Returns(truckB);
            var mockGeoAgent = new Mock<IGeoEncodingAgent>();
            mockGeoAgent
                .Setup(agent => agent.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(new Point(13.884382, 47.247829));
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            ISenderLogic senderLogic =
                new SenderLogic(mockLogger.Object, mockParcelRepo.Object, mockWarehouseRepo.Object, mockGeoAgent.Object,
                    mapperConfig.CreateMapper());
            Parcel blParcel = senderLogic.SubmitParcel(validParcel);
            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(blParcel);
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void SubmitParcel_ParcelNull_ParcelReturned()
        {
            var mockLogger = new Mock<ILogger<SenderLogic>>();
            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DalParcel>(), false)).Returns(new DalParcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = DalParcel.StateEnum.TransferredEnum,
                VisitedHops = new List<DalHopArrival>(),
                FutureHops = new List<DalHopArrival>()
            });
            var mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mockGeoAgent = new Mock<IGeoEncodingAgent>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            ISenderLogic senderLogic =
                new SenderLogic(mockLogger.Object, mockParcelRepo.Object, mockWarehouseRepo.Object, mockGeoAgent.Object,
                    mapperConfig.CreateMapper());
            Assert.Throws<BlArgumentException>(() => senderLogic.SubmitParcel(null));
        }

        [Test]
        public void SubmitParcel_InvalidParcelSenderNull_ParcelReturned()
        {
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
                Sender = null,
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
            var mockLogger = new Mock<ILogger<SenderLogic>>();
            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DalParcel>(), false)).Returns(new DalParcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = DalParcel.StateEnum.TransferredEnum,
                VisitedHops = new List<DalHopArrival>(),
                FutureHops = new List<DalHopArrival>()
            });
            var mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mockGeoAgent = new Mock<IGeoEncodingAgent>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            ISenderLogic senderLogic =
                new SenderLogic(mockLogger.Object, mockParcelRepo.Object, mockWarehouseRepo.Object, mockGeoAgent.Object,
                    mapperConfig.CreateMapper());
            Assert.Throws<BlValidationException>(() => senderLogic.SubmitParcel(validParcel));
        }
    }
}