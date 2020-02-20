using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public struct Occupier: IRotatable
    {
        public Occupier(IRectangleF bounds, Angle angle)
        {
            Bounds = bounds;
            Angle = angle;
        }
        public Angle Angle { get; private set; }
        public IRectangleF Bounds { get; private set; }
    }
}
