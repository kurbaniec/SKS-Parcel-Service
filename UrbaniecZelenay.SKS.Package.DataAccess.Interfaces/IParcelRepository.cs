using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        Parcel Create(Parcel parcel, bool useExistingId = false);
        Parcel Update(Parcel parcel);
        void Delete(Parcel parcel);

        Parcel? GetByTrackingId(string trackingId);
    }
}