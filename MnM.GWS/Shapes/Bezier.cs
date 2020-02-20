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
        struct Bezier : IBezier
        {
            readonly IList<PointF> points;

            #region CONSTRUCTORS
            Bezier(IList<PointF> points): this()
            {
                this.points = points;
            }
            public Bezier(BezierType type, params float[] points) :
                this(type, points as IList<float>)
            { }
            public Bezier(BezierType type, Angle angle, params float[] points) :
                this(type, points as IList<float>, angle)
            { }
            public Bezier(BezierType type, IList<float> points, Angle angle = default(Angle)) :
                this(type, points, null, angle)
            { }
            public Bezier(BezierType type, IList<PointF> pixels, Angle angle = default(Angle)) :
                this(type, null, pixels, angle)
            { }
            public Bezier(BezierType type, ICollection<float> pointData, IList<PointF> pixels, Angle angle = default(Angle)) : this()
            {
                Bounds = InitializeBezier(type, pointData, pixels, out points, angle);
                Option = type;
                Angle = angle.AssignCenter(Bounds.Center());
                ID = Factory.NewID(Name);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static IRectangleF InitializeBezier(BezierType type, ICollection<float> pointData, ICollection<PointF> pixels, out IList<PointF> points, Angle angle = default(Angle))
            {
                IList<PointF> Source;

                if (pointData != null)
                    Source = pointData.ToPoints();
                else
                    Source = pixels.ToArray();

                var rc = Source.ToArea();
                angle = angle.AssignCenter(rc);

                if (angle.Valid)
                    Source = Source.Rotate(angle);
                
                points = InitializeBezier(type, Source);
                return rc;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static IList<PointF> InitializeBezier(BezierType type, IList<PointF> Source)
            {
                bool multiple = type.HasFlag(BezierType.Multiple);

                var points = new List<PointF>(100);

                if (multiple)
                {
                    for (int k = 1; k < Source.Count; k++)
                    {
                        if (k % 3 == 0)
                        {
                            Geometry.GetBezierPoints(4, ref points, new PointF[] { Source[k - 3], Source[k - 2], Source[k - 1], Source[k] });
                            if (points.Count > 0)
                                points.RemoveAt(points.Count - 1);
                        }
                    }
                }
                else
                    Geometry.GetBezierPoints(4,  ref points, Source);
                return points;
            }
            #endregion

            #region PROPERTIES
            public BezierType Option { get; private set; }
            public IRectangleF Bounds { get; private set; }
            public Angle Angle { get; private set; }
            public string Name => "Bezier";
            public string ID { get; private set; }
            #endregion

            #region IEnumerable<IPosF>
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
