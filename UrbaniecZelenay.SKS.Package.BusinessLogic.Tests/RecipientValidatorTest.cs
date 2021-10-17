using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class RecipientValidatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RecipientValidator_ValidRecipient_Success()
        {
            var validRecipient = new Recipient
            {
                City = "Wien",
                Country = "Österreich",
                Name = "Name",
                Street = "Musterstraße 1",
                PostalCode = "A-1010"
            };
            IValidator<Recipient> recipientValidator = new RecipientValidator();
            var validationResult = recipientValidator.Validate(validRecipient);
            Assert.IsTrue(validationResult.IsValid);
        }
        [Test]
        public void RecipientValidator_InvalidStreetRecipient_ValidationFailed()
        {
            var validRecipient = new Recipient
            {
                City = "Wien",
                Country = "Österreich",
                Name = "Name",
                Street = "Musterstraße",
                PostalCode = "A-1010"
            };
            IValidator<Recipient> recipientValidator = new RecipientValidator();
            var validationResult = recipientValidator.Validate(validRecipient);
            Assert.IsFalse(validationResult.IsValid);
        }
        
        [Test]
        public void RecipientValidator_InvalidPostalCodeRecipient_ValidationFailed()
        {
            var validRecipient = new Recipient
            {
                City = "Wien",
                Country = "Österreich",
                Name = "Name",
                Street = "Musterstraße 1",
                PostalCode = "10010"
            };
            IValidator<Recipient> recipientValidator = new RecipientValidator();
            var validationResult = recipientValidator.Validate(validRecipient);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}