using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHopsValidatorTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void WarehouseNextHopsValidator_ValidWarehouseNextHops_Succeed()
        {
            var validWarehouseNextHops = new WarehouseNextHops
            { 
                Hop = new Hop(),
                TraveltimeMins = 10
            };
            IValidator<WarehouseNextHops> warehouseNextHopsValidator = new WarehouseNextHopsValidator();
            var validationResult = warehouseNextHopsValidator.Validate(validWarehouseNextHops);
            Assert.IsTrue(validationResult.IsValid);
        }
        
        [Test]
        public void WarehouseNextHopsValidator_InvalidWarehouseNextHops_ValidationFailed()
        {
            var invalidWarehouseNextHops = new WarehouseNextHops
            { 
                Hop = null,
                TraveltimeMins = 10
            };
            IValidator<WarehouseNextHops> warehouseNextHopsValidator = new WarehouseNextHopsValidator();
            var validationResult = warehouseNextHopsValidator.Validate(invalidWarehouseNextHops);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}