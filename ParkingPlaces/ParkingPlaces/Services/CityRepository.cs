using ParkingPlaces.Data;
using ParkingPlaces.Models;

namespace ParkingPlaces.Services;

public interface ICityRepository
{
    IEnumerable<City> GetAll();
    City? GetById(int id);
    City Create(City city);
    City? Update(int id, City city);
    bool Delete(int id);
}

public class SqliteCityRepository : ICityRepository
{
    private readonly ParkingPlacesDbContext _context;

    public SqliteCityRepository(ParkingPlacesDbContext context)
    {
        _context = context;
    }

    public IEnumerable<City> GetAll() => _context.Cities.ToList();

    public City? GetById(int id) => _context.Cities.Find(id);

    public City Create(City city)
    {
        _context.Cities.Add(city);
        _context.SaveChanges();
        return city;
    }

    public City? Update(int id, City updated)
    {
        var existing = _context.Cities.Find(id);
        if (existing is null) return null;
        existing.Name = updated.Name;
        existing.State = updated.State;
        existing.Country = updated.Country;
        existing.Population = updated.Population;
        _context.SaveChanges();
        return existing;
    }

    public bool Delete(int id)
    {
        var city = _context.Cities.Find(id);
        if (city is null) return false;
        _context.Cities.Remove(city);
        _context.SaveChanges();
        return true;
    }
}
