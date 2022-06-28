using Model.Graph;

namespace Infrastructure
{
    public interface IPathFindingAlgorithm
    {
        void InitAlgorithm(Graph graph);

        string FindShortestPath(string startName, string finishName);
    }
}
