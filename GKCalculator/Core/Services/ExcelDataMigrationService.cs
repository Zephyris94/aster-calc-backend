using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Utility;
using System;
using System.Threading.Tasks;
using System.Linq;
using Model.Domain;

namespace Core.Services
{
    public class ExcelDataMigrationService : IDataMigrationService
    {
        private readonly IExcelParsing _excelParsing;
        private readonly IRouteRepository _repo;

        public ExcelDataMigrationService(
            IExcelParsing excelParsing,
            IRouteRepository repo)
        {
            _excelParsing = excelParsing;

            _repo = repo;
        }

        public async Task SeedData()
        {
            if((await _repo.GetRoutes()).Any())
            {
                return;
            }

            var routes = _excelParsing.ParseExcel();
            var nodes = routes
                .Select(x => x.Source)
                .DistinctBy(x => x.Name)
                .ToList();
            nodes.AddRange(routes.Select(y => y.Destination).DistinctBy(x => x.Name));

            await _repo.RegisterNodes(nodes);

            var sources = await _repo.GetNodes((int)NodeType.Source);
            var destinations = await _repo.GetNodes((int)NodeType.Destination);

            foreach(var route in routes)
            {
                route.Source = sources.FirstOrDefault(x => string.Compare(route.Source.Name, x.Name, StringComparison.OrdinalIgnoreCase) == 0);
                route.Destination = destinations.FirstOrDefault(x => string.Compare(route.Destination.Name, x.Name, StringComparison.OrdinalIgnoreCase) == 0);
            }

            await _repo.RegisterRoutes(routes);
        }
    }
}
