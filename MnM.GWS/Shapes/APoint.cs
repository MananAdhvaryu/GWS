using MnM.GWS.MathExtensions;
using System;

namespace MnM.GWS
{
    public struct APoint : IAPoint
    {
        const string tostr = "Val1: {0}, Val2: {1}, Axis: {2}";
        public APoint( float val,  int axis,  bool isHorizontal,  int len = 0) : this()
        {
            Val = val;
            IVal = val.Ceiling();
            Axis = axis;
            IsHorizontal = isHorizontal;
            Alpha = null;
            IsFloat = true;
            Len = len;
        }
        public APoint( int val,  int axis,  float? alpha,  bool isHorizontal,  int len = 0) : this()
        {
            Val = val;
            IVal = val;
            Axis = axis;
            IsHorizontal = isHorizontal;
            Alpha = alpha;
            IsFloat = false;
            Len = len;
        }
        public bool IsFloat { get; private set; }
        public int Len { get; private set; }

        public int IVal { get; private set; }
        public float? Alpha { get; private set; }
        public float Val { get; private set; }
        public int Axis { get; private set; }
        public bool IsHorizontal { get; private set; }

        public IAPoint Clone()
        {
            if (IsFloat)
                return new APoint(Val, Axis, IsHorizontal, Len);
            return new APoint(IVal, Axis, Alpha, IsHorizontal, Len);
        }
        public IAPoint Add(float stroke)
        {
            if (IsFloat)
                return new APoint(Val + stroke, Axis, IsHorizontal, Len);
            else
                return new APoint(IVal + stroke.Round(), Axis, Alpha, IsHorizontal, Len);
        }

        public void DrawTo(IBuffer Writer, IBufferPen pen)
        {
            if (Writer == null)
                return;
            if (Len == 0)
            {
                var aa = Implementation.Renderer.AntiAlised;
                if (IsFloat)
                    Writer.WritePixel(Val, Axis, IsHorizontal, pen, aa);
                else
                    Writer.WritePixel(IVal, Axis, IsHorizontal, pen, aa, Alpha);
            }
            else
                Writer.WriteLine(pen, IVal, Axis, IVal, IVal + Len, Axis, IsHorizontal, Alpha);
        }
        public override string ToString()
        {
            return string.Format(tostr, Val, Val + Len, Axis);
        }
    }
}