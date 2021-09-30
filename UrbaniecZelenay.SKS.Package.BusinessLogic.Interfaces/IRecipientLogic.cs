using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface IRecipientLogic
    {
        /// <summary>
        /// Find the latest state of a parcel by its tracking ID. 
        /// </summary>
        public Parcel TrackParcel(string? trackingId);
    }
}