using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;

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
            var controller = new WarehouseManagementApiController();
            
            var result = controller.ExportWarehouses();

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var warehouse = objectResult.Value as Warehouse;
            Assert.NotNull(warehouse);
        }
        
        [Test]
        public void ExportWarehouse_InternalError_ErrorReturned()
        {
            var controller = new WarehouseManagementApiController
            {
                triggerFaultyUnitTest = true
            };

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
            
            var controller = new WarehouseManagementApiController();
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
            
            var controller = new WarehouseManagementApiController();
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
                LocationCoordinates = new GeoCoordinate{Lat = 47.247829, Lon = 13.884382},
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            
            var controller = new WarehouseManagementApiController();

            var result = controller.ImportWarehouses(validWarehouse);

            var statusCode = result as StatusCodeResult;
            
            Assert.NotNull(statusCode);
            Assert.AreEqual(200, statusCode.StatusCode);
        }
        
        [Test]
        public void ImportWarehouses_NullWarehouse_ErrorReturned()
        {
            Warehouse validWarehouse = null;
            
            var controller = new WarehouseManagementApiController();

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