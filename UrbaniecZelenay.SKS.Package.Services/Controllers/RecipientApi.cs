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
using UrbaniecZelenay.SKS.Package.BusinessLogic;
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
    public class RecipientApiController : ControllerBase
    {
        private readonly IRecipientLogic recipientLogic;
        private readonly IMapper mapper;
        
        [ActivatorUtilitiesConstructor]
        public RecipientApiController(IParcelLogisticsContext context, IMapper mapper)
        {
            this.recipientLogic = new RecipientLogic(new ParcelRepository(context), mapper);
            this.mapper = mapper;
        }
        
        public RecipientApiController(IMapper mapper, IRecipientLogic recipientLogic)
        {
            this.recipientLogic = recipientLogic;
            this.mapper = mapper;
        }

        /// <summary>
        /// Find the latest state of a parcel by its tracking ID. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Parcel exists, here&#x27;s the tracking information.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TrackParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(TrackingInformation),
            description: "Parcel exists, here&#x27;s the tracking information.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TrackParcel(
            [FromRoute] [Required] [RegularExpression(@"^[A-Z0-9]{9}$")] string trackingId)
        {
            // if (trackingId == null)
            // {
            //     
            // }
            //
            // //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // // return StatusCode(200, default(TrackingInformation));
            //
            // //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // // return StatusCode(400, default(Error));
            //
            // //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // // return StatusCode(404);
            // string exampleJson = null;
            // exampleJson =
            //     "{\n  \"visitedHops\" : [ {\n    \"dateTime\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"code\" : \"code\",\n    \"description\" : \"description\"\n  }, {\n    \"dateTime\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"code\" : \"code\",\n    \"description\" : \"description\"\n  } ],\n  \"futureHops\" : [ null, null ],\n  \"state\" : \"Pickup\"\n}";
            //
            // var example = exampleJson != null
            //     ? JsonConvert.DeserializeObject<TrackingInformation>(exampleJson)
            //     : default(TrackingInformation); //TODO: Change the data returned
            // return new ObjectResult(example);
            
            BlParcel blResult;
            try
            {
                blResult = recipientLogic.TrackParcel(trackingId);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new Error
                {
                    ErrorMessage = "No tracking ID given"
                });
            }

            var svcResult = mapper.Map<TrackingInformation>(blResult);
            return new ObjectResult(svcResult);
        }
    }
}