/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Sql;
using UrbaniecZelenay.SKS.Package.Services.Attributes;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using BlWarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Warehouse;

namespace UrbaniecZelenay.SKS.Package.Services.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class WarehouseManagementApiController : ControllerBase
    {
        private readonly IWarehouseManagementLogic warehouseManagementLogic;
        private readonly IMapper mapper;
        private readonly ILogger<WarehouseManagementApiController> logger;

        public WarehouseManagementApiController(ILogger<WarehouseManagementApiController> logger, IMapper mapper,
            IWarehouseManagementLogic warehouseManagementLogic)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.warehouseManagementLogic = warehouseManagementLogic;
        }


        /// <summary>
        /// Exports the hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">No hierarchy loaded yet.</response>
        [HttpGet]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ExportWarehouses")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult ExportWarehouses()
        {
            logger.LogInformation("Export Warehouses");
            BlWarehouse blResult;
            try
            {
                blResult = warehouseManagementLogic.ExportWarehouses();
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, "Error occured while exporting warehouses.");
                return StatusCode(400, new Error
                {
                    // For security purposes only the last error message in the stack is shown to the end user!
                    ErrorMessage = ex.Message
                });
            }
            catch (BlValidationException ex)
            {
                logger.LogError(ex, "Error occured while exporting warehouses.");
                return StatusCode(400, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, "Error occured while exporting warehouses.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlRepositoryException ex)
            {
                logger.LogError(ex, "Error occured while exporting warehouses.");
                return StatusCode(500, new Error
                {
                    ErrorMessage = ex.Message
                });
            }

            var svcResult = mapper.Map<Warehouse>(blResult);
            return new ObjectResult(svcResult);
        }

        /// <summary>
        /// Get a certain warehouse or truck by code
        /// </summary>
        /// <param name="code"></param>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">Warehouse id not found</response>
        [HttpGet]
        [Route("/warehouse/{code}")]
        [ValidateModelState]
        [SwaggerOperation("GetWarehouse")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult GetWarehouse([FromRoute] [Required] string code)
        {
            logger.LogInformation($"Get Warehouse with Code {code}");
            BlWarehouse? blResult;
            try
            {
                blResult = warehouseManagementLogic.GetWarehouse(code);
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, $"Error occured while retrieving warehouses by code ({code}).");
                return StatusCode(400, new Error
                {
                    // For security purposes only the last error message in the stack is shown to the end user!
                    ErrorMessage = ex.Message
                });
            }
            catch (BlValidationException ex)
            {
                logger.LogError(ex, $"Error occured while retrieving warehouses by code ({code}).");
                return StatusCode(400, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, $"Error occured while retrieving warehouses by code ({code}).");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlRepositoryException ex)
            {
                logger.LogError(ex, $"Error occured while retrieving warehouses by code ({code}).");
                return StatusCode(500, new Error
                {
                    ErrorMessage = ex.Message
                });
            }

            var svcResult = mapper.Map<Warehouse>(blResult);
            logger.LogDebug($"Mapping Bl/Svc {blResult} => {svcResult}");
            return new ObjectResult(svcResult);
        }

        /// <summary>
        /// Imports a hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successfully loaded.</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ImportWarehouses")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ImportWarehouses([FromBody] Warehouse body)
        {
            logger.LogInformation($"Import warehouses with hierarchy {body?.ToJson()}");
            BlWarehouse blWarehouse = null;
            try
            {
                var kek = mapper.Map<BlWarehouse>(body);
                blWarehouse = kek;
            }
            catch (Exception e)
            {
                
            }

            logger.LogDebug($"Mapping Svc/Bl {body} => {blWarehouse}");
            try
            {
                warehouseManagementLogic.ImportWarehouses(blWarehouse);
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, $"Error occured while importing warehouse ({body}).");
                return StatusCode(400, new Error
                {
                    // For security purposes only the last error message in the stack is shown to the end user!
                    ErrorMessage = ex.Message
                });
            }
            catch (BlValidationException ex)
            {
                logger.LogError(ex, $"Error occured while importing warehouse ({body}).");
                return StatusCode(400, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, $"Error occured while importing warehouse ({body}).");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlRepositoryException ex)
            {
                logger.LogError(ex, $"Error occured while importing warehouse ({body}).");
                return StatusCode(400, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            return StatusCode(200);
        }
    }
}