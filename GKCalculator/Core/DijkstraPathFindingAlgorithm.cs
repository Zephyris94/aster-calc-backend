using System.Collections.Generic;
using Infrastructure;
using Model.Graph;

namespace Core
{
    public class DijkstraPathFindingAlgorithm: IPathFindingAlgorithm
    {
        private Graph _graph;

        private List<GraphVertexInfo> _infos;

        public void InitAlgorithm(Graph graph)
        {
            this._graph = graph;
        }

        /// <summary>
        /// Поиск кратчайшего пути по названиям вершин
        /// </summary>
        /// <param name="startName">Название стартовой вершины</param>
        /// <param name="finishName">Название финишной вершины</param>
        /// <returns>Кратчайший путь</returns>
        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(_graph.FindVertex(startName), _graph.FindVertex(finishName));
        }

        /// <summary>
        /// Получение информации о вершине графа
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <returns>Информация о вершине</returns>
        private GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (var i in _infos)
            {
                if (i.Vertex.Equals(v))
                {
                    return i;
                }
            }

            return null;
        }

        /// <summary>
        /// Инициализация информации
        /// </summary>
        private void InitInfo()
        {
            _infos = new List<GraphVertexInfo>();
            foreach (var v in _graph.Vertices)
            {
                _infos.Add(new GraphVertexInfo(v));
            }
        }

        /// <summary>
        /// Поиск непосещенной вершины с минимальным значением суммы
        /// </summary>
        /// <returns>Информация о вершине</returns>
        private GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            GraphVertexInfo minVertexInfo = null;
            foreach (var i in _infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        /// <summary>
        /// Поиск кратчайшего пути по вершинам
        /// </summary>
        /// <param name="startVertex">Стартовая вершина</param>
        /// <param name="finishVertex">Финишная вершина</param>
        /// <returns>Кратчайший путь</returns>
        private string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetSumToNextVertex(current);
            }

            return GetPath(startVertex, finishVertex);
        }

        /// <summary>
        /// Вычисление суммы весов ребер для следующей вершины
        /// </summary>
        /// <param name="info">Информация о текущей вершине</param>
        private void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        /// <summary>
        /// Формирование пути
        /// </summary>
        /// <param name="startVertex">Начальная вершина</param>
        /// <param name="endVertex">Конечная вершина</param>
        /// <returns>Путь</returns>
        private string GetPath(GraphVertex startVertex, GraphVertex endVertex)
        {
            var path = endVertex.ToString();
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                path = $"{endVertex};{path}";
            }

            return path;
        }
    }
}
