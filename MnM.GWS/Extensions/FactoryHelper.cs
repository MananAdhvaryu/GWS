using MnM.GWS.EnumExtensions;
using MnM.GWS.MathExtensions;
using System;
using System.Collections.Generic;

namespace MnM.GWS
{
    public static class FactoryHelper
    {
        #region PEN - BRUSH
        public static ISolidPen newPen(this IFactory factory, Colour color) =>
            factory.newPen(color.Value, 100,100);
        public static IBrush newBrush(this IFactory factory, IFillStyle style, float width, float height) =>
            factory.newBrush(style, width.Ceiling(), height.Ceiling());
        #endregion

        #region FILL STYLE
        public static IFillStyle newFillStyle(this IFactory factory, params int[] values) =>
            factory.newFillStyle(null, Gradient.Horizontal, values);
        public static IFillStyle newFillStyle(this IFactory factory, Gradient g, params int[] values) =>
            factory.newFillStyle(null, g, values);
        #endregion

        #region APOINT
        public static Point[] newPointArray(int len)
        {
            var points = new Point[len];

            for (int i = 0; i < points.Length; i++)
                points[i] = new Point();

            return points;
        }
        public static PointF[] newPointFArray(int len)
        {
            var points = new PointF[len];

            for (int i = 0; i < points.Length; i++)
                points[i] = new PointF();
            return points;
        }

        #endregion

        #region FONT
        public static IFont newFont(this IFactory factory, string fontFile, int fontSize)
        {
            using (var fontStream = System.IO.File.OpenRead(fontFile))
            {
                return factory.newFont(fontStream, fontSize);
            }
        }
        #endregion

        #region CURVE, ELLIPSE, ARC, PIE
        public static IEllipse newEllipse(this IFactory factory, float x, float y, float width, float height, Angle angle = default(Angle), bool assignID = true) =>
            factory.newCurve(x, y, width, height, angle, assignID: assignID);

        public static ICurve newArc(this IFactory factory, float x, float y, float width, float height, float startA, float endA,
            Angle angle = default(Angle), CurveType type = CurveType.Arc, bool assignID = true) =>
            factory.newCurve(x, y, width, height, angle, startA, endA, type.Exclude(CurveType.Pie), assignID);

        public static ICurve newPie(this IFactory factory, float x, float y, float width, float height, float startA, float endA,
            Angle angle = default(Angle), CurveType type = CurveType.Pie, bool assignID = true) =>
            factory.newCurve(x, y, width, height, angle, startA, endA, type & ~CurveType.Arc, assignID);

        public static ICurve newPie(this IFactory factory, IBox area, float startA, float endA, Angle angle = default(Angle), CurveType type = CurveType.Pie, bool assignID = true) =>
            factory.newPie(area.X, area.Y, area.Width, area.Height, startA, endA, angle, type | CurveType.Pie, assignID);

        public static ICurve newArc(this IFactory factory, IBox area, float startA, float endA, Angle angle = default(Angle), CurveType type = CurveType.Arc, bool assignID = true) =>
            factory.newArc(area.X, area.Y, area.Width, area.Height, startA, endA, angle, type | CurveType.Arc, assignID);
        #endregion

        #region BOX
        public static IBox newBox(this IFactory factory, IBox area) =>
            factory.newBox(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);
        public static IBox newBox(this IFactory factory, IRectangle area) =>
            factory.newBox(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);
        public static IBox newBox(this IFactory factory, Point xy, Size wh) =>
            factory.newBox(xy.X, xy.Y, wh.Width, wh.Height);
        public static IBox BoxFromLTRB(this IFactory factory, int x, int y, int right, int bottom) =>
            factory.newBox(x, y, right - x, bottom - y);
        #endregion

        #region BOXF
        public static IBoxF newBoxF(this IFactory factory, IBoxF area) =>
            factory.newBoxF(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);
        public static IBoxF newBoxF(this IFactory factory, IRectangleF area) =>
            factory.newBoxF(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);
        public static IBoxF newBoxF(this IFactory factory, IBox area) =>
            factory.newBoxF(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);

        public static IBoxF newBoxF(this IFactory factory, PointF xy, SizeF wh) =>
            factory.newBoxF(xy.X, xy.Y, wh.Width, wh.Height);
        public static IBoxF BoxFFromLTRB(this IFactory factory, float x, float y, float right, float bottom) =>
            factory.newBoxF(x, y, right - x, bottom - y);
        #endregion

        #region ROUNDED BOX
        public static IRoundedBox RoundedBoxFromLTRB(this IFactory factory, float x, float y, float right, float bottom, float cornerRadius, Angle angle = default(Angle)) =>
            factory.newRoundedBox(x, y, right - x, bottom - y, cornerRadius, angle);
        public static IRoundedBox newRoundedBox(this IFactory factory, IRectangleF rc, float cornerRadius, Angle angle = default(Angle)) =>
            factory.newRoundedBox(rc.X, rc.Y, rc.Width, rc.Height, cornerRadius, angle);
        public static IRoundedBox newRoundedBox(this IFactory factory, IRectangle rc, float cornerRadius, Angle angle = default(Angle)) =>
            factory.newRoundedBox(rc.X, rc.Y, rc.Width, rc.Height, cornerRadius, angle);
        #endregion

