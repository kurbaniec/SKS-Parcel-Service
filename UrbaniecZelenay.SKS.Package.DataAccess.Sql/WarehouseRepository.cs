using System;
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
            var check = context.Warehouses.SingleOrDefault(w => w.Code == warehouse.Code);
            if (check != null)
            {
                // TODO throw error?
                Console.WriteLine("Already exists");
                return check;
            }
            context.Warehouses.Add(warehouse);
            context.SaveChanges();
            return warehouse;
        }

        public Warehouse? GetAll()
        {
            // Source: Max
            // Load everything from DB (using AsEnumerable) and filter at the end
            // (with SingleOrDefault) for the root warehouse
            return context.Hops
                .OfType<Warehouse>()
                .Include(hop => hop.NextHops)
                .ThenInclude(nextHop => nextHop.Hop)
                .AsEnumerable()
                .SingleOrDefault(hop => hop.Level == 0);
        }

        public Hop? GetHopByCode(string code)
        {
            return context.Hops.SingleOrDefault(w => w.Code == code);
        }

        public Warehouse? GetWarehouseByCode(string code)
        {
            return context.Hops
                .OfType<Warehouse>()
                .Include(hop => hop.NextHops)
                .ThenInclude(nextHop => nextHop.Hop)
                .AsEnumerable().SingleOrDefault(w => w.Code == code);
        }
    }
}