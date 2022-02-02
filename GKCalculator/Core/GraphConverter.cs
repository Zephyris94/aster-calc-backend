using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Graph;

namespace Core
{
    public static class GraphConverter
    {
        public static List<PathModel> AllEdges;

        public static void InitTpTable(List<PathModel> tps)
        {
            AllEdges = tps;
        }

        public static List<string> GetVertexes()
        {
            var vertexes = AllEdges.Select(x => x.Source).Union(AllEdges.Select(x => x.Destination)).Distinct().ToList();
            return vertexes;
        }

        public static List<PathModel> GetResultsFromPath(List<PathModel> edges, string path)
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

        public static Graph GetGraph(List<PathModel> edges)
        {
            var g = new Graph();

            var vertexes = edges.Select(x => x.Source).Union(AllEdges.Select(x => x.Destination)).Distinct();
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

        public static List<PathModel> GetRequiredEdges(bool useWyverns, bool useShips, bool useSoe)
        {
            var workingCopy = new PathModel[AllEdges.Count];
            AllEdges.CopyTo(workingCopy);

            var workingList = workingCopy.ToList();

            var baseQuery = workingList.AsEnumerable();

            if (useWyverns)
            {
                baseQuery = RemoveIntersections(baseQuery, MoveType.Wyvern, new List<MoveType>{MoveType.GK});
            }
            else
            {
                baseQuery = baseQuery.Where(x => x.Type != MoveType.Wyvern);
            }

            if (useShips)
            {
                baseQuery = RemoveIntersections(baseQuery, MoveType.Ship, new List<MoveType> { MoveType.GK, MoveType.Wyvern });
            }
            else
            {
                baseQuery = baseQuery.Where(x => x.Type != MoveType.Ship);
            }

            if (useSoe)
            {
                baseQuery = RemoveIntersections(baseQuery, MoveType.SoE, new List<MoveType> { MoveType.GK, MoveType.Wyvern });
            }
            else
            {
                baseQuery = baseQuery.Where(x => x.Type != MoveType.SoE);
            }

            return baseQuery.ToList();
        }

        private static IEnumerable<PathModel> RemoveIntersections(IEnumerable<PathModel> workingList, MoveType preferredMoveType,
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
