using MnM.GWS.MathExtensions;
using System;
using static MnM.GWS.Implementation;

namespace MnM.GWS.StandardVersion
{
#if AllHidden
    partial class NativeFactory
    {
#else
    public
#endif
        sealed class ElementCollection : ElementCollection<ShapeDrawInfo>
        {
            #region CONSTRUCTORS
            public ElementCollection(IParent window) :
                base(window)
            {
                ID = Factory.NewID("Path");
            }
            #endregion

            #region PROPERTIES
            protected override ShapeDrawInfo Empty => ShapeDrawInfo.Empty;
            #endregion

            #region IMPLENTED ABSTRACT METHOS
            public override S Add<S>(S shape, IPenContext context)  
            {
                if (shape == null)
                    return shape;
                AddMode = !Contains(shape);
                Implementation.Renderer.Render(Buffer, shape, context);
                AddMode = false;
                return shape;
            }
            protected override ShapeDrawInfo newDrawInfo( IElement shape) =>
                new ShapeDrawInfo(shape, Buffer.ID);
            protected override void SetCurrentPage( ShapeDrawInfo info3,  bool silent) { }
            protected override bool IsDrawable( ShapeDrawInfo item,  ShapeDrawInfo compareWith) =>
                item.Shape == compareWith.Shape;
        #endregion
    }
#if AllHidden
    }
#endif
}
