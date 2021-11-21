﻿using System;
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
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IMapper mapper;
        private readonly ILogger<SenderLogic> logger;

        public SenderLogic(ILogger<SenderLogic> logger, IParcelRepository parcelRepository, IMapper mapper)
        {
            this.logger = logger;
            this.parcelRepository = parcelRepository;
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

            var dalParcel = mapper.Map<DalParcel>(body);
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