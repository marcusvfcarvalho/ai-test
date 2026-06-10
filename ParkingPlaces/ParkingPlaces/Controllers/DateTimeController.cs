using Microsoft.AspNetCore.Mvc;

namespace ParkingPlaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DateTimeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var now = DateTime.UtcNow;

            return Ok(new
            {
                dateTime = now.ToString("O"),
                date = now.ToString("yyyy-MM-dd"),
                time = now.ToString("HH:mm:ss"),
                timestamp = now,
                unixTimestamp = ((DateTimeOffset)now).ToUnixTimeSeconds()
            });
        }
    }
}
