using System.Collections.Generic;
using Model.Domain;
using Model.Graph;

namespace Infrastructure.Services
{
    public interface IGraphBuildingService
    {
        Graph GetGraphFromEdges(List<RouteModel> edges);

        List<RouteModel> GetRequiredEdges(List<RouteModel> edges, bool useWyverns, bool useShips, bool useSoe);

        List<RouteModel> GetResultsFromPath(List<RouteModel> edges, string path);
    }
}
