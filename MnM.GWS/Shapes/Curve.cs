using MnM.GWS.EnumExtensions;
using MnM.GWS.MathExtensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
    public
#endif
        struct Curve : ICurve
        {
            #region variables
            const bool UseArcLine = true;
            EllipseData data;
            IArcCut ai;
            ICollection<float> List1, List2;
            #endregion

            #region CONSTRUCTORS
            public Curve( float x,  float y,  float width,  float height,  Angle angle = default(Angle),  float startAngle = 0,  float endAngle = 0,
                 CurveType type = CurveType.Pie,  bool assignID = true) : this()
            {
                EllipseData data = new EllipseData(x, y, width, height, angle);
                if (type.SweepAngle())
                    Initialize(startAngle, endAngle + startAngle, data, type, assignID, default(PointF), null);
                else
                    Initialize(startAngle, endAngle, data, type, assignID, default(PointF), null);
            }
            public Curve( ICircle circle,  bool assignID = true) : this()
            {
                if(circle is ICurve)
                    Initialize((circle as ICurve).StartAngle, (circle as ICurve).EndAngle, circle.Data, (circle as ICurve).Type, assignID);
                else
                    Initialize( 0, 0, circle.Data, CurveType.Full, assignID);
            }
            public Curve( ICurve curve,  float stroke,  StrokeMode mode,  FillMode fill,  bool assignID = true): this()
            {
                Geometry.GetStrokeAreas(curve.Data.X, curve.Data.Y, curve.Data.Width, curve.Data.Height, out IRectangleF o, out IRectangleF i, stroke, mode);

                PointF cpt = default(PointF), cptc = default(PointF);

                if (!(curve).Full)
                {
                    IEnumerable<PointF> source = curve.PieTriangle;

                    var Pair = source.StrokePoints("Trapezoid", stroke, mode);
                    IList<PointF> main = Pair.Item1, child = Pair.Item2;
                    if (curve.Type.CrossStroke())
                        MathHelper.Swap(ref main, ref child);

                    cpt = curve.Name == "Pie" ? main[0] :  default(PointF);
                    cptc = curve.Name == "Pie" ? child[0] : default(PointF);
                }

                if (fill == FillMode.Outer)
                    Initialize(curve, o, assignID, cpt);

                else if (fill == FillMode.Inner)
                    Initialize(curve, i, assignID, cptc);
                else
                {
                    var curve2 = new Curve();
                    curve2.Initialize(curve, i, assignID, cptc);
                    Initialize(curve, o, assignID, cpt, curve2);
                }
            }
            #endregion

            #region INITIALIZE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void Initialize(float startAngle, float endAngle, EllipseData data, CurveType type, bool assignID, PointF c = default(PointF), ICurve attachedCurve = null)
            {
                AttachedCurve = attachedCurve;
                this.data = data;
                List1 = new HashSet<float>();
                List2 = new HashSet<float>();

                if (assignID)
                    ID = Factory.NewID(Name);

                if (startAngle == 0 && endAngle == 0)
                    return;

                var StartAngle = startAngle;
                var EndAngle = endAngle;
                var autoCenter = !c.Valid;
                var eAngle = autoCenter ? data.Angle : default(Angle);
                if (autoCenter)
                    c = new PointF(data.Cx, data.Cy);

                if (!autoCenter && data.Angle.Valid)
                    c = data.Angle.Rotate(c, true);

                var a = Geometry.GetEllipsePoint(StartAngle, c.X, c.Y, data.Rx, data.Ry, eAngle, true);
                var b = Geometry.GetEllipsePoint(EndAngle, c.X, c.Y, data.Rx, data.Ry, eAngle, true);

                PointF m, p;
                var move = -Math.Max(data.Rx, data.Ry);
                p = Geometry.MoveLine(a, c, move);
                if (this.EllipseInterception(p, c, out _, out m, false))
                    a = m.Valid ? m : a;

                p = Geometry.MoveLine(b, c, move);
                if (this.EllipseInterception(p, c, out _, out m, false))
                    b = m.Valid ? m : b;

                ai = Factory.newArcCut(type, startAngle, endAngle, a, b, c, UseArcLine, AttachedCurve);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void Initialize(ICurve curve, IRectangleF area, bool assignID, PointF c = default(PointF) , ICurve attachedCurve = null)
            {
                Initialize(curve.StartAngle, curve.EndAngle,
                    new EllipseData(area, curve.Data), curve.Type, assignID, c, attachedCurve);
            }
            #endregion

            #region PROPERTIES
            public float StartAngle => ai?.StartAngle ?? 0;
            public float EndAngle => ai?.EndAngle ?? 0;
            public EllipseData Data => data;
            public Angle Angle => data.Angle;
            public IRectangleF Bounds => data.Bounds;
            public CurveType Type => ai?.Option?? CurveType.Full;
            public string ID { get; private set; }
            public ICurve AttachedCurve { get; private set; }
            public string Name
            {
                get
                {
                    if (ai?.IsEmpty == true)
                    {
                        if (data.Rx == data.Ry)
                            return "Circle";
                        return "Elipse";
                    }
                    if (Type.IsPie())
                        return "Pie";
                    if (Type.HasFlag(CurveType.ClosedArc))
                        return "ClosedArc";
                    if (Type.HasFlag(CurveType.Arc))
                        return "Arc";
                    return "Ellipse";
                }
            }
            public bool Full => ai?.IsEmpty?? true;
            ILine ICurve.ArcLine => ai?.ArcLine;

            #endregion

            #region GET BOUNDARIES
            public float GetMaxPosition(bool horizontal) =>
                 horizontal ? data.YMax : data.XMax;
            public float GetDrawEnd(bool horizontal) =>
                horizontal ? data.YEnd : data.XEnd;
            public float GetDrawStart(bool horizontal) =>
                horizontal ? data.YStart : data.XStart;
            #endregion

            #region GET DATA
            public IList<ILine> GetClosingLines()
            {
                if (Full)
                    return null;

                return ai.Extra;
            }
            public ICollection<float>[] GetDataAt(float position, bool horizontal, bool forOutLinesOnly, out int axis1, out int axis2)
            {
                Type.GetDataAt(ref List1, ref List2, data, ai,
                    position, horizontal, forOutLinesOnly, out axis1, out axis2);
                if (AttachedCurve != null)
                {
                    if (position <= AttachedCurve.GetMaxPosition(horizontal))
                    {
                        var attachList = AttachedCurve.GetDataAt(position, horizontal, forOutLinesOnly, out _, out _);

                        foreach (var item in attachList[0])
                            List1.Add(item);

                        foreach (var item in attachList[1])
                            List2.Add(item);
                    }
                }
                return new ICollection<float>[] { List1, List2 };
            }
            #endregion

            #region CONTAINS
            //public bool Contains(float x, float y)
            //{
            //    if (Empty)
            //        return OriginalArea.Has(x, y);
            //    return ai.CheckPixel(x, y, true);
            //}
            public bool Contains(float val, float axis, bool horizontal = true)
            {
                if (ai.IsEmpty)
                {
                    if (horizontal)
                        return Bounds.Has(val, axis);
                    return Bounds.Has(axis, val);
                }
                return ai.Contains(val, axis, horizontal);
            }
            #endregion

            #region CLONE - DISPOSE
            public object Clone()
            {
                var curve = new Curve(this);
                return curve;
            }
            public void Dispose()
            {
                AttachedCurve = null;
                List1 = null;
                List1 = List2 = null;
            }
            #endregion

            #region IEnumerable<IPosF>
            public IEnumerator<PointF> GetEnumerator()
            {
                var s = Full ? 0 : ai.StartAngle;
                var e = Full ? 359 : ai.EndAngle;

                var pts = Geometry.GetArcPoints(ref s, ref e, !Full && Type.IsPie(), 
                    data.WMajor, data.Cx, data.Cy, data.Rx, data.Ry, Angle, Type.NegativeMotion());
                foreach (var item in pts)
                    yield return item;
             }

            public IEnumerable<PointF> PieTriangle
            {
                get
                {
                    yield return new PointF(data.Cx, data.Cy);

                    var s = Full ? 0 : ai.StartAngle;
                    var e = Full ? 359 : ai.EndAngle;

                    yield return this.GetEllipsePoint(s, !Full);
                    yield return this.GetEllipsePoint(e, !Full);
                }
            }
            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
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
