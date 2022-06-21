using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Services;
using Model;
using Model.DataTransfer;
using Model.Domain;

namespace Core.Services
{
    public class PathFindingService : IPathFindingService
    {
        private readonly IGraphBuildingService _graphBuildingService;
        private readonly INodeCacheService _cacheService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;

        public PathFindingService(
            IGraphBuildingService graphBuildingService,
            INodeCacheService cacheService,
            IPathFindingAlgorithm pathFindingAloAlgorithm)
        {
            _graphBuildingService = graphBuildingService;
            _cacheService = cacheService;
            _pathFindingAlgorithm = pathFindingAloAlgorithm;
        }

        public List<PathModel> FindPath(LineagePathFindingModel request)
        {
            var edges = _graphBuildingService.GetRequiredEdges(_cacheService.Edges, request.UseWyvern, request.UseShips, request.UseSoe);
            var graph = _graphBuildingService.GetGraphFromEdges(edges);

            _pathFindingAlgorithm.InitDijkstraPathFindingAlgorithm(graph);

            var path = _pathFindingAlgorithm.FindShortestPath(request.SourcePoint, request.Destinations[0]);
            for (int i = 0; i < request.Destinations.Count - 1; i++)
            {
                var pathChunk = _pathFindingAlgorithm.FindShortestPath(request.Destinations[i], request.Destinations[i + 1]);
                path += ";" + pathChunk;
            }

            var results = _graphBuildingService.GetResultsFromPath(edges, path);

            return results;
        }
    }
}
