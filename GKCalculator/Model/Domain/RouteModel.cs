namespace Model.Domain
{
    public class RouteModel
    {
        public int Id { get; set; }

        public NodeModel Source { get; set; }

        public NodeModel Destination { get; set; }

        public int Price { get; set; }

        public MoveType MoveType { get; set; }
    }
}
