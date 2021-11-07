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
    public class LogisticsPartnerApiTests
    {
        // MethodNameToTest_Scenario_Outcome
        // ===
        // SubmitParcel_ValidParcel_TrackingIdReturned
        // SubmitParcel_ZeroWeight_ValidationError
        // SubmitParcel_NullParcel_ArgumentExpection
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransitionParcel_ValidParcel_TrackingIdReturned()
        {
            var validParcel = new Parcel
            {
                Weight = 100,
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
            var trackingId = "PYJRB4HZ6";
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfileSvcBl());
            });
            
            Mock<ILogisticsPartnerLogic> mockLogisticsPartnerLogic = new Mock<ILogisticsPartnerLogic>();
            mockLogisticsPartnerLogic.Setup(m => m.TransitionParcel(It.IsAny<BlParcel>())).Returns(new BlParcel
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

            
            // var controller = new LogisticsPartnerApiController(mapperConfig.CreateMapper());
            var controller = new LogisticsPartnerApiController(mapperConfig.CreateMapper(),mockLogisticsPartnerLogic.Object);

            var result = controller.TransitionParcel(validParcel, trackingId);
            
            var okResult = result as ObjectResult;
            Assert.NotNull(okResult);
            var newParcelInfo = okResult.Value as NewParcelInfo;
            Assert.NotNull(newParcelInfo);
            Assert.AreEqual(trackingId, newParcelInfo.TrackingId);
        }
        
        [Test]
        public void TransitionParcel_NegativeWeight_ErrorReturned()
        {
            var validParcel = new Parcel
            {
                Weight = -1,
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
            var trackingId = "PYJRB4HZ6";
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfileSvcBl());
            });
            
            Mock<ILogisticsPartnerLogic> mockLogisticsPartnerLogic = new Mock<ILogisticsPartnerLogic>();
            mockLogisticsPartnerLogic.Setup(m => m.TransitionParcel(It.Is<BlParcel>(p => p.Weight < 0)))
                .Throws(new ArgumentException("Weight must be >= 0"));
            
            var controller = new LogisticsPartnerApiController(mapperConfig.CreateMapper(), mockLogisticsPartnerLogic.Object);
           // var controller = new LogisticsPartnerApiController(mapperConfig.CreateMapper()); 

            var result = controller.TransitionParcel(validParcel, trackingId);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}