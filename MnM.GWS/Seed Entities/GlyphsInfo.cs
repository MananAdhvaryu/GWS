using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MnM.GWS
{
    public struct GlyphsData: IRotatable
    {
        public readonly IList<IGlyph> Glyphs;
        public readonly float MinHBY;
        public readonly bool ContainsNewLine;
        public readonly string Text;

        public static readonly GlyphsData Empty = new GlyphsData();
        GlyphsData(string text, IRectangleF area, IList<IGlyph> glyphs, float minHBY, Angle angle = default(Angle), bool containsNewLine = false)
        {
            Bounds = area;
            Bounds = area;
            Glyphs = glyphs;
            MinHBY = minHBY;
            Angle = angle;
            ContainsNewLine = containsNewLine;
            if (text == null)
                Text = new string(glyphs.Select(x => x.Character).ToArray());
            else
                Text = text;
        }
        public static GlyphsData Create(string text, IRectangleF area, IList<IGlyph> glyphs, float minHBY, Angle angle = default(Angle), bool containsNewLine = false) =>
            new GlyphsData(text, area, glyphs, minHBY, angle, containsNewLine);

        public IRectangleF Bounds { get; private set; }
        public Angle Angle { get; private set; }
    }
}
