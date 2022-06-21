using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using Model;
using Model.Domain;
using Model.Graph;

namespace Core.Services
{
    public class GraphBuildingService : IGraphBuildingService
    {
        public List<PathModel> GetResultsFromPath(List<PathModel> edges, string path)
        {
            var nodes = path.Split(";");

            var results = new List<PathModel>();

            for (int i = 0; i < nodes.Length - 1; i++)
            {
                if (nodes[i] == nodes[i + 1])
                {
                    continue;
                }
                var edge = edges.FirstOrDefault(x => x.Source == nodes[i] && x.Destination == nodes[i + 1]);
                results.Add(edge);
            }

            return results;
        }

        public Graph GetGraphFromEdges(List<PathModel> edges)
        {
            var g = new Graph();

            var vertexes = edges.Select(x => x.Source).Union(edges.Select(x => x.Destination)).Distinct();

            foreach (var vertex in vertexes)
            {
                g.AddVertex(vertex);
            }

            foreach (var edge in edges)
            {
                g.AddEdge(edge);
            }

            return g;
        }

        public List<PathModel> GetRequiredEdges(List<PathModel> edges, bool useWyverns, bool useShips, bool useSoe)
        {
            var workingCopy = new PathModel[edges.Count];
            edges.CopyTo(workingCopy);

            var workingList = workingCopy.ToList();

            var baseQuery = workingList.AsEnumerable();

            baseQuery = useWyverns 
                ? RemoveIntersections(baseQuery, MoveType.Wyvern, new List<MoveType>{MoveType.GK}) 
                : baseQuery.Where(x => x.Type != MoveType.Wyvern);

            baseQuery = useShips 
                ? RemoveIntersections(baseQuery, MoveType.Ship, new List<MoveType> { MoveType.GK, MoveType.Wyvern }) 
                : baseQuery.Where(x => x.Type != MoveType.Ship);

            baseQuery = useSoe 
                ? RemoveIntersections(baseQuery, MoveType.SoE, new List<MoveType> { MoveType.GK, MoveType.Wyvern }) 
                : baseQuery.Where(x => x.Type != MoveType.SoE);

            return baseQuery.ToList();
        }

        private IEnumerable<PathModel> RemoveIntersections(IEnumerable<PathModel> workingList, MoveType preferredMoveType,
            List<MoveType> excludedMoveTypes)
        {
            var list = workingList.ToList();
            var wyvernPaths = list.Where(x => x.Type == preferredMoveType).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (wyvernPaths.Any(x =>
                    x.Source == list[i].Source && x.Destination == list[i].Destination) &&
                    excludedMoveTypes.Contains(list[i].Type))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }

            return list.AsEnumerable();
        }
    }
}
