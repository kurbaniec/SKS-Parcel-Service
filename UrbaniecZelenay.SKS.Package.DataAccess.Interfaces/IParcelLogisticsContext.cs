using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelLogisticsContext
    {
        int SaveChanges();
        EntityEntry Entry(object entity);
        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }
        //DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        DbSet<Hop> Hops { get; set; }
        DbSet<PreviousHop> PreviousHops { get; set; }
        DbSet<HopArrival> HopArrivals { get; set; }
        DbSet<Parcel> Parcels { get; set; }
        DbSet<Recipient> Recipients { get; set; }
        DbSet<Transferwarehouse> Transferwarehouses { get; set; }
        DbSet<Truck> Trucks { get; set; }
        DbSet<Warehouse> Warehouses { get; set; }
        DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        
        DbSet<Webhook> Webhooks { get; set; }
    }
}