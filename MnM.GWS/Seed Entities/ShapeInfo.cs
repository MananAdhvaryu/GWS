using System;
using System.Collections;
using System.Collections.Generic;

namespace MnM.GWS
{
    public struct Shape: IShape
    {
        #region VARIABLES
        public readonly IEnumerable<PointF> Points;
        public readonly string Name;
        public readonly Angle Angle;
        public readonly IRectangleF Bounds;
        public readonly string ID;
        #endregion

        #region CONSTRUCTORS
        public Shape( IEnumerable<PointF> points,  string shapeType,  Angle angle,  IRectangleF originalArea)
        {
            Points = points;
            Name = shapeType;
            Angle = angle;
            Bounds = originalArea?? points.ToArea();
            ID = (points is IElement) ? (points as IElement).ID : "Custom";
            if (ID == null)
                ID = Implementation.Factory.NewID(Points);
        }
        public Shape( IShape shape)
        {
            Points = shape;
            Name = shape.Name;
            Angle = shape.Angle;
            Bounds = shape.Bounds;
            ID = shape.ID;
            if (ID == null)
                shape.AssignIDIfNone();
        }
        #endregion

        #region ISHAPE
        Angle IRotatable.Angle => Angle;
        IEnumerator<PointF> IEnumerable<PointF>.GetEnumerator() => Points.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Points.GetEnumerator();
        string IStoreable.ID => ID;
        IRectangleF IOccupier.Bounds => Bounds;
        string IRecognizable.Name => Name;
        void IStoreable.AssignIDIfNone() { }
        #endregion
    }
}
