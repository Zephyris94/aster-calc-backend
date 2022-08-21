using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Calculation
    {
        public int Id { get; set; }

        public DateTime CalcDate { get; set; }

        public bool UseWyvern { get; set; }

        public bool UseShip { get; set; }

        public bool UseSoe { get; set; }

        public virtual ICollection<Route> Routes { get; set; }
    }
}
