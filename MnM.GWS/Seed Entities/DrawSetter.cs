

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
    public
#endif
        class DrawSetter : GwsSettings, IDrawSetter
        {
            #region CONSTRUCTOR
            DrawSetter() 
            {
                SideDraw = SideDraw.All;
                LineDraw = LineDraw.AA;
            }
            public static DrawSetter Create()
            {
                return new DrawSetter();
            }
            #endregion

            #region PROPERTIES
            public IPenContext ReadContext { get; set ; }
            public override int X { get; set; }
            public override int Y { get; set; }
            public override IRectangle DrawnArea { get; protected set; }
            public IBufferPen ToPen(int? width = null, int? height = null) => 
                ReadContext?.ToPen(width, height);
            #endregion
        }
#if AllHidden
    }
#endif
}
