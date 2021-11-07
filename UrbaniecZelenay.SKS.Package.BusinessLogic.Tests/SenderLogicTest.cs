using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentValidation;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
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
    public class SenderLogicTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SubmitParcel_ValidParcel_ParcelReturned()
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

            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DalParcel>())).Returns(new DalParcel
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
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfileBlDal());
            });

            
            ISenderLogic senderLogic = new SenderLogic(mockParcelRepo.Object, mapperConfig.CreateMapper());
            Parcel blParcel = senderLogic.SubmitParcel(validParcel);
            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(blParcel);
            Assert.IsTrue(validationResult.IsValid);
        }
        
        [Test]
        public void SubmitParcel_ParcelNull_ParcelReturned()
        {
            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DalParcel>())).Returns(new DalParcel
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
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfileBlDal());
            });

            
            ISenderLogic senderLogic = new SenderLogic(mockParcelRepo.Object, mapperConfig.CreateMapper());
            Assert.Throws<ArgumentNullException>(() => senderLogic.SubmitParcel(null));
        }
        
        [Test]
        public void SubmitParcel_InvalidParcelSenderNull_ParcelReturned()
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

            Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DalParcel>())).Returns(new DalParcel
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
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfileBlDal());
            });

            
            ISenderLogic senderLogic = new SenderLogic(mockParcelRepo.Object, mapperConfig.CreateMapper());
            Assert.Throws<ArgumentException>(() => senderLogic.SubmitParcel(validParcel));
        } 
    }
}