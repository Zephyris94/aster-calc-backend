using Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IDataMigrationService _dataMigrationService;

        public SystemController(IDataMigrationService dataMigrationService)
        {
            _dataMigrationService = dataMigrationService;
        }

        [HttpGet("Seed")]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> Sources()
        {
            await _dataMigrationService.SeedDataAsync();
            return Ok();
        }
    }
}
