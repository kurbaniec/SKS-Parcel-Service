using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalRecipient = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Recipient;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class RecipientLogicTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TrackParcel_ValidTrackingId_ParcelReturned()
        {
            string trackingId = "PYJRB4HZ6";
            var mockLogger = new Mock<ILogger<RecipientLogic>>();
            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Returns(new DalParcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = DalParcel.StateEnum.TransferredEnum,
                VisitedHops = new List<DalHopArrival>(),
                FutureHops = new List<DalHopArrival>()
            });
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IRecipientLogic recipientLogic =
                new RecipientLogic(mockLogger.Object, mockParcelRepo.Object, mapperConfig.CreateMapper());
            Parcel blParcel = recipientLogic.TrackParcel(trackingId);
            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(blParcel);
            Assert.IsTrue(validationResult.IsValid);
        }

        [Test]
        public void TrackParcel_TrackingIdNull_ExceptionThrown()
        {
            string trackingId = null;
            var mockLogger = new Mock<ILogger<RecipientLogic>>();
            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Returns(new DalParcel
            {
                TrackingId = "PYJRB4HZ6",
                Weight = 1,
                Recipient = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new DalRecipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = DalParcel.StateEnum.TransferredEnum,
                VisitedHops = new List<DalHopArrival>(),
                FutureHops = new List<DalHopArrival>()
            });
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            IRecipientLogic recipientLogic = new RecipientLogic(mockLogger.Object, mockParcelRepo.Object, mapperConfig.CreateMapper());
            Assert.Throws<BlArgumentException>(() => recipientLogic.TrackParcel(trackingId));
        }
    }
}