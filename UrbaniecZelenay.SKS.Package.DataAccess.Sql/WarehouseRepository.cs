using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ParcelLogisticsContext context;

        public WarehouseRepository(ParcelLogisticsContext context)
        {
            this.context = context;
        }
        
        public Warehouse Create(Warehouse warehouse)
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Warehouse GetByCode(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}