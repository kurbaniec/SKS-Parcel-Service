using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalRecipient = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Recipient;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class WarehouseManagementLogicTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ExportWarehouse_InternalError_ExceptionThrown()
        {
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            mockWarehouseRepo.Setup(m => m.GetAll()).Returns(new DataAccess.Entities.Warehouse
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new DataAccess.Entities.GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                LocationName = "Root",
                ProcessingDelayMins = 186,
                Level = 0,
                NextHops = new List<DataAccess.Entities.WarehouseNextHops>()
            });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            warehouseManagementLogic.TriggerExportWarehouseException = true;
            Assert.Throws<InvalidOperationException>(() => warehouseManagementLogic.ExportWarehouses());
        }

        [Test]
        public void ExportWarehouse_Valid_AllWarehousesReturned()
        {
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            mockWarehouseRepo.Setup(m => m.GetAll()).Returns(new DataAccess.Entities.Warehouse
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new DataAccess.Entities.GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                LocationName = "Root",
                ProcessingDelayMins = 186,
                Level = 0,
                NextHops = new List<DataAccess.Entities.WarehouseNextHops>()
            });


            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            Warehouse result = warehouseManagementLogic.ExportWarehouses();
            Assert.NotNull(result);
        }

        [Test]
        public void GetWarehouse_ValidCode_WarehouseReturned()
        {
            string code = "AUTA05";
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetWarehouseByCode(It.IsAny<string>())).Returns(
                new DataAccess.Entities.Warehouse
                {
                    Code = "AUTA01",
                    Description = "Root Warehouse - Österreich",
                    HopType = "Warehouse",
                    LocationCoordinates = new DataAccess.Entities.GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                    LocationName = "Root",
                    ProcessingDelayMins = 186,
                    Level = 0,
                    NextHops = new List<DataAccess.Entities.WarehouseNextHops>()
                });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            Warehouse result = warehouseManagementLogic.GetWarehouse(code);
            Assert.NotNull(result);
        }

        [Test]
        public void GetWarehouse_CodeNull_ExceptionThrown()
        {
            string code = null;
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetWarehouseByCode(It.IsAny<string>())).Returns(
                new DataAccess.Entities.Warehouse
                {
                    Code = "AUTA01",
                    Description = "Root Warehouse - Österreich",
                    HopType = "Warehouse",
                    LocationCoordinates = new DataAccess.Entities.GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                    LocationName = "Root",
                    ProcessingDelayMins = 186,
                    Level = 0,
                    NextHops = new List<DataAccess.Entities.WarehouseNextHops>()
                });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            Assert.Throws<ArgumentNullException>(() => warehouseManagementLogic.GetWarehouse(code));
        }

        [Test]
        public void ImportWarehouses_ValidWarehouse_NoException()
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
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            warehouseManagementLogic.ImportWarehouses(validWarehouse);
            Assert.True(true);
        }

        [Test]
        public void ImportWarehouses_InvalidWarehouseNextHopsNull_NoException()
        {
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            var invalidWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = "AUTA05",
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = null
            };
            Assert.Throws<ArgumentException>(() => warehouseManagementLogic.ImportWarehouses(invalidWarehouse));
            Assert.True(true);
        }

        [Test]
        public void ImportWarehouses_NullWarehouse_NoException()
        {
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            var mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic(mockLogger.Object, mockWarehouseRepo.Object, mapperConfig.CreateMapper());
            Warehouse invalidWarehouse = null;
            Assert.Throws<ArgumentNullException>(() => warehouseManagementLogic.ImportWarehouses(invalidWarehouse));
            Assert.True(true);
        }
    }
}