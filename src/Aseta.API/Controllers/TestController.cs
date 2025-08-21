using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }
    }
}
