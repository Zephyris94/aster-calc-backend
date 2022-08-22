using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Domain;

namespace Infrastructure.Services
{
    public interface INodeCacheService
    {
        Task<List<RouteModel>> GetOrCreateRoutes(Func<Task<List<RouteModel>>> creationDelegate);

        void Set(List<RouteModel> routes);

        List<RouteModel> GetRoutes();

        List<NodeModel> GetSources();

        List<NodeModel> GetDestinations();
    }
}
