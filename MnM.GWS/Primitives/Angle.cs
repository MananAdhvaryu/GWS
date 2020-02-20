using MnM.GWS.MathExtensions;
using System;
using System.Runtime.CompilerServices;

namespace MnM.GWS
{
    public struct Angle
    {
        #region variables
        public readonly static Angle Empty = new Angle(0);
        public readonly bool Initialized;
        public readonly float Sin, Cos, MSin, MCos;
        public readonly float Degree, CX, CY;
        public readonly bool Skew, CenterAssigned;

        static string tostr = "A:{0}, Cos:{1}, Sin{2}, CX:{3}, CY:{4}";
        #endregion

        #region constructors
        public Angle(float angle, bool skew = false) : this(angle, 0, 0, skew)
        {
            CenterAssigned = false;
        }
        public Angle(float angle, PointF c, bool skew = false) :
            this(angle, c.X, c.Y, skew)
        { }
        public Angle(float angle, float cx, float cy, bool skew = false, bool centerAssigned = true)  
        {
            if (angle >= 360)
                angle %= 360;

            Geometry.SinCos(angle, out Sin, out Cos);
            Geometry.SinCos(-angle, out MSin, out MCos);

            CenterAssigned = centerAssigned;
            Degree = angle;
            CX  = cx;
            CY = cy;
            Skew = skew;
            Initialized = true;
        }

        public Angle(Angle angle, float centerX, float centerY, bool? skew = null) :
            this(angle.Degree, centerX, centerY, skew?? angle.Skew)
        { }

        public Angle(Angle angle, bool? skew = null) : 
            this(angle.Degree, angle.CX, angle.CY,skew?? angle.Skew, angle.CenterAssigned)
        { }
        public Angle(Angle angle, PointF p, bool? skew = null) : 
            this(angle.Degree, p.X, p.Y, skew??angle.Skew, angle.CenterAssigned)
        {  }
        public Angle(float angle, PointF p, bool? skew = null) :
            this(angle, p.X, p.Y, skew?? false, true)
        { }

        public Angle(float angle, IRectangleF area, bool skew = false) :
            this(angle, area.Center(), skew)
        { }
        public Angle(float angle, IRectangle area, bool skew = false) :
            this(angle, area.Center(), skew)
        { }

        public Angle(float x, float y, float cx, float cy, bool skew = false) :
            this((float)(Math.Atan2((y - cy), (x - cx)) * Geometry.RadianInv), cx, cy, skew)
        { }
        #endregion

        #region properties
        public bool Valid => Initialized && Degree != 0 && Degree != 360;
        public bool SkewMode => Skew && Valid && CenterAssigned;
        #endregion

