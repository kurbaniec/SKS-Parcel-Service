using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        public Parcel TransitionParcel(Parcel? body, string? trackingId)
        {
            
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(body);
            if (!validationResult.IsValid)
            {
                string validationErrors = string.Join(Environment.NewLine, validationResult.Errors);
                throw new ArgumentException(validationErrors);
            }

            if (trackingId == null)
            {
                throw new ArgumentNullException(nameof(trackingId));
            }
            
            return new Parcel
            {
                TrackingId = trackingId,
                Weight = 1,
                Recipient = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                Sender = new Recipient
                {
                    Name = "Max Mustermann",
                    Street = "A Street",
                    PostalCode = "1200",
                    City = "Vienna",
                    Country = "Austria"
                },
                State = Parcel.StateEnum.TransferredEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
        }
    }
}