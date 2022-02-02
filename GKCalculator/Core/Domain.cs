using System.Collections.Generic;
using GKCalculator.Models;
using Model;

namespace Core
{
    public class Domain
    {
        public List<PathModel> CalculatePath(PathRequestModel model, bool useWyvern, bool useShips, bool useSoe)
        {
            var edges = GraphConverter.GetRequiredEdges(useWyvern, useShips, useSoe);
            var graph = GraphConverter.GetGraph(edges);
            var dijkstra = new Dijkstra(graph);
            var path = dijkstra.FindShortestPath(model.SourcePoint, model.Destinations[0]);
            for (int i = 0; i < model.Destinations.Count - 1; i++)
            {
                var pathChunk = dijkstra.FindShortestPath(model.Destinations[i], model.Destinations[i + 1]);
                path += ";" + pathChunk;
            }//defaultDijkstra.FindShortestPath(nodeA, nodeB);
            var results = GraphConverter.GetResultsFromPath(edges, path);

            return results;
        }
    }
}
