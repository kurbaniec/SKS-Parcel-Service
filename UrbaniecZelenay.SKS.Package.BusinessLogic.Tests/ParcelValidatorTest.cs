using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParcelValidatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParcelValidator_ValidParcel_Success()
        {
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsTrue(validationResult.IsValid);
        }
        
        [Test]
        public void ParcelValidator_InvalidTrackingIdParcel_ValidationFailed()
        {
            var validParcel = new Parcel
            {
                TrackingId = "ASD",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsFalse(validationResult.IsValid);
        }
        
        [Test]
        public void ParcelValidator_InvalidRecipientParcel_ValidationFailed()
        {
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = null,
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsFalse(validationResult.IsValid);
        }
        
        [Test]
        public void ParcelValidator_InvalidSenderParcel_ValidationFailed()
        {
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = null,
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsFalse(validationResult.IsValid);
        }
        [Test]
        public void ParcelValidator_InvalidStateParcel_ValidationFailed()
        {
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = null,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsFalse(validationResult.IsValid);
        }
        [Test]
        public void ParcelValidator_InvalidVisitedHopsParcel_ValidationFailed()
        {
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = null,
                FutureHops = new List<HopArrival>()
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsFalse(validationResult.IsValid);
        }
        [Test]
        public void ParcelValidator_InvalidFutureHopsParcel_ValidationFailed()
        {
            var validParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = null
            };

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(validParcel);
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}