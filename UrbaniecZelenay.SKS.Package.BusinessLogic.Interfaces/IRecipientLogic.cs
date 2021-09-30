using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface IRecipientLogic
    {
        /// <summary>
        /// Find the latest state of a parcel by its tracking ID.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <returns></returns>
        public Parcel TrackParcel(string? trackingId);
    }
}