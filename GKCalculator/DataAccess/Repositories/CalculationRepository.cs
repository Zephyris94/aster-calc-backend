using AutoMapper;
using Infrastructure.Repositories;
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

        
    }
}
