using System;
using System.Collections;
using System.Collections.Generic;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct RoundedBox : IRoundedBox
        {
            #region VARIABLES
            public readonly float X;
            public readonly float Y;
            public readonly float Width;
            public readonly float Height;
            public readonly float CornerRadius;

            const string toStr = "x:{0}, y:{1}, width:{2}, height:{3}, cornerRadius: {4}";
            IList<PointF> points;
            #endregion

            #region CONSTRUCTORS
            public RoundedBox( float x,  float y,  float w,  float h,  float conerRadius,  Angle angle = default(Angle),  bool positiveLocation = false) : this()
            {
                ID = Factory.NewID(Name);
                X = x;
                Y = y;
                Width = w;
                Height = h;

                if (positiveLocation)
                {
                    if (X < 0)
                    {
                        Width += X;
                        X = 0;
                    }
                    if (Y < 0)
                    {
                        Height += Y;
                        Y = 0;
                    }
                }

                CornerRadius = conerRadius;
                Angle = angle.AssignCenter(this);

                var pts = new List<PointF>(50);

                Geometry.GetBezierPoints(4, ref pts,
                    new PointF[]
                    {
                        new PointF(X, Y + CornerRadius),
                        new PointF(X, Y),
                        new PointF(X + CornerRadius, Y)
                    });
                Geometry.GetBezierPoints(4, ref pts,
                    new PointF[]
                    {
                        new PointF(X + Width - CornerRadius, Y),
                        new PointF(X + Width, Y),
                        new PointF(X + Width, Y + CornerRadius)
                    });

                Geometry.GetBezierPoints(4, ref pts,
                   new PointF[]
                   {
                        new PointF(X + Width, Y + Height - CornerRadius),
                        new PointF(X + Width, Y + Height),
                        new PointF(X + Width - CornerRadius, Y + Height)
                   });

                Geometry.GetBezierPoints(4, ref pts,
                  new PointF[]
                  {
                        new PointF(X + CornerRadius, Y + Height),
                        new PointF(X, Y + Height),
                        new PointF(X, Y + Height - CornerRadius)
                  });

                if (Angle.Valid)
                    points = pts.Rotate(Angle);
                else
                    points = pts;
            }
            #endregion

            #region PROPERTIES
            public IRectangleF Bounds => this;
            public float Right => X + Width;
            public float Bottom => Y + Height;
            public bool IsEmpty =>
                Width == 0 && Height == 0;
            public bool IsSquare => Width == Height;
            public string ID { get; private set; }
            public string Name => "RoundedBox";
            public Angle Angle { get; private set; }
            float IRoundedBox.CornerRadius => CornerRadius;
            float IRectangleF.X => X;
            float IRectangleF.Y => Y;
            float IRectangleF.Width => Width;
            float IRectangleF.Height => Height;
            #endregion

            #region IEnumerable<IPosF>
            public IEnumerator<PointF> GetEnumerator() => points.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
            #endregion

            public override string ToString()
            {
                return string.Format(toStr, X, Y, Width, Height, CornerRadius);
            }

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
