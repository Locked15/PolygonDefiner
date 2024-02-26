using PolygonDefiner.Models;
using PtOri = PolygonDefiner.Models.PointsOrientation;
using Triplet = PolygonDefiner.Models.PointsTriplet;

namespace PolygonDefiner
{
    internal class Worker
    {
        public Ray TracingRay { get; init; }

        public Polygon TargetPolygon { get; init; }

        public Worker(Point targetPoint, Polygon targetPolygon)
        {
            TargetPolygon = targetPolygon;
            TracingRay = new(targetPoint, DefineInversePoint(targetPoint));
        }

        private Point DefineInversePoint(Point targetPoint)
        {
            var maxPolygonXAxisValue = TargetPolygon.Vertexes.Max(lines => lines.Max(point => point.X));
            var maxPolygonYAxisValue = TargetPolygon.Vertexes.Max(lines => lines.Max(point => point.Y));
            var isInversePointStraightForwarded = maxPolygonXAxisValue > targetPoint.X;

            var inversePointXValue = (targetPoint.X - maxPolygonXAxisValue) * -1;
            var inversePointYValue = (targetPoint.Y - maxPolygonYAxisValue) * -1;

            // If point is 'Inverse-forwarded' we must switch axis values.
            if (isInversePointStraightForwarded)
                (inversePointXValue, inversePointYValue) = (inversePointYValue, inversePointXValue);
            return new(inversePointXValue, inversePointYValue);
        }

        public bool CheckThePoint()
        {
            int intersectCount = 0;
            foreach (var line in TargetPolygon.Lines)
            {
                if (CheckIsLinesIntersect(line))
                    intersectCount++;
            }

            return intersectCount % 2 != 0;
        }

        private bool CheckIsLinesIntersect(Line line)
        {
            var triplet1 = new Triplet(TracingRay.StartPoint, TracingRay.FurtherPoints.Last(), line.StartPoint);
            var triplet2 = new Triplet(TracingRay.StartPoint, TracingRay.FurtherPoints.Last(), line.EndPoint);
            var triplet3 = new Triplet(line.StartPoint, line.EndPoint, TracingRay.StartPoint);
            var triplet4 = new Triplet(line.StartPoint, line.EndPoint, TracingRay.FurtherPoints.Last());

            (PtOri, PtOri, PtOri, PtOri) tripletOrientation = (GetTripletOrientation(triplet1),
                                                               GetTripletOrientation(triplet2),
                                                               GetTripletOrientation(triplet3),
                                                               GetTripletOrientation(triplet4));

            // In general, this condition covers all points.
            if (tripletOrientation.Item1 != tripletOrientation.Item2 && tripletOrientation.Item3 != tripletOrientation.Item4)
                return true;

            // But there is also some special cases, when got lines are collinear.
            if (tripletOrientation.Item1 == PtOri.Collinear && CheckIsTripletOnSegment(triplet1))
                return true;
            if (tripletOrientation.Item2 == PtOri.Collinear && CheckIsTripletOnSegment(triplet2))
                return true;
            if (tripletOrientation.Item3 == PtOri.Collinear && CheckIsTripletOnSegment(triplet3))
                return true;
            if (tripletOrientation.Item4 == PtOri.Collinear && CheckIsTripletOnSegment(triplet4))
                return true;

            // In all other cases just return false.
            return false;
        }

        private static PtOri GetTripletOrientation(Triplet triplet)
        {
            double counterValue = (triplet.SecondPoint.Y - triplet.FirstPoint.Y) * (triplet.ThirdPoint.X - triplet.SecondPoint.X);
            double forwardValue = (triplet.SecondPoint.X - triplet.FirstPoint.X) * (triplet.ThirdPoint.Y - triplet.SecondPoint.Y);
            double finalValue = counterValue - forwardValue;

            return finalValue == 0 ? PtOri.Collinear :
                                     (finalValue > 0 ? PtOri.Clockwise :
                                                       PtOri.CounterClockwise);
        }

        private static bool CheckIsTripletOnSegment(Triplet triplet)
        {
            var leftSideCondition = triplet.ThirdPoint.X <= Math.Max(triplet.FirstPoint.X, triplet.SecondPoint.X) &&
                                    triplet.ThirdPoint.X >= Math.Min(triplet.FirstPoint.X, triplet.SecondPoint.X);
            var rightSideCondition = triplet.ThirdPoint.Y <= Math.Max(triplet.FirstPoint.Y, triplet.SecondPoint.Y) &&
                                     triplet.ThirdPoint.Y >= Math.Min(triplet.FirstPoint.Y, triplet.SecondPoint.Y);

            return leftSideCondition && rightSideCondition;
        }
    }
}
