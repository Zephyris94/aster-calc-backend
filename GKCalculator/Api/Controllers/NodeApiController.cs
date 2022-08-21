using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model.DataTransfer;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeApiController : ControllerBase
    {
        private readonly INodeCacheService _nodeCacheService;
        private readonly IMapper _mapper;

        public NodeApiController(INodeCacheService cacheService, IMapper mapper)
        {
            _nodeCacheService = cacheService;

            _mapper = mapper;
        }

        [HttpGet("Sources")]
        [EnableCors("AllowOrigin")]
        public List<NodeResponse> Sources()
        {
            var response = _nodeCacheService.GetSources();
            return _mapper.Map<List<NodeResponse>>(response);
        }

        [HttpGet("Destinations")]
        [EnableCors("AllowOrigin")]
        public List<NodeResponse> Destinations()
        {
            var response = _nodeCacheService.GetDestinations();
            return _mapper.Map<List<NodeResponse>>(response);
        }
    }
}
