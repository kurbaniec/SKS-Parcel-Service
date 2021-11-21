using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IMapper mapper;
        private readonly ILogger<LogisticsPartnerLogic> logger;

        public LogisticsPartnerLogic(ILogger<LogisticsPartnerLogic> logger, IParcelRepository parcelRepository, IMapper mapper)
        {
            this.logger = logger;
            this.parcelRepository = parcelRepository;
            this.mapper = mapper;
        }
        
        public Parcel TransitionParcel(Parcel? parcel)
        {
            logger.LogInformation($"Transition Parcel with ID {parcel?.TrackingId}");
            if (parcel == null)
            {
                BlException e = new BlArgumentException("Parcel must not be null.");
                logger.LogError(e, "Error Parcel is null");
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

            var dalParcel = mapper.Map<DalParcel>(parcel);
            logger.LogDebug($"Mapping Bl/Dal {parcel} => {dalParcel}");
            try
            {
                dalParcel = parcelRepository.Create(dalParcel);
            }
            catch (DalException e)
            {
                logger.LogError(e, "Error creating parcel.");
                throw new BlRepositoryException("Error creating parcel.", e);
            }

            // return new Parcel
            // {
            //     TrackingId = parcel.TrackingId,
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
            //     State = Parcel.StateEnum.TransferredEnum,
            //     VisitedHops = new List<HopArrival>(),
            //     FutureHops = new List<HopArrival>()
            // };
            
            var blResult = mapper.Map<Parcel>(dalParcel);
            logger.LogDebug($"Mapping Dal/Bl {dalParcel} => {blResult}");
            return blResult;
        }
    }
}