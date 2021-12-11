using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
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
            try
            {
                // Link previous hops
                LinkPreviousHops(warehouse, null, null);
                // Delete database and rebuild it
                // See: https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
                context.Database.SetCommandTimeout(600);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Database.SetCommandTimeout(60);
                context.Warehouses.Add(warehouse);
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, "Error updating Warehouse");
                throw new DalSaveException("Error occured while creating a warehouse.", e);
            }
            catch (SqlException e)
            {
                logger.LogError(e, "Error updating ");
                throw new DalConnectionException("Error occured while creating a warehouse.", e);
            }

            return warehouse;
        }
        
        private static void LinkPreviousHops(Hop currentHop, Hop? parentHop, int? travelTime)
        {
            if (parentHop != null)
                currentHop.PreviousHop = new PreviousHop
                {
                    OriginalHop = currentHop,
                    Hop = parentHop,
                    TraveltimeMins = travelTime!.Value
                };
            if (currentHop is not Warehouse w) return;
            foreach (var nextHop in w.NextHops)
            {
                LinkPreviousHops(nextHop.Hop, currentHop, nextHop.TraveltimeMins);
            }
        }

        public Warehouse? GetAll()
        {
            logger.LogInformation("Get all Warehouses");
            // Source: Max
            // Load everything from DB (using AsEnumerable) and filter at the end
            // (with SingleOrDefault) for the root warehouse
            Warehouse? hops = null;
            try
            {
                hops = context.Hops
                    .OfType<Warehouse>()
                    .Include(hop => hop.NextHops)
                    .ThenInclude(nextHop => nextHop.Hop)
                    .AsEnumerable()
                    .SingleOrDefault(hop => hop.Level == 0);
            }
            catch (SqlException e)
            {
                logger.LogError(e, "Error Getting All Warehouses.");
                throw new DalConnectionException("Error occured while retrieving all warehouses.", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, "Error Getting All Warehouses.");
                throw new DalConnectionException("Error occured while retrieving all warehouses.", e);
            }

            return hops;
        }

        public Hop? GetHopByCode(string code)
        {
            logger.LogInformation($"Get Hop with Code {code}");
            Hop? hop = null;
            try
            {
                // If code is warehouse return warehouses else return
                hop = GetWarehouseByCode(code);
                if (hop == null)
                {
                    hop = context.Hops.SingleOrDefault(w => w.Code == code);
                }
                // Hop? test = from h in context.Hops select  
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error getting Hop by code ({code}).");
                throw new DalConnectionException($"Error occured while getting hop by Code ({code}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error getting Hop by code ({code}).");
                throw new DalConnectionException($"Error occured while getting hop by Code ({code}).", e);
            }

            return hop;
        }
        
        

        public Warehouse? GetWarehouseByCode(string code)
        {
            logger.LogInformation($"Get Warehouse with Code {code}");
            Warehouse? warehouse = null;
            try
            {
                warehouse = context.Hops
                    .OfType<Warehouse>()
                    .Include(hop => hop.NextHops)
                    .ThenInclude(nextHop => nextHop.Hop)
                    // .ThenInclude(nextHop => nextHop.LocationCoordinates)
                    .AsEnumerable().SingleOrDefault(w => w.Code == code);
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error getting Warehouse by code ({code}).");
                throw new DalConnectionException($"Error occured while getting Warehouse by Code ({code}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error getting Hop by code ({code}).");
                throw new DalConnectionException($"Error occured while getting Warehouse by Code ({code}).", e);
            }

            return warehouse;
        }

        public Truck? GetTruckByPoint(Point point)
        {
            logger.LogInformation($"Get Truck which Region includes {point}");
            Truck? truck;
            try
            {
                // Even though "ThenInclude" says t can be nullable (which is true)
                // EF Core will still be map it correctly and not throw any errors
                // See: https://github.com/dotnet/efcore/issues/17212
                truck = context.Hops
                    .OfType<Truck>()
                    .AsNoTracking()
                    .Include(t => t.PreviousHop)
                    .ThenInclude(t => t!.Hop)
                    .AsEnumerable()
                    .SingleOrDefault(t => t.Region.Contains(point));
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error getting Truck by point ({point}).");
                throw new DalConnectionException($"Error getting Truck by point ({point}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error getting Truck by point ({point}).");
                throw new DalConnectionException($"Error getting Truck by point ({point}).", e);
            }

            return truck;
        }

        public Transferwarehouse? GetTransferwarehouseByPoint(Point point)
        {
            logger.LogInformation($"Get Transferwarehouse which Region includes {point}");
            Transferwarehouse? transferwarehouse;
            try
            {
                transferwarehouse = context.Hops
                    .OfType<Transferwarehouse>()
                    .AsNoTracking()
                    .Include(tw => tw.PreviousHop)
                    .ThenInclude(tw => tw!.Hop)
                    .AsEnumerable()
                    .SingleOrDefault(t => t.Region.Contains(point));
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error getting Transferwarehouse by point ({point}).");
                throw new DalConnectionException($"Error getting Transferwarehouse by point ({point}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error getting Transferwarehouse by point ({point}).");
                throw new DalConnectionException($"Error getting Transferwarehouse by point ({point}).", e);
            }

            return transferwarehouse;
        }
    }
}