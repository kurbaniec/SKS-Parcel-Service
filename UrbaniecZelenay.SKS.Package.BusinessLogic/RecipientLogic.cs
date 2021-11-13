using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

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
                logger.LogError("ID is null");
                throw new ArgumentNullException(nameof(trackingId));
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
            var dalResult = parcelRepository.GetByTrackingId(trackingId);
            if (dalResult == null) return null;
            var blResult = mapper.Map<Parcel>(dalResult);
            logger.LogDebug($"Mapping Dal/Bl {dalResult} => {blResult}");
            return blResult;
        }
    }
}