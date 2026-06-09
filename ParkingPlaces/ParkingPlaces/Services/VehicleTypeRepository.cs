using ParkingPlaces.Models;

namespace ParkingPlaces.Services
{
    public interface IVehicleTypeRepository
    {
        IEnumerable<VehicleType> GetAll();
        VehicleType? GetById(int id);
        VehicleType Create(VehicleType vt);
        VehicleType? Update(int id, VehicleType vt);
        bool Delete(int id);
    }

    public class InMemoryVehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly List<VehicleType> _types = new()
        {
            new VehicleType { Id = 1, Name = "Car", Description = "Standard passenger car", PricePerHour = 5.00m },
            new VehicleType { Id = 2, Name = "Motorcycle", Description = "Motorcycle or scooter", PricePerHour = 2.50m },
            new VehicleType { Id = 3, Name = "Truck", Description = "Large commercial vehicle", PricePerHour = 10.00m }
        };
        private int _nextId = 4;

        public IEnumerable<VehicleType> GetAll() => _types;
        public VehicleType? GetById(int id) => _types.FirstOrDefault(v => v.Id == id);

        public VehicleType Create(VehicleType vt)
        {
            vt.Id = _nextId++;
            _types.Add(vt);
            return vt;
        }

        public VehicleType? Update(int id, VehicleType updated)
        {
            var existing = GetById(id);
            if (existing is null) return null;
            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.PricePerHour = updated.PricePerHour;
            return existing;
        }

        public bool Delete(int id)
        {
            var vt = GetById(id);
            if (vt is null) return false;
            _types.Remove(vt);
            return true;
        }
    }
}
