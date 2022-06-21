using System.Collections.Generic;
using Model.Domain;

namespace Infrastructure.Services
{
    public interface IPathFindingService
    {
        List<PathModel> FindPath(LineagePathFindingModel request);
    }
}
