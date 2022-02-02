using System.Collections.Generic;
using System.Linq;
using Model;

namespace GKCalculator.Models
{
    public class CalculationResultModel
    {
        public List<PathModel> DefaultPath { get; set; }

        public List<PathModel> CustomPath { get; set; }

        public int DefaultPrice => DefaultPath.Sum(x => x.Price);

        public int CustomPrice => CustomPath.Sum(x => x.Price);
    }
}
