/* Licensed under the MIT/X11 license.
* Copyright (c) 2016-2018 jointly owned by eBestow Technocracy India Pvt. Ltd. & M&M Info-Tech UK Ltd.
* This notice may not be removed from any source distribution.
* See license.txt for detailed licensing details. */
using MnM.GWS;
using MnM.GWS.MathExtensions;
using System;
using System.Collections;
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
        struct FillStyle : IFillStyle
        {
            #region VARIABLES
            readonly int[] ColorArray;
            readonly int[] PositionArray;

            const string toString = "{0}.{1}";
            const string BrushKey = "{0}.{1}.{2}";
            Enumerator list;
            #endregion

            #region CONSTRUCTORS
            public FillStyle( Gradient gradient, params int[] colors) : this()
            {
                if (colors.Length == 0)
                    ColorArray = new int[] { Colour.Black };
                else
                    ColorArray = colors;

                Gradient = gradient;
                var value = ColorArray[0].GetHashCode();
                for (int i = 1; i < ColorArray.Length; i++)
                    value = Implementation.Combine(value.GetHashCode(), ColorArray[i].GetHashCode());
                Key = string.Format(toString, Gradient, value);
               PositionCount = PositionArray?.Length ?? 0;
            }
            public FillStyle(int[] colors,  Gradient gradient,  IList<int> stops) :
               this(gradient, colors)
            {
                if (stops?.Count > 0)
                {
                    PositionArray = stops.ToArray();
                }
                PositionCount = PositionArray?.Length ?? 0;
            }
            #endregion

            #region PROPERTIES
            public int this[int index] => ColorArray[index];
            public int Count => ColorArray.Length;
            public int PositionCount { get; private set; }
            public Gradient Gradient { get; private set; }
            public string Key { get; private set; }
            public int EndColor => ColorArray[ColorArray.Length - 1];
            #endregion

            #region EQUALITY
            public static bool operator ==(FillStyle fs1, IFillStyle fs2) =>
                fs1.Equals(fs2);
            public static bool operator !=(FillStyle fs1, IFillStyle fs2) =>
                !fs1.Equals(fs2);

            public override int GetHashCode() =>
                Key.GetHashCode();
            public override bool Equals(object obj)
            {
                if (!(obj is IFillStyle))
                    return false;

                return Equals((IFillStyle)obj);
            }
            public bool Equals(IFillStyle other) =>
                other.Key == Key;
            #endregion

            #region GET BRUSH
            public IBrush newBrush(int width, int height) =>
                Factory.newBrush(this, width, height);
            
            public IBufferPen ToPen(int? width = null, int? height = null)
            {
                var w = width ?? 100;
                var h = height ?? 100;

                return Factory.newBrush(this, w, h);
            }
            public string GetBrushKey(int width, int height) =>
                string.Format(BrushKey, Key, width, height);
            public FillStyle Clone()
            {
                FillStyle f = new FillStyle(ColorArray.ToArray(), Gradient, PositionArray);
                return f;
            }
            #endregion

            #region GET POSITION
            public int Position(int index) =>
                PositionArray[index];
            #endregion

            #region CHANGE - INVERT
            public IFillStyle Change(Gradient g)
            {
                return new FillStyle(ColorArray, g, PositionArray);
            }
            public IFillStyle Invert()
            {
                var colors = new int[ColorArray.Length];
                ColorArray.CopyTo(colors, 0);
                Array.Reverse(colors);
                if (PositionArray != null)
                    return new FillStyle(colors, Gradient, PositionArray);
                else
                    return new FillStyle(colors, Gradient, null);
            }
            #endregion

            #region GET COLORS
            public int GetColor(float position, float size, bool invert = false)
            {
                GetTweenColorsByStops(ref position, ref size, out int c1, out int c2);

                if (invert)
                    return Colours.Blend(c2, c1, position, size);
                return Colours.Blend(c1, c2, position, size);
            }
            public void GetTweenColorsByStops(ref float position, ref float size, out int c1, out int c2)
            {
                if (Count < 3)
                {
                    c1 = this[0];
                    c2 =  EndColor;
                    return;
                }
                float pos = size / (Count - 1);
                if (pos == 0)
                    pos = 1;
                int i = 0;
                if (PositionCount > 0)
                {
                    for (int j = 0; j < PositionCount; j++)
                    {
                        if (Position(j) > pos)
                            break;
                        i = j;
                    }
                }
                else
                {
                    i = (int)(position / pos);
                }
                position %= pos;
                if (i >= Count)
                    i = Count - 1;
                c1 = this[i];
                ++i;
                if (i > Count - 1)
                    c1 = this[--i];
                c2 = this[i];
                size = pos.Ceiling();
            }
            #endregion

            #region GET COLOR ENUMERATOR
            public IColorEnumerator GetEnumerator()
            {
                if (list == null) 
                    list = new Enumerator(this);
                return list;
            }
            #endregion
            public override string ToString() => Key;

            #region interface
            object ICloneable.Clone() => Clone();
            #endregion

            class Enumerator : IColorEnumerator
            {
                int position;
                readonly IFillStyle Style;
                int? color;
                public Enumerator(IFillStyle style)
                {
                    Style = style;
                }

                public int MaxIteration { get; set; }
                public int CurrentPosition => position;
                public bool Freeze { private get; set; }
                public void Reset()
                {
                    position = 0;
                    color = null;
                }
                public void MoveNext(bool ignoreFreeze = false)
                {
                    if (ignoreFreeze || !Freeze)
                    {
                        color = null;
                        ++position;
                    }
                }
                public int GetCurrent(bool invert = false)
                {
                    if (color == null || !Freeze)
                    {
                        color = Style.GetColor(position, MaxIteration, invert);
                    }
                    return color.Value;
                }
                public IEnumerator<int> GetEnumerator()
                {
                    position = 0;
                    for (int i = 0; i < MaxIteration; i++)
                    {
                        yield return Style.GetColor(position, MaxIteration);
                        ++position;
                    }
                }

                public int[] ToArray(int start, int end, int? maxIteration = null, bool centerToLeftRight = false)
                {
                    MathHelper.Order(ref start, ref end);
                    var array = new int[(end - start)];
                    var max = maxIteration ?? MaxIteration;
                    if (centerToLeftRight)
                    {
                        var half = end / 2;
                        for (int i = start; i <= end / 2; i++)
                        {
                            var idx = i - start;
                            array[idx] = Style.GetColor(i, max);
                            if (idx == half)
                                continue;
                            array[array.Length - 1 - idx] = array[idx];
                        }
                    }
                    else
                    {
                        for (int i = start; i < end; i++)
                            array[i - start] = Style.GetColor(i, max);
                    }
                    return array;
                }
                IEnumerator IEnumerable.GetEnumerator() =>
                    GetEnumerator();
            }
        }
#if AllHidden
    }
#endif
}

