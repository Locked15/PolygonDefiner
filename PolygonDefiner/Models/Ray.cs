using System.Numerics;

namespace PolygonDefiner.Models
{
    public class Ray
    {
        public Point StartPoint { get; init; }

        public Point WayPoint { get; init; }

        public List<Point> FurtherPoints
        {
            get => CalculateFurtherPoints();
        }

        public Ray(Point startPoint, Point wayPoint)
        {
            StartPoint = startPoint;
            WayPoint = wayPoint;
        }

        private List<Point> CalculateFurtherPoints(int distance = 16, double magnitudeMultiplier = 4.096)
        {
            var direction = GetDirectionVector();
            var magnitude = CalculateMagnitudeByDirection(direction);
            (var dx, var dy) = (direction.X / magnitude, direction.Y / magnitude);

            var rayPoints = new List<Point>();
            (double calculatedXValue, double calculatedYValue) = (StartPoint.X,
                                                                  StartPoint.Y);
            for (int i = 0; i < distance; i++)
            {
                calculatedXValue += dx * magnitudeMultiplier;
                calculatedYValue += dy * magnitudeMultiplier;

                rayPoints.Add(new(calculatedXValue, calculatedYValue));
            }

            return rayPoints;
        }

        private Vector2 GetDirectionVector() => new(Convert.ToSingle(WayPoint.X - StartPoint.X),
                                                    Convert.ToSingle(WayPoint.Y - StartPoint.Y));

        /// <summary>
        /// We use multiplying to itself to pow it to the power 2,
        /// because there is no support for 'Float' type in 'Math.Pow' as such as '^' operand.
        /// </summary>
        private static double CalculateMagnitudeByDirection(Vector2 direction) => Math.Sqrt((direction.X * direction.X) +
                                                                                            (direction.Y * direction.Y)); 
    }
}
