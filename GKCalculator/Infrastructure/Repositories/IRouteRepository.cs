using Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IRouteRepository
    {
        Task RegisterNodes(List<NodeModel> nodes);

        Task RegisterRoutes(List<RouteModel> routes);

        Task<List<RouteModel>> GetRoutes();

        Task<List<NodeModel>> GetNodes(int edgeTypeId);

        Task<NodeModel> GetNodeById(int id);

        Task<List<NodeModel>> GetNodesById(List<int> ids);
    }
}
