using System.Collections.Generic;
using Model.Domain;

namespace Infrastructure.Services
{
    public interface INodeCacheService
    {
        List<PathModel> Edges { get; }

        List<NodeModel> GetSources();

        List<NodeModel> GetDestinations();
    }
}
