using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IParcelLogisticsContext context;

        public WarehouseRepository(IParcelLogisticsContext context)
        {
            this.context = context;
        }
        
        public Warehouse Create(Warehouse warehouse)
        {
            // TODO Detect already existing warehouses?
            context.Warehouses.Add(warehouse);
            context.SaveChanges();
            return warehouse;
        }

        public Warehouse? GetAll()
        {
            //return context.Warehouses.FirstOrDefault(w => w.Level == 0);
            return context.Hops.OfType<Warehouse>()
                .Include(hop => hop.NextHops)
                .ThenInclude(nextHop => nextHop.Hop)
                .AsEnumerable()
                .FirstOrDefault(hop => hop.Level == 0);
        }

        public Hop? GetHopByCode(string code)
        {
            Hop? tmp = context.Hops.FirstOrDefault(w => w.Code == code);
            return context.Hops.FirstOrDefault(w => w.Code == code);
        }

        public Warehouse? GetWarehouseByCode(string code)
        {
            return context.Warehouses.FirstOrDefault(w => w.Code == code);
        }
    }
}