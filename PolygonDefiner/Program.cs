using PolygonDefiner.Models;

namespace PolygonDefiner
{
    /// <summary>
    /// Inspiration source available <a href="https://algolist.manual.ru/maths/geom/belong/poly2d.php">here</a>.
    /// </summary>
    public static class Program
    {
        private const char InputPointsDivider = '>';

        public static Point TargetPoint { get; private set; } = null!;

        public static List<Point> PolygonPoints { get; private set; } = null!;

        private static void Main(string[] args)
        {
            Console.WriteLine($"Enter polygon points (use following format: \"(X1; Y1){InputPointsDivider}(X2; Y2){InputPointsDivider}...{InputPointsDivider}(Xn; Yn)\"): ");
            PolygonPoints = ParsePolygonString(Console.ReadLine());

            Console.WriteLine("Input target point (use the same format): ");
            TargetPoint = ParseAxisPointsString(Console.ReadLine());

            var worker = new Worker(TargetPoint, Polygon.GetFromPoints(PolygonPoints));
            var result = worker.CheckThePoint();

            Console.WriteLine(result.ToString());
            Console.ReadKey();
        }

        private static List<Point> ParsePolygonString(string? rawString)
        {
            if (rawString == null)
            {
                Console.WriteLine("Sent value was empty.");
                return new();
            }

            var points = rawString.Split(InputPointsDivider);
            return points.Select(ParseAxisPointsString).ToList();
        }

        private static Point ParseAxisPointsString(string? point)
        {
            if (point == null)
            {
                Console.WriteLine("Sent value was empty.");
                return new(default, default);
            }

            var axisValuesRaw = point.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Split(';');
            (var xValue, var yValue) = (ParseDouble(axisValuesRaw.ElementAtOrDefault(0)), ParseDouble(axisValuesRaw.ElementAtOrDefault(1)));

            return new(Convert.ToDouble(xValue), Convert.ToDouble(yValue));
        }

        private static double ParseDouble(string? v)
        {
            var result = 0.0;
            try
            {
                result = Convert.ToDouble(v);
            }
            catch (FormatException)
            {
                // This may happen if user use '.' instead of ',' as decimal divider while on RU-Culture.
                result = Convert.ToDouble(v?.Replace('.', ','));
            }
            catch (Exception)
            {
                // If something really went wrong, default (0.0) value will be used.
                Console.WriteLine("Missing value was found, default one would be used instead.");
            }

            return result;
        }
    }
}
