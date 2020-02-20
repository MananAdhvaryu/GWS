
using MnM.GWS.MathExtensions;
using System;
using System.Runtime.InteropServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RGBA : IEquatable<RGBA>, IRGBA
    {
        const string toStr = "r:{0}, g:{1}, b:{2}, a:{3}";

        #region variables
        internal static readonly RGBA Empty = new RGBA();
        #endregion

        #region constructors
        public RGBA(byte r, byte g, byte b, byte a = 255) : this()
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public RGBA(int r, int g, int b, int a = 255) :
            this((byte)r, (byte)g, (byte)b, (byte)a)
        { }
        public RGBA(int value)
        {
            Colours.ToRGBA(value, out int r, out int g, out int b, out int a);
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
            A = (byte)a;
        }
        public RGBA(int value, byte externalAlpha)
        {
            Colours.ToRGBA(value, out int r, out int g, out int b, out int a, externalAlpha);
            R = (byte) r;
            G = (byte)g;
            B = (byte)b;
            A = (byte)a;
        }
        public RGBA(float r, float g, float b, float a = 255) : 
            this(r.Round(), g.Round(), b.Round(), a.Round())
        { }
        public RGBA(double r, double g, double b, double a = 255) :
            this(r.Round(), g.Round(), b.Round(), a.Round())
        { }
        public RGBA(IRGBA c, byte newAlpha) :
            this(c.R, c.G, c.B, newAlpha)
        { }
        public RGBA(IRGBA c, float newAlpha) :
            this(c.R, c.G, c.B, (byte)(newAlpha * 255))
        { }
        #endregion

        #region properties
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }
        public byte A { get; private set; }
        public int Value => Colours.ToColor(R, G, B, A);
        #endregion

        #region equality
        public override bool Equals(object other)
        {
            if (!(other is RGBA))
                return false;

            var c = (RGBA)other;
            return c.R == R && c.G == G && c.B == B && c.A == A;
        }
        public bool Equals(RGBA c)
        {
            return c.R == R && c.G == G && c.B == B && c.A == A;
        }
        public override int GetHashCode()
        {
            return Colours.ToColor(R, G, B, A);
        }
        public bool RGBEqual(RGBA other)
        {
            if (other.A == 0 && A == 0)
                return true;
            return R == other.R && G == other.G && B == other.B;
        }
        #endregion

        #region conversion operators
        public static implicit operator int(RGBA rgba)=>
            Colours.ToColor(rgba.R, rgba.G, rgba.B, rgba.A);
        public static implicit operator RGBA(int value)
        {
            Colours.ToRGBA(value, out int r, out int g, out int b, out int a);
            return new RGBA(r, g, b, a);
        }
        #endregion

        #region math operations
        public static RGBA operator +(RGBA A, IRGBA B)
        {
            var r = A.R + B.R;
            var g = A.G + B.G;
            var b = A.B + B.B;
            var a = A.A + B.A;
            return new RGBA(r, g, b, a);
        }
        public static RGBA operator +(RGBA A, float B)
        {
            var r = A.R + B;
            var g = A.G + B;
            var b = A.B + B;
            var a = A.A + B;
            return new RGBA(r, g, b, a);
        }
        public static RGBA operator +(float A, RGBA B)
        {
            return B + A;
        }

        public static RGBA operator -(RGBA A, IRGBA B)
        {
            var r = A.R - B.R;
            var g = A.G - B.G;
            var b = A.B - B.B;
            var a = A.A - B.A;
            return new RGBA(r, g, b, a);
        }
        public static RGBA operator -(RGBA A, float B)
        {
            return A + (-B);
        }
        public static RGBA operator -(float A, RGBA B)
        {
            return B+(-A);
        }

        public static RGBA operator *(RGBA A, IRGBA B)
        {
            var r = A.R * B.R;
            var g = A.G * B.G;
            var b = A.B * B.B;
            var a = A.A * B.A;
            return new RGBA(r, g, b, a);
        }
        public static RGBA operator *(RGBA A, float B)
        {
            var r = A.R * B;
            var g = A.G * B;
            var b = A.B * B;
            var a = A.A * B;
            return new RGBA(r, g, b, a);
        }
        public static RGBA operator *(float A, RGBA B)
        {
            return B*(A);
        }

        public static RGBA operator /(RGBA A, IRGBA B)
        {
            var r = A.R / B.R;
            var g = A.G / B.G;
            var b = A.B / B.B;
            var a = A.A / B.A;
            return new RGBA(r, g, b, a);
        }
        public static RGBA operator /(RGBA A, float B)
        {
            var r = A.R / B;
            var g = A.G / B;
            var b = A.B / B;
            var a = A.A / B;
            return new RGBA(r, g, b, a);
        }
        public static RGBA Divide(float A, RGBA B)
        {
            return B/(A);
        }
        #endregion

        #region hue, brightness, saturation
        public float Hue()
        {
            if (R == G && G == B)
                return 0; // 0 makes as good an UNDEFINED value as any

            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;
        }
        public float Brightness()
        {
            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }
        public float Saturation()
        {
            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;
            float l, s = 0;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            // if max == min, then there is no color and
            // the saturation is zero.
            //
            if (max != min)
            {
                l = (max + min) / 2;

                if (l <= .5)
                {
                    s = (max - min) / (max + min);
                }
                else
                {
                    s = (max - min) / (2 - max - min);
                }
            }
            return s;
        }
        #endregion

        #region change
        public void Change(byte? alpha)
        {
            if (alpha == null || alpha == 255)
                return;

            if (alpha == 0 || alpha == A)
                return;
            this = new RGBA(R, G, B, alpha.Value);
        }
        #endregion

        #region RGB equal
        public bool RGBEqual(IRGBA other)
        {
            if (other.A == 0 && A == 0)
                return true;
            return R == other.R && G == other.G && B == other.B;
        }
        public bool RGBAEqual(IRGBA other)
        {
            if (other.A == 0 && A == 0)
                return true;
            return R == other.R && G == other.G && B == other.B && A==other.A;
        }
        #endregion

        #region lerp
        public RGBA Lerp(IRGBA to, float amount)
        {
            // start colours as lerp-able floats
            float sr = R, sg = G, sb = B;

            // end colours as lerp-able floats
            float er = to.R, eg = to.G, eb = to.B;

            // lerp the colours to get the difference
            byte r = (byte)Lerp(sr, er, amount),
                 g = (byte)Lerp(sg, eg, amount),
                 b = (byte)Lerp(sb, eb, amount);

            // return the new colour
            return new RGBA(r, g, b, (byte)255);
        }
        public RGBA Lerp(Colour color, float amount)
        {
            RGBA to = color;
            return Lerp(to, amount);
        }

        static float Lerp(float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }
        #endregion

        #region light/ dark / invert
        public RGBA Lighten(float percent)
        {
            return Lerp(Colour.White, percent);
        }
        public RGBA Darken(float percent)
        {
            return Lerp(Colour.Black, percent);
        }
        public RGBA Invert()
        {
            byte r, g, b, a;
            RGBA w = Colour.White;

            if (w.R - R < byte.MinValue)
                r = byte.MinValue;
            else
                r = (byte)(w.R - R);
            if (w.G - G < byte.MinValue)
                g = byte.MinValue;
            else
                g = (byte)(w.G - G);
            if (w.B - B < byte.MinValue)
                b = byte.MinValue;
            else
                b = (byte)(w.B - B);
            if (w.A - A < byte.MinValue)
                a = byte.MinValue;
            else
                a = (byte)(w.A - A);
            return new RGBA(r, g, b, a);
        }
        #endregion

        #region color distance
        public RGBA GetGradient(IRGBA second, float k)
        {
            float r, g, b, a;
            r = (float)(R + (second.R - R) * k);
            g = (float)(G + (second.G - G) * k);
            b = (float)(B + (second.B - B) * k);
            a = (float)(A + (second.A - A) * k);
            return new RGBA(r, g, b, a);
        }
        public RGBA GetTweenColor(IRGBA second, float RatioOf2)
        {
            if (RatioOf2 <= 0)
                return this;

            if (RatioOf2 >= 1f)
                return new RGBA(second.R,second.G,second.B,second.A);

            // figure out how much of each color we should be.
            float RatioOf1 = 1f - RatioOf2;
            return new RGBA(
                R * RatioOf1 + second.R * RatioOf2,
                G * RatioOf1 + second.G * RatioOf2,
                B * RatioOf1 + second.B * RatioOf2);
        }
        public float SumOfDistances(IRGBA second)
        {
            float dist = Math.Abs(R - second.R) +
                Math.Abs(G - second.G) + Math.Abs(B - second.B);
            return dist;
        }
        #endregion

        #region BLEND - MIX
        public RGBA Blend(IRGBA c2, float e0 = 0)
        {
            return Colours.Blend(this, c2.Value, e0);
        }
        public RGBA Mix(IRGBA c2, byte? alpha )
        {
            RGBA color2 = c2.Value;
            color2.Change(alpha);
            var color = Blend(this, color2);
            color.Change((byte)(color2.A + ((color2.A * (255 - color2.A)) >> 8)));
            return color;
        }
        #endregion

        #region REPEAT
        public int[] Repeat(int repeat = 0)
        {
            if (repeat <= 0)
                return new int[] { this };

            var data = new int[repeat];
            for (int i = 0; i < repeat; i++)
                data[i] = this;
            return data;
        }
        #endregion

        #region Rotate 
        static int Clamp(double v)
        {
            return System.Convert.ToInt32(Math.Max(0F, Math.Min(v + 0.5, 255.0F)));
        }
        public RGBA Rotate(Angle angle)
        {
            new Matrix3x2();

            float[,] selfMatrix = new float[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            float sqrtOneThirdTimesSin = (float)Math.Sqrt(1f / 3f) * angle.Sin;
            float oneThirdTimesOneSubCos = 1f / 3f * (1f - angle.Cos);
            selfMatrix[0, 0] = angle.Cos + (1f - angle.Cos) / 3f;
            selfMatrix[0, 1] = oneThirdTimesOneSubCos - sqrtOneThirdTimesSin;
            selfMatrix[0, 2] = oneThirdTimesOneSubCos + sqrtOneThirdTimesSin;
            selfMatrix[1, 0] = selfMatrix[0, 2];
            selfMatrix[1, 1] = angle.Cos + oneThirdTimesOneSubCos;
            selfMatrix[1, 2] = selfMatrix[0, 1];
            selfMatrix[2, 0] = selfMatrix[0, 1];
            selfMatrix[2, 1] = selfMatrix[0, 2];
            selfMatrix[2, 2] = angle.Cos + oneThirdTimesOneSubCos;
            float rx = R * selfMatrix[0, 0] + G * selfMatrix[0, 1] + B * selfMatrix[0, 2];
            float gx = R * selfMatrix[1, 0] + G * selfMatrix[1, 1] + B * selfMatrix[1, 2];
            float bx = R * selfMatrix[2, 0] + G * selfMatrix[2, 1] + B * selfMatrix[2, 2];
            return new RGBA(rx, gx, bx);
            //rgb[0] = ClampIt(rx);
            //rgb[1] = ClampIt(gx);
            //rgb[2] = ClampIt(bx);
        }

        #endregion

        public override string ToString() =>
            string.Format(toStr, R, G, B, A);

        #region interface

        bool IEquatable<IRGBA>.Equals(IRGBA c) =>
            c.R == R && c.G == G && c.B == B && c.A == A;
        IBufferPen IPenContext.ToPen(int? width, int? height)
        {
            var w = width ?? 1;
            var h = height ?? 1;
            return Factory.newPen(Colours.ToColor(R, G, B, A));
        }
        #endregion
    }
}
