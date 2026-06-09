using ParkingPlaces.Models;

namespace ParkingPlaces.Services
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAll();
        City? GetById(int id);
        City Create(City city);
        City? Update(int id, City city);
        bool Delete(int id);
    }

    public class InMemoryCityRepository : ICityRepository
    {
        private readonly List<City> _cities = new()
        {
            new City { Id = 1, Name = "Sao Paulo", State = "SP", Country = "Brazil", Population = 12325232 },
            new City { Id = 2, Name = "Rio de Janeiro", State = "RJ", Country = "Brazil", Population = 6747815 }
        };
        private int _nextId = 3;

        public IEnumerable<City> GetAll() => _cities;
        public City? GetById(int id) => _cities.FirstOrDefault(c => c.Id == id);

        public City Create(City city)
        {
            city.Id = _nextId++;
            _cities.Add(city);
            return city;
        }

        public City? Update(int id, City updated)
        {
            var existing = GetById(id);
            if (existing is null) return null;
            existing.Name = updated.Name;
            existing.State = updated.State;
            existing.Country = updated.Country;
            existing.Population = updated.Population;
            return existing;
        }

        public bool Delete(int id)
        {
            var city = GetById(id);
            if (city is null) return false;
            _cities.Remove(city);
            return true;
        }
    }
}
