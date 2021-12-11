using System.Collections.Generic;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelWebhookLogic
    {
        /// <summary>
        /// Get all registered subscriptions for the parcel webhook.
        /// </summary>
        IEnumerable<Webhook> ListParcelWebhooks(string? trackingId);

        /// <summary>
        /// Subscribe to a webhook notification for the specific parcel.
        /// </summary>
        Webhook SubscribeParcelWebhook(string? trackingId, string? url);

        /// <summary>
        /// Remove an existing webhook subscription.
        /// </summary>
        void UnsubscribeParcelWebhook(long? id);
    }
}