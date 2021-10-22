using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
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
            // TODO Detect already existing warehouses?
            context.Warehouses.Add(warehouse);
            return warehouse;
        }

        public Warehouse? GetAll()
        {
            return context.Warehouses.FirstOrDefault(w => w.Level == 0);
        }

        public Warehouse? GetByCode(string code)
        {
            return context.Warehouses.FirstOrDefault(w => w.Code == code);
        }
    }
}