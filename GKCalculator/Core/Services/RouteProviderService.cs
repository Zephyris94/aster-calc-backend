using Core.Settings;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RouteProviderService : IRouteProviderService
    {
        private readonly INodeCacheService _cacheService;
        private readonly IRouteRepository _routeRepo;

        private readonly bool _useCache;

        public RouteProviderService(
            INodeCacheService cacheService,
            IRouteRepository routeRepo,
            IOptions<DataSourceOptions> dataSettings)
        {
            _cacheService = cacheService;
            _routeRepo = routeRepo;

            _useCache = dataSettings.Value.UseCache;
        }

        public async Task<List<NodeModel>> GetDestinationListByIds(List<int> ids)
        {
            if (_useCache)
            {
                var destinations = await GetFromCache(() => _cacheService.GetDestinations());
                var result = new List<NodeModel>();
                foreach (var id in ids)
                {
                    result.Add(destinations.FirstOrDefault(x => x.Id == id));
                }
                return result;
            }

            return await _routeRepo.GetNodesById(ids);
        }

        public async Task<List<NodeModel>> GetDestinations()
        {
            if(_useCache)
            {
                return await GetFromCache(() => _cacheService.GetDestinations());
            }

            return await _routeRepo.GetNodes((int)NodeType.Destination);
        }

        public async Task<List<RouteModel>> GetRoutes()
        {
            if (_useCache)
            {
                return await _cacheService.GetOrCreateRoutes(async () => await _routeRepo.GetRoutes());
            }
            else
            {
                return await _routeRepo.GetRoutes();
            }
        }

        public async Task<NodeModel> GetSourceById(int id)
        {
            if (_useCache)
            {
                return (await GetFromCache(() => _cacheService.GetSources())).FirstOrDefault(x => x.Id == id);
            }

            return await _routeRepo.GetNodeById(id);
        }

        public async Task<List<NodeModel>> GetSources()
        {
            if (_useCache)
            {
                return await GetFromCache(() => _cacheService.GetSources());
            }

            return await _routeRepo.GetNodes((int)NodeType.Source);
        }

        private async Task<List<NodeModel>> GetFromCache(Func<List<NodeModel>> executionMethod)
        {
            var points = executionMethod();

            if (points == null || points.Count == 0)
            {
                var routes = await _routeRepo.GetRoutes();
                _cacheService.Set(routes);
                points = executionMethod();
            }

            return points;
        }
    }
}
