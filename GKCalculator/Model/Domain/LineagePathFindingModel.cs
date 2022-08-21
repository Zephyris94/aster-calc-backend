using System.Collections.Generic;

namespace Model.Domain
{
    public class LineagePathFindingModel
    {
        public NodeModel SourcePoint { get; set; }

        public List<NodeModel> Destinations { get; set; }

        public bool UseWyvern { get; set; }

        public bool UseSoe { get; set; }

        public bool UseShips { get; set; }
    }
}
