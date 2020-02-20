
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
        struct Ring : IRing
        {
            EllipseData ei;

            #region CONSTRUCTORS
            public Ring(Point center, int width, int height, Angle angle = default(Angle)) :
                this(center.X - width / 2, center.Y - height / 2, width, height, angle)
            { }
            public Ring(int x, int y, int width, int height, Angle angle = default(Angle)) : this()
            {
                Initialize(x, y, width, height, angle);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void Initialize(int x, int y, int width, int height, Angle angle = default(Angle))
            {
                ei = new EllipseData(x, y, width, height, angle);
                Bounds = Factory.newRectangleF(x, y, width, height);
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public EllipseData Data => ei;
            public IRectangleF Bounds { get; private set; }
            public Angle Angle => ei.Angle;
            public string Name => "Ring";
            public string ID { get; private set; }
            #endregion

            #region DRAW           
            //public void DrawCircleOrEllipseAt2(ISurface surface, int x, int y, int width, int height, IPenContext context = null, Angle rotation = null)
            //{
            //    //DrawCircleOrEllipseAt1(surface, x, y, width, height, context, rotation);
            //    //return;
            //    var area = CurveHelper.GetInfo(x, y, width, height, false,
            //        out float RadiusX, out float RadiusY, out float CX, out float CY, out Angle Angle,
            //        out float Step, out float MinStep, out bool WMajor, rotation);

            //    var angle = Angle ?? Factory.EmptyAngle;

            //    var total = 90;
            //    var cos = angle.Cos;
            //    var sin = angle.Sin;

            //    var c = (float)Math.Cos(DrawHelper.PI / total);
            //    var s = (float)Math.Sin(DrawHelper.PI / total);
            //    var rxcos = RadiusX * cos;
            //    var rxsin = RadiusX * sin;
            //    var rycos = RadiusY * cos;
            //    var rysin = RadiusY * sin;


            //    float x1 = 1f, y1 = 0f, x2 = 0, y2 = 0;

            //    IPen pen = surface.GetPen(context, area);
            //    var action = LineHelper.CreateLineAction(surface, pen);

            //    List<IPointF> pts = new List<IPointF>(360);

            //    for (float i = 0; i < 180; i += .5f)
            //    {
            //        x2 = x1;
            //        y2 = y1;
            //        x1 = c * x2 - s * y2;
            //        y1 = s * x2 + c * y2;

            //        RotatedPoint(x1, y1, CX, CY, rxcos, rxsin, rycos, rysin, ref pts);
            //    }

            //    foreach (var item  pts)
            //    {
            //        surface.DrawPixel(item.X, item.Y, pen);
            //    }

            //    //Factory.newFigure(pts, "Ellipse", pen).DrawTo(surface, pen);
            //}
            //static void RotatedQuadrants(float x1, float y1, float x2, float y2, float CX, float CY, float rxcos, float rxsin, float rycos, float rysin, ref List<ILine> lines)
            //{
            //    var xrysin = x1 * rysin;
            //    var xrycos = x1 * rycos;
            //    var xrxsin = x1 * rxsin;
            //    var xrxcos = x1 * rxcos;

            //    var yrycos = y1 * rycos;
            //    var yrysin = y1 * rysin;
            //    var yrxcos = y1 * rxcos;
            //    var yrxsin = y1 * rxsin;

            //    var lbx = CX + (xrxcos - yrysin);
            //    var rtx = CX - (xrxcos - yrysin);
            //    var rbx = CX + (yrxcos + xrysin);
            //    var ltx = CX - (xrysin + yrxcos);

            //    var lby = CY + (xrxsin + yrycos);
            //    var rty = CY - (xrxsin + yrycos);
            //    var rby = CY + (yrxsin - xrycos);
            //    var lty = CY + (xrycos - yrxsin);


            //    xrysin = x2 * rysin;
            //    xrycos = x2 * rycos;
            //    xrxsin = x2 * rxsin;
            //    xrxcos = x2 * rxcos;

            //    yrycos = y2 * rycos;
            //    yrysin = y2 * rysin;
            //    yrxcos = y2 * rxcos;
            //    yrxsin = y2 * rxsin;

            //    var lbx1 = CX + (xrxcos - yrysin);
            //    var rtx1 = CX - (xrxcos - yrysin);
            //    var rbx1 = CX + (yrxcos + xrysin);
            //    var ltx1 = CX - (xrysin + yrxcos);

            //    var lby1 = CY + (xrxsin + yrycos);
            //    var rty1 = CY - (xrxsin + yrycos);
            //    var rby1 = CY + (yrxsin - xrycos);
            //    var lty1 = CY + (xrycos - yrxsin);


            //    lines.Add(Factory.newLine(lbx1, lby1, lbx, lby));
            //    lines.Add(Factory.newLine(rbx1, rby1, rbx, rby));

            //    lines.Add(Factory.newLine(ltx1, lty1, ltx, lty));
            //    lines.Add(Factory.newLine(rtx1, rty1, rtx, rty));
            //}

            //static void RotatedPoint(float x1, float y1, float CX, float CY, float rxcos, float rxsin, float rycos, float rysin, ref List<IPointF> points)
            //{
            //    var xrxsin = x1 * rxsin;
            //    var xrxcos = x1 * rxcos;

            //    var yrycos = y1 * rycos;
            //    var yrysin = y1 * rysin;

            //    var lbx = CX + (xrxcos - yrysin);
            //    var lby = CY + (xrxsin + yrycos);

            //    points.Add(new PointF(lbx, lby));
            //}

            #endregion

            #region IReadOnlyList<IPosF>
            public IEnumerator<PointF> GetEnumerator()
            {
                for (float i = 0; i < 360; i += 1)
                    yield return Geometry.GetEllipsePoint(i, ei.Cx, ei.Cy, ei.Rx, ei.Ry, ei.Angle, false);
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
