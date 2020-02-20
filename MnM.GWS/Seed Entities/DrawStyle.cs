using System;

namespace MnM.GWS
{
    public struct DrawStyle : IDrawStyle
    {
        public Angle Angle { get; set; }
        public Size PreferredSize { get; set; }
        public CaseConversion CaseConversion { get; set; }
        public ContentAlignment Position { get; set; }
        public TextBreaker Breaker { get; set; }
        public BreakDelimiter Delimiter { get; set; }
        public TextStyle TextStyle { get; set; }
        public int LineHeight { get; set; }
        public IImageStyle ImageStyle { get; set; }
        public bool DrawGlyphIndividually { get; set; }

        public DrawStyle Clone()
        {
            var d = new DrawStyle();
            d.Angle = new Angle(Angle);
            d.PreferredSize = new Size(PreferredSize.Width, PreferredSize.Height);
            d.CaseConversion = CaseConversion;
            d.Position = Position;
            d.Breaker = Breaker;
            d.Delimiter = Delimiter;
            d.TextStyle = TextStyle;
            d.LineHeight = LineHeight;
            d.ImageStyle = ImageStyle?.Clone() as IImageStyle;
            d.DrawGlyphIndividually = DrawGlyphIndividually;
            return d;
        }

        object ICloneable.Clone() => Clone();
    }
}
