using System;
using System.Data.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IMapper mapper;
        private readonly ILogger<StaffLogic> logger;

        public StaffLogic(ILogger<StaffLogic> logger, IParcelRepository parcelRepository,
            IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            this.logger = logger;
            this.parcelRepository = parcelRepository;
            this.warehouseRepository = warehouseRepository;
            this.mapper = mapper;
        }

        // TODO create NotFoundException
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
            blParcel.State = Parcel.StateEnum.DeliveredEnum;
            dalParcel = mapper.Map<DalParcel>(blParcel);
            logger.LogDebug($"Mapping Bl/Dal {blParcel} => {dalParcel}");
            try
            {
                parcelRepository.Update(dalParcel);
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

            var hopArrival = new HopArrival()
            {
                Code = blHop.Code,
                DateTime = DateTime.Now,
                Description = blHop.Description
            };
            blParcel.VisitedHops.Add(hopArrival);

            dalParcel = mapper.Map<DalParcel>(blParcel);
            logger.LogDebug($"Mapping Bl/Dal {blParcel} => {dalParcel}");
            try
            {
                parcelRepository.Update(dalParcel);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error updating parcel ({dalParcel}).");
                throw new BlRepositoryException($"Error updating parcel ({dalParcel}).", e);
            }
        }
    }
}