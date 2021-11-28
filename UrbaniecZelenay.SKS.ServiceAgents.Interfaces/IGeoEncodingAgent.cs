using NetTopologySuite.Geometries;

namespace UrbaniecZelenay.SKS.ServiceAgents.Interfaces
{
    public interface IGeoEncodingAgent
    {
        Point? EncodeAddress(string street, string postalCode, string city, string country);
    }
}