using GKCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core;
using Model;

namespace GKCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly List<string> _sources;
        private readonly List<string> _destinations;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _destinations = GraphConverter.GetVertexes();
            _sources = GraphConverter.AllEdges.Select(x => x.Source).Distinct().ToList();
        }

        [HttpPost]
        public JsonResult SourceAutoComplete(string prefix)
        {
            var result = string.IsNullOrWhiteSpace(prefix) ? _sources:  _sources.Where(x => x.ToLower().StartsWith(prefix.ToLower()));
            return Json(result);
        }

        [HttpPost]
        public JsonResult DestinationAutoComplete(string prefix)
        {
            var result = string.IsNullOrWhiteSpace(prefix) ? _destinations : _destinations.Where(x => x.ToLower().StartsWith(prefix.ToLower()));
            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetPath(string nodeA, string nodeB, bool useWyv, bool useShips, bool useSoe)
        {
            var defaultEdges = GraphConverter.GetRequiredEdges(true, false, true);
            var defaultGraph = GraphConverter.GetGraph(defaultEdges);
            var defaultDijkstra = new Dijkstra(defaultGraph);
            var defaultPath = defaultDijkstra.FindShortestPath(nodeA, nodeB);
            var defaultResults = GraphConverter.GetResultsFromPath(defaultEdges, defaultPath);

            List<PathModel> results = new List<PathModel>();
            if (useWyv != true || useShips != false || useSoe != true)
            {
                var edges = GraphConverter.GetRequiredEdges(useWyv, useShips, useSoe);

                var graph = GraphConverter.GetGraph(edges);
                var dijkstra = new Dijkstra(graph);
                var path = dijkstra.FindShortestPath(nodeA, nodeB);
                results = GraphConverter.GetResultsFromPath(edges, path);
            }

            ViewBag.DefaultResult = defaultResults;
            ViewBag.DefaultPrice = defaultResults.Sum(x => x.Price);

            ViewBag.Result = results;
            ViewBag.Price = results.Sum(x => x.Price);

            return View("Result");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
