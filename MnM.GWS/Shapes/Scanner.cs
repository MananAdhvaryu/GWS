using System;
using System.Collections.Generic;
using System.Linq;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct Scanner : IScanner
        {
            #region CONSTRUCTORS
            public Scanner(IBuffer surface) : this()
            {
                Data = new List<IAPoint>();
                Bounds = Process(this, surface);
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public IList<IAPoint> Data { get; private set; }
            public IRectangleF Bounds { get; private set; }
            public string Name => "Scanner";
            public string ID { get; private set; }
            #endregion

            #region DRAW
            public void DrawTo(IBuffer  Writer, IBufferPen reader) 
            {
                foreach (var item in Data)
                    item.DrawTo(Writer, reader);
                if(Writer is IUpdateTracker)
                    (Writer as IUpdateTracker).PendingUpdates.Invalidate(Bounds.Expand());
            }
            #endregion

            #region CLONE
            public Scanner Clone()
            {
                var e = new Scanner();
                e.Bounds = Bounds;
                e.Data = Data.ToList();
                return e;
            }
            object ICloneable.Clone() => Clone();
            #endregion

            #region SCAN INTPTR SOURCE
            static unsafe IRectangleF Process(IScanner src, IBuffer surface)
            {
                IBufferPen pen = null;
                int? colorKey = null;
                int srcX, srcY, maxX, maxY;
                srcX = srcY = maxX = maxY = 0;
                src.Data.Clear();

                if (surface is IBufferBackground)
                    Factory.Get((surface as IBufferBackground).BackgroundPen, out pen, ObjType.Buffer);

                if (surface is ITransparent)
                    colorKey = (surface as ITransparent).ColorKey;

                int* b = (int*)surface.Pixels;
                var sw = surface.Width;
                int dy = 0;
                var bLen = surface.Length;
                var sh = surface.Height;
                if (pen == null)
                {
                    if (colorKey == null)
                    {
                        for (int y = 0; y <= sh; y++)
                        {
                            Scan(src, y, sw, b, dy, bLen, ref srcX, ref srcY, ref maxX, ref maxY);
                            dy += sw;
                        }
                    }
                    else
                    {
                        var k = colorKey.Value;

                        for (int y = 0; y <= sh; y++)
                        {
                            Scan(src, y, sw, b, dy, bLen, k, ref srcX, ref srcY, ref maxX, ref maxY);
                            dy += sw;
                        }
                    }
                }
                else
                {
                    if (colorKey == null)
                    {
                        for (int y = 0; y <= sh; y++)
                        {
                            Scan(src, y, sw, b, pen, dy, bLen, ref srcX, ref srcY, ref maxX, ref maxY);
                            dy += sw;
                        }
                    }
                    else
                    {
                        var k = colorKey.Value;
                        for (int y = 0; y <= sh; y++)
                        {
                            Scan(src, y, sw, b, pen, dy, bLen, k, ref srcX, ref srcY, ref maxX, ref maxY);
                            dy += sw;
                        }
                    }
                }
                return Factory.newRectangleF(srcX, srcY, maxX - srcX, maxY - srcY);
            }

            static unsafe void Scan(IScanner src, int y, int w, int* bkg, int dy, int blen, ref int srcX, ref int srcY, ref int maxX, ref int maxY)
            {
                for (int x = 0; x <= w; x++)
                {
                    var i = x + dy;
                    if (i > blen)
                        break;

                    if (bkg[i] == Geometry.TransparentColor)
                        continue;
                    else
                        Add(src, x, y, bkg[i].Alpha(), ref srcX, ref srcY, ref maxX, ref maxY);
                }
            }
            static unsafe void Scan(IScanner src, int y, int w, int* bkg, int dy, int blen, int colorKey, ref int srcX, ref int srcY, ref int maxX, ref int maxY)
            {
                for (int x = 0; x <= w; x++)
                {
                    var i = x + dy;
                    if (i > blen)
                        break;

                    if (bkg[i] == 0 || Colours.RGBEqual(bkg[i], colorKey))
                        continue;
                    Add(src, x, y, bkg[i].Alpha(), ref srcX, ref srcY, ref maxX, ref maxY);
                }
            }
            static unsafe void Scan(IScanner src, int y, int w, int* bkg, IBufferPen pen, int dy, int blen, ref int srcX, ref int srcY, ref int maxX, ref int maxY)
            {
                for (int x = 0; x <= w; x++)
                {
                    var i = x + dy;
                    if (i > blen)
                        break;
                    if (bkg[i] == Geometry.TransparentColor ||
                        bkg[i] == pen.ReadPixel(x, y, true))
                        continue;
                    Add(src, x, y, bkg[i].Alpha(), ref srcX, ref srcY, ref maxX, ref maxY);
                }
            }
            static unsafe void Scan(IScanner src, int y, int w, int* bkg, IBufferPen pen, int dy, int blen, int colorKey, ref int srcX, ref int srcY, ref int maxX, ref int maxY)
            {
                for (int x = 0; x <= w; x++)
                {
                    var i = x + dy;
                    if (i > blen)
                        break;

                    if (bkg[i] == Geometry.TransparentColor ||
                        Colours.RGBEqual(bkg[i], colorKey) ||
                        bkg[i] == pen.ReadPixel(x, y, true))
                        continue;
                    Add(src, x, y, bkg[i].Alpha(), ref srcX, ref srcY, ref maxX, ref maxY);
                }
            }
            static void Add(IScanner src, int x, int y, byte alpha, ref int srcX, ref int srcY, ref int maxX, ref int maxY)
            {
                float? a = alpha / 255f;
                if (a < Geometry.EPSILON)
                    return;
                if (a == 1f)
                    a = null;

                if (srcX == 0)
                    srcX = x;
                else if (srcX > x)
                    srcX = x;

                if (srcY == 0)
                    srcY = y;
                else if (srcY > y)
                    srcY = y;

                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
                src.Data.Add(Factory.newAPoint(x, y, a, true));
            }
            #endregion

            void IStoreable.AssignIDIfNone()
            {
                if (ID == null)
                    ID = Factory.NewID(Name);
            }
        }
#if AllHidden
    }
#endif
}