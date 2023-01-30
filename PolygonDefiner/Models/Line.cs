using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDefiner.Models
{
    internal class Line
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
