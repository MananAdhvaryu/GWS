using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
#endif
        struct Rhombus : IRhombus, IRectangleF
        {
            #region VARIABLES
            IList<PointF> points;
            #endregion

            #region CONSTRUCTORS
            public Rhombus(IBox rc, Angle angle, float? deviation = null) :
                this(rc.X, rc.Y, rc.Width, rc.Height, angle, deviation)
            { }
            public Rhombus(IBoxF rc, Angle angle, float? deviation = null) :
                this(rc.X, rc.Y, rc.Width, rc.Height, angle, deviation)
            { }
            public Rhombus(float x, float y, float w, float h, Angle angle, float? deviation = null) : this()
            {
                w = deviation ?? w;
                Bounds = Factory.newRectangleF(x, y, w, h);
                Angle = angle.AssignCenter(Bounds);

                if (Angle.Valid)
                {
                   var data =  Geometry.GetTrapeziumData(Factory.newLine(x, y, x, y + h), w, Angle, StrokeMode.Outer);
                    points = data.Points;
                }
                else
                {
                    points = new PointF[]
                    {
                            new PointF(x, y),
                            new PointF(x, y + h),
                            new PointF(x + w, y + h),
                            new PointF(x + w, y)
                    };
                }
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public Angle Angle { get; private set; }
            public string Name => "Rhombus";
            public string ID { get; private set; }
            public IRectangleF Bounds { get; private set; }
            #endregion

            #region DRAW
            public bool Contains(float x, float y)
            {
                //NEED TO WORK OUT
                return false;
            }
            #endregion

            #region INTERFACE
            float IRectangleF.X => Bounds.X;
            float IRectangleF.Y => Bounds.Y;
            float IRectangleF.Width => Bounds.Width;
            float IRectangleF.Height => Bounds.Height;
            float IRectangleF.Right => Bounds.Right;
            float IRectangleF.Bottom => Bounds.Bottom;
            #endregion

            #region IEnumerable<IPosF>
            IEnumerator<PointF> IEnumerable<PointF>.GetEnumerator() =>
                points.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() =>
                points.GetEnumerator();
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
