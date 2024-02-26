namespace PolygonDefiner.Models
{
    public class Polygon
    {
        public List<Line> Lines { get; init; }

        public List<List<Point>> Vertexes
        {
            get
            {
                var result = new List<List<Point>>();
                foreach (var line in Lines)
                {
                    result.Add(new());

                    var points = result.Last();
                    points.Add(line.StartPoint);
                    points.Add(line.EndPoint);
                }

                return result;
            }
        }

        public Polygon()
        {
            Lines = new();
        }

        public Polygon(List<Line> lines)
        {
            Lines = lines;
        }

        public static Polygon GetFromPoints(List<Point> polygonPoints)
        {
            var result = new Polygon();
            for (int i = 0; i < polygonPoints.Count; i++)
            {
                var index = new Index(Math.Abs(i - 1), i - 1 < 0);
                var line = new Line(polygonPoints[index], polygonPoints[i]);

                result.Lines.Add(line);
            }

            return result;
        }
    }
}
