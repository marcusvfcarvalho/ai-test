using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingPlaces.Data;

namespace ParkingPlaces.Tests;

public class ParkingPlaceTests : WebApplicationFactory<Program>, IDisposable
{
    private static readonly string DbPath = Path.Combine(Path.GetTempPath(), $"parkingplaces_test_{Guid.NewGuid():N}.db");
    private static bool _initialized;

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

            // Add SQLite using a file-based database for testing
            var connectionString = $"Data Source={DbPath}";
            services.AddDbContext<ParkingPlacesDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

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
            if (File.Exists(DbPath))
                File.Delete(DbPath);
        }
        catch
        {
            // Best effort cleanup
        }
    }
}
