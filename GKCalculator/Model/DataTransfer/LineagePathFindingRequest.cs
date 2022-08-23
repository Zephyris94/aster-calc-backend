using System.Collections.Generic;

namespace Model.DataTransfer
{
    public class LineagePathFindingRequest
    {
        public int SourcePointId { get; set; }

        public List<int> DestinationIds { get; set; }

        public bool UseWyvern { get; set; }

        public bool UseSoe { get; set; }

        public bool UseShips { get; set; }
    }
}