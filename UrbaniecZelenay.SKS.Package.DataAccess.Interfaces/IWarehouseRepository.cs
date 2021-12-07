using NetTopologySuite.Geometries;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        Warehouse Create(Warehouse warehouse);
        Warehouse Update(Warehouse warehouse);
        Warehouse? GetAll();
        Hop? GetHopByCode(string code);
        Warehouse? GetWarehouseByCode(string code);
        Truck? GetTruckByPoint(Point point);
        Transferwarehouse? GetTransferwarehouseByPoint(Point point);
    }
}