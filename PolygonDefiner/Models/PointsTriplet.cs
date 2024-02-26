namespace PolygonDefiner.Models
{
    public class PointsTriplet
    {
        public Point FirstPoint { get; init; }

        public Point SecondPoint { get; init; }

        public Point ThirdPoint { get; init; }

        public PointsTriplet(Point firstPoint, Point secondPoint, Point thirdPoint)
        {
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
            ThirdPoint = thirdPoint;
        }
    }
}
