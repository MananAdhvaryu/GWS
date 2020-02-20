using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MnM.GWS.MathExtensions;
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public unsafe abstract class GwsFloodFill: IFloodFill
    {
        #region VARIABLES
        protected static byte[] TrueArray;
        #endregion

        #region CONSTRUCTORS
        static GwsFloodFill()
        {
            TrueArray = Enumerable.Repeat((byte)255, 7000).ToArray();
        }

        protected GwsFloodFill() { }
        #endregion

        #region PROPERTIES
        public abstract byte this[int index] { get;set; }
        public Func<float, float, bool> CheckPixel { protected get; set; }
        public FillAction FillAction { get; private set; }
        protected FillAction RenderAction { get; private set; }
        public bool Activated { get; private set; }
        protected int X { get; private set; }
        protected int Y { get; private set; }
        protected int Width { get; private set; }
        protected int Height { get; private set; }
        protected int Length { get; private set; }
        protected bool IsCut => CheckPixel != null;
        #endregion

        #region BEGIN
        public unsafe void Begin(FillAction action, float x, float y, float w, float h)
        {
            X = x.Round(); 
            Y = y.Round(); 
            Width = (++w).Round();
            Height = (++h).Round();
            Length = (w * h).Round();
            RenderAction = Renderer.CreateFillAction(this);
            FillAction = action;
            Activated = true;
            BeginInternal();
        }
        protected abstract void BeginInternal();
        #endregion

        #region END
        public void End()
        {
            Activated = false;
            CheckPixel = null;
            RenderAction = null;
            X = Y = Width = Height = Length = 0;
            EndInternal();
        }
        protected abstract void EndInternal();
        #endregion

        #region INDEX OF
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(int val, int axis, bool horizontal)
        {
            var x = horizontal ? val : axis;
            var y = horizontal ? axis : val;

            if (IsCut && !CheckPixel(x, y))
                return -1;

            x -= X;
            y -= Y;

            //if (x < 0)
            //    x = 0;
            //if (y < 0)
            //    y = 0;

            var i = x + y * (Width);
            if (i < 0 || i >= Length)
                return -1;

            return i;
        }
        #endregion

        #region ADD
        public void Add(Point p)
        {
            var i = IndexOf(p.X, p.Y, true);
            if (i != -1)
                this[i] = 255;
        }
        public void Add(ILine l) =>
            Renderer.ProcessLine(l, Renderer.LineDraw, RenderAction);
        public void AddRange(IEnumerable<ILine> lines)
        {
            foreach (var item in lines)
                Add(item);
        }
        #endregion
    }
}