        #region RECTANGLE
        public static IRectangle newRectangle(this IFactory factory, IRectangle area) =>
            factory.newRectangle(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);
        public static IRectangle RectangleFromLTRB(this IFactory factory, int x, int y, int right, int bottom) =>
            factory.newRectangle(x, y, right - x, bottom - y);
        #endregion

        #region RECTANGLEF
        public static IRectangleF newRectangleF(this IFactory factory, IRectangleF area) =>
            factory.newRectangleF(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);

        public static IRectangleF newRectangleF(this IFactory factory, IRectangle area) =>
            factory.newRectangleF(area?.X ?? 0, area?.Y ?? 0, area?.Width ?? 0, area?.Height ?? 0);
        public static IRectangleF RectangleFFromLTRB(this IFactory factory, float x, float y, float right, float bottom) =>
            factory.newRectangleF(x, y, right - x, bottom - y);
        public static IRectangleF newRectangleF(this IFactory factory, PointF p, PointF q) =>
            RectangleFFromLTRB(factory, Math.Min(p.X, q.X), Math.Min(p.Y, q.Y), Math.Max(p.X, q.X), Math.Max(p.Y, q.Y));
        #endregion

        #region LINE
        public static ILine newLine(this IFactory factory, float x1, float y1, float x2, float y2, float deviation) =>
            factory.newLine(x1, y1, x2, y2, default(Angle) , deviation);
        public static ILine newLine(this IFactory factory, ILine l, float deviation) =>
            factory.newLine(l.Start.X, l.Start.Y, l.End.X, l.End.Y, deviation);
        public static ILine newLine(this IFactory factory, IXLine l, Angle angle)
        {
            if (l.A.IsHorizontal)
                return factory.newLine(l.A.Val, l.A.Axis, l.B.Val, l.B.Axis, angle);

            return factory.newLine(l.A.Axis, l.A.Val, l.B.Axis, l.B.Val, angle);
        }
        public static ILine newLine(this IFactory factory, PointF start, PointF end, Angle angle) =>
            factory.newLine(start.X, start.Y, end.X, end.Y, angle);
        public static ILine newLine(this IFactory factory, PointF start, PointF end, Angle angle, float stroke) =>
            factory.newLine(start.X, start.Y, end.X, end.Y, angle, stroke);

        public static ILine newLine(this IFactory factory, Point start, Point end, Angle angle) =>
            factory.newLine(start.X, start.Y, end.X, end.Y, angle);
        public static ILine newLine(this IFactory factory, PointF start, PointF end) =>
            factory.newLine(start.X, start.Y, end.X, end.Y);
        public static ILine newLine(this IFactory factory, PointF start, PointF end, float stroke) =>
            factory.newLine(start.X, start.Y, end.X, end.Y, stroke);

        public static ILine newLine(this IFactory factory, Point start, Point end) =>
            factory.newLine(start.X, start.Y, end.X, end.Y);
        public static ILine newLine(this IFactory factory, PointF start, float x2, float y2) =>
           factory.newLine(start.X, start.Y, x2, y2);
        public static ILine newLine(this IFactory factory, float x1, float y1, PointF end) =>
            factory.newLine(x1, y1, end.X, end.Y);
        #endregion

        #region XLINE
        public static IXLine newXLine(this IFactory factory, float val1, float val2, float axis, bool isHorizontal) =>
            factory.newLine(val1, val2, axis.Ceiling(), isHorizontal);
        public static IXLine newXLine(this IFactory factory, int val1, int val2, int axis, bool isHorizontal) =>
            factory.newLine(val1, val2, axis, isHorizontal);
        public static IXLine newXLine(this IFactory factory, int val1, int val2, int axis, bool isHorizontal, float? alpha) =>
            factory.newLine(val1, val2, axis, isHorizontal, alpha);
        public static IXLine newXLine(this IFactory factory, int val1, int val2, int axis, bool isHorizontal, float? startPixelAlpha, float? endPixelAlpha) =>
            factory.newLine(val1, val2, axis, isHorizontal, startPixelAlpha, endPixelAlpha);
        #endregion

        #region BEZIER ARC - PIE
        public static IBezierCurve newBezierArc(this IFactory factory, IBox area, float startA, float endA, Angle angle = default(Angle)) =>
            factory.newBezierArc(area.X, area.Y, area.Width, area.Height, startA, endA, angle);
        public static IBezierCurve newBezierPie(this IFactory factory, IBox area, float startA, float endA, Angle angle = default(Angle)) =>
            factory.newBezierPie(area.X, area.Y, area.Width, area.Height, startA, endA, angle);
        #endregion

