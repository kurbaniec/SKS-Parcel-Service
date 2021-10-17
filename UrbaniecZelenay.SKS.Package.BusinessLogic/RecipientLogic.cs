using System;
using System.Collections.Generic;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class RecipientLogic : IRecipientLogic
    {
        public Parcel TrackParcel(string? trackingId)
        {
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
                State = Parcel.StateEnum.InTransportEnum,
                VisitedHops = new List<HopArrival>(),
                FutureHops = new List<HopArrival>()
            };
        }
    }
}