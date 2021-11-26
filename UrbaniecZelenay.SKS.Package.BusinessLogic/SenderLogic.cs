using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;
using DalTruck = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Truck;
using DalTransferwarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Transferwarehouse;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;


namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IGeoEncodingAgent geoEncodingAgent;
        private readonly IMapper mapper;
        private readonly ILogger<SenderLogic> logger;

        public SenderLogic(
            ILogger<SenderLogic> logger, IParcelRepository parcelRepository, IWarehouseRepository warehouseRepository,
            IGeoEncodingAgent geoEncodingAgent, IMapper mapper
        )
        {
            this.logger = logger;
            this.parcelRepository = parcelRepository;
            this.warehouseRepository = warehouseRepository;
            this.geoEncodingAgent = geoEncodingAgent;
            this.mapper = mapper;
        }

        public Parcel SubmitParcel(Parcel? body)
        {
            logger.LogInformation($"Submit Parcel {body}");
            if (body == null)
            {
                BlArgumentException e = new BlArgumentException("parcel must not be null.");
                logger.LogError(e, "parcel is null");
                throw e;
            }

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(body);
            if (!validationResult.IsValid)
            {
                string validationErrors = string.Join(Environment.NewLine, validationResult.Errors);
                BlException e = new BlValidationException(validationErrors);
                logger.LogError(e, $"Error validating parcel");
                throw e;
            }

            // Get GeoSpatial Data for Recipients
            var recipient = body.Recipient;
            var recipientGeoLocation = geoEncodingAgent.EncodeAddress(recipient.Street, recipient.PostalCode,
                recipient.City, recipient.Country);
            if (recipientGeoLocation == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Cannot find GeoLocation for Recipient with Address {recipient}");
                logger.LogWarning(e, "Cannot find GeoLocation");
                throw e;
            }

            var sender = body.Sender;
            var senderGeoLocation = geoEncodingAgent.EncodeAddress(sender.Street, sender.PostalCode,
                sender.City, sender.Country);
            if (senderGeoLocation == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Cannot find GeoLocation for Sender with Address {recipient}");
                logger.LogWarning(e, "Cannot find GeoLocation");
                throw e;
            }

            body.Recipient.GeoLocation = recipientGeoLocation;
            body.Sender.GeoLocation = senderGeoLocation;

            // Predict future Hops
            var recipientFutureHops = new List<DalHopArrival>();
            var senderFutureHops = new List<DalHopArrival>();

            DalHop? recipientStart = warehouseRepository.GetTruckByPoint(recipientGeoLocation);
            if (recipientStart == null)
            {
                recipientStart = warehouseRepository.GetTransferwarehouseByPoint(recipientGeoLocation);
                if (recipientStart == null)
                {
                    BlDataNotFoundException e = new(
                        $"Cannot find Truck or Transferwarehouse for Recipient with GeoLocation {recipientGeoLocation}");
                    logger.LogWarning(e, "Cannot find GeoLocation");
                    throw e;
                }
            }

            if (recipientStart is DalTransferwarehouse)
            {
                recipientFutureHops.Add(new DalHopArrival
                {
                    Hop = recipientStart
                });
                recipientStart = recipientStart.PreviousHop;
            }

            var currentRecipientHop = recipientStart;

            DalHop? senderStart = warehouseRepository.GetTruckByPoint(recipientGeoLocation);
            if (senderStart == null)
            {
                BlDataNotFoundException e = new(
                    $"Cannot find Truck or Transferwarehouse for Sender with GeoLocation {senderGeoLocation}");
                logger.LogWarning(e, "Cannot find GeoLocation");
                throw e;
            }

            var currentSenderHop = senderStart;

            while (currentRecipientHop != currentSenderHop &&
                   currentSenderHop != null && currentRecipientHop != null)
            {
                recipientFutureHops.Add(new DalHopArrival
                {
                    Hop = currentRecipientHop
                });
                currentRecipientHop = currentRecipientHop.PreviousHop;
                senderFutureHops.Add(new DalHopArrival
                {
                    Hop = currentSenderHop
                });
                currentSenderHop = currentSenderHop.PreviousHop;
            }

            if (currentSenderHop == null || currentRecipientHop == null)
            {
                BlDataNotFoundException e = new(
                    $"Could not find future Hops Route with Sender {senderGeoLocation} and Recipient {recipientGeoLocation}");
                logger.LogWarning(e, "Cannot find future Hops");
                throw e;
            }

            senderFutureHops.Add(new DalHopArrival
            {
                Hop = currentSenderHop
            });
            recipientFutureHops.Reverse();
            senderFutureHops.AddRange(recipientFutureHops);

            var dalParcel = mapper.Map<DalParcel>(body);
            dalParcel.FutureHops = senderFutureHops;

            logger.LogDebug($"Mapping Bl/Dal {body} => {dalParcel}");
            try
            {
                dalParcel = parcelRepository.Create(dalParcel);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error creating parcel ({dalParcel}).");
                throw new BlRepositoryException($"Error creating parcel ({dalParcel}).", e);
            }


            // return new Parcel
            // {
            //     TrackingId = "PYJRB4HZ6",
            //     Weight = 1,
            //     Recipient = new Recipient
            //     {
            //         Name = "Max Mustermann",
            //         Street = "A Street",
            //         PostalCode = "1200",
            //         City = "Vienna",
            //         Country = "Austria"
            //     },
            //     Sender = new Recipient
            //     {
            //         Name = "Max Mustermann",
            //         Street = "A Street",
            //         PostalCode = "1200",
            //         City = "Vienna",
            //         Country = "Austria"
            //     },
            //     State = Parcel.StateEnum.InTransportEnum,
            //     VisitedHops = new List<HopArrival>(),
            //     FutureHops = new List<HopArrival>()
            // };
            var blResult = mapper.Map<Parcel>(dalParcel);
            logger.LogDebug($"Mapping Dal/Bl {dalParcel} => {blResult}");
            return blResult;
        }
    }
}