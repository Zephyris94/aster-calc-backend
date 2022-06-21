using System.Collections.Generic;
using Model.Domain;
using Model.Graph;

namespace Infrastructure.Services
{
    public interface IGraphBuildingService
    {
        Graph GetGraphFromEdges(List<PathModel> edges);

        List<PathModel> GetRequiredEdges(List<PathModel> edges, bool useWyverns, bool useShips, bool useSoe);

        List<PathModel> GetResultsFromPath(List<PathModel> edges, string path);
    }
}
