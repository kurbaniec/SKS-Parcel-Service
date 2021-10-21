using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using UrbaniecZelenay.SKS.Package.Services.Mappings;
using BlParcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;
using BlRecipient = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Recipient;
using BlWarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Warehouse;
using BlWarehouseNextHops = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.WarehouseNextHops;
using BlHop = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Hop;
using BlHopArrival = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.HopArrival;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;

namespace UrbaniecZelenay.SKS.Package.Services.Tests
{
    [ExcludeFromCodeCoverage]
    public class RecipientApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TrackParcel_ValidTrackingId_TrackingInformationReturned()
        {
            var trackingId = "PYJRB4HZ6";
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });

            Mock<IRecipientLogic> mockRecipientLogic = new Mock<IRecipientLogic>();
            mockRecipientLogic.Setup(m => m.TrackParcel(It.IsAny<string>())).Returns(new BlParcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new BlRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new BlRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = BlParcel.StateEnum.TransferredEnum,
                VisitedHops = new List<BlHopArrival>(),
                FutureHops = new List<BlHopArrival>()
            });


            // var controller = new RecipientApiController(mapperConfig.CreateMapper(), mockRecipientLogic.Object);
            var controller = new RecipientApiController(mapperConfig.CreateMapper());


            var result = controller.TrackParcel(trackingId);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var trackingInfo = objectResult.Value as TrackingInformation;
            Assert.NotNull(trackingInfo);
        }

        [Test]
        public void TrackParcel_NullTrackingId_ErrorReturned()
        {
            string trackingId = null;
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });

            Mock<IRecipientLogic> mockRecipientLogic = new Mock<IRecipientLogic>();
            mockRecipientLogic.Setup(m => m.TrackParcel(It.Is<string>(s => s == null)))
                .Throws(new ArgumentNullException("tracking Id mustnot be null!"));

            // var controller = new RecipientApiController(mapperConfig.CreateMapper(), mockRecipientLogic.Object);
            var controller = new RecipientApiController(mapperConfig.CreateMapper());

            var result = controller.TrackParcel(trackingId);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}