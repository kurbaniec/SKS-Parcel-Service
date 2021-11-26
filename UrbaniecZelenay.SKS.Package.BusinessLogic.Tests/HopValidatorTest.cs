using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class HopValidatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void HopValidator_ValidHop_Success()
        {
            var validHop = new Hop
            {
                Code = "AAAA111",
                Description = "Description",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Vienna",
                ProcessingDelayMins = 10
            };
            IValidator<Hop> hopValidator = new HopValidator();
            var validationResult = hopValidator.Validate(validHop);
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void HopValidator_InvalidCodeEmptyHop_ValidationFailed()
        {
            var validHop = new Hop
            {
                Code = "",
                Description = "Description",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Vienna",
                ProcessingDelayMins = 10
            };
            IValidator<Hop> hopValidator = new HopValidator();
            var validationResult = hopValidator.Validate(validHop);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopValidator_InvalidCodeHop_ValidationFailed()
        {
            var validHop = new Hop
            {
                Code = "1",
                Description = "Description",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Vienna",
                ProcessingDelayMins = 10
            };
            IValidator<Hop> hopValidator = new HopValidator();
            var validationResult = hopValidator.Validate(validHop);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopValidator_InvalidLocationCoordinatesNullHop_ValidationFailed()
        {
            var validHop = new Hop
            {
                Code = "AAAA111",
                Description = "Description",
                HopType = "Warehouse",
                LocationCoordinates = null,
                LocationName = "Vienna",
                ProcessingDelayMins = 10
            };
            IValidator<Hop> hopValidator = new HopValidator();
            var validationResult = hopValidator.Validate(validHop);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopValidator_InvalidDescriptionHop_ValidationFailed()
        {
            var validHop = new Hop
            {
                Code = "AAAA111",
                Description = "#desc!",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Vienna",
                ProcessingDelayMins = 10
            };
            IValidator<Hop> hopValidator = new HopValidator();
            var validationResult = hopValidator.Validate(validHop);
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        public void HopValidator_InvalidEmptyDescriptionHop_ValidationFailed()
        {
            var validHop = new Hop
            {
                Code = "AAAA111",
                Description = "",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Vienna",
                ProcessingDelayMins = 10
            };
            IValidator<Hop> hopValidator = new HopValidator();
            var validationResult = hopValidator.Validate(validHop);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}