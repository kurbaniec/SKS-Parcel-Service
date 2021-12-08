using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class ParcelWebhookLogic : IParcelWebhookLogic
    {
        //private readonly IParcelRepository parcelRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RecipientLogic> logger;

        public ParcelWebhookLogic(IMapper mapper, ILogger<RecipientLogic> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }
        
        public IEnumerable<WebhookResponse> ListParcelWebhooks(string? trackingId)
        {
            logger.LogInformation($"List Parcel Webhooks for parcel with ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new BlArgumentException("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }
            throw new System.NotImplementedException();
        }

        public WebhookResponse SubscribeParcelWebhook(string? trackingId, string? url)
        {
            logger.LogInformation($"Subscribe Webhook with URL {url} for parcel with ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new BlArgumentException("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }
            if (url == null)
            {
                BlArgumentException e = new BlArgumentException("URL must not be null.");
                logger.LogError(e, "URL is null");
                throw e;
            }
            
            throw new System.NotImplementedException();
        }

        public void UnsubscribeParcelWebhook(long? id)
        {
            logger.LogInformation($"Unsubscribe from Webhook with ID {id}");
            if (id == null)
            {
                BlArgumentException e = new BlArgumentException("Webhook ID must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }
            throw new System.NotImplementedException();
        }
    }
}