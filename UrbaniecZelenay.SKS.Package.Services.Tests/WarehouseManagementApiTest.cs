using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using UrbaniecZelenay.SKS.Package.Services.Mappings;
using BlParcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;
using BlRecipient = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Recipient;
using BlWarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Warehouse;
using BlWarehouseNextHops = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.WarehouseNextHops;
using BlHop = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Hop;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;

namespace UrbaniecZelenay.SKS.Package.Services.Tests
{
    [ExcludeFromCodeCoverage]
    public class WarehouseManagementApiTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ExportWarehouse_Valid_AllWarehousesReturned()
        {
            Mock<IWarehouseManagementLogic> mockWarehouseManagementLogic = new Mock<IWarehouseManagementLogic>();
            mockWarehouseManagementLogic.Setup(m => m.ExportWarehouses()).Returns(new BlWarehouse
            {
                HopType = "Warehouse",
                Code = "AUTA05",
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<BlWarehouseNextHops>()
            });
            var mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            var controller =
                new WarehouseManagementApiController(mockLogger.Object, mapperConfig.CreateMapper(),
                    mockWarehouseManagementLogic.Object);
            // var controller = new WarehouseManagementApiController(mapperConfig.CreateMapper());


            var result = controller.ExportWarehouses();

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var warehouse = objectResult.Value as Warehouse;
            Assert.NotNull(warehouse);
        }

        [Test]
        public void ExportWarehouse_InternalError_ErrorReturned()
        {
            Mock<IWarehouseManagementLogic> mockWarehouseManagementLogic = new Mock<IWarehouseManagementLogic>();
            mockWarehouseManagementLogic.Setup(m => m.ExportWarehouses()).Throws(new BlArgumentException("unit test error"));
            var mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            var controller =
                new WarehouseManagementApiController(mockLogger.Object, mapperConfig.CreateMapper(),
                    mockWarehouseManagementLogic.Object);
            // var controller = new WarehouseManagementApiController(mapperConfig.CreateMapper());

            var result = controller.ExportWarehouses();

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }

        [Test]
        public void GetWarehouse_ValidCode_WarehouseReturned()
        {
            var code = "AUTA05";
            Mock<IWarehouseManagementLogic> mockWarehouseManagementLogic = new Mock<IWarehouseManagementLogic>();
            mockWarehouseManagementLogic.Setup(m => m.GetWarehouse(It.IsAny<string>())).Returns(new BlWarehouse
            {
                HopType = "Warehouse",
                Code = "AUTA05",
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<BlWarehouseNextHops>()
            });
            var mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            var controller =
                new WarehouseManagementApiController(mockLogger.Object, mapperConfig.CreateMapper(),
                    mockWarehouseManagementLogic.Object);
            // var controller = new WarehouseManagementApiController(mapperConfig.CreateMapper());
            var result = controller.GetWarehouse(code);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var warehouse = objectResult.Value as Warehouse;
            Assert.NotNull(warehouse);
        }

        [Test]
        public void GetWarehouse_NullCode_ErrorReturned()
        {
            string code = null;
            Mock<IWarehouseManagementLogic> mockWarehouseManagementLogic = new Mock<IWarehouseManagementLogic>();
            mockWarehouseManagementLogic.Setup(m => m.GetWarehouse(It.Is<string>(s => s == null)))
                .Throws(new BlArgumentException("value must not be null"));
            var mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            // var controller = new WarehouseManagementApiController(mapperConfig.CreateMapper());
            var controller = new WarehouseManagementApiController(mockLogger.Object, mapperConfig.CreateMapper(),
                mockWarehouseManagementLogic.Object);
            var result = controller.GetWarehouse(code);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }

        [Test]
        public void ImportWarehouses_ValidWarehouse_SuccessStatusReturned()
        {
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = "AUTA05",
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            Mock<IWarehouseManagementLogic> mockWarehouseManagementLogic = new Mock<IWarehouseManagementLogic>();
            mockWarehouseManagementLogic.Setup(m => m.ImportWarehouses(It.IsAny<BlWarehouse>()));
            var mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            // var controller = new WarehouseManagementApiController(mapperConfig.CreateMapper());
            var controller = new WarehouseManagementApiController(mockLogger.Object, mapperConfig.CreateMapper(),
                mockWarehouseManagementLogic.Object);
            var result = controller.ImportWarehouses(validWarehouse);

            var statusCode = result as StatusCodeResult;

            Assert.NotNull(statusCode);
            Assert.AreEqual(200, statusCode.StatusCode);
        }

        [Test]
        public void ImportWarehouses_NullWarehouse_ErrorReturned()
        {
            Warehouse validWarehouse = null;
            Mock<IWarehouseManagementLogic> mockWarehouseManagementLogic = new Mock<IWarehouseManagementLogic>();
            mockWarehouseManagementLogic.Setup(m => m.ImportWarehouses(It.Is<BlWarehouse>(w => w == null)))
                .Throws(new BlArgumentException("Warehouse must not be null."));
            var mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileSvcBl()); });
            // var controller = new WarehouseManagementApiController(mapperConfig.CreateMapper());
            var controller = new WarehouseManagementApiController(mockLogger.Object, mapperConfig.CreateMapper(),
                mockWarehouseManagementLogic.Object);

            var result = controller.ImportWarehouses(validWarehouse);

            var statusCode = result as StatusCodeResult;

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}