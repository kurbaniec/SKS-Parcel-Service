using System.Collections.Generic;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.WebhookManager.Interfaces
{
    public interface IWebhookRepository
    {
        Webhook Create(Webhook webhook);

        IEnumerable<Webhook> GetAllByTrackingId(string trackingId);

        void Delete(long id);

        void DeleteAll(string trackingId);
    }
}