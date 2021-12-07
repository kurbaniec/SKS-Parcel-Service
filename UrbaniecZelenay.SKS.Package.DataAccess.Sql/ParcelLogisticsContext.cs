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

            /*modelBuilder.Entity<PreviousHop>()
                .HasOne(h => h.Hop)
                .WithOne()
                .HasForeignKey<PreviousHop?>(h => h.HopCode)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);*/
            modelBuilder.Entity<PreviousHop>()
                .HasOne(h => h.Hop)
                .WithMany()
                .HasForeignKey(h => h.HopCode)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // modelBuilder.Entity<PreviousHop>()
            //     .HasOne(h => h.OriginalHop)
            //     .WithOne()
            //     .HasForeignKey<PreviousHop>(h => h.OriginalHopCode)
            //     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Hop>()
                .HasOne(h => h.PreviousHop)
                .WithOne(h => h.OriginalHop)
                .HasForeignKey<PreviousHop>(h => h.OriginalHopCode);

            modelBuilder.Entity<PreviousHop>()
                .HasIndex(x => x.HopCode)
                .IsUnique(false);
            modelBuilder.Entity<PreviousHop>()
                .HasIndex(x => x.OriginalHopCode)
                .IsUnique(false);


            // modelBuilder.Entity<PreviousHop>()
            //     .HasOne(p => p.OriginalHop)
            //     .WithOne(p => p.PreviousHop!)
            //     .HasForeignKey<Hop>(p => p.PreviousHopId)
            //     .OnDelete(DeleteBehavior.Restrict);
            //
            // modelBuilder.Entity<PreviousHop>()
            //     .HasOne(p => p.Hop)
            //     .WithOne()
            //     .OnDelete(DeleteBehavior.Restrict)


            // modelBuilder.Entity<Hop>()
            //     .HasOne(h => h.PreviousHop)
            //     .WithOne(p => p.ChildHop)

            // modelBuilder.Entity<Hop>()
            //     .HasOne(h => h.PreviousHop)
            //     .WithOne(p => p!.Hop)
            //     .HasForeignKey<PreviousHop>(p => p.HopId)
            //     .OnDelete(DeleteBehavior.Cascade);
            //.HasForeignKey<PreviousHop>(h => h.HopId)
            //.OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<PreviousHop>()
            //     .HasOne(p => p.Hop)
            //     .WithOne(p => p.PreviousHop!);

            // modelBuilder.Entity<Warehouse>()
            //     .HasMany(w => w.NextHops)
            //     .WithOne();
            // modelBuilder.Entity<PreviousHop>()
            //     .HasOne(p => p.Hop)
            //     .WithOne()
            //     .HasForeignKey<Hop>(h => h.PreviousHopId)
            //     .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<Hop>().Property(h => h.LocationCoordinates).HasColumnType("geometry (point)");
            // modelBuilder.Entity<Warehouse>().Property(h => h.LocationCoordinates).HasColumnType("geometry (point)");
            // modelBuilder.Entity<Truck>().Property(h => h.LocationCoordinates).HasColumnType("geometry (point)");
            // modelBuilder.Entity<Transferwarehouse>().Property(h => h.LocationCoordinates).HasColumnType("geometry (point)");
            // modelBuilder.Entity<Recipient>().Property(r => r.GeoLocation).HasColumnType("geometry (point)");
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