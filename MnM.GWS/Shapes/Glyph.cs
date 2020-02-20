using MnM.GWS.MathExtensions;
using System;
using System.Collections.Generic;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        sealed class Glyph : IGlyph
        {
            #region VARIABLES
            IGlyphSlot slot;
            int x, y;
            static string tostr = "Char: {0}, X: {1}, Y: {2}, W: {3}, H: {4}";
            #endregion

            #region CONSTRUCTORS
            Glyph() { }
            public Glyph(IGlyphSlot glyph)
            {
                initialize(glyph);
            }
            void initialize(IGlyphSlot glyph)
            {
                slot = glyph;
                Bounds = slot.Area;
                Character = slot.Character;
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public IList<IAPoint> Data => slot.Data;
            public char Character { get; private set; }
            public bool IsOutLine { get; set; }
            public int DrawX
            {
                get => (x + slot.Area.X).Round();
                set => x = value;
            }
            public int DrawY
            {
                get => (y + slot.Area.Y).Round();
                set => y = value;
            }
            public IRectangleF Bounds { get; private set; }
            public string Name => "Glyph";
            public string ID { get; private set; }
            #endregion

            #region METHODS
            public void Rotate(Angle angle)
            {
                if (!angle.Valid)
                    return;
                slot = slot.RotateSlot(angle, x, y, out float newX, out float newY);
                initialize(slot);
                SetDrawXY(newX.Round(), newY.Round());
            }
            public void DrawTo(IBuffer Writer, IBufferPen pen)
            {
                var x = DrawX;
                var y = DrawY;

                foreach (var item in Data)
                {
                    if (item.IsHorizontal)
                    {
                        var x1 = item.IVal + x;
                        var x2 = x1 + item.Len;
                        var axis = item.Axis + y;
                        Writer.WriteLine(pen, x1, axis, x1, x2, axis, true, item.Alpha);
                    }
                    else
                    {
                        var y1 = item.IVal + y;
                        var y2 = y1 + item.Len;
                        var axis = item.Axis + x;
                        Writer.WriteLine(pen, y1, axis, y1, y2, axis, false, item.Alpha);
                    }
                }
            }

            public bool Contains(float x, float y)
            {
                if (x < DrawX)
                    return false;
                if (y < DrawY)
                    return false;
                if (x > DrawX + slot.Area.Width)
                    return false;
                if (y > DrawY + slot.Area.Height)
                    return false;
                return true;
            }
            public void SetDrawXY(int? drawX = null, int? drawY = null)
            {
                x = drawX ?? 0;
                y = drawY ?? 0;
                Bounds = Factory.newRectangleF(DrawX, DrawY, slot.Area.Width, slot.Area.Height);
            }
            public Glyph Clone()
            {
                var g = new Glyph();
                g.x = x;
                g.y = y;
                g.initialize(slot);
                g.IsOutLine = IsOutLine;
                return g;
            }
            object ICloneable.Clone() => Clone();
            #endregion

            public override string ToString()
            {
                return string.Format(tostr, slot.Character,
                   DrawX, DrawY, slot.Area.Width, slot.Area.Height);
            }
        }
#if AllHidden
    }
#endif
}