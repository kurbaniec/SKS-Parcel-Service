using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IMapper mapper;
        
        public LogisticsPartnerLogic(IParcelRepository parcelRepository, IMapper mapper)
        {
            this.parcelRepository = parcelRepository;
            this.mapper = mapper;
        }
        
        public Parcel TransitionParcel(Parcel? parcel)
        {
            if (parcel == null)
            {
                throw new ArgumentNullException(nameof(parcel));
            }

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(parcel);
            if (!validationResult.IsValid)
            {
                string validationErrors = string.Join(Environment.NewLine, validationResult.Errors);
                throw new ArgumentException(validationErrors);
            }

            var dalParcel = mapper.Map<DalParcel>(parcel);
            dalParcel = parcelRepository.Create(dalParcel);

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
            return blResult;
        }
    }
}