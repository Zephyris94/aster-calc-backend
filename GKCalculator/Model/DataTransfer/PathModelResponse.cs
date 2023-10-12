using Model.Domain;

namespace Model.DataTransfer
{
    public class PathModelResponse
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public int Price { get; set; }

        public MoveType MoveType { get; set; }
    }
}
