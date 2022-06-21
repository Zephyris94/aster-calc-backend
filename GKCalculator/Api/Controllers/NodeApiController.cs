using System.Collections.Generic;
using Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeApiController : ControllerBase
    {
        private readonly INodeCacheService _nodeCacheService;

        public NodeApiController(INodeCacheService cacheService)
        {
            _nodeCacheService = cacheService;
        }

        [HttpGet("Sources")]
        [EnableCors("AllowAllOrigins")]
        public List<string> Sources()
        {
            return _nodeCacheService.GetSources();
        }

        [HttpGet("Destinations")]
        [EnableCors("AllowAllOrigins")]
        public List<string> Destinations()
        {
            return _nodeCacheService.GetDestinations();
        }
    }
}
