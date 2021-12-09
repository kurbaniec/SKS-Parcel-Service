using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;
using DalWebhook = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Webhook;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class ParcelWebhookLogic : IParcelWebhookLogic
    {
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
                BlArgumentException e = new("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            IEnumerable<DalWebhook>? dalWebhooks;
            try
            {
                dalWebhooks = webhookManager.ListParcelWebhooks(trackingId);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error retrieving all Webhooks.");
                throw new BlRepositoryException($"Error retrieving all Webhooks.", e);
            }

            var webhooks = mapper.Map<IEnumerable<Webhook>>(dalWebhooks);
            return webhooks;
        }

        public Webhook SubscribeParcelWebhook(string? trackingId, string? url)
        {
            logger.LogInformation($"Subscribe Webhook with URL {url} for parcel with ID {trackingId}");
            if (trackingId == null)
            {
                BlArgumentException e = new("trackingId must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            if (url == null)
            {
                BlArgumentException e = new("URL must not be null.");
                logger.LogError(e, "URL is null");
                throw e;
            }

            DalWebhook dalWebhook;
            try
            {
                dalWebhook = webhookManager.SubscribeParcelWebhook(trackingId, url);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error while subscribing Webhook.");
                throw new BlDataNotFoundException($"Error while subscribing Webhook.", e);
            }

            var webhook = mapper.Map<Webhook>(dalWebhook);
            return webhook;
        }

        public void UnsubscribeParcelWebhook(long? id)
        {
            logger.LogInformation($"Unsubscribe from Webhook with ID {id}");
            if (id == null)
            {
                BlArgumentException e = new("Webhook ID must not be null.");
                logger.LogError(e, "ID is null");
                throw e;
            }

            try
            {
                webhookManager.UnsubscribeParcelWebhook(id.Value);
            }
            catch (DalException e)
            {
                logger.LogError(e, $"Error while unsubscribing Webhook.");
                throw new BlDataNotFoundException($"Error while unsubscribing Webhook.", e);
            }
        }
    }
}