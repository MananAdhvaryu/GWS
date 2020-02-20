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
        struct GlyphSlot : IGlyphSlot
        {
            #region VARIABLES
            const string tostr = "Char: {0}, Area: {1}";
            PointF min, max;
            #endregion

            #region CONSTRUCTORS
            public GlyphSlot( char c,  IList<PointF> data, int[] contours,  float xHeight) : this()
            {
                Area = Factory.RectFEmpty;

                if (data == null)
                {
                    Points = new PointF[4];
                    Data = new IAPoint[0];
                }
                else
                {
                    Data = new List<IAPoint>();
                    Points = data.ToArray();
                }

                XHeight = xHeight.Ceiling();
                Character = c;
                Contours = contours;
                initialize();
            }
            void initialize()
            {
                if (Initialized)
                    return;

                Area = InitializeGlyphSlot(this, out min, out max);

                Initialized = true;

                if (Character == ' ')
                    return;
                var data = Data;
                Factory.FontRenderer.Process(this, (x, x1, y, h, e) =>
                    data.Add(Factory.newAPoint(x, y, e, h, x1 - x)), Area.Width.Round(), Area.Height.Round());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static IRectangleF InitializeGlyphSlot(IGlyphSlot slot, out PointF Min, out PointF Max)
            {
                Min = Max = PointF.Empty;

                float x = slot.Area.X;
                float y = slot.Area.Y;
                float w = slot.Area.Width;
                float h = slot.Area.Height;

                if (slot.Initialized)
                    return Factory.newRectangleF(x, y, w, h);

                if (char.IsWhiteSpace(slot.Character))
                {
                    if (slot.Character == ' ')
                        w = (slot.Points[1].X - slot.Points[0].X).Ceiling();

                    return Factory.newRectangleF(x, y, w, h);
                }
                if (slot.Points.Count < 4)
                    return Factory.newRectangleF(x, y, w, h);

                var num = slot.Points.Count - 4;

                Min = new PointF(float.MaxValue, float.MaxValue);
                Max = new PointF(float.MinValue, float.MinValue);

                var points = slot.Points;

                for (int i = 0; i < num; i++)
                {
                    Min = Min.Min(points[i]);
                    Max = Max.Max(points[i]);
                }

                var off = new PointF(-Min.X, -Min.Y, Min.Quadratic);
                for (int i = 0; i < num; i++)
                    slot.Points[i] = new PointF(points[i].X + off.X, points[i].Y + off.Y, points[i].Quadratic);

                w = Math.Max(slot.Points[num + 1].X - slot.Points[num].X - Min.X, Max.X - Min.X).Round();
                h = (Max.Y - Min.Y).Ceiling();
                x = Min.X.Ceiling();
                y = (slot.XHeight - h) - Min.Y.Ceiling();
                return Factory.newRectangleF(x, y, w, h);
            }
            #endregion

            #region PROPERTIES
            public char Character { get; private set; }
            public int XHeight { get; private set; }
            public IList<PointF> Points { get; private set; }
            public IList<IAPoint> Data { get; private set; }
            public bool IsGlyph => true;

            public IList<int> Contours { get; private set; }
            public PointF Min => min;
            public PointF Max => max;
            public IRectangleF Area { get; private set; }
            public bool Initialized { get; private set; }
            #endregion

            public override string ToString()
            {
                return string.Format(tostr, Character, Area.ToString());
            }
        }
#if AllHidden
    }
#endif
}
