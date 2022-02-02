using System.Collections.Generic;

namespace GKCalculator.Models
{
    public class PathRequestModel
    {
        public string SourcePoint { get; set; }

        public List<string> Destinations { get; set; }

        public bool UseWyvern { get; set; }

        public bool UseSoe { get; set; }

        public bool UseShips { get; set; }
    }
}