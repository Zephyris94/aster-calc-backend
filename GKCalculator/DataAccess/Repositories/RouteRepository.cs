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
                .Include(x => x.NodeType)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<NodeModel>>(result);
        }

        public async Task<List<RouteModel>> GetRoutes()
        {
            var result = await _context.Routes
                .Include(x => x.MoveType)
                .Include(x => x.Source)
                    .ThenInclude(x => x.NodeType)
                .Include(x => x.Destination)
                    .ThenInclude(x => x.NodeType)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<RouteModel>>(result);
        }

        public async Task RegisterRoutes(List<RouteModel> routes)
        {
            _context.Routes.RemoveRange(_context.Routes);

            var routeModels = _mapper.Map<List<Route>>(routes);

            foreach(var route in routeModels)
            {
                route.MoveType = await _context.MoveTypes.FirstOrDefaultAsync(x => x.Id == route.MoveType.Id);
                route.Source = await _context.Nodes.FirstOrDefaultAsync(x => x.Id.Equals(route.Source.Id));
                route.Destination = await _context.Nodes.FirstOrDefaultAsync(x => x.Id.Equals(route.Destination.Id));
            }

            await _context.Routes.AddRangeAsync(routeModels);

            await _context.SaveChangesAsync();
        }

        public async Task RegisterNodes(List<NodeModel> nodes)
        {
            _context.Nodes.RemoveRange(_context.Nodes);

            var nodeModels = _mapper.Map<List<Node>>(nodes);

            foreach(var node in nodeModels)
            {
                node.NodeType = await _context.NodeTypes.FirstOrDefaultAsync(x => x.Id == node.NodeType.Id);
            }

            await _context.Nodes.AddRangeAsync(nodeModels);

            await _context.SaveChangesAsync();
        }

        public async Task<NodeModel> GetNodeById(int id)
        {
            var result = await _context.Nodes.FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<NodeModel>(result);
        }

        public async Task<List<NodeModel>> GetNodesById(List<int> ids)
        {
            var result = await _context.Nodes.Where(x => ids.Contains(x.Id)).ToListAsync();

            return _mapper.Map<List<NodeModel>>(result);
        }
    }
}
