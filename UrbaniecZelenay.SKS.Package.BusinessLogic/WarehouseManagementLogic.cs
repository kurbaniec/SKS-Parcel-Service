using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalWarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Warehouse;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;
//using Truck = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Truck;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class WarehouseManagementLogic : IWarehouseManagementLogic
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IMapper mapper;
        private readonly ILogger<WarehouseManagementLogic> logger;

        public WarehouseManagementLogic(ILogger<WarehouseManagementLogic> logger,
            IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            this.logger = logger;
            this.warehouseRepository = warehouseRepository;
            this.mapper = mapper;
        }

        public bool TriggerExportWarehouseException { get; set; } = false;

        public Warehouse? ExportWarehouses()
        {
            logger.LogInformation("Export Warehouses");
            // TODO: Create custom exception
            if (TriggerExportWarehouseException)
            {
                BlArgumentException e = new BlArgumentException("Exception for Unit tests.");
                logger.LogError(e, "Exception for unit tests");
                throw e;
            }

            // return new Warehouse
            // {
            //     HopType = "Warehouse",
            //     Code = "AUTA05",
            //     Description = "Root Warehouse - Österreich",
            //     ProcessingDelayMins = 186,
            //     LocationName = "Root",
            //     LocationCoordinates = new GeoCoordinate{Lat = 47.247829, Lon = 13.884382},
            //     Level = 0,
            //     NextHops = new List<WarehouseNextHops>()
            // };
            DalWarehouse? dalWarehouse = null;
            try
            {
                dalWarehouse = warehouseRepository.GetAll();
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving all warehouses.");
                throw new BlRepositoryException($"Error retrieving all warehouses.", e);
            }

            if (dalWarehouse == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error no warehouses found");
                logger.LogError(e, "No warehouses found");
                throw e;
            }

            var blWarehouse = mapper.Map<Warehouse>(dalWarehouse);
            logger.LogDebug($"Mapping Dal/Bl {dalWarehouse} => {blWarehouse}");
            return blWarehouse;
        }

        public Hop? GetWarehouse(string code)
        {
            logger.LogInformation($"Get Warehouse with Code {code}");
            if (code == null)
            {
                BlArgumentException e = new BlArgumentException("code must not be null.");
                logger.LogError(e, "code is null");
                throw e;
            }

            // return new Warehouse
            // {
            //     HopType = "Warehouse",
            //     Code = "AUTA05",
            //     Description = "Root Warehouse - Österreich",
            //     ProcessingDelayMins = 186,
            //     LocationName = "Root",
            //     LocationCoordinates = new GeoCoordinate{Lat = 47.247829, Lon = 13.884382},
            //     Level = 0,
            //     NextHops = new List<WarehouseNextHops>()
            // };
            DalHop? dalHop = null;
            try
            {
                dalHop = warehouseRepository.GetHopByCode(code);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving warehouse by code ({code}).");
                throw new BlRepositoryException($"Error retrieving warehouse by code ({code}).", e);
            }

            if (dalHop == null)
            {
                BlDataNotFoundException e =
                    new BlDataNotFoundException($"Error warehouse with code ({code}) not found");
                logger.LogError(e, "Warehouse by code not found");
                throw e;
            }

            var blWarehouse = mapper.Map<Hop>(dalHop);
            logger.LogDebug($"Mapping Dal/Bl {dalHop} => {blWarehouse}");
            return blWarehouse;
        }

        public void ImportWarehouses(Warehouse? body)
        {
            logger.LogInformation($"Import warehouses with hierarchy {body}");
            if (body == null)
            {
                BlArgumentException e = new BlArgumentException("warehouse must not be null.");
                logger.LogError(e, "warehouse (body) is null");
                throw e;
            }

            IValidator<Warehouse> parcelValidator = new WarehouseValidator();
            var validationResult = parcelValidator.Validate(body);
            if (!validationResult.IsValid)
            {
                string validationErrors = string.Join(Environment.NewLine, validationResult.Errors);
                BlException e = new BlValidationException(validationErrors);
                logger.LogError(e, $"Error validating warehouse");
                throw e;
            }
            
            LinkPreviousHops(body, null);

            var dalWarehouse = mapper.Map<DalWarehouse>(body);
            logger.LogDebug($"Mapping Bl/Dal {body} => {dalWarehouse}");
            try
            {
                warehouseRepository.Create(dalWarehouse);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error creating warehouse ({dalWarehouse}).");
                throw new BlRepositoryException($"Error creating warehouse ({dalWarehouse}).", e);
            }
        }

        private static void LinkPreviousHops(Hop currentHop, Hop? parentHop)
        {
            currentHop.PreviousHop = parentHop;
            if (currentHop is not Warehouse w) return;
            foreach (var nextHop in w.NextHops)
            {
                LinkPreviousHops(nextHop.Hop, w);
            }
        }
    }
}