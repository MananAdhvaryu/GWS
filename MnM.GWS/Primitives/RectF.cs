using System;
using System.Runtime.InteropServices;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RectF : IRectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        const string toStr = "x:{0}, y:{1}, width:{2}, height:{3}";

        public RectF( float x,  float y,  float w,  float h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
        public RectF( IBox area)
        {
            X = area.X;
            Y = area.Y;
            Width = area.Width;
            Height = area.Height;
        }
        public RectF( IBoxF area)
        {
            X = area.X;
            Y = area.Y;
            Width = area.Width;
            Height = area.Height;
        }
        public RectF( IRectangle area)
        {
            X = area.X;
            Y = area.Y;
            Width = area.Width;
            Height = area.Height;
        }
        public RectF( IRectangleF area)
        {
            X = area.X;
            Y = area.Y;
            Width = area.Width;
            Height = area.Height;
        }
        public float Right => X + Width;
        public float Bottom => Y + Height;
        public IntPtr Handle => this.ToPtr();


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

        public void DrawTo(IGraphics writer, IPenContext penContext)
        {
            writer.DrawRectangle(X, Y, Width, Height, penContext);
        }
        public override string ToString()
        {
            return string.Format(toStr, X, Y, Width, Height);
        }

        IRectangleF IOccupier.Bounds => this;
        float IRectangleF.X => X;
        float IRectangleF.Y => Y;
        float IRectangleF.Width => Width;
        float IRectangleF.Height => Height;
    }

}
