/* Licensed under the MIT/X11 license.
* Copyright (c) 2016-2018 jointly owned by eBestow Technocracy India Pvt. Ltd. & M&M Info-Tech UK Ltd.
* This notice may not be removed from any source distribution.
* See license.txt for detailed licensing details. */

using MnM.GWS.MathExtensions;

using System;
using System.Collections;
using System.Collections.Generic;
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
        struct Line : ILine
        {
            #region VARIABLES
            const string toStr = "x1:{0}, y1:{1}, x2:{2}, y2:{3}";
            public static readonly Line Empty = new Line(0, 0, 0, 0);
            #endregion

            #region CONSTRUCTORS
            public Line(float x1, float y1, float x2, float y2, Angle angle = default(Angle), float deviation = 0, bool assignID = false) : this()
            {
                if (angle.Valid)
                {
                    Angle a;

                    if (angle.CenterAssigned)
                        a = angle;
                    else
                    {
                        var cx = Math.Min(x1, x2) + Math.Abs(x2 - x1) / 2;
                        var cy = Math.Min(y1, y2) + Math.Abs(y2 - y1) / 2;

                        a = new Angle(angle, cx, cy);
                    }

                    a.Rotate(x1, y1, out x1, out y1);
                    a.Rotate(x2, y2, out x2, out y2);
                }
                if (deviation != 0)
                    Geometry.ParallelLine(x1, y1, x2, y2, deviation, out x1, out y1, out x2, out y2);

                Geometry.RoundLineCoordinates(ref x1, ref y1, ref x2, ref y2, 4);

                DX = x2 - x1;
                DY = y2 - y1;
                M = C = 0;
                M = DY;
                if (DX != 0)
                    M /= DX;
                C = y1 - M * x1;

                IsValid = !(float.IsNaN(x1) || float.IsNaN(y1) || float.IsNaN(x2) || float.IsNaN(y2));

                var w = Math.Abs(x1 - x2).Ceiling();
                var h = Math.Abs(y1 - y2).Ceiling();
                if (w == 0)
                    w = 1;
                if (h == 0)
                    h = 1;
                
                Bounds = Factory.newRectangleF(Math.Min(x1, x2), Math.Min(y1, y2), w, h);

                IsHorizontal = Math.Abs(DY) <= Geometry.EPSILON;
                IsVertical = Math.Abs(DX) <= Geometry.EPSILON;

                IsPoint = IsVertical && IsHorizontal;
                Steep = Bounds.Height > Bounds.Width;

                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
                MaxY = Math.Max(y1, y2);
                MinY = Math.Min(y1, y2);
                MaxX = Math.Max(x1, x2);
                MinX = Math.Min(x1, x2);
                if (assignID)
                    ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public float X1 { get; private set; }
            public float Y1 { get; private set; }
            public float X2 { get; private set; }
            public float Y2 { get; private set; }
            public float MaxY { get; private set; }
            public float MinY { get; private set; }
            public float MaxX { get; private set; }
            public float MinX { get; private set; }
            public PointF Start => new PointF(X1, Y1);
            public PointF End => new PointF(X2, Y2);

            public float M { get; private set; }
            public float C { get; private set; }
            public float DX { get; private set; }
            public float DY { get; private set; }

            public bool IsValid { get; private set; }
            public bool Steep { get; private set; }
            public bool IsHorizontal { get; private set; }
            public bool IsPoint { get; private set; }
            public bool IsVertical { get; private set; }
            public IRectangleF Bounds { get; private set; }
            public string Name => "Line";
            public string ID { get; private set; }
            public Angle Angle { get; private set; }
            #endregion

            #region CONTAINS
            public bool Contains(float x, float y) =>
                (M * x + C) - y == 0;
            #endregion

            #region OPERATORS
            public static bool operator ==(ILine a, Line b) =>
                a.Equals(b);
            public static bool operator !=(ILine a, Line b) =>
                !a.Equals(b);
            #endregion

            #region EQUALITY
            public override int GetHashCode()
            {
                return new { Start, End}.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (!(obj is ILine))
                    return false;
                return Equals((ILine)obj);
            }
            public bool Equals(ILine other)
            {
                return other.Start.Equals(Start) && other.End.Equals(End);
            }
            #endregion

            #region TO STRING
            public override string ToString() =>
                string.Format(toStr, Start.X, Start.Y, End.X, End.Y);
            #endregion

            #region INTERFACE
            public Line Clone()
            {
                var l = new Line();
                l.ID = Factory.NewID(Name);
                l.X1 = X1;
                l.Y1 = Y1;
                l.X2 = X2;
                l.Y2 = Y2;
                l.DX = DX;
                l.DY = DY;
                l.M = M;
                l.C = C;
                l.Bounds = Bounds;
                l.Steep = Steep;
                l.IsHorizontal = IsHorizontal;
                l.IsPoint = IsPoint;
                l.IsValid = IsValid;
                l.MinX = MinX;
                l.MinY = MinY;
                l.MaxX = MaxX;
                l.MaxY = MaxY;
                return l;
            }
            object ICloneable.Clone() => Clone();
            ILine ILine.Clone() => Clone();
            #endregion

            #region ASSIGN ID IF NONE
            public void AssignIDIfNone()
            {
                if (ID == null)
                    ID = Factory.NewID(Name);
            }
            #endregion

            #region IReadOnlyList<IPosF>
            IEnumerator<PointF> IEnumerable<PointF>.GetEnumerator()
            {
                yield return Start;
                yield return End;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return Start;
                yield return End;
            }
            #endregion
        }
#if AllHidden
    }
#endif
}
