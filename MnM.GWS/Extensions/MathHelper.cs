using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MnM.GWS.MathExtensions
{
    public static partial class MathHelper
    {
        #region order
        public static void Order(ref int lower, ref int upper)
        {
            if (lower <= upper)
                return;
            var l = lower;
            lower = upper;
            upper = l;
        }
        public static void Order(ref int? lower, ref int? upper)
        {
            if (lower <= upper)
                return;
            var l = lower;
            lower = upper;
            upper = l;
        }

        public static void Order(ref double lower, ref double upper)
        {
            if (lower <= upper)
                return;
            var l = lower;
            lower = upper;
            upper = l;
        }
        public static void Order(ref float lower, ref float upper)
        {
            if (lower <= upper)
                return;
            var l = lower;
            lower = upper;
            upper = l;
        }
        public static void Order(ref float? lower, ref float? upper)
        {
            if (lower <= upper)
                return;
            var l = lower;
            lower = upper;
            upper = l;
        }

        public static void OrderY(ref float x1, ref float y1, ref float x2, ref float y2)
        {
            if (y1 > y2)
            {
                Swap(ref y1, ref y2);
                Swap(ref x1, ref x2);
            }
        }
        #endregion

        #region Swap
        public static void Swap<T>(ref T x, ref T y)
        {
            var temp = x;
            x = y;
            y = temp;
        }
        #endregion

        #region Assign
        public static void NoNullAssign<T>(this T? newValue, ref T current) where T: struct
        {
            if (newValue == null)
                return;
            current = newValue.Value;
        }
        public static void NoNullAssign<T>(this T newValue, ref T current) where T : class
        {
            if (newValue == null)
                return;
            current = newValue;
        }
        #endregion

        #region rounding
        public static int Ceiling(this float p)
        {
            return (int)Math.Ceiling(p);
        }
        public static int Round(this float p)
        {
            return (int)Math.Round(p);
        }
        public static int Round(this float p, int digits)
        {
            return (int)Math.Round(p, digits);
        }
        public static float RoundF(this float p, int digits)
        {
            return (float)Math.Round(p, digits);
        }

        public static int Floor(this float p)
        {
            return (int)Math.Floor(p);
        }
        public static int Floor(this double p)
        {
            return (int)Math.Floor(p);
        }
        public static int Round(this float p, MidpointRounding r)
        {
            return (int)Math.Round(p, r);
        }
        public static int Ceiling(this double p)
        {
            return (int)Math.Ceiling(p);
        }
        public static int Round(this double p)
        {
            return (int)Math.Round(p);
        }

        #endregion

        #region ABS
        public static float Abs(this float value) => Math.Abs(value);
        public static int Abs(this int value) => Math.Abs(value);
        public static double Abs(this double value) => Math.Abs(value);
        #endregion

        #region Int equal
        public static bool IntEqual(this float a, int b)
        {
            return ((int)a == b || a.Round() == b || a.Ceiling() == b);
        }
        #endregion

        #region fraction
        public static float? Fraction(this float p)
        {
            var f = p - (int)p;
            if (f == 0)
                return null;
            return f;
        }
        public static bool Fractional(this float p)
        {
            var f = p - (int)p;
            return (f != 0);
        }
        #endregion

        #region Square
        public static int Sqr(this int number) =>
            number * number;
        public static double Sqr(this double number) =>
            number * number;
        public static float Sqr(this float number) =>
            number * number;
        #endregion

        #region Average
        public static float Avg(params float[] numbers)
        {
            if (numbers.Length == 0)
                return 0f;
            var num = 0f;
            foreach (var n in numbers)
                num += n;
            return num / numbers.Length;
        }
        public static int Avg(params int[] numbers)
        {
            if (numbers.Length == 0)
                return 0;
            var num = 0;
            foreach (var n in numbers)
                num += n;
            return num / numbers.Length;
        }
        #endregion

        #region Min -Max
        public static float MinFrom(this IEnumerable<float> numbers, int start, int counter)
        {
            int i = -1;
            int j = start;
            var num = -1f;

            foreach (var n in numbers)
            {
                ++i;
                if (i < start)
                    continue;
                if (i == j)
                {
                    j += counter;
                    if (num == -1f || n < num)
                        num = n;
                }
            }
            return num;
        }
        public static float MaxFrom(this IEnumerable<float> numbers, int start, int counter)
        {
            int i = -1;
            int j = start;
            var num = 0f;

            foreach (var n in numbers)
            {
                ++i;
                if (i < start)
                    continue;
                if (i == j)
                {
                    j += counter;
                    if (num == 0 || n > num)
                        num = n;
                }
            }
            return num;
        }
        public static int MinFrom(this IEnumerable<int> numbers, int start, int counter)
        {
            int i = -1;
            int j = start;
            var num = -1;

            foreach (var n in numbers)
            {
                ++i;
                if (i < start)
                    continue;
                if (i == j)
                {
                    j += counter;
                    if (num == -1 || n < num)
                        num = n;
                }
            }
            return num;
        }
        public static int MaxFrom(this IEnumerable<int> numbers, int start, int counter)
        {
            int i = -1;
            int j = start;
            var num = 0;

            foreach (var n in numbers)
            {
                ++i;
                if (i < start)
                    continue;
                if (i == j)
                {
                    j += counter;
                    if (num == 0 || n > num)
                        num = n;
                }
            }
            return num;
        }

        public static float Min(params float[] numbers) =>
            (numbers as IEnumerable<float>).Min();
        public static float Max(params float[] numbers) =>
            (numbers as IEnumerable<float>).Max();
        #endregion

        #region Is within
        public static bool IsWithIn(float min, float max, float value, bool includeMinMax = true)
        {
            Order(ref min, ref max);
            if (includeMinMax)
                return value >= min && value <= max;
            return value > min && value < max;
        }
        public static bool IsWithIn(double min, double max, double value, bool includeMinMax = true)
        {
            Order(ref min, ref max);
            if (includeMinMax)
                return value >= min && value <= max;
            return value > min && value < max;
        }
        public static bool IsWithIn(int min, int max, int value, bool includeMinMax = true)
        {
            Order(ref min, ref max);
            if (includeMinMax)
                return value >= min && value <= max;
            return value > min && value < max;
        }
        #endregion

        #region Middle
        public static float Middle(float min, float max)
        {
            Order(ref min, ref max);
            return min + (max - min) / 2f;
        }
        public static int Middle(int min, int max)
        {
            Order(ref min, ref max);
            return min + (max - min) / 2;
        }
        #endregion

        #region Stay within
        public static void StayWithIn(float min, float max, ref float value)
        {
            Order(ref min, ref max);
            if (value < min)
                value = min;
            if (value > max)
                value = max;
        }
        public static void StayWithIn(int min, int max, ref int value)
        {
            Order(ref min, ref max);
            if (value < min)
                value = min;
            if (value > max)
                value = max;
        }
        #endregion
    }
}
