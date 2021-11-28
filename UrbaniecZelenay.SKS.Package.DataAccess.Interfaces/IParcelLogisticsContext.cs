using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelLogisticsContext
    {
        int SaveChanges();
        DatabaseFacade Database { get; }
        //DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        DbSet<Hop> Hops { get; set; }
        DbSet<HopArrival> HopArrivals { get; set; }
        DbSet<Parcel> Parcels { get; set; }
        DbSet<Recipient> Recipients { get; set; }
        DbSet<Transferwarehouse> Transferwarehouses { get; set; }
        DbSet<Truck> Trucks { get; set; }
        DbSet<Warehouse> Warehouses { get; set; }
        DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
    }
}