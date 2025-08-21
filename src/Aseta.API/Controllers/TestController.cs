using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers
{
    [Route("[controller]")]
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
