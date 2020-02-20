using System;
using System.Runtime.InteropServices;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect: IRectangle, IEquatable<IRectangle>
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        const string toStr = "x:{0}, y:{1}, width:{2}, height:{3}";

        public Rect( int x,  int y,  int w,  int h): this()
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            if (w != 0 && h != 0)
                Handle = this.ToPtr();
        }
        public Rect( IRectangle area) : this(area.X, area.Y, area.Width, area.Height)
        { }

        public int Right => X + Width;
        public int Bottom => Y + Height;
        public IntPtr Handle { get; private set; }

        public bool Contains( float x,  float y)
        {
            return x >= X && y >= Y && x <= Right && y <= Bottom;
        }
        public bool Contains(float x, float y,  int offsetX,  int offsetY)
        {
            x -= offsetX;
            y -= offsetY;
            return x >= X && y >= Y && x <= Right && y <= Bottom;
        }

        int IRectangle.X => X;
        int IRectangle.Y => Y;
        int IRectangle.Width => Width;
        int IRectangle.Height => Height;

        public void DrawTo(IGraphics writer, IPenContext penContext)
        {
            writer.DrawRectangle(X, Y, Width, Height, penContext);
        }

        public override string ToString()
        {
            return string.Format(toStr, X, Y, Width, Height);
        }

        public bool Equals(IRectangle other) =>
            X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        public override bool Equals(object obj)
        {
            if (obj is IRectangle)
                return Equals((IRectangle)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return new { X, Y, Width, Height }.GetHashCode();
        }
    }
}
