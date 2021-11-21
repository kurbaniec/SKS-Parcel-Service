using System.Threading.Tasks;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;

namespace UrbaniecZelenay.SKS.ServiceAgents.Interfaces
{
    public interface IGeoEncodingAgent
    {
        BlGeoCoordinate EncodeAddress(string street, string postalCode, string city, string country);
    }
}