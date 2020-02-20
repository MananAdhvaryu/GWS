using System;
using System.Collections.Generic;
using System.Text;
using MnM.GWS.MathExtensions;
using static MnM.GWS.Implementation;

namespace MnM.GWS.ImageExtensions
{
    public static class ImageHelper
    {
        #region ROTATE - FLIP - SCALE - SHEAR
        /* The MIT License(MIT)
       Copyright(c) 2009-2015 Rene Schulte
       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files(the "Software"), to deal
        the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:

       The above copyright notice and this permission notice shall be included  all
       copies or substantial portions of the Software.

       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.*/
        public static unsafe IGraphics RotatedCopy(this IGraphics surface, Angle angle, bool crop = false)
        {
            bool isAngle = angle.Valid;
            // rotating clockwise, so it's negative relative to Cartesian quadrants
            float cnAngle = -Geometry.Radian * angle.Degree;

            // general iterators
            int i, j;
            // calculated indices  Cartesian coordinates 
            int x, y;
            float fDistance, fPolarAngle;
            // for use  neighboring indices  Cartesian coordinates
            int iFloorX, iCeilingX, iFloorY, iCeilingY;
            // calculated indices  Cartesian coordinates with trailing decimals
            float fTrueX, fTrueY;
            // for interpolation
            float fDeltaX, fDeltaY;

            int iCentreX, iCentreY;
            int iDestCentreX, iDestCentreY;
            int iWidth, iHeight, newWidth, newHeight;

            iWidth = surface.Width;
            iHeight = surface.Height;

            if (crop)
            {
                newWidth = iWidth;
                newHeight = iHeight;
            }
            else
            {
                var rad = angle.Degree / Geometry.RadianInv;
                var nAngle = new Angle(rad);

                newWidth = (int)Math.Ceiling(Math.Abs(nAngle.Sin * iHeight) + Math.Abs(nAngle.Cos * iWidth));
                newHeight = (int)Math.Ceiling(Math.Abs(nAngle.Sin * iWidth) + Math.Abs(nAngle.Cos * iHeight));
            }


            iCentreX = iWidth / 2;
            iCentreY = iHeight / 2;

            iDestCentreX = newWidth / 2;
            iDestCentreY = newHeight / 2;

            var result = surface.ToPen(newWidth, newHeight) as IGraphics;
            if (!angle.Valid)
                return result;

            var newp = (int*)result.Pixels;
            var oldp = (int*)surface.Pixels;
            var oldw = surface.Width;

            // assigning pixels of destination image from source image
            // with bilinear interpolation

            for (i = 0; i < newHeight; ++i)
            {
                for (j = 0; j < newWidth; ++j)
                {
                    // convert raster to Cartesian
                    x = j - iDestCentreX;
                    y = iDestCentreY - i;

                    // convert Cartesian to polar
                    fDistance = (float)Math.Sqrt(x * x + y * y);
                    if (x == 0)
                    {
                        if (y == 0)
                        {
                            // center of image, no rotation needed
                            newp[i * newWidth + j] = oldp[iCentreY * oldw + iCentreX];
                            continue;
                        }
                        fPolarAngle = Geometry.PI;
                        fPolarAngle *= y < 0 ? 1.5f : .5f;
                    }
                    else
                        fPolarAngle = (float)Math.Atan2(y, x);

                    // the crucial rotation part
                    // "reverse" rotate, so minus instead of plus
                    fPolarAngle -= cnAngle;

                    // convert polar to Cartesian
                    fTrueX = (float)(fDistance * Math.Cos(fPolarAngle));
                    fTrueY = (float)(fDistance * Math.Sin(fPolarAngle));

                    // convert Cartesian to raster
                    fTrueX = fTrueX + iCentreX;
                    fTrueY = iCentreY - fTrueY;

                    iFloorX = (int)(Math.Floor(fTrueX));
                    iFloorY = (int)(Math.Floor(fTrueY));
                    iCeilingX = (int)(Math.Ceiling(fTrueX));
                    iCeilingY = (int)(Math.Ceiling(fTrueY));

                    // check bounds
                    if (iFloorX < 0 || iCeilingX < 0 || iFloorX >= iWidth || iCeilingX >= iWidth || iFloorY < 0 ||
                        iCeilingY < 0 || iFloorY >= iHeight || iCeilingY >= iHeight) continue;

                    fDeltaX = fTrueX - iFloorX;
                    fDeltaY = fTrueY - iFloorY;

                    var clrTopLeft = oldp[iFloorY * oldw + iFloorX];
                    var clrTopRight = oldp[iFloorY * oldw + iCeilingX];
                    var clrBottomLeft = oldp[iCeilingY * oldw + iFloorX];
                    var clrBottomRight = oldp[iCeilingY * oldw + iCeilingX];

                    newp[i * newWidth + j] = Colours.Blend(clrTopLeft, clrTopRight, clrBottomLeft, clrBottomRight, fDeltaX, fDeltaY);

                }
            }
            return result;
        }
        public static unsafe IGraphics FlippedCopy(this IGraphics surface, RendererFlip flipMode)
        {
            var w = surface.Width;
            var h = surface.Height;
            var p = (int*)surface.Pixels;
            var i = 0;
            IGraphics result = null;

            if (flipMode == RendererFlip.Horizontal)
            {
                result = surface.ToPen(w, h) as IGraphics;
                var rp = (int*)result.Pixels;
                for (var y = h - 1; y >= 0; y--)
                {
                    for (var x = 0; x < w; x++)
                    {
                        var srcInd = y * w + x;
                        rp[i] = p[srcInd];
                        i++;
                    }
                }
            }
            else if (flipMode == RendererFlip.Vertical)
            {
                result = surface.ToPen(w, h) as IGraphics;
                var rp = (int*)result.Pixels;
                for (var y = 0; y < h; y++)
                {
                    for (var x = w - 1; x >= 0; x--)
                    {
                        var srcInd = y * w + x;
                        rp[i] = p[srcInd];
                        i++;
                    }
                }
            }
            return result;
        }
        public static IGraphics ShearedCopy(this IGraphics surface, float shear)
        {
            IGraphics image;

            image = (IGraphics)surface.ToPen(surface.Width, surface.Height);

            int width = surface.Width, height = surface.Height;
            for (int y = 0; y <= height - 1; y++)
            {
                var skew = shear * (y);
                //var skewi = (skew);
                //var skewf = skew.Fraction();
                int pixel = Geometry.TransparentColor;
                for (int x = 0; x <= width - 1; x++)
                {
                    pixel = surface.ReadPixel(width - x, y, true);
                    image.WritePixel(width - x + skew, y, true, pixel, true);
                }
                image.WritePixel(skew, y, true, pixel, true);
            }
            return image;
        }
        public static unsafe IGraphics ScalledCopy(this IGraphics source, float? widthScale = null, float? heightScale = null)
        {
            if (widthScale == null && heightScale == null)
                return source as IGraphics;

            var xScale = widthScale ?? 1;
            var yScale = heightScale ?? 1;

            var newWidth = (source.Width * xScale).Round();
            var newHeight = (source.Height * yScale).Round();

            IGraphics surface = source.ToPen(newWidth, newHeight) as IGraphics;

            int* oldP = (int*)source.Pixels;
            int* newP = (int*)surface.Pixels;

            var i = newWidth;
            var j = 0;

            for (var y = 0; y < newHeight; y++)
            {
                for (var x = 0; x < newWidth; x++)
                {
                    newP[j++] = GetPixel(source.Width, oldP, x / xScale, y / yScale);
                }
                j = i;
                if (j + newWidth >= surface.Length)
                    break;
                i += newWidth;
            }
            return surface;
        }
        static unsafe int GetPixel(int penWidth, int* penData, float x, float y)
        {
            var left = (int)x;
            var right = x.Ceiling();
            var top = (int)y;
            var bottom = y.Ceiling();

            var i = left + top * penWidth;

            if (left - x == 0 && y - top == 0)
                return penData[i];

            var j = i + 1;
            var k = i + penWidth;
            var l = i + 1 + penWidth;

            var lt = penData[i];
            var rt = penData[j];
            var lb = penData[k];
            var rb = penData[l];
            return Colours.Blend(lt, rt, lb, rb, x - left, y - top);
        }

