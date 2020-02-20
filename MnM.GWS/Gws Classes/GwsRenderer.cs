using MnM.GWS.EnumerableExtensions;
using MnM.GWS.EnumExtensions;
using MnM.GWS.MathExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public abstract class GwsRenderer : GwsSettings, IRenderer
    {
        #region VARIABLES
        Angle ReadAngle, WriteAngle;
        bool freezeReaderXY, freezeWriterXy;
        IPenContext context;
        int minX, minY, maxX, maxY, x, y;
        HashSet<int> Duplicates = new HashSet<int>();
        volatile bool distinctLineProcessing;
        IElement Element;
        bool curveModified;
        #endregion

        #region CONSTRUCTORS
        protected GwsRenderer()
        {
            HorizontalScan = true;
            LineDraw = LineDraw.AA;
            ResetDrawnArea();
        }
        #endregion

        #region PROPERTIES
        public IPenContext ReadContext
        {
            get => context ?? FillStyles.Black;
            set => context = value;
        }
        public bool ReuseCurrentReader { get; set; }
        public sealed override IRectangle DrawnArea
        {
            get => Factory.RectangleFromLTRB(minX, minY, maxX, maxY);
            protected set { }
        }
        public sealed override int X
        {
            get => x;
            set
            {
                if (freezeWriterXy)
                    return;
                x = value;
            }
        }
        public sealed override int Y
        {
            get => y;
            set
            {
                if (freezeWriterXy)
                    return;
                y = value;
            }
        }
        public bool IsRendering { get; private set; }
        public IRectangleF Bounds { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool HorizontalScan { get; set; }
        public bool Activated => !IsDisposed;
        public bool IsDrawingShape { get; set; }
        public int ReadX { get; private set; }
        public int ReadY { get; private set; }
        public DrawMode DrawMode 
        { 
            get;
#if AdvancedVersion
            set;
#endif
        }
        public bool NoStroke => Stroke == 0 || FillMode == FillMode.Original;
        public bool AntiAlised =>
            LineDraw.HasFlag(LineDraw.AA);
        Angle IRotatable.Angle => ReadAngle;
        public string ShapeBeingDrawn { get; protected set; }
        #endregion

        #region DISTICNCT PIXEL DRAW
        public bool IsPixelWriteable(int index)
        {
            if (!distinctLineProcessing)
                return true;
            if (Duplicates.Contains(index))
                return false;
            Duplicates.Add(index);
            return true;
        }
        public bool DistinctLineProcessing
        {
            set
            {
                distinctLineProcessing = value;
                Duplicates.Clear();
            }
        }
        #endregion

        #region BEGIN - END
        protected bool Begin(IBuffer Buffer, IElement element)
        {
            if (IsRendering || Buffer == null)
                return false;

            IsRendering = true;
            Writer = Buffer.ID;
            Element = element;

            ResetDrawnArea();
            if (element != null)
            {
                Shape = element.Name;
                if (Buffer is IContainer)
                {
                    var path = (Buffer as IContainer).Controls;
                    if (path.Contains(element))
                        CopyFrom(path[element]);
                }
                Bounds = element.Bounds;
            }
            if (element is IRotatable)
                ReadAngle = (element as IRotatable).Angle;
            return true;
        }
        protected void End(IBuffer Buffer, IBufferPen reader)
        {
            Buffer.PendingUpdates?.Invalidate(DrawnArea);

            Writer = Buffer.ID;
            Reader = reader.ID;
            IsDrawingShape = false;

            if(Buffer is IContainer) 
            {
                var path = (Buffer as IContainer).Controls;
                if (Element != null && path.Contains(Element))
                    path[Element].CopyFrom(this);
                if (path.AddMode)
                {
                    Factory.Add(reader, ObjType.Buffer);
#if AdvancedVersion
                    if (Element is IControl)
                    {
                        var attachable = Element as IControl;
                        if (!attachable.IsWindow)
                            attachable.BufferChanged(Buffer, path);
                    }
#endif
                }
            }
            Flush();
        }
        #endregion

        #region APPLY AA
        public virtual void ApplyAA(float x0, float y0,  FillAction action, bool steep)
        {
            if (x0 < 0 || y0 < 0)
                return;

            var x = (int)x0;
            var y = (int)y0;

            if (LineDraw.HasFlag(LineDraw.NonAA))
            {
                action(x, x, y, true);
                return;
            }

            //var Alpha = !steep ? (y0 - y) : (x0 - x);
            //var invAlpha = 1 - Alpha;
            //action(x, y, invAlpha, 0);
            //action(x + ix, y + iy, Alpha, 0);
            //action(x - ix, y - iy, invAlpha, 0, ends);
        }

        #endregion

        #region COPY MEMORY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void CopyMemory(int* src, int srcIndex, int* dst, int destIndex, int length);
        #endregion

        #region READ PIXEL - LINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int ReadPixel(IBufferPen pen, int index) =>
            pen[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe virtual void ReadLine(IBufferPen pen, int val1, int val2, int axis, bool horizontal, out IntPtr pixels, out int srcIndex, out int length)
        {
            MathHelper.Order(ref val1, ref val2);
            length = 0;
            pixels = IntPtr.Zero;

            if (val1 < 0)
            {
                length = val1;
                val1 = 0;
            }
            length += val2 - val1;

            if (length == 0)
                length = 1;

            if (length < 0)
                goto mks;

            if (pen is ISolidPen)
            {
                fixed (int* p = (pen as ISolidPen).Colour.Repeat(length))
                    pixels = new IntPtr(p);
                srcIndex = 0;
                return;
            }

            if (IsRotated(Entity.BufferPen))
            {
                srcIndex = 0;
                var data = new int[length];
                int j = 0;
                fixed (int* d = data)
                {
                    for (int i = val1; i < val1 + length; i++)
                    {
                        d[j++] = pen.ReadPixel(i, axis, horizontal);
                    }
                    pixels = new IntPtr(d);
                    return;
                }
            }

            srcIndex = pen.IndexOf(val1, axis, horizontal);

            if (srcIndex < 0)
                goto mks;

            if (pen is IBrush)
            {
                var brush = pen as IBrush;

                if (horizontal)
                {
                    switch (brush.Gradient)
                    {
                        case Gradient.Vertical:
                        case Gradient.VerticalCentral:
                            fixed (int* p = ReadPixel(brush, brush.IndexOf(val1, axis, horizontal)).Repeat(length))
                                pixels = new IntPtr(p);
                            srcIndex = 0;
                            break;
                        default:
                            pixels = brush.Pixels;
                            break;
                    }

                    if (brush.Gradient != Gradient.Vertical && brush.Gradient != Gradient.VerticalCentral)
                    {
                        if (srcIndex + length > brush.Length)
                        {
                            var difference = srcIndex + length - brush.Length;
                            length -= difference;
                        }
                    }
                    if (pixels == IntPtr.Zero || length < 0)
                        goto mks;
                    return;
                }
                else
                {
                    var i = srcIndex;
                    srcIndex = 0;
                    var data = new int[length];

                    fixed (int* d = data)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            d[j] = brush[i];
                            i = brush.IndexOf(axis, ++val1, true);
                        }
                        pixels = new IntPtr(d);
                    }
                    return;
                }
            }

            if (pen is IBufferData)
            {
                var data = new int[length];
                var srcIdx = srcIndex;
                srcIndex = 0;
                int incr;
                incr = horizontal ? 1 : pen.Width;
                var buffer = pen as IBufferData;

                fixed (int* dest = data)
                {
                    if (horizontal)
                        CopyMemory((int*)buffer.Pixels, srcIdx, dest, 0, data.Length);
                    else
                    {
                        int* src = (int*)buffer.Pixels;
                        for (int j = 0; j < length; j++)
                        {
                            dest[j] = src[srcIdx];
                            srcIdx += incr;
                        }
                    }
                    pixels = new IntPtr(dest);
                }
                return;
            }
            else
            {
                var data = new int[length];
                var srcIdx = srcIndex;
                srcIndex = 0;
                int incr;
                incr = horizontal ? 1 : pen.Width;
                fixed (int* dest = data)
                {
                    for (int j = 0; j < length; j++)
                    {
                        dest[j] = pen[srcIdx];
                        srcIdx += incr;
                    }
                }
                return;
            }
        mks:
            srcIndex = 0;
            length = 0;
        }
        #endregion

        #region RENDER AXIS PIXEL - LINE
        public abstract void RenderPixel(IBuffer target, int index, int color, bool blend, float? delta = null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void RenderLine(IBuffer target, IBufferPen pen, int destVal, int destAxis, int start, int end, int axis, bool horizontal, float? delta = null);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public virtual void RenderPixel(IBufferIndex Target, FillAction fillAction, float x, float y, int color, bool blend = true)
        //{
        //    var x0 = (int)x;
        //    var y0 = (int)y;

        //    int i = Target.IndexOf(x0, y0, true);

        //    if (i == -1 || !IsPixelWriteable(i))
        //        return;

        //    if (x0 == x && y0 == y)
        //    {
        //        RenderPixel(Target, i, color, blend);
        //        return;
        //    }
        //    var alpha1 = x - x0;
        //    var invAlpha1 = 1 - alpha1;

        //    var alpha2 = y - y0;
        //    var invAlpha2 = 1 - alpha2;

        //    var x1 = x.Ceiling();
        //    var y1 = y.Ceiling();

        //    RenderPixel(Target, i, color, blend, invAlpha1 * invAlpha2);

        //    if (x1 != x0)
        //    {
        //        ++i;
        //        RenderPixel(Target, i, color, blend, alpha1 * invAlpha2);
        //        --i;
        //    }
        //    if (y1 != y0)
        //    {
        //        i += Target.Width;
        //        RenderPixel(Target, i, color, blend, invAlpha1 * alpha2);
        //    }
        //    ++i;
        //    RenderPixel(Target, i, color, blend, alpha1 * alpha2);
        //}
        #endregion

        #region COMPARE PIXEL WITH PEN IT WAS DRAWN BY
        public virtual bool ComparePixel(IBuffer buffer, int x, int y, IBufferPen reader)
        { 
            var i = buffer.IndexOf(x, y, true);
            var j = reader.IndexOf(x, y, true);
            return reader[j] == buffer[i];
        }
        #endregion

        #region RENDER IMAGE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual unsafe void RenderImage(IBuffer Target, int* source, int srcLen, int srcW, int destX, int destY,
            int? copyX = null, int? copyY = null, int? copyW = null, int? copyH = null)
        {
            ResetDrawnArea();
            var rc = Geometry.CompitibleRC(srcW, srcLen / srcW, copyX, copyY, copyW, copyH);
            var length = Target.Length;
            IRectangle destRc = null;

            int* dest = (int*)Target.Pixels;
            var src = source;

            var destinationRc = CopyBlock(srcLen, rc, srcW, destX, destY, Target.Width, length,
                (si, di, w, i) => CopyMemory(src, si, dest, di, w));

            if (destRc.Width > 0 && destRc.Height > 0)
            {
                NotifyDrawArea(destRc.X, destRc.Y, destRc.Right, destRc.Bottom);
                if (Target is IUpdateTracker)
                    (Target as IUpdateTracker).PendingUpdates.Invalidate(DrawnArea);
                ResetDrawnArea();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual unsafe void RenderImage(IBuffer Target, IBufferCopy source, int destX, int destY, int srcX, int srcY, int copyW, int copyH)
        {
            ResetDrawnArea();
            var rc = source.CompitibleRC(srcX, srcY, copyW, copyH);
            int length = Target.Length;
            IRectangle destRc = null;

            var dest = source.CopyTo(rc, Target.Pixels, length, Target.Width, destX, destY);
            destRc = dest;

            if (destRc.Width > 0 && destRc.Height > 0)
            {
                NotifyDrawArea(destRc.X, destRc.Y, destRc.Right, destRc.Bottom);
                if (Target is IUpdateTracker)
                    (Target as IUpdateTracker).PendingUpdates.Invalidate(DrawnArea);
                ResetDrawnArea();
            }
        }
        #endregion

        #region RENDER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Render(IBuffer Buffer, IEnumerable<PointF> points, string shapeType, Angle angle, IRectangleF originalArea, IPenContext context = null)
        {
            var shape = new Shape(points, shapeType, angle, originalArea);
            RenderShape(Buffer, shape, context);
        }

#if AdvancedVersion
        public abstract void Render(IBuffer Buffer, IElement shape, IPenContext context, int? drawX, int? drawY);
#else
        public abstract void Render(IBuffer Buffer, IElement element, IPenContext context);
#endif
        #endregion

        #region RENDER ELEMENT
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RenderElement(IBuffer Buffer, IElement element, IPenContext context)
        {
            if (element is ICurve)
                RenderCurve(Buffer, element as ICurve, context);

            else if (element is IText)
                RenderText(Buffer, element as IText, context);

            else if (element is IShape)
                RenderShape(Buffer, element as IShape, context);

            else if (element is IDrawable)
            {
                if (!Begin(Buffer, element))
                    return;
                var reader = GetPen(element, context);
                RenderPrimitive(Buffer, element as IDrawable, reader);
                End(Buffer, reader);
            }
            else
            {
                if (!Begin(Buffer, element))
                    return;
                bool ok = RenderCustom(Buffer, element, context, out IBufferPen reader);
                if (!ok)
                    throw new Exception(element.Name + " ID: " + element.ID +
                        " can not get rendered! please provide rendereing routine by overriding RenderCustom method");
                End(Buffer, reader);
            }
        }
        #endregion

        #region RENDER CUSTOM
        /// <summary>
        /// Rotates a custom user defined shape. Drawing routine must be provided  here for any custom element 
        /// that you have created and you are aware of. You must return true once you have handled rendering the element.
        /// Otherwise an exception will get thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <param name="reader">You must have created valid readable source out of read context supplied </param>
        /// <returns></returns>
        public abstract bool RenderCustom(IBuffer Buffer, IElement element, IPenContext context, out IBufferPen reader);
        #endregion

        #region RENDER SHAPE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void RenderShape(IBuffer Buffer, IShape shape, IPenContext context = null)
        {
            if (IsDisposed)
                throw new Exception("Renderer is disposed!");

            if (!Begin(Buffer, shape))
                return;

            IsDrawingShape = true;

            float x, y;
            x = y = 0;
            IEnumerable<PointF> Original = shape;
            IList<PointF> Outer, Inner;

            if (WriteAngle.Valid)
                Original = GetTransformedPoints(Entity.Buffer, shape);

            if (Stroke == 0)
            {
                if (Original is IList<PointF>)
                    Outer = Inner = Original as IList<PointF>;
                else
                    Outer = Inner = Original.ToList();
            }
            else
            {
                var pair = Original.StrokePoints(Shape, Stroke, StrokeMode);
                Outer = pair.Item1;
                Inner = pair.Item2;

                if (ReadAngle.Valid)
                {
                    Bounds.GetStrokeAreas(out IRectangleF o, out IRectangleF i, Stroke, StrokeMode);
                    Bounds = o.Hybrid(i);
                }
                else
                    Bounds = Outer.HybridBounds(Inner);
            }

            var Reader = GetPen(this, context);

            if (ReadAngle.Skew)
            {
                ReadAngle.RotationDifferce(Bounds.X, Bounds.Y, Bounds.Width / 2f, Bounds.Height / 2f, out x, out y);
                SetXY(Entity.Buffer, X + (int)x, Y + (int)y);
            }

            FillMode = GetFillMode(FillMode, Shape, Stroke);

            var Action = CreateFillAction(Buffer, Reader);
            var Draw = LineDraw | GetLineDraw(Shape);

            var Data = GetDrawParams(Original, Outer, Inner);

            GetLineSkip(Shape, Data[1] != null, FillMode, out LineSkip skip0, out LineSkip skip2);

            if (Data[0] != null)
                ProcessLines(Data[0], Action, Draw, skip0);

            if (Data[2] != null)
                ProcessLines(Data[2], Action, Draw, skip2);


            if (Data[1] != null)
            {
                var o = Outer.ToArea().Hybrid(Inner.ToArea());
                Fill(Buffer, Data[1], Reader, HorizontalScan ? (int)(o.Y) : (int)(o.X),
                    HorizontalScan ? (int)(o.Bottom + .5f) : (int)(o.Right + .5f), HorizontalScan, Shape);
            }

            End(Buffer, Reader);

            //if (Name != "Box" && Name != "BoxF")
            //    OA.DrawBoundingBox(Path.Writer, Reader);
        }
        #endregion

        #region RENDER PRIMITIVE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void RenderPrimitive(IBuffer Buffer, IDrawable shape, IBufferPen Reader)
        {
            shape.DrawTo(Buffer, Reader);
            End(Buffer, Reader);
        }
        #endregion

        #region GET READER
        public IBufferPen GetPen(IOccupier shape, IPenContext context)
        {
            bool enabled = true;

            if(shape is IVisible)
                enabled = (shape as IVisible).Enabled;

            if (!enabled)
                return Factory.DisabledPen;

            int x, y, w, h;
            IBufferPen reader;
            IRectangleF Rc = shape.Bounds;

            Rc.Expand(out x, out y, out w, out h);
            if (context is IBufferPen)
            {
                reader = context as IBufferPen;
                goto mks;
            }

            if (context is IPoint)
                CopyFrom(context as IPoint);

            reader = context?.ToPen(w + 1, h + 1);
            if (reader == null)
                reader = ReadContext.ToPen(w + 1, h + 1);

            if (shape is IRotatable && !ReadAngle.Valid)
            {
                ReadAngle = (shape as IRotatable).Angle.AssignCenter(Rc);
            }

        mks:
            --x;
            --y;
            SetXY(Entity.BufferPen, -x, -y);
            return reader;
        }
        public IBufferPen ToPen(int? width = null, int? height = null) =>
            ReadContext.ToPen(width, height);
        #endregion

        #region FILL
        static Dictionary<int, List<float>> scanLines;
        protected static Dictionary<int, List<float>> ScanLines
        {
            get
            {
                if (scanLines == null)
                    scanLines = new Dictionary<int, List<float>>(7000);
                return scanLines;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe virtual void Fill(IBuffer Buffer, IEnumerable<ILine> lines, IBufferPen pen, int start, int end, bool horizontal, 
            string shapeType, PolyFill fillType = PolyFill.OddEven)
        {
            if (Buffer == null || lines == null)
                return;
            ScanLines.Clear();
            var scanLineAction = Renderer.CreateLineAction(ScanLines);
            Predicate<ILine> lineCondition;
            if (horizontal)
                lineCondition = (l) => l.IsValid && !l.IsPoint && !l.IsHorizontal;
            else
                lineCondition = (l) => l.IsValid && !l.IsPoint && !l.IsVertical;

            ProcessLines(lines.Where(l => lineCondition(l)), scanLineAction, start, end, horizontal);
            for (int axis = start; axis <= end; axis++)
            {
                if (!ScanLines.ContainsKey(axis))// || axis !=50)
                    continue;
                Buffer.WriteLine(ScanLines[axis], axis, horizontal, pen, fillType);
            }
        }

        //static List<float> vals = new List<float>(100);
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public override unsafe void Fill(IEnumerable<ILine> lines, IPen pen, int start, int end, bool horizontal, string shapeType)
        //{
        //    if (lines == null)
        //        return;

        //    Predicate<ILine> lineCondition;
        //    if (horizontal)
        //        lineCondition = (l) => l.IsValid && !l.IsPoint && !l.IsHorizontal;
        //    else
        //        lineCondition = (l) => l.IsValid && !l.IsPoint && !l.IsVertical;

        //    Func<ILine, int, bool> fillCondition;
        //    if (horizontal)
        //        fillCondition = (l, axis) => axis >= l.MinY.Ceiling() && axis < l.MaxY.Ceiling();
        //    else
        //        fillCondition = (l, axis) => axis >= l.MinX.Ceiling() && axis < l.MaxX.Ceiling();

        //    var sides = lines.Where(l => lineCondition(l));
        //    vals.Clear();

        //    for (int axis = start; axis <= end; axis++)
        //    {
        //        //if (!MathHelper.IsWithIn(163, 163, axis)) continue;
        //        var xList = sides.Where(l => fillCondition(l, axis)).
        //            Select((l) => horizontal ? (axis - l.C) / l.M : l.M * axis + l.C).ToArray();

        //        Writer.WriteLine(xList, axis, horizontal, pen);
        //    }
        //}
        #endregion

        #region STROKES
        protected virtual IEnumerable<ILine>[] GetDrawParams(IEnumerable<PointF> Original, IList<PointF> Outer, IList<PointF> Inner)
        {
            var mode = FillMode;
            IEnumerable<ILine>[] Data = new IEnumerable<ILine>[3];
            float stroke = mode == FillMode.Original ? 0 : Stroke;

            IList<ILine> outer, inner, outer1, inner1;
            outer1 = inner1 = null;

            var avoidClosingEnds = (mode == FillMode.Outer || mode == FillMode.Inner);
            avoidClosingEnds = avoidClosingEnds && (Shape == "Bezier");
            if (stroke == 0)
            {
                var join = PointJoin.ConnectEach | GetStrokeJoin(Shape);

                outer = Geometry.ToLines(Original, join);
                inner = null;

                if (DontJoinPointsIfTooClose(Shape))
                {
                    join |= PointJoin.AvoidTooClose;
                    outer1 = Geometry.ToLines(Original, join);
                    inner1 = null;
                }
            }
            else
            {
                Geometry.GetLines(Shape, Outer, Inner, stroke, out outer, out inner, false, avoidClosingEnds);
                if (DontJoinPointsIfTooClose(Shape))
                    Geometry.GetLines(Shape, Outer, Inner, stroke, out outer1, out inner1, true, avoidClosingEnds);
                //if (NoeedToSwapPerimeters())
                //    MathHelper.Swap(ref Outer, ref inner);
            }

            if (Stroke == 0 && mode == FillMode.Inner)
                mode = FillMode.Original;

            switch (mode)
            {
                case FillMode.Outer:
                case FillMode.Original:
                default:
                    Data[0] = (outer1 ?? outer);
                    switch (Shape)
                    {
                        case "Bezier":
                        case "Arc":
                        case "BezierArc":
                        case "ClosedArc":
                            return Data;
                        default:
                            break;
                    }
                    Data[1] = outer;
                    break;
                case FillMode.FillOutLine:
                    Data[0] = (outer1 ?? outer);
                    Data[2] = (inner1 ?? inner);
                    if (Shape == "Bezier" && stroke == 0)
                        return Data;
                    Data[1] = outer.AppendItems(inner);
                    break;
                case FillMode.Inner:
                    Data[0] = (inner1 ?? inner);
                    switch (Shape)
                    {
                        case "Bezier":
                        case "Arc":
                        case "BezierArc":
                        case "ClosedArc":
                            return Data;
                        default:
                            break;
                    }
                    Data[1] = inner;
                    break;
                case FillMode.DrawOutLine:
                    Data[0] = (outer1 ?? outer);
                    Data[2] = (inner1 ?? inner);
                    break;
                case FillMode.ExceptOutLine:
                    Data[0] = (outer1 ?? outer);
                    Data[2] = (inner1 ?? inner);
                    switch (Shape)
                    {
                        case "Bezier":
                        case "Arc":
                        case "BezierArc":
                        case "ClosedArc":
                            return Data;
                        default:
                            break;
                    }
                    Data[1] = inner;
                    break;
            }
            return Data;
        }
        public virtual PointJoin GetStrokeJoin(string Name)
        {
            switch (Name)
            {
                case "Polygon":
                    return PointJoin.PolygonJoin;
                case "BezierArc":
                case "Arc":
                    return PointJoin.ArcJoin;
                case "ClosedArc":
                    return PointJoin.CloseArcJoin;
                case "Pie":
                case "BezierPie":
                    return PointJoin.PieJoin;
                case "Bezier":
                case "Line":
                    return PointJoin.ConnectEach;
                default:
                    return PointJoin.CircularJoin;
            }
        }
        public virtual bool DontJoinPointsIfTooClose(string Name)
        {
            switch (Name)
            {
                case "Arc":
                case "ClosedArc":
                case "Pie":
                case "Bezier":
                case "BezierArc":
                case "Ellipse":
                case "Circle":
                    return true;
                default:
                    return false;
            }
        }
        public virtual AfterStroke GetAfterStroke(string Name)
        {
            switch (Name)
            {
                case "Triangle":
                case "Trapazoid":
                case "Rhombus":
                case "Trapezium":
                case "Polygon":
                case "Pie":
                case "BezierPie":
                case "Box":
                case "BoxF":
                case "ClosedArc":
                default:
                    return AfterStroke.Reset1st;
                case "BezierArc":
                case "Arc":
                case "Bezier":
                case "Line":

                    return AfterStroke.JoinEnds;
                case "Ellipse":
                case "Circle":
                    return 0;
            }
        }
        public virtual LineDraw GetLineDraw(string Name)
        {
            switch (Name)
            {
                case "Bezier":
                case "Arc":
                case "Pie":
                case "Circle":
                case "Ellipse":
                case "BezierArc":
                case "BezierPie":
                    return LineDraw.Distinct;
                default:
                    return LineDraw.HVBreshenham;
            }
        }
        public virtual void GetLineSkip(string Name, bool willbeFilling, FillMode mode, out LineSkip forData0, out LineSkip forData2)
        {
            forData0 = forData2 = 0;
        }
        public virtual bool NoeedToSwapPerimeters(string Name)
        {
            switch (Name)
            {
                case "Bezier":
                    return true;
                default:
                    return false;
            }
        }
        public virtual FillMode GetFillMode(FillMode current, string Name, float Stroke)
        {
            if (Stroke != 0 && Name == "Line")
            {
                if (current != FillMode.DrawOutLine)
                    return FillMode.DrawOutLine;
            }

            if (Stroke == 0 && (Name == "Beizer" || Name == "BezierArc"))
            {
                switch (FillMode)
                {
                    case FillMode.Original:
                    case FillMode.Inner:
                    case FillMode.FillOutLine:
                    case FillMode.ExceptOutLine:
                    case FillMode.Outer:
                        return FillMode.DrawOutLine;
                    default:
                        return current;
                }
            }
            return current;
        }
        #endregion

        #region LOCATION SETTINGS
        public void FreezeXY(Entity target, bool value)
        {
            if (target == Entity.BufferPen)
                freezeReaderXY = value;
            else
                freezeWriterXy = value;
        }
        public void SetXY(Entity target, int? x, int? y)
        {
            if (target == Entity.BufferPen)
            {
                if (freezeReaderXY)
                    return;
                if (x != null)
                    ReadX = x.Value;
                if (y != null)
                    ReadY = y.Value;
            }
            else
            {
                if (freezeWriterXy)
                    return;
                if (x != null)
                    X = x.Value;
                if (y != null)
                    Y = y.Value;
            }
        }
        #endregion

        #region GET AXIS LINE INFO
        protected bool GetAxisLineInfo(IBuffer Target, ref int destVal, ref int destAxis, ref int val1, ref int val2, bool horizontal, out int destIndex, out int length)
        {
            length = 0;
            destIndex = -1;
            var drawVal = horizontal ? Renderer.X : Renderer.Y;
            var drawAxis = horizontal ? Renderer.Y : Renderer.X;

            if (val1 == int.MinValue && val2 == int.MinValue)
                return false;

            var maxAllowed = horizontal ? Target.Width : Target.Height;

            if (destAxis + drawAxis < 0)
                return false;

            MathHelper.Order(ref val1, ref val2);
            length = val2 - val1;
            val1 = Math.Max(val1, 0);

            var EffectiveVal = destVal + drawVal;

            if (EffectiveVal < 0)
            {
                length += EffectiveVal;
                destVal = -drawVal;
                val1 = destVal;
                EffectiveVal = val1;
            }

            destIndex = Target.IndexOf(destVal, destAxis, horizontal);

            if (destIndex < 0)
                return false;
            destVal += drawVal;
            destAxis += drawAxis;

            if (EffectiveVal + length > maxAllowed)
                length -= (EffectiveVal + length - maxAllowed);
            if (length < 0)
                return false;
            return true;
        }
        #endregion

        #region CORRECT XY
        public void CorrectXY(Entity target, ref int x, ref int y)
        {
            if (target == Entity.BufferPen)
            {
                x += ReadX;
                y += ReadY;
                if (x < 0)
                    x = 0;
                if (y < 0)
                    y = 0;
            }
            else
            {
                x += X;
                y += Y;
            }
        }
        #endregion

        #region FLUSH
        public override void Flush()
        {
            ReadX = ReadY = 0;
            IsRendering = false;
            Bounds = null;
            Element = null;
            HorizontalScan = true;
            ReadAngle = Angle.Empty;
            ResetDrawnArea();
            Reader = null;
            Writer = null;
            Shape = null;
#if AdvancedVersion
            DrawMode = 0;
#endif
            if (distinctLineProcessing)
                DistinctLineProcessing = false;
            base.Flush();
        }
        #endregion

        #region ROTATE
        public bool IsRotated(Entity caller)
        {
            if (caller == Entity.BufferPen)
                return ReadAngle.Valid;
            return !IsDrawingShape && WriteAngle.Valid;
        }
        public void GetRotatedXY(Entity caller, float val, float axis, bool horizontal, out float x, out float y)
        {
            x = horizontal ? val : axis;
            y = horizontal ? axis : val;

            if (caller == Entity.BufferPen)
            {
                ReadAngle.Rotate(x, y, out x, out y, true);
            }
            else
            {
                WriteAngle.Rotate(x, y, out x, out y, false);
                if (x < -1 || y < -1)
                    return;
            }
            if (x < 0) x = 0;
            if (y < 0) y = 0;
        }
        public void RotateTransform(Entity target, Angle angle)
        {
            if (target == Entity.BufferPen)
                ReadAngle = angle;
            else
                WriteAngle = angle;
        }
        public void ResetTransform(Entity target)
        {
            if (target == Entity.BufferPen)
                ReadAngle = Angle.Empty;
            else
                WriteAngle = Angle.Empty;
        }
        protected IEnumerable<PointF> GetTransformedPoints(Entity caller, IEnumerable<PointF> source)
        {
            if (caller == Entity.Buffer && WriteAngle.Valid)
                return source.Rotate(WriteAngle);
            if (caller == Entity.BufferPen && ReadAngle.Valid)
                return source.Rotate(ReadAngle);
            return source;
        }
        #endregion

        #region RESET DRAWING AREA
        protected void ResetDrawnArea()
        {
            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = 0;
            maxY = 0;
        }
        protected void NotifyDrawPoint(int x, int y)
        {
            if (x < minX)
                minX = x;
            if (y < minY)
                minY = y;
            if (x > maxX)
                maxX = x;
            if (y > maxY)
                maxY = y;

        }
        protected void NotifyDrawArea(int x, int y, int right, int bottom)
        {
            NotifyDrawPoint(x, y);
            NotifyDrawPoint(right, bottom);
        }
        protected void NotifyAxisLine(int val, int axis, bool horizontal, int length)
        {

            var x = horizontal ? val : axis;
            var y = horizontal ? axis : val;
            var x1 = horizontal ? x + length : x;
            var y1 = horizontal ? y : y + length;
            NotifyDrawArea(x, y, x1, y1);
        }
        #endregion

        #region MISC
        //protected void CutPoints(PointF cutPointA, PointF cutPointB, int start, int end, ref IList<PointF> Inner, ref IList<PointF> Outer, FigureCut cut = FigureCut.Inner)
        //{
        //if (Shape != "Pie")
        //return;
        //{
        //    int start = 2;

        //    if (Math.Abs(Inner[0].Y - Inner[1].Y) > Geometry.EPSILON)
        //        start = 1;
        //    CutPoints(Inner[1], Inner[Inner.Count - 1], start, Inner.Count - 1, FigureCut.Inner);
        //}
        //    var l = Factory.newLine(cutPointA, cutPointB);
        //    IList<PointF> list;

        //    if (cut == FigureCut.Inner)
        //        list = Inner;
        //    else
        //        list = Outer;

        //    for (int j = start; j < end; j++)
        //    {
        //        if (j >= list.Count)
        //            break;
        //        if (list[j] == null)
        //            continue;
        //        if (!l.IsGreaterThan(list[j].X, list[j].Y))
        //            list[j] = PointF.Empty;
        //    }
        //    list = list.Where(x => x != null).ToList();

        //    if (cut == FigureCut.Inner)
        //        Inner = list;
        //    else
        //        Outer = list;
        //}
        #endregion

        #region RENDER LINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void RenderLine(IBuffer Buffer, float x1, float y1, float x2, float y2, IPenContext context = null, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;

            if (angle.Valid)
            {
                Angle a;

                if (angle.CenterAssigned)
                    a = angle;
                else
                    a = new Angle(angle, MathHelper.Middle(x1, x2), MathHelper.Middle(y1, y2));

                a.Rotate(x1, y1, out x1, out y1);
                a.Rotate(x2, y2, out x2, out y2);
            }

            float DeltaX, DeltaY, m, c;
            if (!Geometry.GetDrawableLinePoints(ref x1, ref y1, ref x2, ref y2, out DeltaX, out DeltaY, out m, out c))
                return;

            var minX = Math.Min(x1, x2);
            var minY = Math.Min(y1, y2);
            var maxX = Math.Max(x1, x2);
            var maxY = Math.Max(y1, y2);

            var w = (maxX - minX).Ceiling();
            var h = (maxY - minY).Ceiling();
            if (w == 0)
                w = 1;
            if (h == 0)
                h = 1;

            if (Stroke != 0 && FillMode != FillMode.Original)
            {
                Render(Buffer, new PointF[] { new PointF(x1, y1), new PointF(x2, y2) }, "Line", angle, Factory.newRectangleF(minX, minY, w, h), context);
                return;
            }

            if (!Begin(Buffer, null))
                return;

            var Reader = context?.ToPen(w, h);
            if (Reader == null)
                Reader = ReadContext.ToPen(w, h);

            var action = CreateFillAction(Buffer, Reader);
            ProcessLine(x1, y1, x2, y2, m, c, h > w, LineDraw, action);
            End(Buffer, Reader);
        }
        public virtual void RenderLine(IBuffer Buffer, ILine line, IPenContext context)
        {
            if (line == null || !line.IsValid || !Begin(Buffer, line))
                return;

            if (!NoStroke)
            {
                if (FillMode != FillMode.DrawOutLine)
                    FillMode = FillMode.FillOutLine;

                Render(Buffer, line, "Line", Angle.Empty, line.Bounds, context);
                return;
            }

            if (!Begin(Buffer, line))
                return;

            var Reader = GetPen(line, context);

            if (!AntiAlised || line.DX == 0 || line.DY == 0)
            {
                var x = line.X1.Round();
                var y = line.Y1.Round();
                if (line.DX == 0 && line.DY == 0)
                {
                    Buffer.WritePixel(x, y, true, Reader.ReadPixel(x, y, true), true);
                    return;
                }
                ProcessBresenhamLine(Buffer, x, y, line.X2.Round(), line.Y2.Round(), Reader);
                return;
            }

            int step = LineDraw.HasFlag(LineDraw.Dotted) ? 2 : 1;
            int s, e;

            s = (line.Steep ? line.MinY : line.MinX).Round();
            e = (line.Steep ? line.MaxY : line.MaxX).Round();

            if (s == e)
            {
                var x = line.Steep ? line.X1.Round() : line.X2.Round();
                var y = line.Steep ? line.Y1.Round() : line.Y2.Round();
                Buffer.WritePixel(x, y, true, Reader.ReadPixel(x, y, true), true);
                return;
            }

            for (var axis = s; axis <= e; axis += step)
            {
                var val = line.Steep ? (axis - line.C) / line.M : line.M * (axis) + line.C;
                Buffer.WritePixel(val, axis, line.Steep, Reader, true);
            }
            End(Buffer, Reader);
        }
        #endregion

        #region BRESENHAM LINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool ProcessBresenhamLine(int x1, int y1, int x2, int y2, FillAction action, LineDraw draw)
        {
            int step = 1;
            if (draw.HasFlag(LineDraw.Dotted))
                step = 2;

            int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? step : -step;
            int dy = -Math.Abs(y2 - y1), sy = y1 < y2 ? step : -step;
            int err = dx + dy, e2;
            for (; ; )
            {
                action(x1, x1, y1, true);
                e2 = 2 * err;
                if (e2 >= dy)
                {
                    if (x1 == x2)
                        break;
                    err += dy; x1 += sx;
                }
                if (e2 <= dx)
                {
                    if (y1 == y2)
                        break;
                    err += dx; y1 += sy;
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool ProcessBresenhamLine(IBuffer Buffer, int x1, int y1, int x2, int y2, IBufferPen pen)
        {
            if (!Begin(Buffer, null))
                return false;

            int step = 1;
            if (Renderer.LineDraw.HasFlag(LineDraw.Dotted))
                step = 2;

            int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? step : -step;
            int dy = -Math.Abs(y2 - y1), sy = y1 < y2 ? step : -step;
            int err = dx + dy, e2;
            bool ok = false;
            for (; ; )
            {
                Buffer.WritePixel(x1, y1, true, pen.ReadPixel(x1, y1, true), false);
                ok = true;
                e2 = 2 * err;
                if (e2 >= dy)
                {
                    if (x1 == x2)
                        break;
                    err += dy; x1 += sx;
                }
                if (e2 <= dx)
                {
                    if (y1 == y2)
                        break;
                    err += dx; y1 += sy;
                }
            }

            End(Buffer, pen);
            return ok;
        }
        #endregion

        #region PROCESS LINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void ProcessLines(IEnumerable<ILine> lines, FillAction action, LineDraw draw, LineSkip skip = LineSkip.None)
        {
            if (lines == null)
                return;

            if (draw.HasFlag(LineDraw.Distinct))
                DistinctLineProcessing = true;

            foreach (var l in lines)
            {
                if ((l.Steep && skip.Includes(LineSkip.Steep)) || (!l.Steep && skip.Includes(LineSkip.NonSteep)))
                    continue;
                ProcessLine(l, draw, action);
            }

            if (draw.HasFlag(LineDraw.Distinct))
                DistinctLineProcessing = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool ProcessLine(ILine line, LineDraw draw, FillAction action, float stroke = 0)
        {
            if (line == null || !line.IsValid || line.IsPoint)
                return false;
            if (!line.GetDrawablePoints(out float x1, out float y1, out float x2, out float y2))
                return false;
            return ProcessLine(line.X1, line.Y1, line.X2, line.Y2, line.M, line.C, line.Steep, draw, action, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool ProcessLine(float x1, float y1, float x2, float y2, float m, float c, bool steep, LineDraw draw,
            FillAction action, float stroke = 0)
        {
            if (!draw.HasFlag(LineDraw.AA) || x1 == x2)
                return ProcessBresenhamLine(x1.Round(), y1.Round(), x2.Round(), y2.Round(), action, draw);

            int step = draw.HasFlag(LineDraw.Dotted) ? 2 : 1;
            int s, e;

            s = (steep ? Math.Min(y1, y2) : Math.Min(x1, x2)).Round();
            e = (steep ? Math.Max(y1, y2) : Math.Max(x1, x2)).Round();
            if (s == e)
            {
                float val = steep ? x1 : y2;
                int axis = steep ? y1.Round() : x2.Round();
                action(val, val, axis, steep);
                return true;
            }
            for (var axis = s; axis <= e; axis += step)
            {
                var val = steep ? (axis - c) / m : m * (axis) + c;
                action(val, val, axis, steep);
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void ProcessLines(IEnumerable<ILine> lines, FillAction action, int start, int end, bool horizontal)
        {
            int min, max;
            if (horizontal)
            {
                foreach (var l in lines)
                {
                    min = Math.Max(l.MinY, start).Ceiling();
                    max = Math.Min(l.MaxY, end).Ceiling();

                    for (int i = min; i < max; i++)
                    {
                        var x = (i - l.C) / l.M;
                        action(x, x, i, true);
                    }
                }
            }
            else
            {
                foreach (var l in lines)
                {
                    min = Math.Max(l.MinX, start).Ceiling();
                    max = Math.Min(l.MaxX, end).Ceiling();

                    for (int i = min; i < max; i++)
                    {
                        var y = l.M * i + l.C;
                        action(y, y, i, false);
                    }
                }
            }
        }
        #endregion

        #region RENDER CIRCLE - ELLIPSE
        public virtual void RenderCircleOrEllipse(IBuffer Buffer, float x, float y, float width, float height, IPenContext context = null, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;
            var Angle = angle;
            if (WriteAngle.Valid)
            {
                Angle += WriteAngle;
                IsDrawingShape = true;
                curveModified = true;
            }
            curveModified = true;
            var curve = Factory.newCurve(x, y, width, height, Angle, assignID: false);
            RenderCurve(Buffer, curve, context);
            curveModified = false;
        }
        #endregion

        #region RENDER ARC - PIE
        public virtual void RenderArcOrPie(IBuffer Buffer, float x, float y, float width, float height,
            float startAngle, float endAngle, IPenContext context = null, Angle angle = default(Angle), CurveType type = CurveType.Pie)
        {
            if (Buffer == null)
                return;

            var Angle = angle;
            if (WriteAngle.Valid)
            {
                Angle += WriteAngle;
                IsDrawingShape = true;
                curveModified = true;
            }
            var curve = Factory.newCurve(x, y, width, height, Angle, startAngle, endAngle, type, false);
            RenderCurve(Buffer, curve, context);
            curveModified = false;
        }
        #endregion

        #region RENDER CURVE
        public virtual void RenderCurve(IBuffer Buffer, ICurve Curve, IPenContext context = null)
        {
            if (!Begin(Buffer, Curve))
                return;

            IBufferPen reader = null;
            var curve = Curve;

            if (!curveModified && WriteAngle.Valid)
            {
                curve = Factory.newCurve(Curve.Data.X, Curve.Data.Y, Curve.Data.Width,
                    Curve.Data.Height, Curve.Angle + WriteAngle, curve.StartAngle, Curve.EndAngle, Curve.Type, false);
                IsDrawingShape = true;
            }

            if (Renderer.Stroke == 0 || Renderer.FillMode == FillMode.Original)
            {
                reader = DrawCurve(curve, Buffer, context, FillMode == FillMode.DrawOutLine || Curve.Type.IsArc());
                goto last;
            }

            var outerCurve = Factory.newCurve(curve, Stroke, StrokeMode, FillMode);
            Bounds = outerCurve.Bounds;

            switch (FillMode)
            {
                case FillMode.Outer:
                case FillMode.Inner:
                    reader = DrawCurve(outerCurve, Buffer, context, outerCurve.Type.IsArc());
                    break;
                case FillMode.FillOutLine:
                    reader = DrawCurve(outerCurve, Buffer, context, false);
                    break;
                case FillMode.DrawOutLine:
                    reader = DrawCurve(outerCurve, Buffer, context, true);
                    break;
                case FillMode.ExceptOutLine:
                    reader = DrawCurve(outerCurve, Buffer, context, true);
                    DrawCurve(outerCurve.AttachedCurve, Buffer, context, false);
                    break;
                default:
                    break;
            }
        last:
            End(Buffer, reader);
        }
        IBufferPen DrawCurve(ICurve curve, IBuffer Buffer, IPenContext context = null, bool drawOutLinesOnly = false)
        {
            if (Buffer == null)
                return null;

            ReadAngle = curve.Angle;
            Bounds = curve.Bounds;

            var x = X;
            var y = Y;

            var pen = GetPen(curve, context);
            var aa = AntiAlised;

            var xStart = curve.GetDrawStart(false);

            if (Stroke == 0 && curve.Type.IsArc())
                drawOutLinesOnly = true;

            int a1, a2;
            PolyFill fill = PolyFill.OddEven;
            fill |= PolyFill.AAEnds;
            fill |= PolyFill.DrawEndsOnly;

            for (int i = (int)xStart; i >= 0; i -= 1)
            {
                var list = curve.GetDataAt(i, false, true, out a1, out a2);
                Buffer.WriteLine(list[0], a1, false, pen, fill);
                Buffer.WriteLine(list[1], a2, false, pen, fill);
            }

            var yMax = drawOutLinesOnly ? curve.GetDrawStart(true) : curve.GetMaxPosition(true);

            if (!drawOutLinesOnly)
                fill = fill.Exclude(PolyFill.DrawEndsOnly);

            for (float i = (int)yMax; i >= 0; i -= 1)
            {
                var list = curve.GetDataAt(i, true, drawOutLinesOnly, out a1, out a2);
                //if (a2 != 205)
                //    continue;
                Buffer.WriteLine(list[0], a1, true, pen, fill);
                Buffer.WriteLine(list[1], a2, true, pen, fill);
            }

            bool Empty = curve.Full;
            if (!Empty)
            {
                var action = CreateFillAction(Buffer, pen);
                var Draw = LineDraw;
                ProcessLines(curve.GetClosingLines(), action, Draw);
            }

            SetXY(Entity.Buffer, x, y);
            return pen;
        }
        #endregion

        #region RENDER BEZIER ARC - PIE
        public virtual void RenderBezierArcOrPie(IBuffer Buffer, float x, float y, float width, float height, float arcStart, float arcEnd,
            bool isArc, IPenContext context = null, Angle angle = default(Angle), bool noSweepAngle = false)
        {
            if (Buffer == null)
                return;
            var end = arcEnd;
            if (!noSweepAngle)
                end += arcStart;

            var ei = new EllipseData(x, y, width, height, angle);

            List<PointF> points = new List<PointF>(100);

            if (end > 179)
            {
                var pts = Geometry.GetBezier4Points(x, y, ei.Rx, ei.Ry, arcStart, 179);
                Geometry.GetBezierPoints(4, ref points, pts);
                pts = (Geometry.GetBezier4Points(x, y, ei.Rx, ei.Ry, 179, end));
                Geometry.GetBezierPoints(4, ref points, pts);
            }
            else
            {
                var pts = Geometry.GetBezier4Points(x, y, ei.Rx, ei.Ry, arcStart, end);
                Geometry.GetBezierPoints(4, ref points, pts);
            }
            IEnumerable<PointF> Points = points;

            if (ei.Angle.Valid)
                Points = points.Rotate(ei.Angle);

            if (!isArc)
                Points = Points.PrependItems(new PointF[] { new PointF(ei.Cx, ei.Cy) });

            Render(Buffer, Points, isArc ? "BezierArc" : "BezierPie", angle, Factory.newRectangleF(x, y, width, height), context);
        }
        #endregion

        #region RENDER BEZIER
        public virtual void RenderBezier(IBuffer Buffer, IEnumerable<float> pts, BezierType type = BezierType.Cubic, IPenContext context = null, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;

            var original = pts.ToPoints();
            var rc = original.ToArea();
            angle = angle.AssignCenter(rc);
            var Source = original.Rotate(angle);

            bool multiple = type.HasFlag(BezierType.Multiple);

            var points = new List<PointF>(100);

            if (multiple)
            {
                for (int k = 1; k < Source.Count; k++)
                {
                    if (k % 3 == 0)
                    {
                        Geometry.GetBezierPoints(4, ref points, new PointF[] { Source[k - 3], Source[k - 2], Source[k - 1], Source[k] });
                        if (points.Count > 0)
                            points.RemoveAt(points.Count - 1);
                    }
                }
            }
            else
                Geometry.GetBezierPoints(4, ref points, Source);

            Render(Buffer, points, "Bezier", angle, rc, context);
        }
        #endregion

        #region RENDER TRIANGLE
        public virtual void RenderTriangle(IBuffer Buffer, float x1, float y1, float x2, float y2, float x3, float y3, IPenContext context = null, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;
            IList<PointF> points = new PointF[]
            {
                new PointF(x1, y1),
                new PointF(x2, y2),
                new PointF(x3, y3)
            };

            var rc = points.ToArea();
            angle = angle.AssignCenter(rc, true);
            points = points.Rotate(angle);
            Render(Buffer, points, "Triangle", angle, rc, context);
        }
        #endregion

        #region RENDER POLYGON
        public virtual void RenderPolygon(IBuffer Buffer, IEnumerable<float> polyPoints, IPenContext context = null, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;
            IList<PointF> points = polyPoints.ToPoints();
            var rc = points.ToArea();
            angle = angle.AssignCenter(rc);
            points = points.Rotate(angle);
            Render(Buffer, points, "Polygon", angle, rc, context);
        }
        #endregion

        #region RENDER RECTANGLE - ROUNDED AREA
        public virtual void RenderRectangle(IBuffer Buffer, float x, float y, float width, float height, IPenContext context = null, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;

            if (!angle.Valid && !IsRotated(Entity.Buffer))
            {
                var r = x + width;
                var b = y + height;

                if (StrokeMode == StrokeMode.Middle || FillMode == FillMode.Original)
                    goto mks;
                var s = -Stroke / 2f;
                if (StrokeMode == StrokeMode.Inner)
                    s = -s;
                x += s;
                y += s;
                r -= s;
                b -= s;
            mks:
                PathHelper.RenderRectangleFromLTRB(x, y, r, b, Buffer, context);
                return;
            }

            angle = angle.AssignCenter(x, y, width, height);

            var data =  Geometry.GetTrapeziumData(Factory.newLine(x, y, x, y + height), width, angle, StrokeMode.Outer);
            Render(Buffer, data.Points, angle.Valid ? "Rhombus" : "Box", angle, Factory.newRectangleF(x, y, width, height), context);
        }
        protected static void RenderRectangleFromLTRB(float x, float y, float r, float b, IBuffer Buffer, IPenContext context, float? Stroke = null, FillMode? fillMode = null)
        {
            if (Buffer == null)
                return;

            var mode = fillMode ?? Renderer.FillMode;
            var stroke = Stroke ?? Renderer.Stroke;
            IBufferPen pen;
            if (stroke == 0 || mode == FillMode.Original || (stroke == 0 && mode == FillMode.DrawOutLine))
            {
                pen = Renderer.GetPen(Factory.RectangleFFromLTRB(x, y, r, b), context);
                if (mode == FillMode.DrawOutLine)
                {
                    mode = Renderer.FillMode;
                    Renderer.CopySettings(fillMode: FillMode.Original);

                    Buffer.WriteLine(x.Round(), r.Round(), y.Round(), true, pen);
                    Buffer.WriteLine(x.Round(), r.Round(), b.Round(), true, pen);
                    Buffer.WriteLine(y.Round(), b.Round(), x.Round(), false, pen);
                    Buffer.WriteLine(y.Round(), b.Round(), r.Round(), false, pen);
                    Renderer.CopySettings(fillMode: mode);
                }
                else
                {
                    for (int i = (int)y; i <= b; i++)
                        Buffer.WriteLine(x, r, i, true, pen, Ends.NoDraw);
                }
                return;
            }

            float outer = (stroke / 2f);
            IRectangleF Area = Factory.RectangleFFromLTRB(x - outer, y - outer, r + outer, b + outer);
            pen = Renderer.GetPen(Area, context);
            Renderer.FreezeXY(Entity.BufferPen, true);

            switch (mode)
            {
                case FillMode.Outer:
                default:
                    RenderRectangleFromLTRB(x - outer, y - outer, r + outer, b + outer, Buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
                case FillMode.FillOutLine:
                    RenderRectangleFromLTRB(x - outer, y - outer, x + outer, b + outer, Buffer, pen, 0);
                    RenderRectangleFromLTRB(r - outer, y - outer, r + outer, b + outer, Buffer, pen, 0);
                    RenderRectangleFromLTRB(x, y - outer, r, y + outer, Buffer, pen, 0);
                    RenderRectangleFromLTRB(x, b - outer, r, b + outer, Buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
                case FillMode.Inner:
                    RenderRectangleFromLTRB(x + outer, y + outer, r - outer, b - outer, Buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
                case FillMode.DrawOutLine:
                    RenderRectangleFromLTRB(x - outer, y - outer, r + outer, b + outer, Buffer, pen, 0);
                    RenderRectangleFromLTRB(x + outer, y + outer, r - outer, b - outer, Buffer, pen, 0);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return; ;
                case FillMode.ExceptOutLine:
                    RenderRectangleFromLTRB(x - outer, y - outer, r + outer, b + outer, Buffer, pen, 0, FillMode.DrawOutLine);
                    RenderRectangleFromLTRB(x + outer, y + outer, r - outer, b - outer, Buffer, pen, 0, FillMode.Original);
                    Renderer.FreezeXY(Entity.BufferPen, false);
                    return;
            }
        }
        public virtual void RenderRoundedBox(IBuffer Buffer, float x, float y, float width, float height, float cornerRadius, IPenContext context, Angle angle = default(Angle))
        {
            if (Buffer == null)
                return;

            IList<PointF> points;

            var pts = new List<PointF>(50);

            Geometry.GetBezierPoints(4, ref pts,
                new PointF[]
                {
                        new PointF(x, y + cornerRadius),
                        new PointF(x, y),
                        new PointF(x + cornerRadius, y)
                });

            Geometry.GetBezierPoints(4, ref pts,
                new PointF[]
                {
                        new PointF(x + width - cornerRadius, y),
                        new PointF(x + width, y),
                        new PointF(x + width, y + cornerRadius)
                });

            Geometry.GetBezierPoints(4, ref pts,
               new PointF[]
               {
                        new PointF(x + width, y + height - cornerRadius),
                        new PointF(x + width, y + height),
                        new PointF(x + width - cornerRadius, y + height)
               });

            Geometry.GetBezierPoints(4, ref pts,
             new PointF[]
             {
                        new PointF(x + cornerRadius, y + height),
                        new PointF(x, y + height),
                        new PointF(x, y + height - cornerRadius)
             });

            angle = angle.AssignCenter(x, y, width, height, true);

            if (angle.Valid)
                points = pts.Rotate(angle);
            else
                points = pts;

            Render(Buffer, points, "RoundedBox", angle, Factory.newRectangleF(x, y, width, height), context);
        }
        #endregion

        #region RENDER RHOMBUS
        public virtual void RenderRhombus(IBuffer Buffer, float x, float y, float width, float height, Angle angle, float? deviation = null, IPenContext context = null) =>
            RenderRectangle(Buffer, x, y, (deviation ?? width), height, context, angle);
        #endregion

        #region RENDER TRAPEZIUM
        public virtual void RenderTrapezium(IBuffer Buffer, ILine first, float parallelLineDeviation, float parallelLineSizeDifference = 0,
            Angle angle = default(Angle), IPenContext context = null)
        {
            if (Buffer == null)
                return;

            var data = Geometry.GetTrapeziumData(first, parallelLineDeviation, angle, StrokeMode.Outer, parallelLineSizeDifference);
            Render(Buffer, data.Points, "Trapezium", data.Angle , data.Bounds, context);
        }
        #endregion

        #region RENDER TEXT
        public void RenderText(IBuffer Buffer, IText text, IPenContext context = null)
        {
            if (!Begin(Buffer, text))
                return;
            if (text.Changed)
                text.Measure();
            IBufferPen Reader = Renderer.GetPen(this, context);
            var value = freezeReaderXY;
            freezeReaderXY = true;
            foreach (var item in text)
                item.DrawTo(Buffer, Reader);
            freezeReaderXY = value;
            End(Buffer, Reader);
        }
        public virtual void RenderGlyphs(IBuffer Buffer, IEnumerable<IGlyph> glyphs, IBufferPen Reader)
        {
            if (Buffer == null)
                return;
            if (!Begin(Buffer, null))
                return;
            var value = freezeReaderXY;
            freezeReaderXY = true;
            foreach (var item in glyphs)
                item.DrawTo(Buffer, Reader);
            freezeReaderXY = value;
            End(Buffer, Reader);
        }
        #endregion

        #region CREATE LINE ACTION
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual GlyphFillAction CreateGlyphFillAction(IBuffer surface, IBufferPen pen)
        {
            GlyphFillAction action = (val1, val2, axis, horizontal, e) =>
            {
                if (val1 == val2)
                    surface.WritePixel(val1, axis, horizontal, pen.ReadPixel(val1, axis, horizontal), true, e);
                else
                    surface.WriteLine(val1, val2, axis, horizontal, pen, Ends.Draw);
            };
            return action;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual FillAction CreateFillAction(IBuffer surface, IBufferPen pen)
        {
            return (val1, val2, axis, horizontal) =>
            {
                if (val1 == val2)
                {
                    surface.WritePixel(val1, axis, horizontal, pen, true);
                }
                else
                    surface.WriteLine(val1, val2, axis, horizontal, pen, Ends.Draw);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual FillAction CreateFillAction(ICollection<Point> list)
        {
            FillAction action = (val1, val2, axis, horizontal) =>
            {
                if (val1 == val2)
                {
                    if (horizontal)
                        list.Add(new Point((int)val1, axis));
                    else
                        list.Add(new Point(axis, (int)val1));
                }
                else
                {
                    if (horizontal)
                    {
                        for (int i = (int)val1; i < (int)val2; i++)
                            list.Add(new Point(i, axis));
                    }
                    else
                    {
                        for (int i = (int)val1; i < (int)val2; i++)
                            list.Add(new Point(axis, i));
                    }
                }
            };
            return action;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe virtual FillAction CreateFillAction(IColorEnumerator enumerator, int* t, int tLen, int tMax)
        {
            enumerator.Reset();
            return (val1, val2, axis, horizontal) =>
            {
                if (val1 == val2)
                {
                    var index = horizontal ? (int)val1 + axis * tMax : axis + (int)val1 * tMax;
                    if (index < tLen && index >= 0)
                    {
                        t[index] = enumerator.GetCurrent();
                        enumerator.MoveNext();
                    }
                }
                else
                {
                    if (horizontal)
                    {
                        var arr = enumerator.ToArray((int)val1, (int)val2);
                        var index = (int)val1 + axis * tMax;
                        if (index < tLen && index >= 0)
                        {
                            Implementation.CopyMemory(arr, 0, t, index, arr.Length);
                        }
                    }
                    else
                    {
                        for (int i = (int)val1; i < val2.Round(); i++)
                        {
                            var index = axis + i * tMax;
                            if (index < tLen && index >= 0)
                            {
                                t[index] = enumerator.GetCurrent();
                                enumerator.MoveNext();
                            }
                        }
                    }
                }
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual unsafe FillAction CreateLineAction(Dictionary<int, List<float>> Data)
        {
            return (val, val1, axis, horizontal) =>
            {
                List<float> list;
                if (!Data.ContainsKey(axis))
                {
                    list = new List<float>(4);
                    Data.Add(axis, list);
                }
                else
                    list = Data[axis];

                list.Add(val);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual unsafe FillAction CreateFillAction(IFloodFill src)
        {
            return (val1, val2, axis, horizontal) =>
            {
                var i = src.IndexOf((int)val1, axis, horizontal);
                if (i == -1)
                    return;
                var alpha = val1.Fraction();
                src[i] = (byte)((alpha??1 * 255).Ceiling());
                src.FillAction?.Invoke(val1.Ceiling(), val2.Ceiling(), axis, horizontal);
            };
        }
        #endregion

        #region DISPOSE
        public void Dispose()
        {
            IsDisposed = true;
            Flush();
        }
        #endregion
    }
}
