using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class HopArrivalValidatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void HopArrivalValidator_ValidHop_Success()
        {
            var validHopArrival = new HopArrival
            {
                Code = "AAAA11",
                DateTime = new DateTime(2021, 1, 1, 0, 0, 0),
                Description = "Description"
            };
            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            var validationResult = hopArrivalValidator.Validate(validHopArrival);
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void HopArrivalValidator_InvalidCodeHop_ValidationFailed()
        {
            var validHopArrival = new HopArrival
            {
                Code = "123",
                DateTime = new DateTime(2021, 1, 1, 0, 0, 0),
                Description = "Description"
            };
            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            var validationResult = hopArrivalValidator.Validate(validHopArrival);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopArrivalValidator_InvalidEmptyCodeHop_ValidationFailed()
        {
            var validHopArrival = new HopArrival
            {
                Code = "",
                DateTime = new DateTime(2021, 1, 1, 0, 0, 0),
                Description = "Description"
            };
            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            var validationResult = hopArrivalValidator.Validate(validHopArrival);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopArrivalValidator_InvalidDescriptionHop_ValidationFailed()
        {
            var validHopArrival = new HopArrival
            {
                Code = "AAAA11",
                DateTime = new DateTime(2021, 1, 1, 0, 0, 0),
                Description = "#desc!"
            };
            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            var validationResult = hopArrivalValidator.Validate(validHopArrival);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopArrivalValidator_InvalidDescriptionEmptyHop_ValidationFailed()
        {
            var validHopArrival = new HopArrival
            {
                Code = "AAAA11",
                DateTime = new DateTime(2021, 1, 1, 0, 0, 0),
                Description = ""
            };
            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            var validationResult = hopArrivalValidator.Validate(validHopArrival);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}