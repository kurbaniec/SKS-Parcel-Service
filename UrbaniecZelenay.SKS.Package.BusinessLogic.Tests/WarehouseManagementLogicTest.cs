using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;

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
            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic { TriggerExportWarehouseException = true };
            Assert.Throws<InvalidOperationException>(() => warehouseManagementLogic.ExportWarehouses());
        }

        [Test]
        public void ExportWarehouse_Valid_AllWarehousesReturned()
        {
            IWarehouseManagementLogic warehouseManagementLogic =
                new WarehouseManagementLogic { TriggerExportWarehouseException = false };
            Warehouse result = warehouseManagementLogic.ExportWarehouses();
            Assert.NotNull(result);
        }

        [Test]
        public void GetWarehouse_ValidCode_WarehouseReturned()
        {
            string code = "AUTA05";
            IWarehouseManagementLogic warehouseManagementLogic = new WarehouseManagementLogic();
            Warehouse result = warehouseManagementLogic.GetWarehouse(code);
            Assert.NotNull(result);
        }

        [Test]
        public void GetWarehouse_CodeNull_ExceptionThrown()
        {
            string code = null;
            IWarehouseManagementLogic warehouseManagementLogic = new WarehouseManagementLogic();
            Assert.Throws<ArgumentNullException>(() => warehouseManagementLogic.GetWarehouse(code));
        }

        [Test]
        public void ImportWarehouses_ValidWarehouse_NoException()
        {
            IWarehouseManagementLogic warehouseManagementLogic = new WarehouseManagementLogic();
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
            warehouseManagementLogic.ImportWarehouses(validWarehouse);
            Assert.True(true);
        }

        [Test]
        public void ImportWarehouses_InvalidWarehouseNextHopsNull_NoException()
        {
            IWarehouseManagementLogic warehouseManagementLogic = new WarehouseManagementLogic();
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
            IWarehouseManagementLogic warehouseManagementLogic = new WarehouseManagementLogic();
            Warehouse invalidWarehouse = null;
            Assert.Throws<ArgumentNullException>(() => warehouseManagementLogic.ImportWarehouses(invalidWarehouse));
            Assert.True(true);
        }
    }
}