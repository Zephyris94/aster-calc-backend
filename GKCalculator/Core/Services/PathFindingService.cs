using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Settings;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Model.Domain;

namespace Core.Services
{
    public class PathFindingService : IPathFindingService
    {
        private readonly IGraphBuildingService _graphBuildingService;
        private readonly INodeCacheService _cacheService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;
        private readonly IRouteRepository _routeRepo;

        private readonly bool _useCache;

        public PathFindingService(
            IGraphBuildingService graphBuildingService,
            INodeCacheService cacheService,
            IPathFindingAlgorithm pathFindingAloAlgorithm,
            IRouteRepository routeRepo,
            IOptions<DataSourceOptions> dataSettings)
        {
            _graphBuildingService = graphBuildingService;
            _cacheService = cacheService;
            _pathFindingAlgorithm = pathFindingAloAlgorithm;
            _routeRepo = routeRepo;

            _useCache = dataSettings.Value.UseCache;
        }

        public async Task<List<RouteModel>> FindPath(LineagePathFindingModel request)
        {
            if (_useCache)
            {
                var routes = await _cacheService.GetOrCreateRoutes(async () => await _routeRepo.GetRoutes());
            }
            else
            {
                var routes = await _routeRepo.GetRoutes();
            }

            var edges = _graphBuildingService.GetRequiredEdges(_cacheService.GetRoutes(), request.UseWyvern, request.UseShips, request.UseSoe);
            var graph = _graphBuildingService.GetGraphFromEdges(edges);

            _pathFindingAlgorithm.InitAlgorithm(graph);

            var path = _pathFindingAlgorithm.FindShortestPath(request.SourcePoint.Name, request.Destinations[0].Name);
            for (int i = 0; i < request.Destinations.Count - 1; i++)
            {
                var pathChunk = _pathFindingAlgorithm.FindShortestPath(request.Destinations[i].Name, request.Destinations[i + 1].Name);
                path += ";" + pathChunk;
            }

            var results = _graphBuildingService.GetResultsFromPath(edges, path);

            return results;
        }
    }
}
