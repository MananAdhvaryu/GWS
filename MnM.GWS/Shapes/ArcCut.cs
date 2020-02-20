using MnM.GWS.EnumerableExtensions;
using MnM.GWS.EnumExtensions;
using MnM.GWS.MathExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct ArcCut : IArcCut
        {
            #region VARIABLES
            public static readonly ArcCut Empty = new ArcCut();
            public readonly bool Initialized;
            public readonly bool NegativeMotion;
            PointF Center;
            #endregion

            #region CONSTRUCTORS
            public ArcCut( CurveType type, float startAngle, float endAngle,  PointF arcPoint1,  PointF arcPoint2,  PointF centerOfArc,
                 bool UseArcLine = true,  ICurve AttachedCurve = null) : this()
            {
                Option = type;
                Initialized = !arcPoint1.Equals(arcPoint2);
                Center = centerOfArc;

                if (!Initialized)
                {
                    Option = Option.Exclude(CurveType.Arc, CurveType.Pie).Include(CurveType.Full);
                    return;
                }

                if (Option.IsPie())
                    Option = Option.Exclude(CurveType.Arc);
                else
                    Option = Option.Exclude(CurveType.Pie);

                if (Option.IsArc())
                {
                    StartAngle = startAngle;
                    EndAngle = endAngle;
                }
                else
                {
                    StartAngle = arcPoint1.GetAngle(centerOfArc);
                    EndAngle = arcPoint2.GetAngle(centerOfArc);
                }
                if (!UseArcLine)
                {
                    if (EndAngle < StartAngle)
                        Option = Option.Include(CurveType.NegativeMotion);
                    else
                        Option = Option.Exclude(CurveType.NegativeMotion);
                }

                NegativeMotion = Option.NegativeMotion();
                ArcLine = Factory.newLine(arcPoint1, arcPoint2);
                Line1 = Factory.newLine(arcPoint1, centerOfArc);
                Line2 = Factory.newLine(arcPoint2, centerOfArc);
                var l = ArcLine;

                /*If arc line is perfect then cutting ellipse using ArcLine is faster than using Math.Atan2
                * However, it needs to be thoroughly tested as it is knownn to be fragile.*/
                if (UseArcLine)
                    Contains = CheckPixel;
                else
                    Contains = CheckPixel1;

                Extra = GetClosingLines(AttachedCurve);
            }
            #endregion

            #region PROPERTIES
            public bool IsEmpty => !Initialized;
            public Func<float, float, bool, bool> Contains { get; private set; }
            public float StartAngle { get; private set; }
            public float EndAngle { get; private set; }
            public CurveType Option { get; private set; }
            public ILine ArcLine { get; private set; }
            public ILine Line1 { get; private set; }
            public ILine Line2 { get; private set; }
            public IList<ILine> Extra { get; private set; }
            #endregion

            #region METHODS
            bool CheckPixel(float val, float axis, bool horizontal)
            {
                if (horizontal)
                {
                    //if (ArcLine.X1.RoundF(4) == val.RoundF(4) && axis == ArcLine.Y1)
                    //    return false;
                    //if (ArcLine.X2.RoundF(4) == val.RoundF(4) && axis == ArcLine.Y2)
                    //    return false;

                    if (NegativeMotion)
                        return !ArcLine.IsGreaterThan(val, axis);
                    return !ArcLine.IsLessThan(val, axis);
                }
                else
                {
                    if (NegativeMotion)
                        return !ArcLine.IsGreaterThan(axis, val);
                    return !ArcLine.IsLessThan(axis, val);
                }
            }
            bool CheckPixel1(float val, float axis, bool horizontal)
            {
                float value;
                if (horizontal)
                    value = Geometry.GetAngle(val, axis, Center);
                else
                    value = Geometry.GetAngle(axis, val, Center);

                if (NegativeMotion)
                    return !MathHelper.IsWithIn(StartAngle, EndAngle, value, false);
                return MathHelper.IsWithIn(StartAngle, EndAngle, value);
            }
            IList<ILine> GetClosingLines( ICurve AttachedCurve)
            {
                //if (!Option.IsArc())
                //    return null;

                IList<ILine> lines, closingLines = null;
                ILine arcLine = null;

                if (AttachedCurve != null)
                {
                    closingLines = AttachedCurve.GetClosingLines();
                    arcLine = AttachedCurve.ArcLine;
                }

                if (Option.IsPie())
                {
                    lines = new ILine[] { Line1, Line2 };
                    if (AttachedCurve == null)
                        return lines;

                    return lines.AppendItems(closingLines).ToArray();
                }
                else if (Option.IsClosedArc())
                {
                    lines = new ILine[] { ArcLine };
                    if (AttachedCurve == null)
                        return lines;
                    return lines.AppendItems(closingLines).ToArray();
                }
                else if (Option.IsArc())
                {
                    if (AttachedCurve == null)
                        return null;
                    if (arcLine != null)
                    {
                        return new ILine[]
                        {
                        Factory.newLine(ArcLine.Start, arcLine.Start),
                        Factory.newLine(ArcLine.End, arcLine.End)
                        };
                    }
                }
                return null;
            }
            public void DrawLine(IBuffer surface, IBufferPen pen, ref ICollection<float> list, float x1, float x2, int y, bool horizontal = true)
            {
                list.Clear();
                Geometry.AddValSafe(x1, y, horizontal, ref list, Contains);
                Geometry.AddValSafe(x2, y, horizontal, ref list, Contains);

                Option.AddValsSafe(y, horizontal, ref list, this);
                surface.WriteLine(list, y, horizontal, pen, PolyFill.OddEven | PolyFill.AAEnds);
            }
            public void DrawPixel(IBuffer Writer, IBufferPen pen, float val, int axis, bool horizontal = true)
            {
                if (!Contains(val, axis, horizontal))
                    return;
                Writer.WritePixel(val, axis, horizontal, pen, Implementation.Renderer.AntiAlised);
            }
            #endregion
        }
#if AllHidden
    }
#endif
}