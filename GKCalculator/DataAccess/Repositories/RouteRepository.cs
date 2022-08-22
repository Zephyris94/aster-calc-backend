using AutoMapper;
using DataAccess.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public RouteRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NodeModel>> GetNodes(int edgeTypeId)
        {
            var result = await _context.Nodes.Where(x => x.NodeType.Id == edgeTypeId)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<NodeModel>>(result);
        }

        public async Task<List<RouteModel>> GetRoutes()
        {
            var result = await _context.Routes.ToListAsync();

            return _mapper.Map<List<RouteModel>>(result);
        }

        public async Task RegisterRoutes(List<RouteModel> routes)
        {
            _context.Routes.RemoveRange(_context.Routes);

            var routeModels = _mapper.Map<List<Route>>(routes);

            await _context.Routes.AddRangeAsync(routeModels);

            await _context.SaveChangesAsync();
        }

        public async Task RegisterNodes(List<NodeModel> nodes)
        {
            _context.Nodes.RemoveRange(_context.Nodes);

            var nodeModels = _mapper.Map<List<Node>>(nodes);

            await _context.Nodes.AddRangeAsync(nodeModels);

            await _context.SaveChangesAsync();
        }
    }
}
