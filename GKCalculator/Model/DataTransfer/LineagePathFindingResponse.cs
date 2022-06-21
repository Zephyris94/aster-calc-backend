using System.Collections.Generic;
using System.Linq;

namespace Model.DataTransfer
{
    public class LineagePathFindingResponse
    {
        public List<PathModelResponse> DefaultPath { get; set; }

        public List<PathModelResponse> CustomPath { get; set; }

        public int DefaultPrice => DefaultPath.Sum(x => x.Price);

        public int CustomPrice => CustomPath.Sum(x => x.Price);
    }
}
