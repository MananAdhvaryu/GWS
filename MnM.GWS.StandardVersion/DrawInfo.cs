namespace MnM.GWS.StandardVersion
{
#if AllHidden
    partial class NativeFactory
    {
#else
    public
#endif
        sealed class ShapeDrawInfo: GwsSettings, IDrawInfo3
        {
            public static readonly ShapeDrawInfo Empty = new ShapeDrawInfo();
            ShapeDrawInfo() { }
            public ShapeDrawInfo(IElement shape, string writerID, string reader = null) 
            {
                Shape = shape.ID;
                Writer = writerID;
                Reader = reader;
                LineDraw = LineDraw.AA;
            }

            #region PROPERTIES
            public override int X { get; set; }
            public override int Y { get; set; }
            public override IRectangle DrawnArea { get ; protected set; }
            #endregion

            #region COPY- RESTORE SETTINGS
            public override void Flush() { }
            #endregion
        }
#if AllHidden
    }
#endif
}
