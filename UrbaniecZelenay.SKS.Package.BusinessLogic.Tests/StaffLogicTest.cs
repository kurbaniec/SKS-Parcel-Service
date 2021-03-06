using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalRecipient = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Recipient;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;
using Parcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class StaffLogicTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReportParcelDelivery_TrackingIdNull_ExceptionThrown()
        {
            string trackingId = null;
            var mockLogger = new Mock<ILogger<StaffLogic>>();
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

            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetHopByCode(It.IsAny<string>())).Returns(new DalHop
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Root",
                ProcessingDelayMins = 186
            });


            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });
            Mock<ITransferWarehouseAgent> mockTransferWarehouseAgent = new Mock<ITransferWarehouseAgent>();
            mockTransferWarehouseAgent
                .Setup(m => m.TransferParcel(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<float>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(true);
            var mockWebhookManager = new Mock<IWebhookManager>();
            IStaffLogic staffLogic = new StaffLogic(
                mockParcelRepo.Object,
                mockWarehouseRepo.Object,
                mockTransferWarehouseAgent.Object,
                mockWebhookManager.Object,
                mapperConfig.CreateMapper(),
                mockLogger.Object
            );

            Assert.Throws<BlArgumentException>(() => staffLogic.ReportParcelDelivery(trackingId));
        }

        [Test]
        public void ReportParcelDelivery_ValidTrackingId_Success()
        {
            string trackingId = "PYJRB4HZ6";
            var mockLogger = new Mock<ILogger<StaffLogic>>();
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
                State = DalParcel.StateEnum.InTransportEnum,
                VisitedHops = new List<DalHopArrival>(),
                FutureHops = new List<DalHopArrival>()
            });
            mockParcelRepo.Setup(m => m.ChangeParcelState(It.IsAny<string>(), It.IsAny<DalParcel.StateEnum>())).Returns(
                new DalParcel
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
                    State = DalParcel.StateEnum.InTransportEnum,
                    VisitedHops = new List<DalHopArrival>(),
                    FutureHops = new List<DalHopArrival>()
                });
            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetHopByCode(It.IsAny<string>())).Returns(new DalHop
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Root",
                ProcessingDelayMins = 186
            });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            Mock<ITransferWarehouseAgent> mockTransferWarehouseAgent = new Mock<ITransferWarehouseAgent>();
            mockTransferWarehouseAgent
                .Setup(m => m.TransferParcel(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<float>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(true);
            var mockWebhookManager = new Mock<IWebhookManager>();
            IStaffLogic staffLogic = new StaffLogic(
                mockParcelRepo.Object,
                mockWarehouseRepo.Object,
                mockTransferWarehouseAgent.Object,
                mockWebhookManager.Object,
                mapperConfig.CreateMapper(),
                mockLogger.Object
            );

            staffLogic.ReportParcelDelivery(trackingId);
        }

        [Test]
        public void ReportParcelHop_TrackingIdNull_ExceptionThrown()
        {
            string trackingId = null;
            string code = "AUTA05";
            var mockLogger = new Mock<ILogger<StaffLogic>>();
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

            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetHopByCode(It.IsAny<string>())).Returns(new DalHop
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Root",
                ProcessingDelayMins = 186
            });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });
            Mock<ITransferWarehouseAgent> mockTransferWarehouseAgent = new Mock<ITransferWarehouseAgent>();
            mockTransferWarehouseAgent
                .Setup(m => m.TransferParcel(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<float>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(true);
            var mockWebhookManager = new Mock<IWebhookManager>();
            IStaffLogic staffLogic = new StaffLogic(
                mockParcelRepo.Object,
                mockWarehouseRepo.Object,
                mockTransferWarehouseAgent.Object,
                mockWebhookManager.Object,
                mapperConfig.CreateMapper(),
                mockLogger.Object
            );

            Assert.Throws<BlArgumentException>(() => staffLogic.ReportParcelHop(trackingId, code));
        }

        [Test]
        public void ReportParcelHop_CodeNull_ExceptionThrown()
        {
            string trackingId = "PYJRB4HZ6";
            string code = null;
            var mockLogger = new Mock<ILogger<StaffLogic>>();
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

            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetHopByCode(It.IsAny<string>())).Returns(new DalHop
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Root",
                ProcessingDelayMins = 186
            });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });
            Mock<ITransferWarehouseAgent> mockTransferWarehouseAgent = new Mock<ITransferWarehouseAgent>();
            mockTransferWarehouseAgent
                .Setup(m => m.TransferParcel(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<float>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(true);
            var mockWebhookManager = new Mock<IWebhookManager>();
            IStaffLogic staffLogic = new StaffLogic(
                mockParcelRepo.Object,
                mockWarehouseRepo.Object,
                mockTransferWarehouseAgent.Object,
                mockWebhookManager.Object,
                mapperConfig.CreateMapper(),
                mockLogger.Object
            );

            Assert.Throws<BlArgumentException>(() => staffLogic.ReportParcelHop(trackingId, code));
        }

        [Test]
        public void ReportParcelHop_ValidTrackingIdAndCode_Success()
        {
            string trackingId = "PYJRB4HZ6";
            string code = "AUTA01";
            var mockLogger = new Mock<ILogger<StaffLogic>>();
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
                FutureHops = new List<DalHopArrival>
                {
                    new HopArrival
                    {
                        DateTime = null,
                        Hop = new DalHop
                        {
                            Code = "AUTA01",
                            Description = "Root Warehouse - Österreich",
                            HopType = "Warehouse",
                            LocationCoordinates = new Point(13.884382, 47.247829),
                            LocationName = "Root",
                            ProcessingDelayMins = 186
                        }
                    }
                }
            });

            Mock<IWarehouseRepository> mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(m => m.GetHopByCode(It.IsAny<string>())).Returns(new DalHop
            {
                Code = "AUTA01",
                Description = "Root Warehouse - Österreich",
                HopType = "Warehouse",
                LocationCoordinates = new Point(13.884382, 47.247829),
                LocationName = "Root",
                ProcessingDelayMins = 186
            });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });

            Mock<ITransferWarehouseAgent> mockTransferWarehouseAgent = new Mock<ITransferWarehouseAgent>();
            mockTransferWarehouseAgent
                .Setup(m => m.TransferParcel(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<float>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(true);
            var mockWebhookManager = new Mock<IWebhookManager>();
            IStaffLogic staffLogic = new StaffLogic(
                mockParcelRepo.Object,
                mockWarehouseRepo.Object,
                mockTransferWarehouseAgent.Object,
                mockWebhookManager.Object,
                mapperConfig.CreateMapper(),
                mockLogger.Object
            );

            staffLogic.ReportParcelHop(trackingId, code);
        }

        // [Test]
        // public void TrackParcel_ValidTrackingId_ParcelReturned()
        // {
        //     string trackingId = "PYJRB4HZ6";
        //     var mockLogger = new Mock<ILogger<StaffLogic>>();
        //     Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
        //     mockParcelRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Returns(new DalParcel
        //     {
        //         TrackingId = "PYJRB4HZ6",
        //         Weight = 1,
        //         Recipient = new DalRecipient
        //         {
        //             Name = "Max Mustermann",
        //             Street = "A Street",
        //             PostalCode = "1200",
        //             City = "Vienna",
        //             Country = "Austria"
        //         },
        //         Sender = new DalRecipient
        //         {
        //             Name = "Max Mustermann",
        //             Street = "A Street",
        //             PostalCode = "1200",
        //             City = "Vienna",
        //             Country = "Austria"
        //         },
        //         State = DalParcel.StateEnum.TransferredEnum,
        //         VisitedHops = new List<DalHopArrival>(),
        //         FutureHops = new List<DalHopArrival>()
        //     });
        //     var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });
        //     IRecipientLogic recipientLogic = new RecipientLogic(mockLogger.Object, mockParcelRepo.Object, mapperConfig.CreateMapper());
        //     Parcel blParcel = recipientLogic.TrackParcel(trackingId);
        //     IValidator<Parcel> parcelValidator = new ParcelValidator();
        //     var validationResult = parcelValidator.Validate(blParcel);
        //     Assert.IsTrue(validationResult.IsValid);
        // }
        //
        // [Test]
        // public void TrackParcel_TrackingIdNull_ExceptionThrown()
        // {
        //     string trackingId = null;
        //     Mock<IParcelRepository> mockParcelRepo = new Mock<IParcelRepository>();
        //     mockParcelRepo.Setup(m => m.GetByTrackingId(It.IsAny<string>())).Returns(new DalParcel
        //     {
        //         TrackingId = "PYJRB4HZ6",
        //         Weight = 1,
        //         Recipient = new DalRecipient
        //         {
        //             Name = "Max Mustermann",
        //             Street = "A Street",
        //             PostalCode = "1200",
        //             City = "Vienna",
        //             Country = "Austria"
        //         },
        //         Sender = new DalRecipient
        //         {
        //             Name = "Max Mustermann",
        //             Street = "A Street",
        //             PostalCode = "1200",
        //             City = "Vienna",
        //             Country = "Austria"
        //         },
        //         State = DalParcel.StateEnum.TransferredEnum,
        //         VisitedHops = new List<DalHopArrival>(),
        //         FutureHops = new List<DalHopArrival>()
        //     });
        //     var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingsProfileBlDal()); });
        //     IRecipientLogic recipientLogic = new RecipientLogic(mockParcelRepo.Object, mapperConfig.CreateMapper());
        //     Assert.Throws<ArgumentNullException>(() => recipientLogic.TrackParcel(trackingId));
        // }
    }
}