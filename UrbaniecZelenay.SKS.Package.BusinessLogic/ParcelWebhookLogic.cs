using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;
using DalWebhook = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Webhook;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class ParcelWebhookLogic : IParcelWebhookLogic
    {
        //private readonly IParcelRepository parcelRepository;
        private readonly IWebhookManager webhookManager;
        private readonly IMapper mapper;
        private readonly ILogger<RecipientLogic> logger;

        public ParcelWebhookLogic(
            IWebhookManager webhookManager,
            IMapper mapper,
            ILogger<RecipientLogic> logger
        )
        {
            this.webhookManager = webhookManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IEnumerable<Webhook> ListParcelWebhooks(string? trackingId)
        {
            logger.LogInformation($"List Parcel Webhooks for parcel with ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new BlArgumentException("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            // TODO: try catch
            var dalWebhooks = webhookManager.ListParcelWebhooks(trackingId);
            var webhooks = mapper.Map<IEnumerable<Webhook>>(dalWebhooks);
            return webhooks;
        }

        public Webhook SubscribeParcelWebhook(string? trackingId, string? url)
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

            // TODO: try catch
            var dalWebhook = webhookManager.SubscribeParcelWebhook(trackingId, url);
            var webhook = mapper.Map<Webhook>(dalWebhook);
            return webhook;
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

            // TODO: try catch
            webhookManager.UnsubscribeParcelWebhook(id.Value);
        }
    }
}