        #region ROTATE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float x, float y, out float newX, out float newY, bool negative = false) =>
            Rotate(x, y, CX, CY, out newX, out newY, negative);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float x, float y, out int newX, out int newY, bool negative = false)
        {
            Rotate(x, y, CX, CY, out float nx, out float ny, negative);
            newX = nx.Round();
            newY = ny.Round();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float x, float y, float cx, float cy, out float newX, out float newY, bool negative = false)
        {
            if (!Valid)
            {
                newX = x;
                newY = y;
                return;
            }
            x -= cx;
            y -= cy;

            if (negative)
            {
                newX = x * MCos - y * MSin;
                newY = x * MSin + y * MCos;
            }
            else
            {
                newX = x * Cos - y * Sin;
                newY = x * Sin + y * Cos;
            }
            newX += cx;
            newY += cy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float val, float axis, bool horizontal, float cx, float cy, out float newX, out float newY, bool negative = false)
        {
            if (horizontal)
                Rotate(val, axis, cx, cy, out newX, out newY, negative);
            else
                Rotate(axis, val, cx, cy, out newX, out newY, negative);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float val, float axis, bool horizontal, out float newX, out float newY, bool negative = false)
        {
            if (horizontal)
                Rotate(val, axis, CX, CY, out newX, out newY, negative);
            else
                Rotate(axis, val, CX, CY, out newX, out newY, negative);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointF Rotate(float x, float y, bool negative = false)
        {
            if (Valid)
                Rotate(x, y, out x, out y, negative);
            return new PointF(x, y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointF Rotate(PointF p, bool negative = false)
        {
            if (!p.Valid)
                return p;
            if (Valid)
            {
                Rotate(p.X, p.Y, out float x, out float y, negative);
                return new PointF(x, y, p.Quadratic);
            }
            return new PointF(p.X, p.Y, p.Quadratic);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointF Rotate(Point p, bool negative = false)
        {
            if (!p.Valid)
                return p;

            if (Valid)
            {
                Rotate(p.X, p.Y, out float x, out float y, negative);
                return new PointF(x, y, p.Quadratic);
            }
            return new PointF((float)p.X, p.Y, p.Quadratic);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointF Rotate(PointF p, PointF center, bool negative = false)
        {
            if (!p.Valid)
                return p;

            if (Valid)
            {
                Rotate(p.X, p.Y, center.X, center.Y, out float x, out float y, negative);
                return new PointF(x, y, p.Quadratic);
            }
            return new PointF(p.X, p.Y, p.Quadratic);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Rotate(int val, int axis, bool horizontal, out float x, out float y, out bool isRotated, bool negative)
        {
            if (Valid)
            {
                Rotate(val, axis, horizontal, out x, out y, negative);
                isRotated = true;
            }
            else
            {
                x = horizontal ? val : axis;
                y = horizontal ? axis : val;
                isRotated = false;
            }

            if (x < -1 || y < -1)
                return false;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Rotate(float val, float axis, bool horizontal, out float x, out float y, out bool isRotated, bool negative)
        {
            if (Valid)
            {
                Rotate(val, axis, horizontal, out x, out y, negative);
                isRotated = true;
            }
            else
            {
                x = horizontal ? val : axis;
                y = horizontal ? axis : val;
                isRotated = false;
            }
            if (x < -1 || y < -1)
                return false;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotationDifferce(float x, float y, float rx, float ry, out float differenceOfX, out float differenceOfY)
        {
            differenceOfX = 0;
            differenceOfY = 0;
            if (!Valid)
                return;
            Rotate(x, y, CX - rx, CY - ry, out differenceOfX, out differenceOfY);

            differenceOfX -= x;
            differenceOfY -= y;
            return;
        }
        #endregion

        #region ASSIGN CENTER
        public Angle AssignCenter(float cx, float cy, bool permanent = false)
        {
            if (CenterAssigned)
                return this;
            var a = new Angle(Degree, cx, cy, Skew, permanent);
            return a;
        }
        public Angle AssignCenter(PointF c, bool permanent = false)
        {
            if (CenterAssigned)
                return this;
            var a = new Angle(Degree, c.X, c.Y, Skew, permanent);
            return a;
        }
        public Angle AssignCenter(IRectangleF rc, bool permanent = false)
        {
            if (CenterAssigned)
                return this;
            var a = new Angle(Degree, rc.X + rc.Width / 2f, rc.Y + rc.Height / 2f, Skew, permanent);
            return a;
        }
        public Angle AssignCenter(float x, float y, float width, float height, bool permanent = false) =>
          AssignCenter(MathHelper.Middle(x, x + width), MathHelper.Middle(y, y + height), permanent);
        #endregion

        # region OPERATORS
        public static Angle operator -(Angle a) =>
            new Angle(-a.Degree, a.CX, a.CY, a.Skew);
        public static Angle operator +(Angle a, Angle b)
        {
            if (!a.Valid && !b.Valid)
                return a;
            return new Angle(a.Degree + b.Degree, a.CX + b.CX, a.CY + b.CY, 
                a.Skew || b.Skew, a.CenterAssigned || b.CenterAssigned);
        }
        public static Angle operator -(Angle a, Angle b)
        {
            if (!a.Valid && !b.Valid)
                return a;
            return new Angle(a.Degree - b.Degree, a.CX - b.CX, a.CY - b.CY, 
                a.Skew || b.Skew, a.CenterAssigned || b.CenterAssigned);
        }
        #endregion

        public override string ToString()
        {
            return string.Format(tostr, Degree, Cos, Sin, CX, CY);
        }
    }
}
