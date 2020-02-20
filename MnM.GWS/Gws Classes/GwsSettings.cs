using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    public abstract class GwsSettings: IDrawSettings
    {
        #region VARIABLES
        #endregion

        #region Constructors
        protected GwsSettings()
        {
            LineDraw = LineDraw.AA;
        }
        protected GwsSettings(IElement shape, IElementCollection path, string reader = null) : this()
        {
            Shape = shape.ID;
            Writer = path.BufferID;
            Reader = reader;
        }
        #endregion

        #region PROPERTIES
        public abstract IRectangle DrawnArea { get; protected set; }
        public abstract int X { get; set; }
        public abstract int Y { get; set; }
        public string Reader { get; protected set; }
        public string Writer { get; protected set; }
        public string Shape { get; protected set; }

        public LineDraw LineDraw { get; set; }
        public bool DrawBoundingBox { get; set; }
        public float Stroke { get; set; }
        public FillMode FillMode { get; set; }
        public SideDraw SideDraw { get;  set; }
        public StrokeMode StrokeMode { get; set; }
        #endregion

        #region SET PARAMS
        public void CopySettings( FillMode? fillMode = null,  float? stroke = null,
             StrokeMode? strokeMode = null,  LineDraw? lineDraw = null,  SideDraw? sideDraw = null,  bool? boundngBox = null)
        {
            if (fillMode != null)
                FillMode = fillMode.Value;
            if (stroke != null)
                Stroke = stroke.Value;
            if (strokeMode != null)
                StrokeMode = strokeMode.Value;
            if (lineDraw != null)
                LineDraw = lineDraw.Value;
            if (sideDraw != null)
                SideDraw = sideDraw.Value;
            if (boundngBox != null)
                DrawBoundingBox = boundngBox.Value;
        }
        #endregion

        #region COPY- RESTORE SETTINGS
        public virtual void CopyFrom<T>( T settings) where T : IPoint
        {
            X = (settings).X;
            Y = (settings).Y;

            if (settings is IDrawInfo)
                DrawnArea = (settings as IDrawInfo).DrawnArea;
           
            if (settings is IDrawInfo2)
            {
                var drawSettings = settings as IDrawInfo2;
                FillMode = drawSettings.FillMode;
                StrokeMode = drawSettings.StrokeMode;
                Stroke = drawSettings.Stroke;
                LineDraw = drawSettings.LineDraw;
                SideDraw = drawSettings.SideDraw;
                DrawBoundingBox = drawSettings.DrawBoundingBox;
            }
            if (settings is IDrawInfo3)
            {
                var shapeInfo = settings as IDrawInfo3;
                Reader = shapeInfo.Reader;
                Writer = shapeInfo.Writer;
            }
        }
        public virtual void Flush()
        {
            X = Y = 0;
            FillMode = FillMode.Original;
            StrokeMode = StrokeMode.Middle;
            Stroke = 0;
            LineDraw = LineDraw.AA;
            SideDraw = SideDraw.All;
            DrawBoundingBox = false;
        }
        #endregion
    }
}
