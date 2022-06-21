namespace Model.Domain
{
    public class PathModel
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public int Price { get; set; }

        public MoveType Type { get; set; }
    }
}
