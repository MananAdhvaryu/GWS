using System;

namespace MnM.GWS
{
    public interface IDrawInfo
    {
        IRectangle DrawnArea { get; }
    }
    public interface IDrawParams: IPoint
    {
        FillMode FillMode { get; }
        float Stroke { get; }
        LineDraw LineDraw { get; }
        bool DrawBoundingBox { get; }
        SideDraw SideDraw { get; }
        StrokeMode StrokeMode { get; }
    }
    public interface IDrawInfo2 : IPoint, IDrawParams { }
    public interface IDrawSettings : IDrawInfo2, IDrawInfoReceiver
    {
        new int X { get; set; }
        new int Y { get; set; }
        new FillMode FillMode { get; set; }
        new float Stroke { get; set; }
        new LineDraw LineDraw { get; set; }
        new bool DrawBoundingBox { get; set; }
        new SideDraw SideDraw { get; set; }
        new StrokeMode StrokeMode { get; set; }
    }
    public interface IDrawSetter: IDrawSettings, IPenContext
    {
        IPenContext ReadContext { get; set; }
    }
    public interface IDrawInfo3 : IDrawInfo2 , IDrawInfoReceiver, IDrawInfo
    {
        string Reader { get; }
        string Writer { get; }
        string Shape { get; }
    }
    public interface IDrawInfoReceiver 
    {
        void CopySettings( FillMode? fillMode = null, 
             float? stroke = null, 
             StrokeMode? strokeMode = null, 
             LineDraw? lineDraw = null, 
             SideDraw? sideDraw = null, 
             bool? boundngBox = null);

        void CopyFrom<T>( T settings) where T : IPoint;
        void Flush();
    }
}
