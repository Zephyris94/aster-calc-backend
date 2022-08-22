using AutoMapper;
using DataAccess.Models;
using Infrastructure.Repositories;
using Model.Domain;
using System.Threading.Tasks;

namespace DataAccess.Repositories
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

        public async Task SaveCalculation(CalculationModel calculation)
        {
            var calculationDao = _mapper.Map<Calculation>(calculation);

            await _context.Calculations.AddAsync(calculationDao);

            await _context.SaveChangesAsync();
        }
    }
}
