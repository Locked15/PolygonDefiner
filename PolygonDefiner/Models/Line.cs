namespace PolygonDefiner.Models
{
    public class Line
    {
        public Point StartPoint { get; private set; }

        public Point EndPoint { get; private set; }

        public Line(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }
    }
}
