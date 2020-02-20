using MnM.GWS.MathExtensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SizeF
    {
        public float Width, Height;
        public readonly static SizeF Empty = new SizeF();

        public SizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public  bool Valid =>
            !(Width < 1 || float.IsNaN(Width) || Height < 1 || float.IsNaN(Height));

        public Size Ceiling() =>
            new Size(Width.Ceiling(), Height.Ceiling());
        public Size Round(MidpointRounding r = MidpointRounding.ToEven) =>
            new Size(Width.Round(r), Height.Round(r));
        public Size Floor() =>
            new Size((int)Width, (int)Height);

        public static explicit operator PointF (SizeF wh)=>
            new PointF(wh.Width, wh.Height);
    }
}
