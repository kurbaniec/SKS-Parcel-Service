using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class ParcelLogisticsContext : DbContext, IParcelLogisticsContext
    {
        public ParcelLogisticsContext()
        {
        }

        // Constructor used by ASP.NET Core
        public ParcelLogisticsContext(DbContextOptions<ParcelLogisticsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Source: Max
            // Setup database properties for Azure
            // But Breaks local development
            // See: https://stackoverflow.com/a/61475516/12347616
            // Environment Workaround
            // See: https://stackoverflow.com/a/55620080/12347616
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                modelBuilder.HasDatabaseMaxSize("1 GB");
                modelBuilder.HasServiceTier("Basic");
            }
            
            // Map PreviousHop with fluent API because of data annotations limitations
            // -----------------------------------------------------------------------
            // This does not work, results in missing hop codes
            // modelBuilder.Entity<PreviousHop>()
            //     .HasOne(h => h.Hop)
            //     .WithOne()
            //     .HasForeignKey<PreviousHop?>(h => h.HopCode)
            //     .OnDelete(DeleteBehavior.Restrict)
            //     .IsRequired(false);
            // (https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#manual-configuration)
            // Docs say that withOne should work (no collection) but in praxis it does not
            // Maybe because of HopCode of WarehouseNextHops?
            modelBuilder.Entity<PreviousHop>()
                .HasOne(h => h.Hop)
                .WithMany()
                .HasForeignKey(h => h.HopCode)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            
            modelBuilder.Entity<Hop>()
                .HasOne(h => h.PreviousHop)
                .WithOne(h => h.OriginalHop)
                .HasForeignKey<PreviousHop>(h => h.OriginalHopCode);
            
            // Disable Unique constraint on generated indexes 
            modelBuilder.Entity<PreviousHop>()
                .HasIndex(x => x.HopCode)
                .IsUnique(false);
            modelBuilder.Entity<PreviousHop>()
                .HasIndex(x => x.OriginalHopCode)
                .IsUnique(false);

            base.OnModelCreating(modelBuilder);
        }

        // Create properties
        // See: https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli#create-the-model
        //public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public DbSet<Hop> Hops { get; set; }
        public DbSet<PreviousHop> PreviousHops { get; set; }
        public DbSet<HopArrival> HopArrivals { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Transferwarehouse> Transferwarehouses { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
    }
}