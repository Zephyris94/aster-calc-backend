using Model.Graph;

namespace Infrastructure
{
    public interface IPathFindingAlgorithm
    {
        void InitDijkstraPathFindingAlgorithm(Graph graph);

        string FindShortestPath(string startName, string finishName);
    }
}
