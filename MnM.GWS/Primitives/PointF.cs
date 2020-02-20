using MnM.GWS.MathExtensions;
using System;
using System.Runtime.InteropServices;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PointF : IDrawable
    {
        public float X, Y;
        public byte Quadratic;
        const string toStr = "x:{0}, y:{1}, Quadratic:{2}";

        public readonly static PointF Empty = new PointF();

        #region CONSTRUCTORS
        public PointF(float x, float y, Angle angle, byte quadratic  = 0) : this()
        {
            this.X = x;
            this.Y = y;
            Quadratic = quadratic;
            if (!float.IsNaN(X) && !float.IsNaN(Y))
                Valid = true;

            if (Valid && angle.Valid)
                angle.Rotate(x, y, out x, out y);
        }
        public PointF(float x, float y, byte quadratic = 0) : this()
        {
            X = x;
            Y = y;
            if (!float.IsNaN(X) && !float.IsNaN(Y))
                Valid = true;
            Quadratic = quadratic;
        }
        public PointF(PointF p, byte quadratic) : this()
        {
            X = p.X;
            Y = p.Y;
            if (!float.IsNaN(X) && !float.IsNaN(Y))
                Valid = true;

            Quadratic = quadratic;
        }
        public PointF(PointF p) : this()
        {
            X = p.X;
            Y = p.Y;
            if (!float.IsNaN(X) && !float.IsNaN(Y))
                Valid = true;
            Quadratic = p.Quadratic;
        }
        #endregion

        #region PROPERTIES
        public static PointF UnitX = new PointF(1f, 0);
        public static PointF UnitY = new PointF(0f, 1f);
        public bool Valid { get; private set; }
        #endregion

        #region MATH OPERTIONS
        public float Dot(PointF p2) =>
            X * p2.X + Y * p2.Y;

        public float LengthSquared() =>
            (X * X + Y * Y);

        public PointF SquareRoot() =>
            new PointF((float)Math.Sqrt(X), (float)Math.Sqrt(Y), Quadratic);

        public float Length() =>
            (float)Math.Sqrt(X * X + Y * Y);

        public PointF Normalize()
        {
            float ls = X * X + Y * Y;
            float invNorm = 1.0f / (float)Math.Sqrt(ls);

            return new PointF(X * invNorm, Y * invNorm, Quadratic);
        }
        public PointF TransformNormal(Matrix3x2 matrix)
        {
            return new PointF(
                X * matrix.M11 + Y * matrix.M21,
                X * matrix.M12 + Y * matrix.M22);
        }

        public PointF Offset(PointF o)
        {
            return new PointF(X + o.X, Y + o.Y, Quadratic);
        }
        public static PointF Reflect(PointF p, PointF normal)
        {
            float dot = p.X * normal.X + p.Y * normal.Y;

            return new PointF(
                p.X - 2.0f * dot * normal.X,
                p.Y - 2.0f * dot * normal.Y);
        }

        public PointF Add(PointF p2) =>
            new PointF(X + p2.X, Y + p2.Y, Quadratic);

        public PointF Subtract(PointF p2) =>
            new PointF(X - p2.X, Y - p2.Y, Quadratic);

        public PointF Multiply(PointF p2) =>
            new PointF(X * p2.X, Y * p2.Y, Quadratic);

        public PointF Divide(PointF p2) =>
            new PointF(X / p2.X, Y / p2.Y, Quadratic);

        public PointF Multiply(float b) =>
            new PointF(X * b, Y * b, Quadratic);

        public PointF Add(float b) =>
            new PointF(X + b, Y + b, Quadratic);

        public PointF Subtract(float b) =>
            new PointF(X - b, Y - b, Quadratic);

        public PointF Divide(float b) =>
            new PointF(X / b, Y / b, Quadratic);

        public PointF Negate() =>
            new PointF(-X, -Y, Quadratic);
        #endregion

        #region ROUNDING
        public Point Ceiling()
        {
            if (!this.Valid)
                return new Point();
           return new Point(X.Ceiling(), Y.Ceiling(), Quadratic);
        }
        public Point Round()
        {
            if (!this.Valid)
                return new Point();

            return new Point(X.Round(), Y.Round(), Quadratic);
        }
        public Point Floor()
        {
            if (!this.Valid)
                return new Point();

           return  new Point((int)X, (int)Y, Quadratic);
        }

        public PointF CeilingF()
        {
            if (!this.Valid)
                return new PointF();
            return new PointF(X.Ceiling(), Y.Ceiling(), Quadratic);
        }
        public PointF RoundF()
        {
            if (!this.Valid)
                return new PointF();

            return new PointF(X.Round(), Y.Round(), Quadratic);
        }
        public PointF FloorF()
        {
            if (!Valid)
                return new PointF();

            return new PointF((int)X, (int)Y, Quadratic);
        }
        #endregion

        #region  MIN - MAX
        public PointF Min(PointF p2)
        {
            return new PointF(
                (X < p2.X) ? X : p2.X,
                (Y < p2.Y) ? Y : p2.Y, Quadratic);
        }
        public PointF Max(PointF p2)
        {
            return new PointF(
                (X > p2.X) ? X : p2.X,
                (Y > p2.Y) ? Y : p2.Y, Quadratic);
        }
        #endregion

        #region DISTANCE SQUARED
        public float DistanceSquared(PointF p2)
        {
            float dx = X - p2.X;
            float dy = Y - p2.Y;

            float ls = dx * dx + dy * dy;
            return (ls);
        }
        public static float DistanceSquared(Point p1, Point p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;

            float ls = dx * dx + dy * dy;
            return (ls);
        }
        public static float Distance(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;

            float ls = dx * dx + dy * dy;

            return (float)Math.Sqrt(ls);
        }
        public float DistanceFrom(PointF p2)
        {
            float dx = X - p2.X;
            float dy = Y - p2.Y;

            float ls = dx * dx + dy * dy;

            return (float)Math.Sqrt(ls);
        }
        #endregion

        #region CLAMP & LERP
        public static PointF Clamp(PointF p, PointF min, PointF max)
        {
            // This compare order is very important!!!
            // We must follow HLSL behavior  the case user specified min value is bigger than max value.
            float x = p.X;
            x = (min.X > x) ? min.X : x;  // max(x, minx)
            x = (max.X < x) ? max.X : x;  // min(x, maxx)

            float y = p.Y;
            y = (min.Y > y) ? min.Y : y;  // max(y, miny)
            y = (max.Y < y) ? max.Y : y;  // min(y, maxy)

            return new PointF(x, y);
        }
        public static PointF Lerp(PointF p1, PointF p2, float amount)
        {
            return new PointF(
                p1.X + (p2.X - p1.X) * amount,
                p1.Y + (p2.Y - p1.Y) * amount);
        }
        #endregion

        #region TRANSFORM
        public static PointF Transform(PointF position, Matrix3x2 matrix)
        {
            return new PointF(
                position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M31,
                position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M32);
        }
        public static PointF TransformNormal(PointF normal, Matrix3x2 matrix)
        {
            return new PointF(
                normal.X * matrix.M11 + normal.Y * matrix.M21,
                normal.X * matrix.M12 + normal.Y * matrix.M22);
        }
        #endregion

        #region ANGLE
        public float AngleFrom(PointF b)
        {
            float angle = (float)(Math.Atan2(b.Y, b.X) - Math.Atan2(Y, X));
            return angle;
        }
        public float Angle() =>
            (float) Math.Atan2(Y, X);
        public static float GetAngle(PointF a, PointF b)
        {
            float angle = (float)(Math.Atan2(b.Y, b.X) - Math.Atan2(a.Y, a.X));
            return angle;
        }
        #endregion

        #region OPERATOR OVERLOADING
        public static PointF operator + (PointF p1, PointF p2) =>
            new PointF(p1.X + p2.X, p1.Y + p2.Y, p1.Quadratic);

        public static PointF operator -(PointF p1, PointF p2) =>
            new PointF(p1.X - p2.X, p1.Y - p2.Y, p1.Quadratic);

        public static PointF operator *(PointF p1, PointF p2) =>
            new PointF(p1.X * p2.X, p1.Y * p2.Y, p1.Quadratic);

        public static PointF operator /(PointF p1, PointF p2) =>
            new PointF(p1.X / p2.X, p1.Y / p2.Y, p1.Quadratic);

        public static PointF operator *(PointF p1, float b) =>
            new PointF(p1.X * b, p1.Y * b, p1.Quadratic);

        public static PointF operator +(PointF p1, float b) =>
            new PointF(p1.X + b, p1.Y + b, p1.Quadratic);

        public static PointF operator -(PointF p1, float b) =>
            new PointF(p1.X - b, p1.Y - b, p1.Quadratic);

        public static PointF operator /(PointF p1, float b) =>
            new PointF(p1.X / b, p1.Y / b, p1.Quadratic);
        #endregion

        #region EQUALITY
        public static bool operator ==(PointF p1, PointF p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(PointF p1, PointF p2)
        {
            return !p1.Equals(p2);
        }

        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }

        public bool Equals(PointF other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return other.X == X && other.Y == Y;
        }
        public bool Equals(float x, float y) =>
            X == x && Y == y;

        public override bool Equals(object obj)
        {
            if (obj is PointF)
                return Equals((PointF)obj);

            return false;
        }
        #endregion

        public override string ToString()
        {
            return string.Format(toStr, X, Y, Quadratic);
        }

        public void DrawTo(IBuffer Writer, IBufferPen reader)
        {
            Writer.WritePixel(X, Y, true, reader.ReadPixel((int)X, (int)Y, true), Implementation.Renderer.AntiAlised);
        }
    }
}