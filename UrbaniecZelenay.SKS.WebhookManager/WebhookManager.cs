using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Entities;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;

namespace UrbaniecZelenay.SKS.WebhookManager
{
    public class WebhookManager : IWebhookManager
    {
        private readonly HttpClient client;
        private readonly IWebhookRepository webhookRepository;
        private readonly IParcelRepository parcelRepository;
        private readonly IMapper mapper;
        private readonly ILogger<WebhookManager> logger;

        public WebhookManager(
            IWebhookRepository webhookRepository,
            IParcelRepository parcelRepository,
            IMapper mapper,
            ILogger<WebhookManager> logger
        )
        {
            this.client = new HttpClient();
            this.webhookRepository = webhookRepository;
            this.parcelRepository = parcelRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        public WebhookManager(
            HttpClient client,
            IWebhookRepository webhookRepository,
            IParcelRepository parcelRepository,
            IMapper mapper,
            ILogger<WebhookManager> logger
        )
        {
            this.client = client;
            this.webhookRepository = webhookRepository;
            this.parcelRepository = parcelRepository;
            this.mapper = mapper;
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

        public void NotifyParcelWebhooks(Parcel parcel)
        {
            var webhookMessage = mapper.Map<WebhookMessage>(parcel);
            var webhookJson = JsonConvert.SerializeObject(webhookMessage);
            var webhooks = webhookRepository.GetAllByTrackingId(parcel.TrackingId!);
            client.DefaultRequestHeaders.Referrer = new Uri("https://sks-team-x.azurewebsites.net/");
            foreach (var webhook in webhooks)
            {
                var url = $"{webhook.Url}?trackingId={webhook.TrackingId}";
                try
                {
                    var task = Task.Run(() => client.PostAsync(url, new StringContent(
                        webhookJson, Encoding.UTF8, "application/json"
                    )));
                    task.Wait();
                } 
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        if (e is HttpRequestException)
                        {
                            logger.LogError(e, $"Error notifying Webhook with URL {url}.");
                        }

                        logger.LogError(e, "Error unhandled Exception!");
                    }
                }
            }
        }

        public void UnsubscribeAllParcelWebhooks(string trackingId)
        {
           webhookRepository.DeleteAll(trackingId);
        }
    }
}