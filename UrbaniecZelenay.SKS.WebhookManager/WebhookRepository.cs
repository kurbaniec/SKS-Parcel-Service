using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager.Entities;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;

namespace UrbaniecZelenay.SKS.WebhookManager
{
    public class WebhookRepository : IWebhookRepository
    {
        private readonly IParcelLogisticsContext context;
        private readonly ILogger<WebhookRepository> logger;

        public WebhookRepository(ILogger<WebhookRepository> logger, IParcelLogisticsContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public Webhook Create(Webhook webhook)
        {
            logger.LogInformation($"Create Webhook {webhook}");
            try
            {
                context.Webhooks.Add(webhook);
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, "Error creating Webhook");
                throw new DalSaveException("Error occured while creating a Webhook.", e);
            }
            catch (SqlException e)
            {
                logger.LogError(e, "Error creating Webhook");
                throw new DalConnectionException("Error occured while creating a Webhook.", e);
            }

            return webhook;
        }

        public IEnumerable<Webhook> GetAllByTrackingId(string trackingId)
        {
            logger.LogInformation($"Get all Webhooks for trackingId {trackingId}");
            IEnumerable<Webhook> webhooks;
            try
            {
                webhooks = context.Webhooks
                    .Include(w => w.Parcel)
                    .AsEnumerable()
                    .Where(w => w.Parcel.TrackingId == trackingId)
                    .ToList();
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error getting Webhooks for trackingId ({trackingId}).");
                throw new DalConnectionException($"Error occured while getting Webhooks for trackingId ({trackingId}).",
                    e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error getting Webhooks for trackingId ({trackingId}).");
                throw new DalConnectionException($"Error occured while getting Webhooks for trackingId ({trackingId}).",
                    e);
            }

            return webhooks;
        }

        public void Delete(long id)
        {
            logger.LogInformation($"Delete Webhook for Id {id}");
            try
            {
                // Create temporary object for deletion
                // See: https://stackoverflow.com/a/28050510/12347616
                var webhook = new Webhook { Id = id };
                context.Webhooks.Attach(webhook);
                context.Webhooks.Remove(webhook);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error deleting Webhook with Id ({id}).");
                throw new DalConnectionException($"Error occured while deleting Webhook with Id ({id}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error deleting Webhook with Id ({id}).");
                throw new DalConnectionException($"Error occured while deleting Webhook with Id ({id}).", e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                logger.LogError(e, $"Error deleting Webhook with Id ({id}).");
                throw new DalConnectionException($"Error occured while deleting Webhook with Id ({id}).", e);
            }
        }

        public void DeleteAll(string trackingId)
        {
            logger.LogInformation($"Delete Webhooks for trackingId {trackingId}");
            try
            {
                var webhooks = GetAllByTrackingId(trackingId);
                context.Webhooks.RemoveRange(webhooks);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                logger.LogError(e, $"Error deleting Webhooks for trackingId ({trackingId}).");
                throw new DalConnectionException(
                    $"Error occured while deleting Webhooks for trackingId ({trackingId}).", e);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, $"Error deleting Webhooks for trackingId ({trackingId}).");
                throw new DalConnectionException(
                    $"Error occured while deleting Webhooks for trackingId ({trackingId}).", e);
            }
            catch (DbUpdateConcurrencyException e)
            {
                logger.LogError(e, $"Error deleting Webhooks for trackingId ({trackingId}).");
                throw new DalConnectionException(
                    $"Error occured while deleting Webhooks for trackingId ({trackingId}).", e);
            }
        }
    }
}