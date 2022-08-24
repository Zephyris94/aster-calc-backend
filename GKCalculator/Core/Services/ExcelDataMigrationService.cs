﻿using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Utility;
using System;
using System.Threading.Tasks;
using System.Linq;
using Model.Domain;
using System.IO;
using System.Collections.Generic;

namespace Core.Services
{
    public class ExcelDataMigrationService : IDataMigrationService
    {
        private readonly IExcelParsing _excelParsing;
        private readonly IRouteRepository _repo;
        private readonly IBlobStorageService _blobStorageService;

        public ExcelDataMigrationService(
            IExcelParsing excelParsing,
            IBlobStorageService blobStorageService,
            IRouteRepository repo)
        {
            _excelParsing = excelParsing;
            _blobStorageService = blobStorageService;

            _repo = repo;
        }

        public async Task SeedData()
        {
            if((await _repo.GetRoutes()).Any())
            {
                return;
            }

            List<RouteModel> routes = await ImportRoutes();

            await RegisterNodes(routes);

            await BindNodes(routes);

            await _repo.RegisterRoutes(routes);
        }

        private async Task<List<RouteModel>> ImportRoutes()
        {
            using (var ms = new MemoryStream())
            {
                await _blobStorageService.DownloadToStream(ms);
                return _excelParsing.ParseExcelFromStream(ms);
            }
        }

        private async Task RegisterNodes(List<RouteModel> routes)
        {
            var nodes = routes
               .Select(x => x.Source)
               .DistinctBy(x => x.Name)
               .ToList();
            nodes.AddRange(routes.Select(y => y.Destination).DistinctBy(x => x.Name));

            await _repo.RegisterNodes(nodes);
        }

        private async Task BindNodes(List<RouteModel> routes)
        {
            var sources = await _repo.GetNodes((int)NodeType.Source);
            var destinations = await _repo.GetNodes((int)NodeType.Destination);

            foreach (var route in routes)
            {
                route.Source = sources.FirstOrDefault(x => string.Compare(route.Source.Name, x.Name, StringComparison.OrdinalIgnoreCase) == 0);
                route.Destination = destinations.FirstOrDefault(x => string.Compare(route.Destination.Name, x.Name, StringComparison.OrdinalIgnoreCase) == 0);
            }
        }
    }
}
