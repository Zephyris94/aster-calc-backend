using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CalculationRepository : ICalculationRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public CalculationRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Order> StoreOrder(Order order)
        {
            var sqlOrder = _mapper.Map<OrderDAO>(order);

            var result = await _context.Orders.AddAsync(sqlOrder);

            return _mapper.Map<Order>(result);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            await Task.CompletedTask;
            return order;
        }
    }
}
