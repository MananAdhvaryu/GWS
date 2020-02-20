using MnM.GWS.MathExtensions;

namespace MnM.GWS
{
    public static partial class Colours
    {
        static int[] knownColors = new[]
{
             16777215,
             16777216,
             16842752,
             16908288,
             16973824,
             17039360,
             17104896,
             17170432,
             17235968,
             17301504,
             17367040,
             17432576,
             17498112,
             17563648,
             17629184,
             17694720,
             17760256,
             17825792,
             17891328,
             17956864,
             18022400,
             18087936,
             18153472,
             18219008,
             18284544,
             18350080,
             18415616,
             18481152,
             18546688,
             18612224,
             18677760,
             18743296,
             18808832,
             18874368,
             -16777216,
             -8388608,
             -7667712,
             -3342336,
             -65536,
             -16751616,
             -16744448,
             -8355840,
             -7632128,
             -16640,
             -3027456,
             -6620672,
             -16711936,
             -8388864,
             -256,
             -256,
             -9430759,
             -28642,
             -5590496,
             -14513374,
             -11039954,
             -11579601,
             -13447886,
             -9325764,
             -3088320,
             -2004671,
             -4947386,
             -7652024,
             -3354296,
             -8257461,
             -13669547,
             -6250913,
             -1206940,
             -5583514,
             -9868951,
             -3319190,
             -14446997,
             -7307152,
             -6715273,
             -1152901,
             -16712580,
             -16711809,
             -2818177,
             -16777088,
             -8388480,
             -16744320,
             -8355712,
             -1323385,
             -340345,
             -1954934,
             -16777077,
             -7667573,
             -15514229,
             -7619441,
             -7278960,
             -2396013,
             -2948972,
             -6751336,
             -3394919,
             -13447782,
             -13806944,
             -14013787,
             -5658199,
             -1648467,
             -13631571,
             -1118545,
             -2177872,
             -1646416,
             -14540110,
             -16021832,
             -2927174,
             -7368772,
             -9717827,
             -4144960,
             -8055353,
             -10724147,
             -12614195,
             -14784046,
             -7555886,
             -2894893,
             -2572328,
             -2723622,
             -14637606,
             -7114533,
             -12839716,
             -2302756,
             -2252579,
             -7882530,
             -32,
             -334106,
             -8743191,
             -1146130,
             -5576466,
             -8355600,
             -7543056,
             -1808,
             -983056,
             -16,
             -10443532,
             -4989195,
             -2296331,
             -657931,
             -327691,
             -1800,
             -9273094,
             -2626566,
             -1642246,
             -2950406,
             -1640963,
             -16776961,
             -65281,
             -65281,
             -7138049,
             -16759297,
             -12098561,
             -4953601,
             -11501569,
             -16741121,
             -8740609,
             -16734721,
             -4081921,
             -3424001,
             -16721921,
             -4596993,
             -5382401,
             -4856577,
             -3873537,
             -1972993,
             -3281921,
             -2756609,
             -659201,
             -1116673,
             -2295553,
             -3278081,
             -984321,
             -328961,
             -16711681,
             -2031617,
             -983041,
             -1,
            };

        #region CONSTRUCTORS
        static Colours()
        {
            RShift = 16;
            GShift = 8;
            BShift = 0;
            AShift = 24;
            Colour.Reset();
        }
        #endregion

        #region RGBA COLOR SCHEME
        internal static int RShift { get; private set; }
        internal static int GShift { get; private set; }
        internal static int BShift { get; private set; }
        internal static int AShift { get; private set; }
        public static void ChangeScheme(int? rShift, int? gShift, int? bShift, int? aShift)
        {
            RShift = rShift ?? RShift;
            GShift = gShift ?? GShift;
            BShift = bShift ?? BShift;
            AShift = aShift ?? AShift;
            Colour.Reset();
            FillStyles.Initialize();
        }
        #endregion

        #region COLOR LIST
        public static int[] GetKnownColors() =>
            knownColors;
        #endregion

        #region NEW COLOR FROM ARRAY
        unsafe static int getColor(int* src, int srcIndex, int srcLen, byte? externalAlpha = null)
        {
            if (srcIndex < 0)
                srcIndex = 0;

            if (srcIndex <= srcLen)
            {
                if (externalAlpha == null)
                    return src[srcIndex];
                return src[srcIndex].Change(externalAlpha);
            }
            return 0;
        }
        unsafe static int getColor(int* src, ref int srcIndex, int srcLen, byte? externalAlpha = null)
        {
            if (srcIndex < 0)
                srcIndex = 0;

            if (srcIndex <= srcLen)
            {
                var c = externalAlpha == null ? src[srcIndex] : src[srcIndex].Change(externalAlpha);
                srcIndex++;
                return c;
            }
            return 0;
        }
        #endregion

