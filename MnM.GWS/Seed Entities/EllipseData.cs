using System;

using MnM.GWS.MathExtensions;

namespace MnM.GWS
{
    public struct EllipseData : IRotatable
    {
        #region VARIABLES
        public readonly float X;
        public readonly float Y;

        public readonly float Rx, Ry, RxSqr, RySqr;
        public readonly int Cx, Cy;
        public readonly Angle Angle;
        public readonly bool WMajor, HMajor;
        public readonly float A, B, C, XStart, YStart, XEnd, YEnd, YMax, XMax;
        public readonly bool RightYGreater, TopXGreater;
        #endregion

        #region CONSTRUCTORS
        public EllipseData(float x, float y, float width, float height, Angle angle = default(Angle)) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Rx = (width / 2f);
            Ry = (height / 2f);
            Cx = (x + Rx).Round();
            Cy = (y + Ry).Round();
            WMajor = Rx > Ry;

            if (angle.Valid && angle.CenterAssigned)
            {
                angle.Rotate(Cx, Cy, out Cx, out Cy);
                X = Cx - Rx;
                Y = Cy - Ry;
            }

            if (angle.Valid)
                Angle = new Angle(angle, Cx, Cy);

            RxSqr = MathHelper.Sqr(Rx);
            RySqr = MathHelper.Sqr(Ry);

            float cos = 1;
            float sin = 0;

            if (Angle.Valid)
            {
                cos = Angle.MCos;
                sin = Angle.MSin;
            }

            float b0 = ((1 / RxSqr) - (1 / RySqr));
            A = ((cos * cos) / RxSqr) + ((sin * sin) / RySqr);
            B = 2 * cos * sin * b0;
            C = ((cos * cos) / RySqr) + ((sin * sin) / RxSqr);

            var HMajor = Rx <= Ry;

            if (HMajor)
            {
                var newA = (-(Angle.Degree) - 90) * Geometry.Radian;
                cos = (float)Math.Cos(newA);
                sin = (float)Math.Sin(newA);
            }

            var a = Rx > Ry ? Rx : Ry;
            var b = Rx > Ry ? Ry : Rx;
            var MajorSqr = a * a;

            var Distance = (float)Math.Sqrt(a * a - b * b);
            var xc = Distance * cos;
            var yc = Distance * sin;
            float A2 = (MajorSqr - xc * xc);
            float B2 = (MajorSqr - yc * yc);
            float C2 = (-xc * yc);

            var minus2C = Math.Sqrt(A2 + B2 - (2 * C2));
            var plus2C = Math.Sqrt(A2 + B2 + (2 * C2));

            var XTopMidPositive = (float)((B2 - C2) / minus2C);
            var XMidBotPositive = (float)((B2 + C2) / plus2C);

            var YTopMidPositive = (float)((A2 - C2) / minus2C);
            var YMidBotPositive = (float)(-(A2 + C2) / plus2C);

            YMax = (float)Math.Sqrt(A2);
            XMax = (float)Math.Sqrt(B2);

            TopXGreater = XTopMidPositive > XMidBotPositive;
            var yPos1 = TopXGreater ? YTopMidPositive : YMidBotPositive;
            var yPos2 = TopXGreater ? YMidBotPositive : YTopMidPositive;
            RightYGreater = YTopMidPositive > -YMidBotPositive;
            YStart = (RightYGreater ? YTopMidPositive : -YMidBotPositive);
            YEnd = (RightYGreater ? -YMidBotPositive : YTopMidPositive);

            float val1, val2, val3, val4;
            Geometry.GetRotatedEllipseDataAt((int)yPos1, true, A, B, C, out val1, out val2);
            Geometry.GetRotatedEllipseDataAt((int)yPos2, true, A, B, C, out val3, out val4);
            MathHelper.Order(ref val1, ref val2);
            MathHelper.Order(ref val3, ref val4);

            XStart = val2;
            XEnd = val4;
        }
        public EllipseData(IRectangleF rc, Angle angle) :
            this(rc.X, rc.Y, rc.Width, rc.Height, angle)
        { }
        public EllipseData(IRectangleF rc, EllipseData original) :
           this(rc.X, rc.Y, rc.Width, rc.Height, original.Angle)
        {
        }
        #endregion

        #region PROPERTIES
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Right => X + Width;
        public float Bottom => Y + Height;
        Angle IRotatable.Angle => Angle;
        public IRectangleF Bounds => new RectF(X, Y, Width, Height);
        #endregion

    }
}
