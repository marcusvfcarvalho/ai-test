using Microsoft.EntityFrameworkCore;
using ParkingPlaces.Models;

namespace ParkingPlaces.Data;

public class ParkingPlacesDbContext : DbContext
{
    public ParkingPlacesDbContext(DbContextOptions<ParkingPlacesDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities => Set<City>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VehicleType>().HasData(
            new VehicleType { Id = 1, Name = "Car", Description = "Standard passenger car", PricePerHour = 5.00m },
            new VehicleType { Id = 2, Name = "Motorcycle", Description = "Motorcycle or scooter", PricePerHour = 2.50m },
            new VehicleType { Id = 3, Name = "Truck", Description = "Large commercial vehicle", PricePerHour = 10.00m }
        );
    }
}
