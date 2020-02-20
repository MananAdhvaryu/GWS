using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
        [StructLayout(LayoutKind.Sequential)]
#else
        [StructLayout(LayoutKind.Sequential)]
        public
#endif
        struct BoxF : IBoxF, ICloneable, ISquareF
        {
            #region VARIABLES
            public readonly float X;
            public readonly float Y;
            public readonly float Width;
            public readonly float Height;

            const string toStr = "x:{0}, y:{1}, width:{2}, height:{3}";
            public static readonly BoxF ZeroIntersection =
                new BoxF(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
            public static readonly BoxF Empty = new BoxF();
            #endregion

            #region CONSTRUCTORS
            public BoxF( float x,  float y,  float w,  float h,  bool positiveLocation = false) : this()
            {
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
                    if (y < 0)
                    {
                        Height += Y;
                        Y = 0;
                    }
                }
                X = x;
                Y = y;
                Width = w;
                Height = h;

                ID = Factory.NewID(Name);
            }

            BoxF( float? x,  float? y,  float? w,  float? h,  bool positiveLocation = false) :
                this(x ?? 0, y ?? 0, w ?? 0, h ?? 0, positiveLocation)
            { }
            #endregion

            #region PROPERTIES
            public float Right => X + Width;
            public float Bottom => Y + Height;
            public bool IsEmpty =>
                Width == 0 && Height == 0;
            public bool IsSquare => Width == Height;
            public string ID { get; private set; }
            public string Name => "BoxF";
            Angle IRotatable.Angle => Angle.Empty;
            IRectangleF IOccupier.Bounds => this;
            float ISquareF.X => X;
            float ISquareF.Y => Y;
            float ISquareF.Width => Width;
            float IRectangleF.X => X;
            float IRectangleF.Y => Y;
            float IRectangleF.Width => Width;
            float IRectangleF.Height => Height;
            #endregion

            #region NORMALIZE
            public BoxF Normalize()
            {
                float t, x, y, r, b;
                x = X;
                y = Y;
                r = Right;
                b = Bottom;

                if (X > Right)
                {
                    t = X;
                    x = Right;
                    r = t;
                }
                if (y > Bottom)
                {
                    t = Bottom;
                    b = Y;
                    y = t;
                }
                return new BoxF(x, y, r - x, b - y);
            }
            #endregion

            #region CONTAINS
            public bool Contains(float x, float y)
            {
                return x >= X && x <= Right && y >= Y && y <= Bottom;
            }
            public bool Contains(int x, int y)
            {
                return x >= X && x <= Right && y >= Y && y <= Bottom;
            }
            #endregion

            #region CLONE
            public BoxF Clone() => 
                new BoxF(X, Y, Width, Height);
            object ICloneable.Clone() => Clone();
            #endregion

            #region STATIC METHODS
            public static BoxF Union(BoxF a, BoxF b)
            {
                float x1 = Math.Min(a.X, b.X);
                float x2 = Math.Max(a.X + a.Width, b.X + b.Width);
                float y1 = Math.Min(a.Y, b.Y);
                float y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

                return new BoxF(x1, y1, x2 - x1, y2 - y1);
            }
            #endregion

            #region IEnumerable<IPosF>
            public IEnumerator<PointF> GetEnumerator()
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            yield return new PointF(X, Y);
                            break;
                        case 1:
                            yield return new PointF(X, Bottom);
                            break;
                        case 2:
                            yield return new PointF(Right, Bottom);
                            break;
                        case 3:
                        default:
                            yield return new PointF(Right, Y);
                            break;
                    }
                }
            }
            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
            #endregion

            public override string ToString()
            {
                return string.Format(toStr, X, Y, Width, Height);
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

