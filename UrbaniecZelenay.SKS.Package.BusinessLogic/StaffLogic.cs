using System;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
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