        #region INT TO RGBA
        public static int Change(this int c, byte? alpha)
        {
            if (alpha == null || alpha == 255)
                return c;

            if (alpha == c.Alpha())
                return c;
            ToRGB(c, out int r, out int g, out int b);
            return ToColor(r, g, b, alpha.Value);
        }
        public static int ToColor(int r, int g, int b, int a)
        {
            return ((byte)a << AShift)
                 | ((byte)((r) & 0xFF) << RShift)
                 | ((byte)((g) & 0xFF) << GShift)
                 | ((byte)((b) & 0xFF) << BShift);
        }
        public static int ToColor(int r, int g, int b)
        {
            return ((byte)255 << AShift)
                 | ((byte)((r) & 0xFF) << RShift)
                 | ((byte)((g) & 0xFF) << GShift)
                 | ((byte)((b) & 0xFF) << BShift);
        }
        public static int ToColor(float r, float g, float b, float a)
        {
            return ((byte)a.Round() << AShift)
                 | ((byte)(r.Round() & 0xFF) << RShift)
                 | ((byte)(g.Round() & 0xFF) << GShift)
                 | ((byte)(b.Round() & 0xFF) << BShift);
        }
        public static int ToColor(float r, float g, float b)
        {
            return ((byte)255 << AShift)
                 | ((byte)(r.Round() & 0xFF) << RShift)
                 | ((byte)(g.Round() & 0xFF) << GShift)
                 | ((byte)(b.Round() & 0xFF) << BShift);
        }

        public static void ToRGB(this int value, out int r, out int g, out int b)
        {
            r = (byte)((value >> RShift) & 0xFF);
            g = (byte)((value >> GShift) & 0xFF);
            b = (byte)((value >> BShift) & 0xFF);
        }
        public static void ToRGB(this int value, out byte r, out byte g, out byte b)
        {
            r = (byte)((value >> RShift) & 0xFF);
            g = (byte)((value >> GShift) & 0xFF);
            b = (byte)((value >> BShift) & 0xFF);
        }

        public static void ToRGBA(this int color, out int r, out int g, out int b, out int a, byte? externalAlpha = null)
        {
            ToRGB(color, out r, out g, out b);

            if (externalAlpha != null)
                a = externalAlpha.Value;
            else
                a = (byte)((color >> AShift) & 0xFF);
        }
        public static byte Alpha(this int value)
        {
            return (byte)((value >> AShift) & 0xFF);
        }
        public static void ToBytes(this int color, out byte r, out byte g, out byte b, out byte a)
        {
            r = (byte)((color >> RShift) & 0xFF);
            g = (byte)((color >> GShift) & 0xFF);
            b = (byte)((color >> BShift) & 0xFF);
            a = (byte)((color >> AShift) & 0xFF);
        }
        #endregion

        #region REPEAT
        public static unsafe int[] Repeat(this int c, int repeat = 0)
        {
            if (repeat <= 0)
                return new int[] { c };

            var data = new int[repeat];

            fixed (int* d = data)
            {
                for (int i = 0; i < repeat; i++)
                    d[i] = c;
            }
            return data;
        }
        #endregion

        #region BLEND
        public static unsafe void Blend(int* source, int srcIndex, int copyLen, int* dest, int destIndex, float? alpha = null)
        {
            if (alpha == null)
            {
                for (int i = 0; i < copyLen; i++)
                {
                    dest[destIndex++] = Blend(dest[destIndex], source[srcIndex++]);
                }
            }
            else
            {
                for (int i = 0; i < copyLen; i++)
                {
                    dest[destIndex++] = Blend(dest[destIndex], source[srcIndex++], alpha.Value);
                }
            }
        }
        public static unsafe int BlendVertically(int* source, int srcIndex, int copyLen, int* dest, int destIndex, int destW, float? alpha = null)
        {
            if (alpha == null)
            {
                for (int i = 0; i < copyLen; i++)
                {
                    dest[destIndex] = Blend(dest[destIndex], source[srcIndex++]);
                    destIndex += destW;
                }
            }
            else
            {
                for (int i = 0; i < copyLen; i++)
                {
                    dest[destIndex] = Blend(dest[destIndex], source[srcIndex++], alpha.Value);
                    destIndex += destW;
                }
            }
            return destIndex;
        }

