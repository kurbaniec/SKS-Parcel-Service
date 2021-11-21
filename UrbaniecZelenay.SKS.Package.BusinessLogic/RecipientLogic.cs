using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class RecipientLogic : IRecipientLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RecipientLogic> logger;

        public RecipientLogic(ILogger<RecipientLogic> logger, IParcelRepository parcelRepository, IMapper mapper)
        {
            this.logger = logger;
            this.parcelRepository = parcelRepository;
            this.mapper = mapper;
        }

        public Parcel? TrackParcel(string? trackingId)
        {
            logger.LogInformation($"Track Parcel with ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new BlArgumentException("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            // return new Parcel
            // {
            //     TrackingId = trackingId,
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
            DalParcel? dalResult = null;
            try
            {
                dalResult = parcelRepository.GetByTrackingId(trackingId);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving parcel by Id ({trackingId}).");
                throw new BlRepositoryException($"Error retrieving parcel by Id ({trackingId}).", e);
            }

            if (dalResult == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error parcel with tracking id ({trackingId}) not found");
                logger.LogError(e, "Error parcel with tracking id ({trackingId}) not found");
                throw e;
            }

            var blResult = mapper.Map<Parcel>(dalResult);
            logger.LogDebug($"Mapping Dal/Bl {dalResult} => {blResult}");
            return blResult;
        }
    }
}