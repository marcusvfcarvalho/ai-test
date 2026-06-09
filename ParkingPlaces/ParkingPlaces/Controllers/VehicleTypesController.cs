using Microsoft.AspNetCore.Mvc;
using ParkingPlaces.Models;
using ParkingPlaces.Services;

namespace ParkingPlaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeRepository _repository;
        public VehicleTypesController(IVehicleTypeRepository repository) => _repository = repository;

        [HttpGet]
        public ActionResult<IEnumerable<VehicleType>> GetAll() => Ok(_repository.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<VehicleType> GetById(int id)
        {
            var vt = _repository.GetById(id);
            return vt is null ? NotFound(new { message = $"Vehicle type with id {id} not found." }) : Ok(vt);
        }

        [HttpPost]
        public ActionResult<VehicleType> Create([FromBody] VehicleType vt)
        {
            var created = _repository.Create(vt);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public ActionResult<VehicleType> Update(int id, [FromBody] VehicleType vt)
        {
            var updated = _repository.Update(id, vt);
            return updated is null ? NotFound(new { message = $"Vehicle type with id {id} not found." }) : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return _repository.Delete(id) ? NoContent() : NotFound(new { message = $"Vehicle type with id {id} not found." });
        }
    }
}
