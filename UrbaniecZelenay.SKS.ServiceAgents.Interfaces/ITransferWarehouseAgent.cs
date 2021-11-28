namespace UrbaniecZelenay.SKS.ServiceAgents.Interfaces
{
    public interface ITransferWarehouseAgent
    {
        bool TransferParcel(string baseUrl, string trackingId, float weight,
            string recipientName, string recipientStreet, string recipientPostalCode, string recipientCity,
            string recipientCountry, string senderName, string senderStreet, string senderPostalCode, string senderCity,
            string senderCountry);
    }
}