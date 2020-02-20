using MnM.GWS.MathExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using static MnM.GWS.Geometry;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public static class FontHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGlyphSlot RotateSlot(this IGlyphSlot slot, Angle angle, int currentX, int currentY, out float newX, out float newY)
        {
            newX = currentX;
            newY = currentY;

            if (slot.Points == null || slot.Points.Count == 0 || !angle.Valid || char.IsWhiteSpace(slot.Character))
                return slot;

            Angle a = new Angle(-angle.Degree, 0, 0);
            var Points = new PointF[slot.Points.Count];
            for (int i = 0; i < Points.Length; i++)
            {
                var p = new PointF(slot.Points[i].X + slot.Min.X, slot.Points[i].Y + slot.Min.Y, slot.Points[i].Quadratic);
                Points[i] = a.Rotate(p);
            }

            angle.Rotate(currentX, currentY, out newX, out newY);
            return Factory.newGlyphSlot(slot.Character, Points, slot.Contours.ToArray(), slot.XHeight);
        }
        public static GlyphsData MeasureText(this IFont font, string text, float destX, float destY, IDrawStyle drawStyle = null)
        {
            if (font == null || string.IsNullOrEmpty(text))
                return GlyphsData.Empty;
            var Glyphs = new IGlyph[(text + "").Length];

            for (int i = 0; i < Glyphs.Length; i++)
                Glyphs[i] = font.GetGlyph(text[i]);

            return MeasureGlyphs(font, Glyphs, destX, destY, drawStyle);
        }
        public static GlyphsData MeasureGlyphs(this IFont font, IList<IGlyph> Glyphs, float destX, float destY, IDrawStyle drawStyle = null)
        {
            if (font == null)
                return GlyphsData.Empty;

            var DrawStyle = drawStyle ?? new DrawStyle();
            DrawStyle.LineHeight = font.Info.LineHeight.Ceiling();

            float lineHeight = DrawStyle.LineHeight;

            float newx, newy, right, bottom, minX, minY, kerning;

            for (int i = 0; i < Glyphs.Count; i++)
            {
                Glyphs[i].SetDrawXY(0, 0);
            }
            var minHBY = Glyphs.Min(g => g.Bounds.Y);
            if (minHBY < 0)
                destY += -minHBY;

            if (Glyphs[0].Bounds.X < 0)
                destX += -Glyphs[0].Bounds.X;

            newx = right = minX = destX;
            newy = bottom = minY = destY;

            bool start = true;
            kerning = 0;

            for (int i = 0; i < Glyphs.Count; i++)
            {
                IGlyph g = Glyphs[i];
                lineHeight = Math.Max(lineHeight, g.Bounds.Height);

                if (IsSpace(Glyphs, i))
                {
                    switch (DrawStyle.Breaker)
                    {
                        case TextBreaker.None:
                        case TextBreaker.Word:
                            if (DrawStyle.Delimiter == BreakDelimiter.Word)
                                goto case TextBreaker.SingleWord;
                            break;
                        case TextBreaker.Line:
                        default:
                            if (!start)
                                newx += g.Bounds.Width;
                            goto mks;
                        case TextBreaker.SingleWord:
                            newx = destX;
                            newy += lineHeight;
                            start = true;
                            break;
                    }
                }
                else if (IsCR(Glyphs, i) || IsLF(Glyphs, i))
                {
                    if (IsPreviousCR(Glyphs, i))
                        goto last;
                    else
                    {
                        newx = destX;
                        newy += lineHeight;
                        goto mks;
                    }
                }

                kerning = GetKerning(Glyphs, font, i);

            mks:
                g.SetDrawXY((newx + kerning).Round(), newy.Round());

                minX = Math.Min(g.DrawX, minX);
                minY = Math.Min(g.DrawY, minY);

                newx = g.DrawX + g.Bounds.Width;
                right = Math.Max(newx, right);
                bottom = Math.Max(bottom, g.DrawY + g.Bounds.Height);
            last:
                start = false;
            }

            IRectangleF Area = Factory.RectangleFFromLTRB(minX, minY, ++right, ++bottom);
            //IAreaF PenArea = Factory.AreaFFromLTRB(destX, destY, right, bottom); 

            if (DrawStyle.Angle.Valid)
            {
                var glyphs = new IGlyph[Glyphs.Count];
                Glyphs.CopyTo(glyphs, 0);

                for (int i = 0; i < glyphs.Length; i++)
                    glyphs[i].Rotate(DrawStyle.Angle.AssignCenter(Area.Center()));
                Area = Factory.newRhombus(glyphs[0].Bounds, DrawStyle.Angle, Area.Width);
                return GlyphsData.Create(null, Area, glyphs as IList<IGlyph>, minHBY, DrawStyle.Angle);
            }
            return GlyphsData.Create(null, Area, Glyphs, minHBY);
        }

        static bool IsSpace(IList<IGlyph> glyphs, int index) =>
            glyphs[index].Character == ' ';
        static bool IsCR(IList<IGlyph> glyphs, int index) =>
            glyphs[index].Character == '\r';
        static bool IsPreviousCR(IList<IGlyph> glyphs, int index)
        {
            if (index == 0)
                return false;
            return IsCR(glyphs, index - 1);
        }
        static bool IsLF(IList<IGlyph> glyphs, int index) =>
            glyphs[index].Character == '\n';
        static int GetKerning(IList<IGlyph> glyphs, IFont font, int i)
        {
            if (font == null || i == 0)
                return 0;
            var c = glyphs[i].Character;
            if (font.Kerning && i > 0 && c != 0)
                return font.GetKerning(glyphs[i - 1].Character, c);
            return 0;
        }

    }
}
