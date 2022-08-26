using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Model.DataTransfer;
using Model.Domain;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PathFindingController : ControllerBase
    {
        private readonly ILogger<PathFindingController> _logger;

        private readonly IMapper _mapper;
        private readonly IPathFindingService _pathFindingService;
        private readonly IRouteProviderService _nodeProviderService;

        public PathFindingController(
            IPathFindingService pathFindingService,
            IRouteProviderService nodeProviderService,
            IMapper mapper,
            ILogger<PathFindingController> logger)
        { 
            _logger = logger;
            _mapper = mapper;

            _pathFindingService = pathFindingService;
            _nodeProviderService = nodeProviderService;
        }

        [HttpPost("Calculate")]
        [EnableCors("AllowOrigin")]
        public async Task<LineagePathFindingResponse> Post(LineagePathFindingRequest request)
        {
            _logger.LogInformation("Calculation requested", request);

            var requestModel = _mapper.Map<LineagePathFindingModel>(request);
            requestModel.SourcePoint = await _nodeProviderService.GetSourceById(request.SourcePoint);
            requestModel.Destinations = await _nodeProviderService.GetDestinationListByIds(request.Destinations);

            var defaultRequestModel = CreateDefaultRequest(requestModel);

            var resultTask = _pathFindingService.FindPath(requestModel);
            var defaultResultTask = _pathFindingService.FindPath(defaultRequestModel);

            await Task.WhenAll(resultTask, defaultResultTask);

            _logger.LogInformation("Calculation succeeded", request);

            return new LineagePathFindingResponse
            {
                DefaultPath = _mapper.Map<List<PathModelResponse>>(defaultResultTask.Result),
                CustomPath = _mapper.Map<List<PathModelResponse>>(resultTask.Result),
            };
        }

        private LineagePathFindingModel CreateDefaultRequest(LineagePathFindingModel prototype)
        {
            return new LineagePathFindingModel
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
