using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utility;
using Infrastructure.Services;
using Model.Domain;

namespace Core.Services
{
    public class InMemoryNodeCacheService : INodeCacheService
    {
        private readonly object _cacheLocker = new object();
        private readonly object _sourceLocker = new object();
        private readonly object _destLocker = new object();

        private List<RouteModel> _routes;
        private List<NodeModel> _sources;
        private List<NodeModel> _destinations;

        public void Set(List<RouteModel> routes)
        {
            if (_routes == null)
            {
                lock (_cacheLocker)
                {
                    if (_routes == null)
                    {
                        _routes = routes;
                    }
                }
            }
        }

        public async Task<List<RouteModel>> GetOrCreateRoutes(Func<Task<List<RouteModel>>> creationDelegate)
        {
            if (_routes == null)
            {
                var routes = await creationDelegate();
                lock (_cacheLocker)
                {
                    if (_routes == null)
                    {
                        _routes = routes;
                    }
                }
            }

            lock (_cacheLocker)
            {
                return _routes;
            }
        }

        public List<RouteModel> GetRoutes()
        {
            lock (_cacheLocker)
            {
                return _routes;
            }
        }

        public List<NodeModel> GetSources()
        {
            if (_sources == null || _sources.Count == 0)
            {
                var source = GetRoutes()?.Select(x => x.Source).DistinctBy(x => x.Id).ToList();
                lock (_sourceLocker)
                {
                    if (_sources == null)
                    {
                        _sources = source;
                    }
                }
            }

            return _sources;
        }

        public List<NodeModel> GetDestinations()
        {
            if (_destinations == null || _destinations.Count == 0)
            {
                var destinations = GetRoutes()?.Select(x => x.Destination).DistinctBy(x => x.Id).ToList();
                lock (_destLocker)
                {
                    if (_destinations == null)
                    {
                        _destinations = destinations;
                    }
                }
            }

            return _destinations;
        }
    }
}
