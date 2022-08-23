using Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IRouteProviderService
    {
        Task<NodeModel> GetSourceById(int id);

        Task<List<NodeModel>> GetDestinationListByIds(List<int> ids);

        Task<List<NodeModel>> GetSources();

        Task<List<NodeModel>> GetDestinations();

        Task<List<RouteModel>> GetRoutes();
    }
}
