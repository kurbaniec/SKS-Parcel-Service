using System;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface ILogisticsPartnerLogic
    {
        public Parcel TransitionParcel(Parcel? body, string? trackingId);
    }
}