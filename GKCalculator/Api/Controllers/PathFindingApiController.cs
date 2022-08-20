using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Model.DataTransfer;
using Model.Domain;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PathFindingApiController : ControllerBase
    {
        private readonly ILogger<PathFindingApiController> _logger;

        private readonly IMapper _mapper;
        private readonly IPathFindingService _pathFindingService;

        public PathFindingApiController(
            IPathFindingService pathFindingService,
            IMapper mapper,
            ILogger<PathFindingApiController> logger)
        { 
            _logger = logger;
            _mapper = mapper;

            _pathFindingService = pathFindingService;
        }

        [HttpPost("Calculate")]
        [EnableCors("AllowOrigin")]
        public LineagePathFindingResponse Post(LineagePathFindingRequest request)
        {
            var defaultRequestModel = _mapper.Map<LineagePathFindingModel>(CreateDefaultRequest(request));
            var requestModel = _mapper.Map<LineagePathFindingModel>(request);

            var defaultResults = _pathFindingService.FindPath(defaultRequestModel);
            var results = _pathFindingService.FindPath(requestModel);

            return new LineagePathFindingResponse
            {
                DefaultPath = _mapper.Map<List<PathModelResponse>>(defaultResults),
                CustomPath = _mapper.Map<List<PathModelResponse>>(results),
            };
        }

        private LineagePathFindingRequest CreateDefaultRequest(LineagePathFindingRequest prototype)
        {
            return new LineagePathFindingRequest
            {
                Destinations = prototype.Destinations,
                SourcePoint = prototype.SourcePoint,
                UseWyvern = true,
                UseShips = true,
                UseSoe = true
            };
        }
    }
}