        /* The MIT License(MIT)
       Copyright(c) 2009-2015 Rene Schulte
       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files(the "Software"), to deal
        the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:

       The above copyright notice and this permission notice shall be included  all
       copies or substantial portions of the Software.

       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.*/
        static unsafe int[] Scale(int* pixels, int widthSource, int heightSource, int width, int height, Interpolation interpolation = Interpolation.BiLinear)
        {
            var pd = new int[width * height];
            var xs = (float)widthSource / width;
            var ys = (float)heightSource / height;

            float fracx, fracy, ifracx, ifracy, sx, sy, l0, l1, rf, gf, bf;
            int c, x0, x1, y0, y1;
            byte c1a, c1r, c1g, c1b, c2a, c2r, c2g, c2b, c3a, c3r, c3g, c3b, c4a, c4r, c4g, c4b;
            byte a, r, g, b;

            // Nearest Neighbor
            if (interpolation == Interpolation.NearestNeighbor)
            {
                var srcIdx = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        sx = x * xs;
                        sy = y * ys;
                        x0 = (int)sx;
                        y0 = (int)sy;

                        pd[srcIdx++] = pixels[y0 * widthSource + x0];
                    }
                }
            }

            // Bilinear
            else if (interpolation == Interpolation.BiLinear)
            {
                var srcIdx = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        sx = x * xs;
                        sy = y * ys;
                        x0 = (int)sx;
                        y0 = (int)sy;

                        // Calculate coordinates of the 4 interpolation points
                        fracx = sx - x0;
                        fracy = sy - y0;
                        ifracx = 1f - fracx;
                        ifracy = 1f - fracy;
                        x1 = x0 + 1;
                        if (x1 >= widthSource)
                        {
                            x1 = x0;
                        }
                        y1 = y0 + 1;
                        if (y1 >= heightSource)
                        {
                            y1 = y0;
                        }


                        // Read source color
                        c = pixels[y0 * widthSource + x0];
                        c1a = (byte)(c >> 24);
                        c1r = (byte)(c >> 16);
                        c1g = (byte)(c >> 8);
                        c1b = (byte)(c);

                        c = pixels[y0 * widthSource + x1];
                        c2a = (byte)(c >> 24);
                        c2r = (byte)(c >> 16);
                        c2g = (byte)(c >> 8);
                        c2b = (byte)(c);

                        c = pixels[y1 * widthSource + x0];
                        c3a = (byte)(c >> 24);
                        c3r = (byte)(c >> 16);
                        c3g = (byte)(c >> 8);
                        c3b = (byte)(c);

                        c = pixels[y1 * widthSource + x1];
                        c4a = (byte)(c >> 24);
                        c4r = (byte)(c >> 16);
                        c4g = (byte)(c >> 8);
                        c4b = (byte)(c);


                        // Calculate colors
                        // Alpha
                        l0 = ifracx * c1a + fracx * c2a;
                        l1 = ifracx * c3a + fracx * c4a;
                        a = (byte)(ifracy * l0 + fracy * l1);

                        // Red
                        l0 = ifracx * c1r + fracx * c2r;
                        l1 = ifracx * c3r + fracx * c4r;
                        rf = ifracy * l0 + fracy * l1;

                        // Green
                        l0 = ifracx * c1g + fracx * c2g;
                        l1 = ifracx * c3g + fracx * c4g;
                        gf = ifracy * l0 + fracy * l1;

                        // Blue
                        l0 = ifracx * c1b + fracx * c2b;
                        l1 = ifracx * c3b + fracx * c4b;
                        bf = ifracy * l0 + fracy * l1;

                        // Cast to byte
                        r = (byte)rf;
                        g = (byte)gf;
                        b = (byte)bf;

                        // Write destination
                        pd[srcIdx++] = (a << 24) | (r << 16) | (g << 8) | b;
                    }
                }
            }
            return pd;
        }
        #endregion
    }
}
