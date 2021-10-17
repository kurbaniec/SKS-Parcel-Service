using System;
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
    public class LogisticsPartnerLogicTest
    {   
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransitionParcel_WeightLessThanZero_ExceptionThrown()
        {
            var invalidParcel = new Parcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = -1,
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

            ILogisticsPartnerLogic logisticsPartnerLogic = new LogisticsPartnerLogic();
            Assert.Throws<ArgumentException>(() => logisticsPartnerLogic.TransitionParcel(invalidParcel));
        }
        [Test]
        public void TransitionParcel_ParcelNull_ExceptionThrown()
        {
            Parcel invalidParcel = null;

            ILogisticsPartnerLogic logisticsPartnerLogic = new LogisticsPartnerLogic();
            Assert.Throws<ArgumentNullException>(() => logisticsPartnerLogic.TransitionParcel(invalidParcel));
        } 
        [Test]
        public void TransitionParcel_ValidParcel_NewParcelReturned()
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

            ILogisticsPartnerLogic logisticsPartnerLogic = new LogisticsPartnerLogic();
            Parcel blParcel = logisticsPartnerLogic.TransitionParcel(validParcel);
            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(blParcel);
            Assert.IsTrue(validationResult.IsValid);
        }
    }
}