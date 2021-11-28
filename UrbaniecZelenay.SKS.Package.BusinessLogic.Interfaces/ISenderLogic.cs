using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface ISenderLogic
    {
        /// <summary>
        /// Submit a new parcel to the logistics service. 
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public Parcel SubmitParcel(Parcel? parcel);
    }
}