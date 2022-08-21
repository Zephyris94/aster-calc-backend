﻿using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using Infrastructure.Utility;
using Model.Domain;

namespace Core.Services
{
    public class InMemoryNodeCacheService : INodeCacheService
    {
        private readonly IExcelParsing _excelParsing;

        private readonly object _cacheLocker = new object();

        private List<PathModel> _edges;
        private List<NodeModel> _sources;
        private List<NodeModel> _destinations;

        public InMemoryNodeCacheService(IExcelParsing excelParsing)
        {
            _excelParsing = excelParsing;
        }

        public List<PathModel> Edges
        {
            get
            {
                if (_edges == null)
                {
                    lock (_cacheLocker)
                    {
                        if (_edges == null)
                        {
                            _edges = _excelParsing.ParseExcel();
                        }
                    }
                }

                return _edges;
            }
        }

        public List<NodeModel> GetSources()
        {
            if (_sources == null || _sources.Count == 0)
            {
                InitSources();
            }

            return _sources;
        }

        public List<NodeModel> GetDestinations()
        {
            if (_sources == null || _sources.Count == 0)
            {
                InitDestinations();
            }

            return _destinations;
        }
        private void InitSources()
        {
            _sources = Edges.Select(x => x.Source).Distinct().ToList();
        }

        private void InitDestinations()
        {
            _destinations = Edges.Select(x => x.Source).Union(Edges.Select(x => x.Destination)).Distinct().ToList();
        }
    }
}
