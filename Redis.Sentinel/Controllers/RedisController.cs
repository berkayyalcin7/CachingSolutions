using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Redis.Sentinel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {

        [HttpGet("[action]/{key}/{value}")]
        public IActionResult SetValue(string key , string value)
        {
            return Ok();
        }

        [HttpGet("[action]/{key}")]
        public IActionResult GetValue(string key)
        {
            return Ok();
        }
    }
}
