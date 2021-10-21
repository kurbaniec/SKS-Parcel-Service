using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    public class ParcelRepository : IParcelRepository
    {
        private readonly ParcelLogisticsContext context;

        public ParcelRepository(ParcelLogisticsContext context)
        {
            this.context = context;
        }
        
        public Parcel Create(Parcel parcel)
        {
            // Working with transactions
            // See: https://docs.microsoft.com/en-us/ef/ef6/saving/transactions
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            var trackingId = context.Parcels.Max(p => p.TrackingId) ?? "000000000";
            // See: https://stackoverflow.com/a/5418361/12347616
            var newTrackingId = $"{int.Parse(trackingId) + 1:D9}";
            parcel.TrackingId = newTrackingId;
            context.Parcels.Add(parcel);
            context.SaveChanges();
            transaction.Commit();
            return parcel;
        }

        public Parcel Update(Parcel parcel)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Parcel parcel)
        {
            throw new System.NotImplementedException();
        }

        public Parcel GetByTrackingId(string trackingId)
        {
            throw new System.NotImplementedException();
        }
    }
}