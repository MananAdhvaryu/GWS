using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public struct TrapeziumData
    {
        public static TrapeziumData Empty = new TrapeziumData();

        public readonly IList<PointF> Points;
        public readonly Angle Angle;
        public readonly IRectangleF Bounds;
        public readonly bool Initialized;
        public TrapeziumData(IList<PointF> points, Angle angle, IRectangleF bounds)
        {
            Points = points;
            Angle = angle;
            Bounds = bounds;
            Initialized = true;
        }
    }
}
