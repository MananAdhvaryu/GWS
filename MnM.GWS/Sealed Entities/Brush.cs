using MnM.GWS.MathExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
    public
#endif
       class Brush : IBrush
        {
            #region VARIABLES
            int[] Data;
            IFillStyle style;
            #endregion

            #region CONSTRUCTORS
            Brush() { }
            void Initialize( IFillStyle style,  int width,  int height)
            {
                ID = (style?? FillStyles.Black).GetBrushKey(width, height);
                Width = width;
                Height = height;
                this.style = style;
                Length = GetSize();
                Data = Fill();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IBrush CreateInstance( IFillStyle style,  int width,  int height)
            {
                var ID = (style ?? FillStyles.Black).GetBrushKey(width, height);
                if (!Factory.Get(ID, out IBrush b, ObjType.Buffer))
                {
                    var brush = new Brush();
                    brush.Initialize(style, width, height);
                    return brush;
                }
                return b;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IBrush CreateInstance( IFillStyle style,  IPoint settings,  int width,  int height)
            {
                var ID = (style?? FillStyles.Black).GetBrushKey(width, height);
                Brush brush;
                if (!Factory.Get(ID, out brush, ObjType.Buffer))
                {
                    brush = new Brush();
                    brush.Initialize(style, width, height);
                    return brush;
                }
                return brush;
            }
            #endregion

            #region PROPERTIES
            IFillStyle Style  => style ?? FillStyles.Black;
            public int Length { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public string ID { get; private set; }
            public bool IsDisposed { get; private set; }
            public unsafe int this[int index] => pixels[index];
            public Gradient Gradient => Style.Gradient;
            unsafe int* pixels
            {
                get
                {
                    fixed(int* pixels = Data)
                    {
                        return pixels;
                    }
                }
            }
            public unsafe IntPtr Pixels => (IntPtr)pixels;
            #endregion

            #region INDEX OF
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int IndexOf(int val, int axis,  bool horizontal = true)
            {
                if (IsDisposed)
                {
                    throw new System.Exception("Object is disposed!");
                }

                if (Length == 0)
                    return 0;
                int x = horizontal ? val : axis;
                int y = horizontal ? axis : val;

                Renderer.CorrectXY( Entity.BufferPen, ref x, ref y);

                int i;
                switch (Style.Gradient)
                {
                    case Gradient.Central:
                    case Gradient.CentralD:
                        return (x + y * Width);
                    case Gradient.None:
                    case Gradient.Horizontal:
                    case Gradient.HorizontalCentral:
                    default:
                        i = x;
                        break;
                    case Gradient.HorizontalSwitch:
                        i = x;
                        if (y > Height / 2)
                            i = Width + x % Width;
                        break;
                    case Gradient.Vertical:
                    case Gradient.VerticalCentral:
                        i = y;
                        break;
                    case Gradient.ForwardDiagonal:
                    case Gradient.DiagonalCentral:
                        i = x + y;
                        break;
                    case Gradient.BackwardDiagonal:
                        y = Geometry.FlipVertical(y, Height);
                        i = (x + y);
                        break;
                }
                return (i % Length);
            }
            public void XYOf( int index, out int x, out int y)
            {
                y = index / Width;
                x = index - y * Width;
            }
            #endregion

            #region COPY TO
            public unsafe IRectangle CopyTo( IRectangle copyRc, IntPtr dest,  int destLen,  int destW,  int destX,  int destY)
            {
                var dst = (int*)dest;
                BlockCopyPtr action = (k, si, di, w, i) => CopyMemory((int*)k, si, dst, di, w);
                var dstx = destX;
                var dsty = destY;

                if (dstx < 0)
                    dstx = 0;
                if (dsty < 0)
                    dsty = 0;

                var destIndex = dstx + dsty * destW;
                var y = copyRc.Y;
                var b = y + copyRc.Height;

                if (y < 0)
                {
                    b += y;
                    y = 0;
                }
                var x = copyRc.X;
                var r = x + copyRc.Width;

                for (int j = y; j <= b; j++)
                {
                    Renderer.ReadLine(this, x, r, j, true, out IntPtr k, out int srcIndex, out int copyLen);
                    if (copyLen <= 0)
                        break;

                    if (destIndex + copyLen > destLen)
                        copyLen -= (destIndex + copyLen - destLen);
                    if (copyLen < 0)
                        break;
                    action(k, srcIndex, destIndex, copyLen, j);
                    destIndex += destW;
                }
                return Factory.RectangleFromLTRB(destX, destY, r, b);
            }
            #endregion

            #region TO PEN
            public IBufferPen ToPen(int? width = null, int? height = null)
            {
                var brush = CreateInstance(style, width ?? Width, height ?? Height);
                return brush;
            }
            #endregion

            #region FILL
            int GetSize()
            {
                var w = Width;
                var h = Height;
                ++w;
                ++h;
                int size;
                switch (Style.Gradient)
                {
                    case Gradient.None:
                    case Gradient.Horizontal:
                    case Gradient.HorizontalCentral:
                    default:
                        size = w;
                        break;
                    case Gradient.Vertical:
                    case Gradient.VerticalCentral:
                        size = h;
                        break;
                    case Gradient.ForwardDiagonal:
                    case Gradient.BackwardDiagonal:
                    case Gradient.DiagonalCentral:
                        size = MathHelper.Avg(w, h) * 2;
                        break;
                    case Gradient.HorizontalSwitch:
                        size = w * 2;
                        break;
                    case Gradient.Central:
                    case Gradient.CentralD:
                        size = (w * h);
                        break;
                }
                return Math.Abs(size);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            unsafe int[] Fill()
            {
                int bLen = Length;
                if (bLen == 0)
                    bLen = GetSize();

                var w = Width;
                var h = Height;
                var data = new int[bLen];
                var destLen = data.Length;

                int j = 0;
                var CX = w / 2f;
                var CY = h / 2f;
                var dbw = w * 2;
                var dbh = h * 2;
                FillAction action;
                FillAction action1;
                IColorEnumerator enumerator;

                LineDraw draw = LineDraw.NonAA;

                fixed (int* d = data)
                {
                    switch (Style.Gradient)
                    {
                        #region None
                        case Gradient.None:
                            data = Style[0].Repeat(bLen);
                            break;
                        #endregion

                        #region Horizontal, Vertical, ForwardDiagonal, BackwardDiagonal, default
                        case Gradient.Horizontal:
                        case Gradient.Vertical:
                        case Gradient.ForwardDiagonal:
                        case Gradient.BackwardDiagonal:

                        default:
                            data = Style.GetEnumerator().ToArray(0, data.Length, data.Length);
                            break;
                        #endregion

                        #region HorizontalCentral, VerticalCentral, DiagonalCentral, Followshape, HorizontalSwitch
                        case Gradient.HorizontalCentral:
                        case Gradient.VerticalCentral:
                        case Gradient.DiagonalCentral:
                        case Gradient.HorizontalSwitch:
                            data = Style.GetEnumerator().ToArray(0, data.Length, Length, true);
                            break;
                        #endregion

                        #region Central
                        case Gradient.Central:
                            var pixels = new List<Point>(500);
                            action = Renderer.CreateFillAction(pixels);
                            enumerator = Style.GetEnumerator();

                            Renderer.ProcessLine(Factory.newLine(0, 0, 0, h), draw, action);//l
                            Renderer.ProcessLine(Factory.newLine(w, 0, w, h), draw, action); //r
                            enumerator.MaxIteration = pixels.Count;

                            action1 = Renderer.CreateFillAction(enumerator, d, data.Length, w);
                            enumerator.Freeze = true;
                            foreach (var line in pixels.Select(x => Factory.newLine(x.X, x.Y, CX, CY)))
                            {
                                Renderer.ProcessLine(line, LineDraw.NonAA, action1);
                                enumerator.MoveNext(true);
                            }
                            pixels.Clear();

                            Renderer.ProcessLine(Factory.newLine(0, 0, w, 0), draw, action); //t
                            Renderer.ProcessLine(Factory.newLine(0, h, w, h), draw, action); //b
                            enumerator.Reset();
                            enumerator.MaxIteration = pixels.Count;

                            foreach (var line in pixels.Select(x => Factory.newLine(x.X, x.Y, CX, CY)))
                            {
                                Renderer.ProcessLine(line, LineDraw.NonAA, action1);
                                enumerator.MoveNext(true);
                            }
                            break;
                        #endregion

                        #region CentralD
                        case Gradient.CentralD:
                            enumerator = Style.GetEnumerator();
                            action1 = Renderer.CreateFillAction(enumerator, d, data.Length, w);
                            enumerator.MaxIteration = h;
                            enumerator.Freeze = true;

                            for (int i = 0; i <= h; i++)
                            {
                                Renderer.ProcessLine(Factory.newLine(0, i, CX, CY), LineDraw.NonAA, action1);
                                enumerator.MoveNext(true);
                                if (enumerator.CurrentPosition == dbh)
                                    continue;
                                Renderer.ProcessLine(Factory.newLine(w, i, CX, CY), LineDraw.NonAA, action1);
                            }
                            enumerator.MaxIteration = w;
                            enumerator.Reset();
                            for (int i = 0; i <= w; i++)
                            {
                                Renderer.ProcessLine(Factory.newLine(i, 0, CX, CY), LineDraw.NonAA, action1);
                                enumerator.MoveNext(true);
                                if (enumerator.CurrentPosition == dbw)
                                    continue;

                                Renderer.ProcessLine(Factory.newLine(i, h, CX, CY), LineDraw.NonAA, action1);
                            }
                            break;
                            #endregion
                    }
                }
                return data;
            }

            public void Dispose()
            {
                IsDisposed = true;
                Data = null;
            }
            void IStoreable.AssignIDIfNone()
            {
                if (ID == null)
                    ID = (style ?? FillStyles.Black).GetBrushKey(Width, Height);
            }
            #endregion
        }
#if AllHidden
    }
#endif
}
