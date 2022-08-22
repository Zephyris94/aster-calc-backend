using System;
using System.Collections.Generic;

namespace Model.Domain
{
    public class CalculationModel
    {
        public int Id { get; set; }

        public DateTime CalcDate { get; set; }

        public bool UseWyvern { get; set; }

        public bool UseShip { get; set; }

        public bool UseSoe { get; set; }

        public List<RouteModel> Routes { get; set; }
    }
}
