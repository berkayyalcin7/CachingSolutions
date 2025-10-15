using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace DistributedCaching.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("Name");
            var surnameBinary = await _distributedCache.GetAsync("Surname");

            var surname=Encoding.UTF8.GetString(surnameBinary);

            return Ok(new { name, surname });
        }

        [HttpGet("Set")]
        public async Task<IActionResult> Set(string name,string surname)
        {
            await _distributedCache.SetStringAsync("Name", name,options : new()
            {
                AbsoluteExpiration=DateTime.Now.AddSeconds(30),
                SlidingExpiration=TimeSpan.FromSeconds(5)
            });

            await _distributedCache.SetAsync("Surname", Encoding.UTF8.GetBytes(surname));

            return Ok();
        }
    }
}
