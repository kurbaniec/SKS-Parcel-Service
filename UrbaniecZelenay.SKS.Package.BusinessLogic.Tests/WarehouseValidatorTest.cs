using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class WarehouseValidatorTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void WarehouseValidator_ValidWarehouse_Succeed()
        {
            var validWarehouseNextHops = new Warehouse
            {
                Code = "AAAA1",
                NextHops = new List<WarehouseNextHops>(),
                Description = "Test",
                LocationCoordinates = new GeoCoordinate()
            };
            IValidator<Warehouse> warehouseValidator = new WarehouseValidator();
            var validationResult = warehouseValidator.Validate(validWarehouseNextHops);
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void WarehouseValidator_InvalidNextHopsNullWarehouse_ValidationFailed()
        {
            var invalidWarehouseNextHops = new Warehouse
            {
                Code = "AAAA1",
                NextHops = null,
                Description = "Test",
                LocationCoordinates = new GeoCoordinate()
            };
            IValidator<Warehouse> warehouseValidator = new WarehouseValidator();
            var validationResult = warehouseValidator.Validate(invalidWarehouseNextHops);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}