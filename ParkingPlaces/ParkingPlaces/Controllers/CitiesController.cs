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

        public CitiesController(ICityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<City>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id:int}")]
        public ActionResult<City> GetById(int id)
        {
            var city = _repository.GetById(id);
            if (city is null)
                return NotFound(new { message = $"City with id {id} not found." });
            return Ok(city);
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
            if (updated is null)
                return NotFound(new { message = $"City with id {id} not found." });
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleted = _repository.Delete(id);
            if (!deleted)
                return NotFound(new { message = $"City with id {id} not found." });
            return NoContent();
        }
    }
}
