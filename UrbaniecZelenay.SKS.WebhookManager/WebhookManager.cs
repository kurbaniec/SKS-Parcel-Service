using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;

namespace UrbaniecZelenay.SKS.WebhookManager
{
    public class WebhookManager : IWebhookManager
    {
        private readonly IWebhookRepository webhookRepository;
        private readonly IParcelRepository parcelRepository;
        private readonly ILogger<WebhookManager> logger;

        public WebhookManager(
            IWebhookRepository webhookRepository,
            IParcelRepository parcelRepository,
            ILogger<WebhookManager> logger
        )
        {
            this.webhookRepository = webhookRepository;
            this.parcelRepository = parcelRepository;
            this.logger = logger;
        }

        public IEnumerable<Webhook> ListParcelWebhooks(string trackingId)
        {
            return webhookRepository.GetAllByTrackingId(trackingId);
        }

        public Webhook SubscribeParcelWebhook(string trackingId, string url)
        {
            var parcel = parcelRepository.GetByTrackingId(trackingId);
            if (parcel == null)
            {
                logger.LogError($"No Parcel for given trackingId ({trackingId}) found.");
                throw new DalException($"Error occured while getting Parcel with trackingId ({trackingId}).");
            }
            
            var webhook = new Webhook
            {
                Parcel = parcel,
                Url = url,
                CreatedAt = DateTime.Now
            };
            webhookRepository.Create(webhook);
            return webhook;
        }

        public void UnsubscribeParcelWebhook(long id)
        {
            webhookRepository.Delete(id);
        }
    }
}