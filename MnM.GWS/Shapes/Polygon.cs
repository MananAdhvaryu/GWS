
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct Polygon : IPolygon
        {
            #region CONSTRUCTORS
            public Polygon(IList<PointF> polyPoints, Angle angle = default(Angle)) : this()
            {
                Bounds = polyPoints.ToArea();
                angle = angle.AssignCenter(Bounds);
                Points = polyPoints.Rotate(angle);
                ID = Factory.NewID(Name);
            }
            public Polygon(Angle angle, params float[] points) :
                this(points.ToPoints(), angle)
            { }
            #endregion

            #region PROPRTIES
            public IList<PointF> Points { get; private set; }
            public IRectangleF Bounds { get; private set; }
            public string Name =>"Polygon";
            public string ID { get; private set; }
            public Angle Angle { get; private set; }
            #endregion

            #region DRAW
            public bool Contains(float x, float y)
            {
                int counter = 0;
                int i;
                double xinters;
                PointF p1, p2;
                var N = Points.Count - 1;

                p1 = Points[0];
                for (i = 1; i <= N; i++)
                {
                    p2 = Points[i % N];
                    if (y > Math.Min(p1.Y, p2.Y))
                    {
                        if (y <= Math.Max(p1.Y, p2.Y))
                        {
                            if (x <= Math.Max(p1.X, p2.X))
                            {
                                if (p1.Y != p2.Y)
                                {
                                    xinters = (y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                                    if (p1.X == p2.X || x <= xinters)
                                        counter++;
                                }
                            }
                        }
                    }
                    p1 = p2;
                }
                return !(counter % 2 == 0);
            }
            #endregion

            #region IReadOnlyList<IPosF>
            public PointF this[int index] =>Points[index];
            public int Count => Points.Count;
            IEnumerator<PointF> IEnumerable<PointF>.GetEnumerator() =>
                Points.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() =>
                Points.GetEnumerator();
            #endregion

            void IStoreable.AssignIDIfNone()
            {
                if (ID == null)
                    ID = Factory.NewID(Name);
            }
        }
#if AllHidden
    }
#endif
}

