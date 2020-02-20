
using System;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct SLine : ISLine
        {
            IXLine main;
            public SLine(IXLine parent, IXLine child) : this()
            {
                Initialize(parent, child);
            }

            void Initialize(IXLine parent, IXLine child)
            {
                main = parent;

                if (child != null)
                {
                    Child = child;
                    if (!float.IsNaN(child.A.Val) && !float.IsNaN(child.B.Val))

                    {
                        OutLine1 = new XLine(main.A, Child.A);
                        OutLine2 = new XLine(main.B, Child.B);
                        HasOutLines = true;
                    }
                }
            }

            #region properties
            public IXLine Main => main;
            public IAPoint A => main.A;
            public IAPoint B => main.B;
            public int X => main.X;
            public int Y => main.Y;
            public IXLine Child { get; private set; }
            public IXLine OutLine1 { get; private set; }
            public IXLine OutLine2 { get; private set; }
            public bool HasOutLines { get; private set; }
            public int Len => main.Len;
            #endregion

            #region Methods
            public void DrawTo(IBuffer Writer, IBufferPen pen)
            {
                switch (Implementation.Renderer.FillMode)
                {
                    case FillMode.Outer:
                    case FillMode.Original:
                        this.main.DrawTo(Writer, pen);
                        break;
                    case FillMode.FillOutLine:
                        if (this.Child == null)
                            goto case FillMode.Original;
                        this.OutLine1.DrawTo(Writer, pen);
                        this.OutLine2.DrawTo(Writer, pen);
                        break;
                    case FillMode.Inner:
                        if (this.Child == null)
                            goto case FillMode.Original;
                        Implementation.Renderer.CopySettings(fillMode: FillMode.Original);
                        this.Child.DrawTo(Writer, pen);
                        Implementation.Renderer.CopySettings(fillMode: FillMode.Inner);
                        break;
                    case FillMode.DrawOutLine:
                        this.main.DrawTo(Writer, pen);
                        if (this.Child == null)
                            this.Child.DrawTo(Writer, pen);
                        break;
                }
            }
            public bool Contains(float x, float y)
            {
                if (HasOutLines)
                    return OutLine1.Contains(x, y) || OutLine2.Contains(x, y);
                return main.Contains(x, y);
            }

            ISLine IXLine.Add(float stroke) =>
                main.Add(stroke);
            #endregion
        }
#if AllHidden
    }
#endif
}