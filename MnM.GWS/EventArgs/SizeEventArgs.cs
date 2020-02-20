using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public class SizeEventArgs : EventArgs, ISizeEventArgs
    {
        const string toStr = "width:{0}, height:{1}";

        public SizeEventArgs(Size e)
        {
            Width = e.Width;
            Height = e.Height;
        }
        public SizeEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public override string ToString()
        {
            return string.Format(toStr, Width, Height);
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Size Size => new Size(Width, Height);
    }
}