        #region BEZIER
        public static IBezier newBezier(this IFactory factory, Angle angle, BezierType type, params float[] points) =>
            factory.newBezier(type, points, null, angle);
        public static IBezier newBezier(this IFactory factory, BezierType type, params float[] points) =>
            factory.newBezier(type, points, null, default(Angle));
        public static IBezier newBezier(this IFactory factory, Angle angle, params float[] points) =>
            factory.newBezier(0, points, null, angle);
        public static IBezier newBezier(this IFactory factory, params float[] points) =>
            factory.newBezier(0, points, null);
        public static IBezier newBezier(this IFactory factory, Angle angle, BezierType type, IList<PointF> points) =>
            factory.newBezier(type, null, points, angle);
        public static IBezier newBezier(this IFactory factory, Angle angle, IList<PointF> points) =>
            factory.newBezier(0, null, points, angle);
        public static IBezier newBezier(this IFactory factory, BezierType type, IList<PointF> points) =>
            factory.newBezier(type, null, points, default(Angle));
        public static IBezier newBezier(this IFactory factory, IList<PointF> points) =>
            factory.newBezier(0, null, points, default(Angle));
        #endregion

        #region TRAPEZIUM
        public static ITrapezium newTrapezium(this IFactory factory, Angle angle, params float[] values)
        {
            var first = factory.newLine(values[0], values[1], values[2], values[3]);
            float parallelLineDeviation = 30f;
            float parallelLineSizeDifference = 0;
            if (values.Length < 6)
                parallelLineDeviation = values[4];
            if (values.Length > 5)
                parallelLineSizeDifference = values[5];
            return factory.newTrapezium(first, parallelLineDeviation, parallelLineSizeDifference, angle);
        }
        public static ITrapezium newTrapezium(this IFactory factory, PointF p1, PointF p2, float parallelLineDeviation, float parallelLineSizeDifference = 0, Angle angle = default(Angle)) =>
            factory.newTrapezium(factory.newLine(p1.X, p1.Y, p2.X, p2.Y), parallelLineDeviation, parallelLineSizeDifference, angle);
        public static ITrapezium newTrapezium(this IFactory factory, Point p1, Point p2, float parallelLineDeviation, float parallelLineSizeDifference = 0, Angle angle = default(Angle)) =>
            factory.newTrapezium(factory.newLine(p1.X, p1.Y, p2.X, p2.Y), parallelLineDeviation, parallelLineSizeDifference, angle);
        public static ITrapezium newTrapezium(this IFactory factory, float x1, float y1, float x2, float y2, float parallelLineDeviation, float parallelLineSizeDifference = 0,
            Angle angle = default(Angle)) =>
            factory.newTrapezium(factory.newLine(x1, y1, x2, y2), parallelLineDeviation, parallelLineSizeDifference, angle);
        #endregion

        #region RHOMBUS
        public static IRhombus newRhombus(this IFactory factory, IRectangleF area, Angle angle = default(Angle), float? deviation = null) =>
            factory.newRhombus(area.X, area.Y, area.Width, area.Height, angle, deviation);
        public static IRhombus newRhombus(this IFactory factory, IRectangle area, Angle angle = default(Angle), float? deviation = null) =>
            factory.newRhombus(area.X, area.Y, area.Width, area.Height, angle, deviation);
        #endregion

        #region POLYGON
        public static IPolygon newPolygon(this IFactory factory, Angle angle, IList<PointF> points) =>
            factory.newPolygon(points, angle);
        public static IPolygon newPolygon(this IFactory factory, Angle angle, params float[] points) =>
            factory.newPolygon(points.ToPoints(), angle);
        public static IPolygon newPolygon(this IFactory factory, params float[] points) =>
            factory.newPolygon(default(Angle), points.ToPoints());
        #endregion

        #region TRIANGLE
        public static ITriangle newTriangle(this IFactory factory, Point a, Point b, Point c, Angle angle = default(Angle)) =>
            factory.newTriangle(a.X, a.Y, b.X, b.Y, c.X, c.Y, angle);
        public static ITriangle newTriangle(this IFactory factory, PointF a, PointF b, PointF c, Angle angle = default(Angle)) =>
            factory.newTriangle((int)a.X, (int)a.Y, (int)b.X, (int)b.Y, (int)c.X, (int)c.Y, angle);
        public static ITriangle newTriangle(this IFactory factory, ILine l, Point b, Angle angle = default(Angle)) =>
           factory.newTriangle((int)l.Start.X, (int)l.Start.Y, (int)l.End.X, (int)l.End.Y, b.X, b.Y, angle);
        public static ITriangle newTriangle(this IFactory factory, ILine l, PointF b, Angle angle = default(Angle)) =>
            factory.newTriangle((int)l.Start.X, (int)l.Start.Y, (int)l.End.X, (int)l.End.Y, (int)b.X, (int)b.Y, angle);
        #endregion
    }
}
