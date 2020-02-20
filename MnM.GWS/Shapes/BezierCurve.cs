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
        struct BezierCurve : IBezierCurve
        {       
            IList<PointF> points;
            EllipseData ei;

            #region CONSTRUCTORS
            public BezierCurve(float x, float y, float width, float height, Angle angle = default(Angle)) :
                this(x, y, width, height, 0, 0, false, angle)
            { }
            public BezierCurve(float x, float y, float width, float height, float arcstart, float arcEnd, bool isArc, Angle angle = default(Angle)) : this()
            {
                Initialize(x, y, width, height, arcstart, arcEnd, isArc, angle);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void Initialize(float x, float y, float width, float height, float arcstart, float arcEnd, bool isArc, Angle angle = default(Angle))
            {
                Bounds = Factory.newRectangleF(x, y, width, height);

                StartAngle = arcstart;
                EndAngle = arcEnd;
                ei = new EllipseData(x, y, width, height, angle);

                IsArc = isArc;
                points = GetBezierArcPoints(this);
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public float StartAngle { get; private set; }
            public float EndAngle { get; private set; }
            public bool IsArc { get; private set; }
            public bool NoSweepAngle { get; set; }
            public EllipseData Data => ei;
            public bool FillCorrect { get; set; }
            public IRectangleF Bounds { get; private set; }
            public Angle Angle => ei.Angle;
            public bool IsClose { get; set; }
            public string Name => IsArc? "BezierArc" : "BezierPie";
            public bool Close { get; set; }
            public string ID { get; private set; }
            #endregion

            #region METHODS
            static IList<PointF> GetBezierArcPoints(IBezierCurve e)
            {
                List<PointF> points = new List<PointF>(100);
                Angle angle = e.Data.Angle;
                var x = e.Data.X;
                var y = e.Data.Y;
               
                if (e.EndAngle > 179)
                {
                    var pts = Geometry.GetBezier4Points(x, y, e.Data.Rx, e.Data.Ry, e.StartAngle, 179);
                    Geometry.GetBezierPoints(4, ref points, pts);
                    pts = (Geometry.GetBezier4Points(x, y, e.Data.Rx, e.Data.Ry, 179, e.EndAngle));
                    Geometry.GetBezierPoints(4, ref points, pts);
                }
                else
                {
                    var pts = Geometry.GetBezier4Points(x, y, e.Data.Rx, e.Data.Ry, e.StartAngle, e.EndAngle);
                    Geometry.GetBezierPoints(4, ref points, pts);
                }
                IList<PointF> Points = points;

                if (angle.Valid)
                    Points = points.Rotate(angle);

                if (!e.IsArc)
                    Points.Insert(0, new PointF(e.Data.Cx, e.Data.Cy));

                return Points;
            }
            #endregion

            #region IReadOnlyList<IPosF>
            public PointF this[int index] => points[index];
            public int Count => points.Count;
            public IEnumerator<PointF> GetEnumerator() =>
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
