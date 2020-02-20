using MnM.GWS.MathExtensions;
using System;
using System.Runtime.InteropServices;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point : IDrawable, IPoint
    {
        #region Variables
        public int X, Y;
        public byte Quadratic;

        const string toStr = "x:{0}, y:{1}";
        public readonly static Point Empty = new Point();
        #endregion

        #region Constructors
        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
            if (x != int.MinValue && y != int.MinValue)
                Valid = true;
        }
        public Point(int x, int y, byte quadratic) : this()
        {
            X = x;
            Y = y;
            Quadratic = quadratic;
            if (x != int.MinValue && y != int.MinValue)
                Valid = true;
        }
        public Point(Point p) : this()
        {
            X = p.X;
            Y = p.Y;
            Quadratic = p.Quadratic;
            if (X != int.MinValue && Y != int.MinValue)
                Valid = true;
        }
        public Point(Point p, byte quadratic) : this()
        {
            X = p.X;
            Y = p.Y;
            Quadratic = quadratic;
            if (X != int.MinValue && Y != int.MinValue)
                Valid = true;
        }
        #endregion

        #region Properties
        public static Point One => new Point(1, 1);
        public static Point UnitX => new Point(1, 0);
        public static Point UnitY => new Point(1, 0);
        public Point Yx => new Point(Y, X);
        public bool Valid { get; }
        int IPoint.X => X;
        int IPoint.Y => Y;
        #endregion

        #region MATH OPERATIONS
        public int Dot(Point p2) =>
            X * p2.X + Y * p2.Y;

        public int LengthSquared() =>
            (X * X + Y * Y);

        public Point Add(Point p2) =>
            new Point(X + p2.X, Y + p2.Y, Quadratic);

        public Point Subtract(Point p2) =>
            new Point(X - p2.X, Y - p2.Y, Quadratic);

        public Point Multiply(Point p2) =>
            new Point(X * p2.X, Y * p2.Y, Quadratic);

        public Point Divide(Point p2) =>
            new Point(X / p2.X, Y / p2.Y, Quadratic);

        public Point Multiply(int b) =>
            new Point(X * b, Y * b, Quadratic);

        public Point Add(int b) =>
            new Point(X + b, Y + b, Quadratic);

        public Point Subtract(int b) =>
            new Point(X - b, Y - b, Quadratic);

        public Point Divide(int b) =>
            new Point(X / b, Y / b, Quadratic);

        public Point Normalize()
        {
            int ls = X * X + Y * Y;
            float invNorm = 1.0f / (float)Math.Sqrt(ls);

            return new Point((X * invNorm).Round(), (Y * invNorm).Round(), Quadratic);
        }
        public Point TransformNormal(Matrix3x2 matrix)
        {
            return new Point(
                (X * matrix.M11 + Y * matrix.M21).Round(),
                (X * matrix.M12 + Y * matrix.M22).Round());
        }

        public Point Offset(Point o)
        {
            return new Point(X + o.X, Y + o.Y, Quadratic);
        }
        public Point Offset(int x, int y)
        {
            return new Point(X + x, Y + y, Quadratic);
        }
        #endregion

        #region equality
        public static bool operator ==(Point p1, Point p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Point p1, Point p2)
        {
            return !p1.Equals(p2);
        }
        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return other.X == X && other.Y == Y;
        }
        public override bool Equals(object obj)
        {
            if (obj is Point)
                return Equals((Point)obj);

            return false;
        }
        #endregion

        #region OPERATOR OVERLOADING
        public static Point operator +(Point p1, Point p2) =>
            new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Quadratic);

        public static Point operator -(Point p1, Point p2) =>
            new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Quadratic);

        public static Point operator *(Point p1, Point p2) =>
            new Point(p1.X * p2.X, p1.Y * p2.Y, p1.Quadratic);

        public static Point operator /(Point p1, Point p2) =>
            new Point(p1.X / p2.X, p1.Y / p2.Y, p1.Quadratic);

        public static Point operator *(Point p1, int b) =>
            new Point(p1.X * b, p1.Y * b, p1.Quadratic);

        public static Point operator +(Point p1, int b) =>
            new Point(p1.X + b, p1.Y + b, p1.Quadratic);

        public static Point operator -(Point p1, int b) =>
            new Point(p1.X - b, p1.Y - b, p1.Quadratic);

        public static Point operator /(Point p1, int b) =>
            new Point(p1.X / b, p1.Y / b, p1.Quadratic);
        #endregion

        #region Conversion 
        public static implicit operator PointF(Point p) =>
            new PointF(p.X, p.Y, p.Quadratic);
        #endregion

        public override string ToString()
        {
            return string.Format(toStr, X, Y);
        }

        public void DrawTo(IBuffer Writer, IBufferPen reader)
        {
            Writer.WritePixel(X, Y, true, reader.ReadPixel((int)X, (int)Y, true), Implementation.Renderer.AntiAlised);
        }
    }
}
