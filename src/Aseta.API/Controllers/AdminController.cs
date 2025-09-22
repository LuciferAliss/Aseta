using Aseta.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;

        [HttpPut]
        public async Task<ActionResult> LockoutUser(Guid userId)
        {
            await _adminService.BlockUserAsync(userId);

            return Ok();
        }
    }
}