        public static int Blend(int c1, int c2)
        {
            Blend(c1, c2, c2.Alpha() / 255f, out float r, out float g, out float b, out float a);
            return ToColor(r, g, b, a);
        }
        public static int Blend(int clrTopLeft, int clrTopRight, int clrBottomLeft, int clrBottomRight, float Dx, float Dy)
        {
            Blend(clrTopLeft, clrTopRight, Dx, out float r1, out float g1, out float b1, out float a1);
            Blend(clrBottomLeft, clrBottomRight, Dx, out float r2, out float g2, out float b2, out float a2);

            var invDy = 1f - Dy;

            var r = (invDy * r1 + Dy * r2);
            var g = (invDy * g1 + Dy * g2);
            var b = (invDy * b1 + Dy * b2);
            var a = (invDy * a1 + Dy * a2);
            return ToColor(r, g, b, a);
        }
        public static int Blend(int c1, int c2, float delta)
        {
            Blend(c1, c2, delta, out float r1, out float g1, out float b1, out float a1);
            return ToColor(r1, g1, b1, a1);
        }
        public static void Blend(int c1, int c2, float delta, out float r, out float g, out float b, out float a)
        {
            c1.ToRGBA(out int r1, out int g1, out int b1, out int a1);
            c2.ToRGBA(out int r2, out int g2, out int b2, out int a2);
            var oneMinusDelta = 1f - delta;

            r = ((r1 * oneMinusDelta) + (r2 * delta));
            g = ((g1 * oneMinusDelta) + (g2 * delta));
            b = ((b1 * oneMinusDelta) + (b2 * delta));
            a = ((a1 * oneMinusDelta) + (a2 * delta));
        }
        public static int Blend(int c1, int c2, float position, float size)
        {
            return Blend(c1, c2, position / size);
        }
        #endregion

        #region MIX
        public static unsafe bool Mix(int* dest, int destIndex, int destLen, int color, byte? externalAlpha = null)
        {
            if (destIndex >= destLen)
                return false;
            dest[destIndex] = Mix(dest[destIndex], color);
            return true;
        }
        public static int Mix(int color1, int color2, byte? alpha = null)
        {
            color2 = color2.Change(alpha);
            var a2 = color2.Alpha();
            var color = Blend(color1, color2);
            return color.Change((byte)(a2 + ((a2 * (255 - a2)) >> 8)));
        }
        #endregion

        #region GET DIAGONAL COLOR
        public static int GetDiagonalColor(int c1, int c2, float wh, float ij)
        {
            c1.ToRGBA(out int r1, out int g1, out int b1, out int a1);
            c2.ToRGBA(out int r2, out int g2, out int b2, out int a2);

            var whij = wh - ij;

            var iRed1 = (r1 * whij).Round();
            var iGreen1 = (g1 * whij).Round();
            var iBlue1 = (b1 * whij).Round();
            var iAlpha1 = (a1 * whij).Round();

            var iRed2 = (r2 * ij).Round();
            var iGreen2 = (g2 * ij).Round();
            var iBlue2 = (b2 * ij).Round();
            var iAlpha2 = (a2 * ij).Round();

            iRed1 += iRed2;
            iGreen1 += iGreen2;
            iBlue1 += iBlue2;
            iAlpha1 += iAlpha2;

            iRed1 = (iRed1 / wh).Round();
            iGreen1 = (iGreen1 / whij).Round();
            iBlue1 = (iBlue1 / whij).Round();
            iAlpha1 = (iAlpha1 / whij).Round();

            return ToColor(iRed1, iGreen1, iBlue1, iAlpha1);
        }
        #endregion

        #region LERP
        public static float Lerp(float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }
        #endregion

        #region RGB EQUAL
        public static bool RGBEqual(this int first, int other)
        {
            if (other.Alpha() == 0 && first.Alpha() == 0)
                return true;

            ToRGB(first, out int r1, out int g1, out int b1);
            ToRGB(other, out int r2, out int g2, out int b2);

            return r1 == r2 && g1 == g2 && b1 == b2;
        }
        public static bool RGBEqual(this int first, int? other)
        {
            if (other == null)
                return false;
            return RGBEqual(first, other.Value);
        }
        #endregion
    }
}
