using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalWarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Warehouse;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class WarehouseManagementLogic : IWarehouseManagementLogic
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IMapper mapper;
        private readonly ILogger<WarehouseManagementLogic> logger;

        public WarehouseManagementLogic(ILogger<WarehouseManagementLogic> logger, IWarehouseRepository warehouseRepository, IMapper mapper)
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
                throw new InvalidOperationException();
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

            var dalWarehouse = warehouseRepository.GetAll();
            if (dalWarehouse == null) return null;
            var blWarehouse = mapper.Map<Warehouse>(dalWarehouse);
            logger.LogDebug($"Mapping Dal/Bl {dalWarehouse} => {blWarehouse}");
            return blWarehouse;
        }

        public Warehouse? GetWarehouse(string code)
        {
            logger.LogInformation($"Get Warehouse with Code {code}");
            if (code == null)
            {
                logger.LogError("Code is null");
                throw new ArgumentNullException(nameof(code));
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
            var dalWarehouse = warehouseRepository.GetWarehouseByCode(code);
            if (dalWarehouse == null) return null;
            var blWarehouse = mapper.Map<Warehouse>(dalWarehouse);
            logger.LogDebug($"Mapping Dal/Bl {dalWarehouse} => {blWarehouse}");
            return blWarehouse;
        }

        public void ImportWarehouses(Warehouse? body)
        {
            logger.LogInformation($"Import warehouses with hierarchy {body}");
            if (body == null)
            {
                logger.LogError("Warehouse is null");
                throw new ArgumentNullException(nameof(body));
            }
            IValidator<Warehouse> parcelValidator = new WarehouseValidator();
            var validationResult = parcelValidator.Validate(body);
            if (!validationResult.IsValid)
            {
                string validationErrors = string.Join(Environment.NewLine, validationResult.Errors);
                throw new ArgumentException(validationErrors);
            }

            var dalWarehouse = mapper.Map<DalWarehouse>(body);
            logger.LogDebug($"Mapping Bl/Dal {body} => {dalWarehouse}");
            warehouseRepository.Create(dalWarehouse);
        }
    }
}