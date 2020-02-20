using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct Triangle : ITriangle
        {
            #region VARIABLES
            PointF c;
            float cx, cy, TArea;
            IList<PointF> points;
            #endregion

            #region CONSTRUCTORS
            public Triangle(float x1, float y1, float x2, float y2, float x3, float y3, Angle angle = default(Angle)) : this()
            {
                points = new PointF[] 
                {
                        new PointF(x1, y1),
                        new PointF(x2, y2),
                        new PointF(x3, y3)
                };

                Bounds = points.ToArea();
                angle = angle.AssignCenter(Bounds);

                points = points.Rotate(angle);
                c = points.AvgXY(); 
                Angle = angle.AssignCenter(Bounds);
                TArea = area(x1, y1, x2, y2, x3, y3);
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public PointF Centre => c;
            public IRectangleF Bounds { get; private set; }
            public string Name => "Triangle";
            public Angle Angle { get; private set; }
            public string ID { get; private set; }
            #endregion

            #region METHODS
            public bool Contains(float x, float y)
            {
                /* Calculate area of triangle PBC */
                var A1 = area(x, y, points[1].X, points[1].Y, points[2].X, points[2].Y);

                /* Calculate area of triangle PAC */
                var A2 = area(points[0].X, points[0].Y, x, y, points[2].X, points[2].Y);

                /* Calculate area of triangle PAB */
                var A3 = area(points[0].X, points[0].Y, points[1].X, points[1].Y, x, y);

                /* Check if sum of A1, A2 and A3 is same as A */
                return (TArea == A1 + A2 + A3);
            }
            static float area(float x1, float y1, float x2, float y2, float x3, float y3)
            {
                return Math.Abs((x1 * (y2 - y3) +
                                 x2 * (y3 - y1) +
                                 x3 * (y1 - y2)) / 2f);
            }
            #endregion

            #region IReadOnlyList<IPosF>
            public PointF this[int index] => points[index];
            public int Count => points.Count;
            IEnumerator<PointF> IEnumerable<PointF>.GetEnumerator() =>
                points.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() =>
                points.GetEnumerator();
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
