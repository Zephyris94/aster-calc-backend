using Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface INodeProviderService
    {
        Task<List<NodeModel>> GetSources();

        Task<List<NodeModel>> GetDestinations();
    }
}
