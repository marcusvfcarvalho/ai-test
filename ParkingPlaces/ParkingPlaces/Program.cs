using Microsoft.EntityFrameworkCore;
using ParkingPlaces.Data;
using ParkingPlaces.Services;

namespace ParkingPlaces
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var connectionString = builder.Configuration.GetConnectionString("ParkingPlaces")
                ?? $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "parkingplaces.db")}";

            builder.Services.AddDbContext<ParkingPlacesDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddScoped<ICityRepository, SqliteCityRepository>();
            builder.Services.AddScoped<IVehicleTypeRepository, SqliteVehicleTypeRepository>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ParkingPlacesDbContext>();
                db.Database.EnsureCreated();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
