using Microsoft.AspNetCore.Mvc;
using ParkingPlaces.Models;
using ParkingPlaces.Services;

namespace ParkingPlaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository _repository;
        public CitiesController(ICityRepository repository) => _repository = repository;

        [HttpGet]
        public ActionResult<IEnumerable<City>> GetAll() => Ok(_repository.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<City> GetById(int id)
        {
            var city = _repository.GetById(id);
            return city is null ? NotFound(new { message = $"City with id {id} not found." }) : Ok(city);
        }

        [HttpPost]
        public ActionResult<City> Create([FromBody] City city)
        {
            var created = _repository.Create(city);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public ActionResult<City> Update(int id, [FromBody] City city)
        {
            var updated = _repository.Update(id, city);
            return updated is null ? NotFound(new { message = $"City with id {id} not found." }) : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return _repository.Delete(id) ? NoContent() : NotFound(new { message = $"City with id {id} not found." });
        }
    }
}
