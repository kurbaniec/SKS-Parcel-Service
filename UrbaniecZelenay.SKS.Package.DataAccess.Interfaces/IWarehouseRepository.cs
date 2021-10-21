using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        Warehouse Create(Warehouse warehouse);

        Warehouse GetAll();
        Warehouse GetByCode(string code);
    }
}