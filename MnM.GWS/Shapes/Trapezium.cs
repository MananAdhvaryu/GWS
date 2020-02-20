
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
        struct Trapezium : ITrapezium
        {
            #region VARIABLES
            readonly TrapeziumData Data;
            #endregion

            #region CONSTRUCTORS
            public Trapezium(ILine first, float parallelLineDeviation, float parallelLineSizeDifference = 0, Angle angle = default(Angle)) : this()
            {
                Data = Geometry.GetTrapeziumData(first, parallelLineDeviation, angle, StrokeMode.Outer, parallelLineSizeDifference);
                ID = Factory.NewID(Name);
            }
            public Trapezium(Angle angle, params float[] values): this()
            {
                var first = Factory.newLine(values[0], values[1], values[2], values[3]);
                float parallelLineDeviation = 30f;
                float parallelLineSizeDifference = 0;
                if (values.Length < 6)
                    parallelLineDeviation = values[4];
                if (values.Length > 5)
                    parallelLineSizeDifference = values[5];
                Data = Geometry.GetTrapeziumData(first, parallelLineDeviation, angle, StrokeMode.Outer, parallelLineSizeDifference);
                ID = Factory.NewID(Name);
            }
            Trapezium(TrapeziumData data, string id) 
            {
                ID = id;
                Data = data;
            }
            #endregion

            #region PROPERTIES
            public string Name => "Trapezium";
            public Angle Angle => Data.Angle;
            public string ID { get; private set; }
            public IRectangleF Bounds => Data.Bounds;
            #endregion

            #region METHODS
            public bool Contains(float x, float y)
            {
                //NEED TO WORK OUT
                return false;
            }
            #endregion

            #region INTERFACE
            float IRectangleF.X => Data.Bounds.X;
            float IRectangleF.Y => Data.Bounds.Y;
            float IRectangleF.Width => Data.Bounds.Width;
            float IRectangleF.Height => Bounds.Height;
            float IRectangleF.Right => Data.Bounds.Right;
            float IRectangleF.Bottom => Data.Bounds.Bottom;
            #endregion

            #region IReadOnlyList<IPosF>
            public PointF this[int index] => Data.Points[index];
            public int Count => Data.Points.Count;
            IEnumerator<PointF> IEnumerable<PointF>.GetEnumerator() =>
                Data.Points.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() =>
                Data.Points.GetEnumerator();
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
