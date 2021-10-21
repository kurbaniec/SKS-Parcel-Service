using System;
using System.Collections.Generic;
using FluentValidation;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IParcelRepository parcelRepository;
        
        public SenderLogic(IParcelRepository parcelRepository)
        {
            this.parcelRepository = parcelRepository;
        }
        
        public Parcel SubmitParcel(Parcel? body)
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

            var p = new DataAccess.Entities.Parcel();
            parcelRepository.Create(p);
            
            return new Parcel
            {
                TrackingId = "PYJRB4HZ6",
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
                State = Parcel.StateEnum.InTransportEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
        }
    }
}