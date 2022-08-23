using Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface INodeProviderService
    {
        Task<NodeModel> GetSourceById(int id);

        Task<List<NodeModel>> GetDestinationListByIds(List<int> ids);

        Task<List<NodeModel>> GetSources();

        Task<List<NodeModel>> GetDestinations();
    }
}
