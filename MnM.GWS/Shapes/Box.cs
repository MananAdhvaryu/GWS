using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
        [StructLayout(LayoutKind.Sequential)]
#else
        [StructLayout(LayoutKind.Sequential)]
        public
#endif
        struct Box : IBox, ISquare, IRectangle
        {
            #region VARIABLES
            public readonly int X;
            public readonly int Y;
            public readonly int Width;
            public readonly int Height;
            public static readonly Box Empty = new Box();
            const string toStr = "x:{0}, y:{1}, width:{2}, height:{3}";
            #endregion

            #region CONSTRUCTORS
            public Box( int x,  int y,  int w,  int h,  bool positiveLocation = false) : this()
            {
                X = x;
                Y = y;
                Width = w;
                Height = h;

                if (positiveLocation)
                {
                    if (X < 0)
                    {
                        Width += X;
                        X = 0;
                    }
                    if (Y < 0)
                    {
                        Height += Y;
                        Y = 0;
                    }
                }
                ID = Factory.NewID(Name);
            }
            #endregion

            #region PROPERTIES
            public string ID { get; private set; }
            public int Right => X + Width;
            public int Bottom => Y + Height;
            public string Name => "Box";
            IntPtr IHandle.Handle => this.ToPtr();
            #endregion

            #region METHODS
            public bool Contains(float x, float y) =>
                x >= X && x <= Right && y >= Y && y <= Bottom;
            #endregion

            #region Interface
            int IRectangle.X => X;
            int IRectangle.Y => Y;
            int IRectangle.Width => Width;    
            int IRectangle.Height => Height;
            int ISquare.X => X;
            int ISquare.Y => Y;
            int ISquare.Width => Width;

            IRectangleF IOccupier.Bounds => new RectF(X, Y, Width, Height);
            Angle IRotatable.Angle => Angle.Empty;
            #endregion

            public override string ToString() =>
                string.Format(toStr, X, Y, Width, Height);

            #region IEnumerable<IPosF>
            public IEnumerator<PointF> GetEnumerator()
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            yield return new PointF(X, Y);
                            break;
                        case 1:
                            yield return new PointF(X, Bottom);
                            break;
                        case 2:
                            yield return new PointF(Right, Bottom);
                            break;
                        case 3:
                        default:
                            yield return new PointF(Right, Y);
                            break;
                    }
                }
            }
            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
            #endregion

            public static implicit operator BoxF(Box rc)=>
                new BoxF(rc.X, rc.Y, rc.Width, rc.Height);
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
