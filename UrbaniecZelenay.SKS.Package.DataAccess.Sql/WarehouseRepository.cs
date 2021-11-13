using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IParcelLogisticsContext context;
        private readonly ILogger<WarehouseRepository> logger;

        public WarehouseRepository(ILogger<WarehouseRepository> logger, IParcelLogisticsContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public Warehouse Create(Warehouse warehouse)
        {
            logger.LogInformation($"Create Warehouse {warehouse}");
            // TODO Detect already existing warehouses?
            var check = context.Warehouses.SingleOrDefault(w => w.Code == warehouse.Code);
            if (check != null)
            {
                // TODO throw error?
                logger.LogWarning("Root Warehouse already exists");
                return check;
            }
            context.Warehouses.Add(warehouse);
            context.SaveChanges();
            return warehouse;
        }

        public Warehouse? GetAll()
        {
            logger.LogInformation("Get all Warehouses");
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
            logger.LogInformation($"Get Hop with Code {code}");
            return context.Hops.SingleOrDefault(w => w.Code == code);
        }

        public Warehouse? GetWarehouseByCode(string code)
        {
            logger.LogInformation($"Get Warehouse with Code {code}");
            return context.Hops
                .OfType<Warehouse>()
                .Include(hop => hop.NextHops)
                .ThenInclude(nextHop => nextHop.Hop)
                .AsEnumerable().SingleOrDefault(w => w.Code == code);
        }
    }
}