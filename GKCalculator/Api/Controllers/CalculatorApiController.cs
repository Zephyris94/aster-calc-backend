using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core;
using GKCalculator.Models;
using Microsoft.AspNetCore.Cors;

namespace Api.Controllers
{
    [ApiController]
    [Route("CalcApi")]
    public class CalculatorApiController : ControllerBase
    {
        private readonly ILogger<CalculatorApiController> _logger;
        private readonly List<string> _sources;
        private readonly List<string> _destinations;
        private readonly Domain _domain = new Domain();

        public CalculatorApiController(ILogger<CalculatorApiController> logger)
        { 
            _logger = logger;

            _destinations = GraphConverter.GetVertexes();
            _sources = GraphConverter.AllEdges.Select(x => x.Source).Distinct().ToList();
        }

        [HttpGet("Sources")]
        [EnableCors("AllowAllOrigins")]
        public List<string> Sources()
        {
            return _sources;
        }

        [HttpGet("Destinations")]
        [EnableCors("AllowAllOrigins")]
        public List<string> Destinations()
        {
            return _destinations;
        }

        [HttpGet("Counter")]
        [EnableCors("AllowAllOrigins")]
        public int Counter()
        {
            return Core.Counter.CounterValue;
        }

        [HttpPost("Calculate")]
        [EnableCors("AllowAllOrigins")]
        public CalculationResultModel Post(PathRequestModel model)
        {
            Core.Counter.Increment();

            var defaultResults = _domain.CalculatePath(model, true, true, true);

            var results = _domain.CalculatePath(model, model.UseWyvern, model.UseShips, model.UseSoe);

            return new CalculationResultModel
            {
                DefaultPath = defaultResults,
                CustomPath = results
            };
        }
    }
}
