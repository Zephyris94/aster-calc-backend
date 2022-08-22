using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Domain;

namespace Infrastructure.Services
{
    public interface IPathFindingService
    {
        Task<List<RouteModel>> FindPath(LineagePathFindingModel request);
    }
}
