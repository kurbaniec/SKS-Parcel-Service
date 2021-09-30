namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface IStaffLogic
    {
        /// <summary>
        /// Report that a Parcel has been delivered at it&#x27;s final destination address.
        /// </summary>
        /// <param name="trackingId"></param>
        public void ReportParcelDelivery(string? trackingId);

        /// <summary>
        /// Report that a Parcel has arrived at a certain hop either Warehouse or Truck. 
        /// </summary>
        /// <param name="trackingId"></param>
        /// <param name="code"></param>
        public void ReportParcelHop(string? trackingId, string? code);
    }
}