using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    public class ParcelRepository : IParcelRepository
    {
        private readonly IParcelLogisticsContext context;
        public bool UseTransactions { get; set; } = true;
        private readonly ILogger<ParcelRepository> logger;

        public ParcelRepository(ILogger<ParcelRepository> logger, IParcelLogisticsContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public Parcel Create(Parcel parcel)
        {
            try
            {
                logger.LogInformation($"Create Parcel {parcel}");
                // Working with transactions
                // See: https://docs.microsoft.com/en-us/ef/ef6/saving/transactions
                using IDbContextTransaction transaction = context.Database.BeginTransaction();
                // ParcelLogisticsContext c = new ParcelLogisticsContext();
                // c.Database.BeginTransaction();
                var trackingId = context.Parcels.Max(p => p.TrackingId) ?? "000000000";
                // See: https://stackoverflow.com/a/5418361/12347616
                var newTrackingId = $"{int.Parse(trackingId) + 1:D9}";
                parcel.TrackingId = newTrackingId;
                context.Parcels.Add(parcel);
                context.SaveChanges();
                transaction.Commit();
            }
            catch (SqlException e)
            {
                logger.LogError(e, "Error occured when creating a Parcel.");
                throw new DalSaveException("Error occured when creating a parcel", e);
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, "Error occured when creating a Parcel.");
                throw new DalSaveException("Error occured when creating a parcel", e);
            }

            return parcel;
        }

        public Parcel Update(Parcel parcel)
        {
            logger.LogInformation($"Update Parcel {parcel}");
            context.Parcels.Update(parcel);
            context.SaveChanges();
            return parcel;
        }

        public void Delete(Parcel parcel)
        {
            logger.LogInformation($"Delete Parcel with Tracking Id {parcel}");
            context.Parcels.Remove(parcel);
            context.SaveChanges();
        }

        public Parcel? GetByTrackingId(string trackingId)
        {
            logger.LogInformation($"Return Parcel with Tracking Id {trackingId}");
            return context.Parcels
                .Include(p => p.VisitedHops)
                .Include(p => p.FutureHops)
                .FirstOrDefault(p => p.TrackingId == trackingId);
        }
    }
}