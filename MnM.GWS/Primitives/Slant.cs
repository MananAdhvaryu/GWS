using MnM.GWS.MathExtensions;
using System;

namespace MnM.GWS
{
    public struct Slant : ISlant
    {
        public Slant(int x1, int x2)
        {
            MathHelper.Order(ref x1, ref x2);
            X1 = x1;
            X2 = x2;
            Angle = (X2 - X1);
        }

        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Angle { get; }
    }
}
