namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface IStaffLogic
    {
        public void ReportParcelDelivery(string? trackingId);

        public void ReportParcelHop(string? trackingId, string? code);
    }
}