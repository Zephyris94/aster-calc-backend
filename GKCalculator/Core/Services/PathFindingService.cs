using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Settings;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Model.Domain;

namespace Core.Services
{
    public class PathFindingService : IPathFindingService
    {
        private readonly IGraphBuildingService _graphBuildingService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;
        private readonly IRouteProviderService _routeProviderService;

        private readonly bool _useCache;

        public PathFindingService(
            IGraphBuildingService graphBuildingService,
            IPathFindingAlgorithm pathFindingAloAlgorithm,
            IRouteProviderService routeProviderService,
            IOptions<DataSourceOptions> dataSettings)
        {
            _graphBuildingService = graphBuildingService;
            _pathFindingAlgorithm = pathFindingAloAlgorithm;
            _routeProviderService = routeProviderService;

            _useCache = dataSettings.Value.UseCache;
        }

        public async Task<List<RouteModel>> FindPath(LineagePathFindingModel request)
        {
            var routes = await _routeProviderService.GetRoutes();

            var edges = _graphBuildingService.GetRequiredEdges(routes, request.UseWyvern, request.UseShips, request.UseSoe);
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
