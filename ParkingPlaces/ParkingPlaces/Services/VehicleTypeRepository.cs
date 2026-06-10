using ParkingPlaces.Data;
using ParkingPlaces.Models;

namespace ParkingPlaces.Services;

public interface IVehicleTypeRepository
{
    IEnumerable<VehicleType> GetAll();
    VehicleType? GetById(int id);
    VehicleType Create(VehicleType vt);
    VehicleType? Update(int id, VehicleType vt);
    bool Delete(int id);
}

public class SqliteVehicleTypeRepository : IVehicleTypeRepository
{
    private readonly ParkingPlacesDbContext _context;

    public SqliteVehicleTypeRepository(ParkingPlacesDbContext context)
    {
        _context = context;
    }

    public IEnumerable<VehicleType> GetAll() => _context.VehicleTypes.ToList();

    public VehicleType? GetById(int id) => _context.VehicleTypes.Find(id);

    public VehicleType Create(VehicleType vt)
    {
        _context.VehicleTypes.Add(vt);
        _context.SaveChanges();
        return vt;
    }

    public VehicleType? Update(int id, VehicleType updated)
    {
        var existing = _context.VehicleTypes.Find(id);
        if (existing is null) return null;
        existing.Name = updated.Name;
        existing.Description = updated.Description;
        existing.PricePerHour = updated.PricePerHour;
        _context.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var vt = _context.VehicleTypes.Find(id);
        if (vt is null) return false;
        _context.VehicleTypes.Remove(vt);
        _context.SaveChanges();
        return true;
    }
}
