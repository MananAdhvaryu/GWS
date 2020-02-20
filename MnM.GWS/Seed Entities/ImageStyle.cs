using System;

namespace MnM.GWS
{
    struct ImageStyle : IImageStyle
    {
        public ImagePosition Alignment { get; set; }
        public ImageDraw Draw { get; set; }
        public IBuffer Image { get; set; }

        public unsafe ImageStyle Clone()
        {
            var i = new ImageStyle();
            i.Alignment = Alignment;
            i.Draw = Draw;
            i.Image = Implementation.Factory.newBuffer((int*)Image.Pixels, Image.Width, Image.Height);
            return i;
        }
        object ICloneable.Clone() => Clone();
    }
}
