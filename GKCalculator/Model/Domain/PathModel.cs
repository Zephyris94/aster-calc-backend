namespace Model.Domain
{
    public class PathModel
    {
        public NodeModel Source { get; set; }

        public NodeModel Destination { get; set; }

        public int Price { get; set; }

        public MoveType Type { get; set; }
    }
}
