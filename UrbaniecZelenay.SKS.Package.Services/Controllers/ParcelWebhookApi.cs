using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.Services.Attributes;
using UrbaniecZelenay.SKS.Package.Services.DTOs;

namespace UrbaniecZelenay.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ParcelWebhookApiController : ControllerBase
    {
        private readonly IParcelWebhookLogic webhookLogic;
        private readonly IMapper mapper;
        private readonly ILogger<ParcelWebhookApiController> logger;
        
        public ParcelWebhookApiController(ILogger<ParcelWebhookApiController> logger, IMapper mapper,
            IParcelWebhookLogic webhookLogic)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.webhookLogic = webhookLogic;
        }

        /// <summary>
        /// Get all registered subscriptions for the parcel webhook.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <response code="200">List of webooks for the &#x60;trackingId&#x60;</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("ListParcelWebhooks")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<WebhookResponse>), description: "List of webooks for the &#x60;trackingId&#x60;")]
        public virtual IActionResult ListParcelWebhooks([FromRoute][Required][RegularExpression(@"^[A-Z0-9]{9}$")]string trackingId)
        {
            logger.LogInformation($"List Parcel Webhooks for parcel with ID {trackingId}");
            IEnumerable<Webhook> blWebhooks;
            try
            {
                blWebhooks = webhookLogic.ListParcelWebhooks(trackingId);
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, "Error occured while listing webhooks.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, "Error occured while listing webhooks.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            
            var webhooks = mapper.Map<IEnumerable<WebhookResponse>>(blWebhooks);
            logger.LogDebug($"Mapping Bl/Svc {blWebhooks} => {webhooks}");
            return new ObjectResult(webhooks);
        }

        /// <summary>
        /// Subscribe to a webhook notification for the specific parcel.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <param name="url"></param>
        /// <response code="200">Successful response</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("SubscribeParcelWebhook")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponse), description: "Successful response")]
        public virtual IActionResult SubscribeParcelWebhook([FromRoute][Required][RegularExpression(@"^[A-Z0-9]{9}$")]string trackingId, [FromQuery][Required()]string url)
        { 
            logger.LogInformation($"Subscribe Webhook with URL {url} for parcel with ID {trackingId}");
            Webhook blWebhook;
            try
            {
                blWebhook = webhookLogic.SubscribeParcelWebhook(trackingId, url);
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, "Error occured while subscribing to webhook.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, "Error occured while subscribing to webhook.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            
            var webhook = mapper.Map<WebhookResponse>(blWebhook);
            logger.LogDebug($"Mapping Bl/Svc {blWebhook} => {webhook}");
            return new ObjectResult(webhook);
        }

        /// <summary>
        /// Remove an existing webhook subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Subscription does not exist.</response>
        [HttpDelete]
        [Route("/parcel/webhooks/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UnsubscribeParcelWebhook")]
        public virtual IActionResult UnsubscribeParcelWebhook([FromRoute][Required]long? id)
        { 
            logger.LogInformation($"Unsubscribe from Webhook with ID {id}");
            try
            {
                webhookLogic.UnsubscribeParcelWebhook(id);
            }
            catch (BlArgumentException ex)
            {
                logger.LogError(ex, "Error occured while unsubscribing from webhook.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }
            catch (BlDataNotFoundException ex)
            {
                logger.LogError(ex, "Error occured while unsubscribing to webhook.");
                return StatusCode(404, new Error
                {
                    ErrorMessage = ex.Message
                });
            }

            return StatusCode(200);
        }
    }
}
