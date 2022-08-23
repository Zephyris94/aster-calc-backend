using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Model.DataTransfer;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly IRouteProviderService _providerService;
        private readonly IMapper _mapper;

        public NodeController(IRouteProviderService providerService, IMapper mapper)
        {
            _providerService = providerService;

            _mapper = mapper;
        }

        [HttpGet("Sources")]
        [EnableCors("AllowOrigin")]
        public async Task<List<NodeResponse>> Sources()
        {
            var response = await _providerService.GetSources();
            return _mapper.Map<List<NodeResponse>>(response);
        }

        [HttpGet("Destinations")]
        [EnableCors("AllowOrigin")]
        public async Task<List<NodeResponse>> Destinations()
        {
            var response = await _providerService.GetDestinations();
            return _mapper.Map<List<NodeResponse>>(response);
        }
    }
}
