using Microsoft.EntityFrameworkCore;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    public class ParcelLogisticsContext : DbContext
    {
        // Constructor used by ASP.NET Core
        public ParcelLogisticsContext(DbContextOptions<ParcelLogisticsContext> options) : base(options)
        {
        }

        // Create properties
        // See: https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli#create-the-model
        public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public DbSet<Hop> Hops { get; set; }
        public DbSet<HopArrival> HopArrivals { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Transferwarehouse> Transferwarehouses { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
    }
}