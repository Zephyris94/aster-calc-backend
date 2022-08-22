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
        public List<RouteModel> GetResultsFromPath(List<RouteModel> edges, string path)
        {
            var nodes = path.Split(";");

            var results = new List<RouteModel>();

            for (int i = 0; i < nodes.Length - 1; i++)
            {
                if (nodes[i] == nodes[i + 1])
                {
                    continue;
                }
                var edge = edges.FirstOrDefault(x => x.Source.Name == nodes[i] && x.Destination.Name == nodes[i + 1]);
                results.Add(edge);
            }

            return results;
        }

        public Graph GetGraphFromEdges(List<RouteModel> edges)
        {
            var g = new Graph();

            var vertexes = edges.Select(x => x.Source).Union(edges.Select(x => x.Destination)).Distinct();

            foreach (var vertex in vertexes)
            {
                g.AddVertex(vertex.Name);
            }

            foreach (var edge in edges)
            {
                g.AddEdge(edge);
            }

            return g;
        }

        public List<RouteModel> GetRequiredEdges(List<RouteModel> edges, bool useWyverns, bool useShips, bool useSoe)
        {
            var workingCopy = new RouteModel[edges.Count];
            edges.CopyTo(workingCopy);

            var workingList = workingCopy.ToList();

            var baseQuery = workingList.AsEnumerable();

            baseQuery = useWyverns 
                ? RemoveIntersections(baseQuery, MoveType.Wyvern, new List<MoveType>{MoveType.GK}) 
                : baseQuery.Where(x => x.MoveType != MoveType.Wyvern);

            baseQuery = useShips 
                ? RemoveIntersections(baseQuery, MoveType.Ship, new List<MoveType> { MoveType.GK, MoveType.Wyvern }) 
                : baseQuery.Where(x => x.MoveType != MoveType.Ship);

            baseQuery = useSoe 
                ? RemoveIntersections(baseQuery, MoveType.SoE, new List<MoveType> { MoveType.GK, MoveType.Wyvern }) 
                : baseQuery.Where(x => x.MoveType != MoveType.SoE);

            return baseQuery.ToList();
        }

        private IEnumerable<RouteModel> RemoveIntersections(IEnumerable<RouteModel> workingList, MoveType preferredMoveType,
            List<MoveType> excludedMoveTypes)
        {
            var list = workingList.ToList();
            var wyvernPaths = list.Where(x => x.MoveType == preferredMoveType).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (wyvernPaths.Any(x =>
                    x.Source == list[i].Source && x.Destination == list[i].Destination) &&
                    excludedMoveTypes.Contains(list[i].MoveType))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }

            return list.AsEnumerable();
        }
    }
}
