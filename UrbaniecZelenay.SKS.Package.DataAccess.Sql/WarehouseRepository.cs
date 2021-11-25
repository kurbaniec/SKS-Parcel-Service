﻿using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
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
            // Detect already existing warehouses?
            // try
            // {
            //     var check = context.Warehouses.SingleOrDefault(w => w.Code == warehouse.Code);
            //     if (check != null)
            //     {
            //         DalException e = new DalDuplicateEntryException($"Error warehouse already exists (Code: " +
            //                                                         $"{check.Code}.");
            //         logger.LogError(e, "Root Warehouse already exists");
            //         throw e;
            //         // return check;
            //     }
            //
            //     context.Warehouses.Add(warehouse);
            //     context.SaveChanges();
            // }
            try
            {
                // Delete database and rebuild it
                // See: https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
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

        public Warehouse? GetAll()
        {
            logger.LogInformation("Get all Warehouses");
            // Source: Max
            // Load everything from DB (using AsEnumerable) and filter at the end
            // (with SingleOrDefault) for the root warehouse
            Warehouse? hops = null;
            try
            {
                // TODO Check if this is indeed correct
                hops = context.Hops
                    .OfType<Warehouse>()
                    .Include(hop => hop.LocationCoordinates)
                    .Include(hop => hop.NextHops)
                    .ThenInclude(nextHop => nextHop.Hop)
                    .ThenInclude(nextHop => nextHop.LocationCoordinates)
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
                hop = context.Hops.SingleOrDefault(w => w.Code == code);
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
                    .Include(hop => hop.LocationCoordinates)
                    .Include(hop => hop.NextHops)
                    .ThenInclude(nextHop => nextHop.Hop)
                    .ThenInclude(nextHop => nextHop.LocationCoordinates)
                    .AsEnumerable().SingleOrDefault(w => w.Code == code);
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error getting warehouse by code ({code}).");
                throw new DalConnectionException($"Error occured while getting warehouse by Code ({code}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error getting Hop by code ({code}).");
                throw new DalConnectionException($"Error occured while getting warehouse by Code ({code}).", e);
            }

            return warehouse;
        }
    }
}