using MnM.GWS.MathExtensions;
using System;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct XLine : IXLine
        {
            #region constructors
            public XLine(int val1, int val2, int axis, bool isHorizontal, float? alpha1 = null, float? alpha2 = null) : this()
            {
                MathHelper.Order(ref val1, ref val2);
                Len = val2 - val1;
                A = Factory.newAPoint(val1, axis, alpha1, isHorizontal);
                B = Factory.newAPoint(val2, axis, alpha2, isHorizontal);
            }
            public XLine(float val1, float val2, int axis, bool isHorizontal) 
                : this()
            {
                MathHelper.Order(ref val1, ref val2);
                A = Factory.newAPoint(val1, axis, isHorizontal);
                B = Factory.newAPoint(val2, axis, isHorizontal);
                Len = B.IVal - A.IVal;
            }
            public XLine(IAPoint a, IAPoint b)
               : this()
            {
                A = a.Clone();
                B = b.Clone();
                Len = B.IVal - A.IVal;
            }
            #endregion

            #region properties
            public IAPoint A { get; private set; }
            public IAPoint B { get; private set; }
            public int Len { get; private set; }
            public int X => A.IsHorizontal ? A.IVal : A.Axis;
            public int Y => A.IsHorizontal ? A.Axis : A.IVal;
            #endregion

            #region METHODS
            public void DrawTo(IBuffer Writer, IBufferPen pen)
            {
                if (Implementation.Renderer.FillMode != FillMode.DrawOutLine)
                    Writer.WriteLine(A.IVal, B.IVal, A.Axis, A.IsHorizontal, pen, Ends.Draw);

                A.DrawTo(Writer, pen);
                B.DrawTo(Writer, pen);
            }
            public bool Contains(float x, float y)
            {
                if (A.IsHorizontal)
                    return y == A.Axis && x >= A.Val && x <= B.Val;
                return x == A.Axis && y >= A.Val && y <= A.Val;
            }
            public ISLine Add(float stroke)
            {
                if (stroke == 0)
                    return new SLine(this, null);
                return new SLine(this, new XLine(A.Add(stroke), B.Add(-stroke)));
            }
            #endregion

            public override string ToString()
            {
                if (A.IsHorizontal)
                    return "x1:" + A.IVal + ", x2:" + B.IVal + ", y:" + A.Axis + ", A:" + A.Alpha * 255;
                return "y1:" + A.IVal + ", y2:" + B.IVal + ", x:" + A.Axis + ", A:" + B.Alpha * 255;
            }

            #region equality
            public static bool operator ==(XLine l1, Point p2)
            {
                return l1.Equals(p2);
            }
            public static bool operator !=(XLine p1, Point p2)
            {
                return !p1.Equals(p2);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null))
                    return false;
                if (obj is IXLine)
                    return Equals(obj as IXLine);
                return false;
            }

            public override int GetHashCode()
            {
                return  new { A, B}.GetHashCode();
            }
            public bool Equals(IXLine l)
            {
                if (l == null)
                    return false;
                return l.A.IsHorizontal == A.IsHorizontal 
                    && l.A.IVal == A.IVal && 
                    l.B.IVal == B.IVal && l.A.Axis == A.Axis;
            }
            #endregion
        }
#if AllHidden
    }
#endif
}