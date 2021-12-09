using System;
using System.Data.Common;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly ITransferWarehouseAgent transferWarehouseAgent;
        private readonly IWebhookManager webhookManager;
        private readonly IMapper mapper;
        private readonly ILogger<StaffLogic> logger;

        public StaffLogic(
            IParcelRepository parcelRepository,
            IWarehouseRepository warehouseRepository,
            ITransferWarehouseAgent transferWarehouseAgent,
            IWebhookManager webhookManager,
            IMapper mapper,
            ILogger<StaffLogic> logger
        )
        {
            this.parcelRepository = parcelRepository;
            this.warehouseRepository = warehouseRepository;
            this.transferWarehouseAgent = transferWarehouseAgent;
            this.webhookManager = webhookManager;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        public void ReportParcelDelivery(string? trackingId)
        {
            logger.LogInformation($"Report Parcel Delivery for ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new BlArgumentException("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            DalParcel? dalParcel = null;
            try
            {
                dalParcel = parcelRepository.GetByTrackingId(trackingId);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving parcel by Id ({trackingId}).");
                throw new BlRepositoryException($"Error retrieving parcel by Id ({trackingId}).", e);
            }

            if (dalParcel == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error parcel with trackingId ({trackingId}) not found");
                logger.LogError(e, "Parcel by trackingId not found");
                throw e;
            }

            var blParcel = mapper.Map<Parcel>(dalParcel);
            if (blParcel.FutureHops.Count != 0)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error cannot deliver Parcel ({blParcel}) with future hops.");
                logger.LogError(e, "Cannot deliver parcel which has future hops");
                throw e;
            }

            // blParcel.State = Parcel.StateEnum.DeliveredEnum;

            dalParcel = mapper.Map<DalParcel>(blParcel);
            logger.LogDebug($"Mapping Bl/Dal {blParcel} => {dalParcel}");
            try
            {
                // parcelRepository.Update(dalParcel);
                var parcel = parcelRepository.ChangeParcelState(dalParcel.TrackingId!, DalParcel.StateEnum.DeliveredEnum);
                webhookManager.NotifyParcelWebhooks(parcel);
                webhookManager.UnsubscribeAllParcelWebhooks(parcel.TrackingId!);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error updating parcel by ({dalParcel}).");
                throw new BlRepositoryException($"Error updating parcel by ({dalParcel}).", e);
            }
        }

        public void ReportParcelHop(string? trackingId, string? code)
        {
            logger.LogInformation($"Report Parcel Hop for Parcel with ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new BlArgumentException("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            if (code == null)
            {
                BlArgumentException e = new BlArgumentException("code must not be null.");
                logger.LogError(e, "code is null");
                throw e;
            }

            // TODO test this with database
            // property hop must be generated from BlHopArrvial info

            // Query hop from DAL
            // Convert to BL 
            // Create HopArrival from it
            // Add HopArrival to parcel
            // Convert to DAL

            DalParcel? dalParcel = null;
            try
            {
                dalParcel = parcelRepository.GetByTrackingId(trackingId);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving parcel by Id ({trackingId}).");
                throw new BlRepositoryException($"Error retrieving parcel by Id ({trackingId}).", e);
            }

            if (dalParcel == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error parcel with trackingId ({trackingId}) not found");
                logger.LogError(e, "Parcel by trackingId not found");
                throw e;
            }

            DalHop? dalHop = null;
            try
            {
                dalHop = warehouseRepository.GetHopByCode(code);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving hop by code ({code}).");
                throw new BlRepositoryException($"Error retrieving hop by code ({code}).", e);
            }

            if (dalHop == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error hop with code ({code}) not found");
                logger.LogError(e, "Hop by code not found");
                throw e;
            }

            var blParcel = mapper.Map<Parcel>(dalParcel);
            var blHop = mapper.Map<Hop>(dalHop);
            if (blParcel.FutureHops.Count > 0 && blHop.Code != blParcel.FutureHops[0].Code)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException(
                        $"Error hop with code ({code}) is not the next Hop of Parcel ({blParcel})");
                logger.LogError(e, "Hop is not future Hop.");
                throw e;
            }

            var hopArrival = new HopArrival()
            {
                Hop = blHop,
                DateTime = DateTime.Now,
            };

            // var visitedHop = blParcel.FutureHops[0];
            // visitedHop.DateTime = DateTime.UtcNow;
            // logger.LogDebug($"Adding Current Hop to future hops.");
            // blParcel.FutureHops.RemoveAt(0);
            // blParcel.VisitedHops.Add(visitedHop);

            try
            {
                if (blHop is Warehouse)
                {
                    var parcel = parcelRepository.ChangeParcelState(blParcel.TrackingId!, DalParcel.StateEnum.InTransportEnum);
                    webhookManager.NotifyParcelWebhooks(parcel);
                    // blParcel.State = Parcel.StateEnum.InTransportEnum;
                }
                else if (blHop is Truck)
                {
                    var parcel = parcelRepository.ChangeParcelState(blParcel.TrackingId!, DalParcel.StateEnum.InTruckDeliveryEnum);
                    webhookManager.NotifyParcelWebhooks(parcel);
                    // blParcel.State = Parcel.StateEnum.InTruckDeliveryEnum;
                }
                else if (blHop is Transferwarehouse blTransferwarhouse)
                {
                    // blParcel.State = Parcel.StateEnum.TransferredEnum;

                    bool hasSucceeded = transferWarehouseAgent.TransferParcel(
                        baseUrl: blTransferwarhouse.LogisticsPartnerUrl, trackingId: trackingId, weight: blParcel.Weight,
                        recipientName: blParcel.Recipient.Name, recipientStreet: blParcel.Recipient.Street,
                        recipientPostalCode: blParcel.Recipient.PostalCode, recipientCity: blParcel.Recipient.City,
                        recipientCountry: blParcel.Recipient.Country,
                        senderName: blParcel.Sender.Name, senderStreet: blParcel.Sender.Street,
                        senderPostalCode: blParcel.Sender.PostalCode, senderCity: blParcel.Sender.City,
                        senderCountry: blParcel.Sender.Country);
                    if (!hasSucceeded)
                    {
                        BlArgumentException e = new BlArgumentException(
                            $"Error occured while transferring parcel: {blParcel} to transfer warehouse: {blTransferwarhouse}.");
                        logger.LogError(e, "code is null");
                        throw e;
                    }

                    var parcel = parcelRepository.ChangeParcelState(blParcel.TrackingId!, DalParcel.StateEnum.TransferredEnum);
                    webhookManager.NotifyParcelWebhooks(parcel);
                }
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error updating parcel's state ({dalParcel}).");
                throw new BlRepositoryException($"Error updating parcel's state ({dalParcel}).", e);
            }

            logger.LogDebug($"Adding Current Hop to future hops.");

            dalParcel = mapper.Map<DalParcel>(blParcel);
            logger.LogDebug($"Mapping Bl/Dal {blParcel} => {dalParcel}");
            try
            {
                parcelRepository.AddFutureHopToVisited(blParcel.TrackingId!, DateTime.UtcNow);

                // parcelRepository.Update(dalParcel);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error updating parcel's state ({dalParcel}).");
                throw new BlRepositoryException($"Error updating parcel's state ({dalParcel}).", e); 
            }
        }
    }
}