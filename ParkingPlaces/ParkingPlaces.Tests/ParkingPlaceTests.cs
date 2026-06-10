using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingPlaces.Data;

namespace ParkingPlaces.Tests;

public class ParkingPlaceTests : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"parkingplaces_test_{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove all services related to the DbContext (registered by AddDbContext in Program)
            var toRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<ParkingPlacesDbContext>) ||
                d.ServiceType == typeof(ParkingPlacesDbContext) ||
                d.ImplementationType == typeof(ParkingPlacesDbContext)).ToList();

            foreach (var svc in toRemove)
                services.Remove(svc);

            // Also remove any remaining service whose type name contains the DbContext type
            var remaining = services.ToList();
            foreach (var svc in remaining)
            {
                if (svc.ServiceType.FullName != null &&
                    svc.ServiceType.FullName.Contains("ParkingPlacesDbContext") &&
                    !toRemove.Contains(svc))
                    services.Remove(svc);
            }

            // Add SQLite using a file-based database for testing (unique per instance)
            var connectionString = $"Data Source={_dbPath}";
            services.AddDbContext<ParkingPlacesDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            // Delete stale db file if exists (from previous crashed runs)
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);

            // Create schema
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ParkingPlacesDbContext>();
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Testing");
    }

    public new void Dispose()
    {
        base.Dispose();
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch
        {
            // Best effort cleanup
        }
    }
}
