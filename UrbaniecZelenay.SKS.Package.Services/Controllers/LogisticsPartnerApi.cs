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
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Sql;
using UrbaniecZelenay.SKS.Package.Services.Attributes;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using BlParcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.Services.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class LogisticsPartnerApiController : ControllerBase
    {
        private readonly ILogisticsPartnerLogic logisticsPartnerLogic;
        private readonly IMapper mapper;
        private readonly ILogger<LogisticsPartnerApiController> logger;

        public LogisticsPartnerApiController(ILogger<LogisticsPartnerApiController> logger, IMapper mapper,
            ILogisticsPartnerLogic logisticsPartnerLogic)
        {
            this.logger = logger;
            this.logisticsPartnerLogic = logisticsPartnerLogic;
            this.mapper = mapper;
        }

        /// <summary>
        /// Transfer an existing parcel into the system from the service of a logistics partner. 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully transitioned the parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TransitionParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(NewParcelInfo),
            description: "Successfully transitioned the parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TransitionParcel([FromBody] Parcel body,
            [FromRoute] [Required] [RegularExpression(@"^[A-Z0-9]{9}$")]
            string trackingId)
        {
            logger.LogInformation($"Transition Parcel with ID {trackingId}");
            var blParcel = mapper.Map<BlParcel>(body,
                opt => opt.AfterMap((_, dest) => dest.TrackingId = trackingId));
            logger.LogDebug($"Mapping Svc/Bl {body} => {blParcel}");

            BlParcel blResult;
            try
            {
                blResult = logisticsPartnerLogic.TransitionParcel(blParcel);
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, "Error occured while transitioning parcel.");
                return StatusCode(400, new Error
                {
                    // For security purposes only the last error message in the stack is shown to the end user!
                    ErrorMessage = ex.Message
                });
            }
            catch (BlValidationException ex)
            {
                logger.LogError(ex, "Error occured while transitioning parcel.");
                return StatusCode(400, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, "Error occured while transitioning parcel.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlRepositoryException ex)
            {
                logger.LogError(ex, "Error occured while transitioning parcel.");
                return StatusCode(500, new Error
                {
                    ErrorMessage = ex.Message
                });
            }

            var svcResult = mapper.Map<NewParcelInfo>(blResult);
            logger.LogDebug($"Mapping Bl/Svc {blResult} => {svcResult}");
            return new ObjectResult(svcResult);
        }
    }
}