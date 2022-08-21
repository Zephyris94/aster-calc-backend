using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Route
    {
        public int Id { get; set; }

        public Node Source { get; set; }

        public Node Destination { get; set; }

        public virtual ICollection<Calculation> Calculations { get; set; }
    }
}
