using System;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IParcelRepository parcelRepository;
        
        public StaffLogic(IParcelRepository parcelRepository)
        {
            this.parcelRepository = parcelRepository;
        }
        
        // TODO create NotFoundException
        public void ReportParcelDelivery(string? trackingId)
        {
            if (trackingId == null)
            {
                throw new ArgumentNullException(nameof(trackingId));
            }
        }

        public void ReportParcelHop(string? trackingId, string? code)
        {
            if (trackingId == null)
            {
                throw new ArgumentNullException(nameof(trackingId));
            }

            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }
        }
    }
}