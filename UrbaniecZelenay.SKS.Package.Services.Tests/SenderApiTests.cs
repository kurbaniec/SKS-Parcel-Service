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
    public class SenderApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SubmitParcel_ValidParcel_NewParcelInfoReturned()
        {
            var validParcel = new Parcel
            {
                Weight = 0,
                Sender = new Recipient
                {
                    City = "Wien",
                    Country = "Austria",
                    Name = "Max Mustermann",
                    PostalCode = "1200",
                    Street = "Teststrasse"
                },
                Recipient = new Recipient
                {
                    City = "Wien",
                    Country = "Austria",
                    Name = "Maxine Mustermann",
                    PostalCode = "1210",
                    Street = "Teststrasse 2"
                },
            };
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            
            Mock<ISenderLogic> mockSenderLogic = new Mock<ISenderLogic>();
            mockSenderLogic.Setup(m => m.SubmitParcel(It.IsAny<BlParcel>()))
                .Returns(new BlParcel
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

            var controller = new SenderApiController(mapperConfig.CreateMapper(), mockSenderLogic.Object);
            // var controller = new SenderApiController(mapperConfig.CreateMapper());

            var result = controller.SubmitParcel(validParcel);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var parcelInfo = objectResult.Value as NewParcelInfo;
            Assert.NotNull(parcelInfo);
        }

        [Test]
        public void SubmitParcel_NullParcel_ErrorReturned()
        {
            Parcel nullParcel = null;
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });

            Mock<ISenderLogic> mockSenderLogic = new Mock<ISenderLogic>();
            mockSenderLogic.Setup(m => m.SubmitParcel(It.Is<BlParcel>(p => p == null)))
                .Throws(new ArgumentNullException("Error Parcel must not be null!"));

            var controller = new SenderApiController(mapperConfig.CreateMapper(), mockSenderLogic.Object);
            // var controller = new SenderApiController(mapperConfig.CreateMapper());

            var result = controller.SubmitParcel(nullParcel);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}