using Microsoft.AspNetCore.Mvc;

namespace ParkingPlaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private static readonly DateTime StartUtc = DateTime.UtcNow;

        [HttpGet]
        public IActionResult Get()
        {
            var uptime = DateTime.UtcNow - StartUtc;

            return Ok(new
            {
                status = "ok",
                uptime = $"{uptime.Days}d {uptime.Hours:D2}h {uptime.Minutes:D2}m {uptime.Seconds:D2}s",
                uptimeSeconds = (long)uptime.TotalSeconds,
                timestamp = DateTime.UtcNow.ToString("O")
            });
        }
    }
}
