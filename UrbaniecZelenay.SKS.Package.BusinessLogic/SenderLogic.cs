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
            ILogger<SenderLogic> logger, IParcelRepository parcelRepository, 
            IWarehouseRepository warehouseRepository,
            IGeoEncodingAgent geoEncodingAgent, IMapper mapper
        )
        {
            this.logger = logger;
            this.parcelRepository = parcelRepository;
            this.warehouseRepository = warehouseRepository;
            this.geoEncodingAgent = geoEncodingAgent;
            this.mapper = mapper;
        }

        public Parcel SubmitParcel(Parcel? parcel)
        {
            logger.LogInformation($"Submit Parcel {parcel}");
            if (parcel == null)
            {
                BlArgumentException e = new BlArgumentException("parcel must not be null.");
                logger.LogError(e, "parcel is null");
                throw e;
            }

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(parcel);
            if (!validationResult.IsValid)
            {
                string validationErrors = string.Join(Environment.NewLine, validationResult.Errors);
                BlException e = new BlValidationException(validationErrors);
                logger.LogError(e, $"Error validating parcel");
                throw e;
            }

            // Get GeoSpatial Data for Recipients
            var recipient = parcel.Recipient;
            var recipientGeoLocation = geoEncodingAgent.EncodeAddress(recipient.Street, recipient.PostalCode,
                recipient.City, recipient.Country);
            if (recipientGeoLocation == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Cannot find GeoLocation for Recipient with Address {recipient}");
                logger.LogWarning(e, "Cannot find GeoLocation");
                throw e;
            }

            var sender = parcel.Sender;
            var senderGeoLocation = geoEncodingAgent.EncodeAddress(sender.Street, sender.PostalCode,
                sender.City, sender.Country);
            if (senderGeoLocation == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Cannot find GeoLocation for Sender with Address {recipient}");
                logger.LogWarning(e, "Cannot find GeoLocation");
                throw e;
            }

            parcel.Recipient.GeoLocation = recipientGeoLocation;
            parcel.Sender.GeoLocation = senderGeoLocation;

            // Predict future Hops
            // Get recipient Hop
            // Can be a Truck or Transferwarehouse
            DalHop? dalCurrentRecipientHop = warehouseRepository.GetTruckByPoint(recipientGeoLocation);
            if (dalCurrentRecipientHop == null)
            {
                dalCurrentRecipientHop = warehouseRepository.GetTransferwarehouseByPoint(recipientGeoLocation);
                if (dalCurrentRecipientHop == null)
                {
                    BlDataNotFoundException e = new(
                        $"Cannot find Truck or Transferwarehouse for Recipient with GeoLocation {recipientGeoLocation}");
                    logger.LogWarning(e, "Cannot find GeoLocation");
                    throw e;
                }
            }
            var currentRecipientHop = mapper.Map<Hop>(dalCurrentRecipientHop);
            
            // Get sender Hop
            // Can only be a Truck
            DalHop? dalCurrentSenderHop = warehouseRepository.GetTruckByPoint(senderGeoLocation);
            if (dalCurrentSenderHop == null)
            {
                BlDataNotFoundException e = new(
                    $"Cannot find Truck or Transferwarehouse for Sender with GeoLocation {senderGeoLocation}");
                logger.LogWarning(e, "Cannot find GeoLocation");
                throw e;
            }
            var currentSenderHop = mapper.Map<Hop>(dalCurrentSenderHop);
            
            var futureHops = SharedLogic.PredictFutureHops(currentRecipientHop, currentSenderHop);
            // Check if Route was found
            if (futureHops == null)
            {
                BlDataNotFoundException e = new(
                    $"Could not find future Hops Route with Sender {senderGeoLocation} and Recipient {recipientGeoLocation}");
                logger.LogWarning(e, "Cannot find future Hops");
                throw e;
            }
            var dalFutureHops = mapper.Map<List<DalHopArrival>>(futureHops);

            // Set parcel state to "Pickup"
            parcel.State = Parcel.StateEnum.PickupEnum;

            // Convert to DAL DAO and add future Hops
            var dalParcel = mapper.Map<DalParcel>(parcel);
            dalParcel.FutureHops = dalFutureHops;

            logger.LogDebug($"Mapping Bl/Dal {parcel} => {dalParcel}");
            try
            {
                dalParcel = parcelRepository.Create(dalParcel);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error creating parcel ({dalParcel}).");
                throw new BlRepositoryException($"Error creating parcel ({dalParcel}).", e);
            }

            var blResult = mapper.Map<Parcel>(dalParcel);
            logger.LogDebug($"Mapping Dal/Bl {dalParcel} => {blResult}");
            return blResult;
        }
    }
}