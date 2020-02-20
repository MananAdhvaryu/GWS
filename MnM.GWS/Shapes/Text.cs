using MnM.GWS.MathExtensions;

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
        sealed class Text : IText
        {
            #region VARIABLES
            IDrawStyle drawStyle, oldDrawStyle;
            IFont font;
            IList<IGlyph> originalGlyphs;
            IRectangleF area;
            #endregion

            #region CONSTRUCTORS
            Text() { }
            public Text(IList<IGlyph> glyphs, IDrawStyle drawStyle = null,  int? destX = null,  int? destY = null) : this()
            {
                this.originalGlyphs = glyphs;
                ChangeDrawStyle(drawStyle ?? new DrawStyle(), false);
                if (destX != null)
                    DrawX = destX.Value;
                if (destY != null)
                    DrawY = destY.Value;
            }
            public Text(IFont font,  string text,  int destX,  int destY, IDrawStyle drawStyle = null) : this()
            {
                Initialize(font, text, drawStyle, destX, destY);
            }
            void Initialize(IFont font,  string text, IDrawStyle drawStyle = null,  int? destX = null,  int? destY = null)
            {
                if (drawStyle != null)
                    ChangeDrawStyle(drawStyle, false);

                else if (this.drawStyle == null)
                    ChangeDrawStyle(new DrawStyle(), false);

                if (destX != null)
                    DrawX = destX.Value;
                if (destY != null)
                    DrawY = destY.Value;

                if (font != null)
                {
                    this.font = font;
                    this.drawStyle.LineHeight = font.Info.LineHeight.Ceiling();
                }
                originalGlyphs = new IGlyph[(text + "").Length];

                for (int i = 0; i < originalGlyphs.Count; i++)
                    originalGlyphs[i] = font.GetGlyph(text[i]);

                Changed = true;
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public IGlyph this[int index] => originalGlyphs[index];
            public int Count => originalGlyphs.Count;
            public IDrawStyle DrawStyle => drawStyle;
            public bool Changed { get; private set; }
            public IRectangleF Bounds
            {
                get
                {
                    if (originalGlyphs.Count == 0)
                        return Factory.RectFEmpty;
                    if(area == null)
                    {
                        Changed = true;
                        Measure();
                        Changed = false;
                    }
                    return area;
                }
            }
            public int DrawX { get; private set; }
            public int DrawY { get; private set; }
            public string Name => "Text";
            public string ID { get; private set; }
            public Angle Angle
            {
                get
                {
                    if (drawStyle != null)
                        return drawStyle.Angle;
                    return Angle.Empty;
                }
            }
            #endregion

            #region METHODS
            public IText Measure()
            {
                if (originalGlyphs.Count == 0 || !Changed)
                    return this;
                MeasureText();
                Changed = false;
                return this;
            }
            public int GetKerning(int i)
            {
                if (font == null || i == 0)
                    return 0;
                var c = originalGlyphs[i].Character;
                if (font.Kerning && i > 0 && c != 0)
                    return font.GetKerning(originalGlyphs[i - 1].Character, c);
                return 0;
            }
            #endregion

            #region CHANGE
            public void ChangeFont(IFont newFont)
            {
                if (newFont == null)
                    return;
                var text = new string(originalGlyphs.Select(x => x.Character).ToArray());
                Initialize(newFont, text);
            }
            public void ChangeText(string text)
            {
                if (font == null)
                    return;
                Initialize(font, text);
            }
            public void ChangeDrawStyle(IDrawStyle value, bool temporary = true)
            {
                if (value == null)
                    return;

                Changed = Changed || value != drawStyle;
                if (!temporary)
                    oldDrawStyle = value;
                drawStyle = value;
            }
            public void RestoreDrawStyle()
            {
                drawStyle = oldDrawStyle;
            }
            public void SetDrawXY(int? drawX = null, int? drawY = null)
            {
                if (drawX == null && drawY == null)
                    return;
                if (drawX != null)
                    DrawX = drawX.Value;
                if (drawY != null)
                    DrawY = drawY.Value;
                Changed = true;
            }
            #endregion

            #region MEASURE TEXT
            public IRectangleF MeasureText(IDrawStyle style = null)
            {
                if (!Changed)
                    return Bounds;
                ChangeDrawStyle(style);
                var info = font.MeasureGlyphs(originalGlyphs, DrawX, DrawY, DrawStyle);
                area = info.Bounds;
                originalGlyphs = info.Glyphs;
                RestoreDrawStyle();
                return area;
            }
            #endregion
            
            #region CLONE
            public object Clone()
            {
                var g = new Text();
                g.ID = Factory.NewID(Name);

                g.originalGlyphs = originalGlyphs.ToArray();
                g.drawStyle = drawStyle.Clone() as IDrawStyle;
                g.oldDrawStyle = oldDrawStyle.Clone() as IDrawStyle;
                g.DrawX = DrawX;
                g.DrawY = DrawY;
                g.font = font;
                g.area = Bounds;
                return g;
            }
            #endregion

            #region Interface
            IEnumerator<IGlyph> IEnumerable<IGlyph>.GetEnumerator() =>
                originalGlyphs.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() =>
                originalGlyphs.GetEnumerator();
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
