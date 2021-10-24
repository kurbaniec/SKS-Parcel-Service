using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
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
        
        public WarehouseManagementLogic(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            this.warehouseRepository = warehouseRepository;
            this.mapper = mapper;
        }
        
        public bool TriggerExportWarehouseException { get; set; } = false;
        public Warehouse? ExportWarehouses()
        {
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
            return blWarehouse;
        }

        public Warehouse? GetWarehouse(string code)
        {
            if (code == null)
            {
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
            return blWarehouse;
        }

        public void ImportWarehouses(Warehouse? body)
        {
            if (body == null)
            {
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
            warehouseRepository.Create(dalWarehouse);
        }
    }
}