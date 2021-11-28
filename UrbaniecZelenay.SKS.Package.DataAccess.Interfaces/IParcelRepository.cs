using System;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        Parcel Create(Parcel parcel, bool useExistingId = false);
        Parcel Update(Parcel parcel);
        void Delete(Parcel parcel);

        Parcel AddFutureHopToVisited(string trackingId, DateTime dateTime);
        Parcel ChangeParcelState(string trackingId, Parcel.StateEnum parcelState);

        Parcel? GetByTrackingId(string trackingId);
    }
}