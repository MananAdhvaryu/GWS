using System;
using MnM.GWS.MathExtensions;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
    public
#endif
       sealed class SolidPen : ISolidPen
        {
            #region CONSTRUCTORS
            SolidPen() { }
            SolidPen( int color): this()
            {
                Colour = color;
                ID = Factory.NewID(color);
            }
            public static SolidPen CreateInstance( int color) =>
                CreateInstance(color, 1, 1);
            public static SolidPen CreateInstance( int color,  int width,  int height)
            {
                var key = color.ToString();
                SolidPen b;

                if (!Factory.Get(key, out b, ObjType.Buffer))
                    b = new SolidPen(color);
                b.Width = width;
                b.Height = height;
                return b;
            }
            #endregion

            #region PROPERTIES
            public int this[int index] => Colour;
            public int Colour { get; private set; }

            public int Length => Width * Height;
            public int Width { get; private set; }
            public int Height { get; private set; }
            public bool IsDisposed { get; private set; }
            public string ID { get; private set; }
            #endregion

            #region methods
            public int IndexOf(int val, int axis,  bool horizontal)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");
                var x = horizontal ? val : axis;
                var y = horizontal ? axis : val;
                return x + y * Width;
            }
            public void XYOf( int index, out int x, out int y)
            {
                if (IsDisposed)
                    throw new System.Exception("Object is disposed!");
                y = index / Width;
                x = index - y * Width;
            }

            public IBufferPen ToPen(int? width, int? height) =>
                CreateInstance(Colour, width ?? 1, height ?? 1);

            public void Dispose() =>
                Factory.Remove(this, ObjType.Buffer);

            public unsafe IRectangle CopyTo( IRectangle copyRc, IntPtr dest,  int destLen,  int destW,  int destX,  int destY)
            {
                var source = Colour.Repeat(copyRc.Width * copyRc.Height);
                var dst = (int*)dest;
                fixed (int* src = source)
                {
                   return CopyBlock(source.Length, copyRc, copyRc.Width, destX, destY, destW, destLen,
                        (si, di, w, i) => Implementation.CopyMemory(source, si, dst, di, w));
                }
            }
            #endregion

            void IStoreable.AssignIDIfNone()
            {
                if (ID == null)
                    ID = Factory.NewID(Colour);
            }
        }
#if AllHidden
    }
#endif
}
