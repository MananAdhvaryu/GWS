using System.Runtime.InteropServices;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Size: ISize
    {
        public int Width, Height;
        public readonly static Size Empty = new Size();
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static implicit operator SizeF(Size s)=>
            new SizeF(s.Width, s.Height);
        public bool IsEmpty => Width == 0 && Height == 0;
        int ISize.Width => Width;
        int ISize.Height => Height;

        public static implicit operator bool(Size s) =>
            !(s.Width < 1 || int.MinValue == (s.Width) || s.Height < 1 || int.MinValue == (s.Height));

    }
}
