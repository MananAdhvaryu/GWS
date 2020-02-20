using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS
{
    /// <summary>
    /// Defines the way a shape is drawn. This is a static object there is only one.
    /// You can create a new renderer and use it with a different windowing system.
    /// </summary>
    public partial interface IRenderer : IRotatable, IDispose, IDrawInfo3, IDrawInfo, IDrawSetter
    {
        #region PROPERTIES
        /// <summary>
        /// True by default. Indicates whether renderer scans left to right or top to bottom.
        /// Scan from minimum to maximum of the axis in either the x(horizontal) or y(vertical) direction.
        /// </summary>
        bool HorizontalScan { get; set; }
      
        /// <summary>
        /// Return true if the renderer is drawing a shape. Uses ShapeInfo to hold the information about the shape being drawn.
        /// </summary>
        bool IsDrawingShape { get; }
       
        /// <summary>
        /// Returns true if the renderer is currently switched anti-aliased mode otherwise false.
        /// </summary>
        bool AntiAlised { get; }
      
        /// <summary>
        /// True when there is no Stroke setting available for drawing a shape otherwise false.
        /// </summary>
        bool NoStroke { get; }
        
        /// <summary>
        /// Gets or sets (advanvced) the DrawMode enum.
        /// </summary>
        DrawMode DrawMode
        {
            get;
#if AdvancedVersion
            set;
#endif
        }

        /// <summary>
        /// Stores current settings at the end of the rendering process. This is useful if the same shape is repeated. 
        /// Otherwise settings are cleared at the end of the rendering process.
        /// </summary>
        bool ReuseCurrentReader { get; set; }

        /// <summary>
        /// Indicates which shape is being drawn now.
        /// </summary>
        string ShapeBeingDrawn { get; }
        #endregion

        #region DISTICNCT PIXEL DRAW
        /// <summary>
        /// Returns false if the renderer is writing a pixel where it is not allowed or writing over a previously written pixel.
        /// Prevents overlapping lines during the drawing (only)for example.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool IsPixelWriteable(int index);
      
        /// <summary>
        /// Indicates whether renderer is currently drawing under a disticnt line processig mode or not.
        /// </summary>
        bool DistinctLineProcessing { set; }
        #endregion

        #region COPY MEMORY
        /// <summary>
        /// Copies source menory block to destination memory block.
        /// </summary>
        /// <param name="src">Source memory block</param>
        /// <param name="srcIndex">Index in source from where copy operation must start</param>
        /// <param name="dst">Destination memory block</param>
        /// <param name="destIndex">Index in destination where paste operation should start</param>
        /// <param name="length">Length of pixels to be copied</param>
        unsafe void CopyMemory(int* src, int srcIndex, int* dst, int destIndex, int length);
        #endregion

        #region READ PIXEL
        /// <summary>
        /// Read pixel from buffer pen on a given index.
        /// </summary>
        /// <param name="pen">Buffer pen which to read pixel from</param>
        /// <param name="index">Position in the buffer pen which to read pixel at</param>
        /// <returns></returns>
        int ReadPixel(IBufferPen pen, int index);

        /// <summary>
        /// Reads a pixel from buffer pen after applying a floating point translation to get the correct co-ordinate.
        /// </summary>
        /// <param name="pen">Buffer pen which to read pixel from</param>
        /// <param name="start">Start position of reading on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position of reading on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Position of reading on axis i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="horizontal">Direction of reading: if true horizontally otherwise vertically</param>
        /// <param name="data">Pointer to a data read</param>
        /// <param name="srcIndex">Index from which data should be copied from the pointer</param>
        /// <param name="length">Length upto which data can be read from start index</param>
        void ReadLine(IBufferPen pen, int start, int end, int axis, bool horizontal, out IntPtr data, out int srcIndex, out int length);
        #endregion

        #region RENDER PIXEL - LINE
        /// <summary>
        /// Renders pixel to the specified buffer using specified parameters
        /// </summary>
        /// <param name="buffer">Buffer which to render pixel on</param>
        /// <param name="index">Position on buffer where to render pixel</param>
        /// <param name="color">colour of pixel.</param>
        /// <param name="blend">True if Pixel should be blended with current value</param>
        ///<param name="alpha">Vale by which blending should happen if at all it is supplied</param>
        void RenderPixel(IBuffer buffer, int index, int color, bool blend, float? alpha = null);

        /// <summary>
        /// Renders an axial line (either horizontal or vertical) to the specified buffer target using specified parameters
        /// </summary>
        /// <param name="buffer">Buffer which to render an axial line on</param>
        /// <param name="pen">Buffer pen which to read pixel from</param>
        /// <param name="destVal">Axial position on target buffer: X coordinate if horizontal otherwise Y</param>
        /// <param name="destAxis">>Axial position on target buffer: Y coordinate if horizontal otherwise X</param>
        /// <param name="start">Start position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Position of reading from buffer pen on axis i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical</param>
        ///<param name="alpha">Value by which blending should happen if at all it is supplied</param>
        void RenderLine(IBuffer buffer, IBufferPen pen, int destVal, int destAxis, int start, int end, int axis, bool horizontal, float? alpha = null);
        #endregion

        #region COMPARE PIXEL WITH PEN IT WAS DRAWN BY
        /// <summary>
        /// Compare existing pixel on buffer for specified x and y coordinates to the corresponding pixel on buffer pen.
        /// </summary>
        /// <param name="buffer">Buffer where to read pixel from</param>
        /// <param name="x">Value of X cordinate of location where to read value from</param>
        /// <param name="y">Value of Y cordinate of location where to read value from</param>
        /// <param name="pen">Buffer pen which to compare pixel with</param>
        /// <returns>True if pixels are identical otherwise false</returns>
        bool ComparePixel(IBuffer buffer, int x, int y, IBufferPen pen);
        #endregion

        #region RENDER
        /// <summary>
        /// Renders a shape on specified buffer using specified parameters.
        /// </summary>
        /// <param name="Buffer">Buffer target which to render a shape on</param>
        /// <param name="points">Points which defines perimiter of the shape</param>
        /// <param name="shapeType">Name of the shape</param>
        /// <param name="angle">Angle determines rotation to apply before rendering the shape</param>
        /// <param name="originalArea">Un-rotated area of the shape</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void Render(IBuffer Buffer, IEnumerable<PointF> points, string shapeType, Angle angle, IRectangleF originalArea, IPenContext context = null);

        /// <summary>
        /// Rotates a custom user defined shape. Drawing routine must be provided  here for any custom element 
        /// that you have created and you are aware of. You must return true once you have handled rendering the element.
        /// Otherwise an exception will get thrown.
        /// If you are calling this method directly then you must do so in the follwing manner:
        /// 1. if(!Begin(Path Path, element) return;
        /// 2. RenderCustom(Path, element, context, out IBufferSource reader);
        /// 3. End(Path, reader).
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="element">An element whoch is to be rendered</param>
        /// <param name="context"></param>
        /// <param name="pen">A pen context which to create a buffer pen from</param>
        /// <returns>True if custom rendering is handled otherwise false</returns>
        bool RenderCustom(IBuffer buffer, IElement element, IPenContext context, out IBufferPen pen);
        #endregion

        #region RENDER SHAPE
#if AdvancedVersion
        /// <summary>
        /// Renders any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="drawX">X coordinate of a location where draw should take place</param>
        /// <param name="drawY">Y coordinate of a location where draw should take place</param>
        void Render(IBuffer buffer, IElement shape, IPenContext context, int? drawX, int? drawY);
#else
        /// <summary>
        /// Renders any element on the given path. This renderer has a built-in support for the following kind of elements:
        /// 1. IShape
        /// 2. IDrawable
        /// 3. ICurve
        /// 4. IText
        /// Please note that in case your element does not implement any of the above, you must provide your own rendering routine
        /// by overriding RenderCustom method. Once you have handled it return true otherwise an exception wiil be raised.
        /// 
        /// </summary>
        /// <param name="buffer">Buffer target which to render a shape on</param>
        /// <param name="shape">Element which is to be rendered</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void Render(IBuffer Buffer, IElement shape, IPenContext context = null) ;
#endif
        #endregion

        #region RENDER IMAGE
        /// <summary>
        /// Copies an area from a 1D array representing a rectangele to the given destination.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">1D array interpreted as a 2D array of Pixels with specified srcW width</param>
        /// <param name="srcLen">Length of an entire source</param>
        /// <param name="srcW">Width of the entire source</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        unsafe void RenderImage(IBuffer buffer, int* source, int srcLen, int srcW, int destX, int destY, int? copyX = null, int? copyY = null,
            int? copyW = null, int? copyH = null);

        /// <summary>
        /// Copies an area from a source - capable of being copied to the given destination buffer.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="source">Source - capable of being copied to any buffer</param>
        /// <param name="destX">Top Left x co-ordinate of destination on buffer</param>
        /// <param name="destY">Top left y co-ordinate of destination on buffer</param>
        /// <param name="copyX">Top left x co-ordinate of area in source to cop.</param>
        /// <param name="copyY">Top left y co-ordinate of area in source to copy</param>
        /// <param name="copyW">Width of area in the source to copy.</param>
        /// <param name="copyH">Height of area in the source to copy</param>
        void RenderImage(IBuffer buffer, IBufferCopy source, int destX, int destY, int copyX, int copyY, int copyW, int copyH);
        #endregion

        #region PEN MANAGEMENT
        /// <summary>
        /// Gets an existing pen or creates one matching the size of the shape which is to be rendered.
        /// </summary>
        /// <param name="shape">An element for which buffer pen is required</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <returns>Buffer pen</returns>
        IBufferPen GetPen(IOccupier shape, IPenContext context);
        #endregion

        #region FILL
        /// <summary>
        /// Fills the area covered by specified lines.
        /// </summary>
        /// <param name="buffer">Buffer which to render an axial line on</param>
        /// <param name="lines">Lines which forms the perimeter of a shape</param>
        /// <param name="pen">Buffer pen which to read pixel from</param>
        /// <param name="start">Start position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position of reading from buffer pen on axis i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="shapeType">Name of the shape - which lines form the perimeter of</param>
        /// <param name="fillType">Type of filling operation method</param>
        void Fill(IBuffer buffer, IEnumerable<ILine> lines, IBufferPen pen, int start, int end, bool horizontal, string shapeType, PolyFill fillType = PolyFill.OddEven);
        #endregion

        #region CREATE LINE ACTION
        /// <summary>
        /// Retuns an action delegate for rendering a glyph on specified buffer target using specified buffer pen.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="pen">Buffer pen which to read pixeld from</param>
        /// <returns>An instance of GlyphFillAction delegate</returns>
        GlyphFillAction CreateGlyphFillAction(IBuffer buffer, IBufferPen pen);

        /// <summary>
        /// Retuns an action delegate for rendering an axial line or pixel on specified buffer target using specified buffer pen.
        /// </summary>
        /// <param name="buffer">Buffer which to render a memory block on</param>
        /// <param name="pen">Buffer pen which to read pixeld from</param>
        /// <returns>An instance of FillAction delegate</returns>
        FillAction CreateFillAction(IBuffer buffer, IBufferPen pen);

        /// <summary>
        /// Retuns an action delegate for storing an axial line or pixel information in specified list.
        /// </summary>
        /// <param name="list">A list to accumulate pixels and axial lines resulted from executing an action</param>
        /// <returns>An instance of FillAction delegate</returns>
        FillAction CreateFillAction(ICollection<Point> list);

        /// <summary>
        /// Retuns an action delegate for rendering an axial line or pixel on specified memory block using specified color enumerator.
        /// </summary>
        /// <param name="enumerator">Colour enumerator to read colors from</param>
        /// <param name="target">Target memory block to perform an action on - usally a rendering</param>
        /// <param name="targetLen">Length of a target memory block</param>
        /// <param name="targetWidth">Width of a target memory block</param>
        /// <returns></returns>
        unsafe FillAction CreateFillAction(IColorEnumerator enumerator, int* target, int targetLen, int targetWidth);

        /// <summary>
        /// Retuns an action delegate for storing an axial line information on specified buffer target using specified buffer pen.
        /// </summary>
        /// <param name="Data">Dictionary of which a key denotes to an axis and values denotes to a list of values which are the result of scanning</param>
        /// <returns>An instance of FillAction delegate</returns>
        FillAction CreateLineAction(Dictionary<int, List<float>> Data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">Instance represented by IFloodFill interface to record pixel hits and blending alpha information faking a rendering of any shape</param>
        /// <returns>An instance of FillAction delegate</returns>
        FillAction CreateFillAction(IFloodFill target);
        #endregion

        #region BRESENHAM LINE
        /// <summary>
        /// Processes a breshenham line using breshenham line algorithm between two points specified by x1, y1 and x2, y2 using specified action.
        /// </summary>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="action">A FillAction delegate which has routine to do something with the information emerges by using breshenham line algorithm</param>
        /// <param name="draw">LineDraw option indicating the use of Anti-Aliasing etc</param>
        /// <returns>True if operation is successful otherwise false</returns>
        bool ProcessBresenhamLine(int x1, int y1, int x2, int y2, FillAction action, LineDraw draw);

        /// <summary>
        /// Processes a breshenham line on specified buffer using breshenham line algorithm between two points specified by x1, y1 and x2, y2 using specified buffer pen.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="pen">A pen context which to create a buffer pen from</param>
        /// <returns>True if operation is successful otherwise false</returns>
        bool ProcessBresenhamLine(IBuffer Buffer, int x1, int y1, int x2, int y2, IBufferPen pen);
        #endregion

        #region PROCESS LINE
        /// <summary>
        /// Processes a collection of lines using standard line algorithm between two points of a line segment using specified action.
        /// </summary>
        /// <param name="lines">Collection of lines to render</param>
        /// <param name="action">A FillAction delegate which has routine to do something with the information emerges by using standard line algorithm</param>
        /// <param name="draw">LineDraw option indicating the use of Anti-Aliasing etc</param>
        /// <param name="skip">LineSkip option used to filter the lines so that shallow and steep gradients can be processed seperately.</param>
        void ProcessLines(IEnumerable<ILine> lines, FillAction action, LineDraw draw, LineSkip skip = LineSkip.None);

        /// <summary>
        /// Processes a line using standard line algorithm between two points of a line segment using specified action.
        /// </summary>
        /// <param name="line">A line to render</param>
        /// <param name="draw">LineDraw option indicating the use of Anti-Aliasing etc</param>
        /// <param name="action">A FillAction delegate which has routine to do something with the information emerges by using standard line algorithm</param>
        /// <param name="stroke">Vale of stroke to be applied to original line.
        /// If value is other than 0, it results in a rendering of a trapezium instead of the line</param>
        /// <returns>True if operation is successful otherwise false</returns>
        bool ProcessLine(ILine line, LineDraw draw, FillAction action, float stroke = 0);

        /// <summary>
        /// Processes a line using standard line algorithm between two points specified by x1, y1 and x2, y2 of a line segment using specified action.
        /// </summary>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="m">The slope of the line - specified by x1, y1 and x2, y2 and calculaed using standard line equation y = mx + c</param>
        /// <param name="c">The Y-Intersect of the line - specified by x1, y1 and x2, and calculaed using standard line equation y = mx + c</param>
        /// <param name="steep">steep indicates to draw from X1 to X2 or Y1 to Y2. This value is true if absolute DY > absolute DX otherwise false</param>
        /// <param name="draw">LineDraw option indicating the use of Anti-Aliasing etc</param>
        /// <param name="action">A FillAction delegate which has routine to do something with the information emerges by using standard line algorithm</param>
        /// <param name="stroke">Vale of stroke to be applied to original line.If value is other than 0, it results in a rendering of a trapezium instead of the line</param>
        /// <returns>True if operation is successful otherwise false</returns>
        bool ProcessLine(float x1, float y1, float x2, float y2, float m, float c, bool steep, LineDraw draw, FillAction action, float stroke = 0);

        /// <summary>
        /// Processes a collection of lines using standard line algorithm between two points of a line segment using specified action.
        /// While processing each line, the processing will not exceed the boundaries defined by start and end values.
        /// For example if horizontal start = MinY and End = MaxY. Let say it is 100 and 300 respectively.
        /// Now while processing a line say 50, 99, 90, 301, position of y1 at 99 and y2 at 301 will be ignored as they do not fall in the range of 100 -300.
        /// </summary>
        /// <param name="lines">Collection of lines to render</param>
        /// <param name="action">A FillAction delegate which has routine to do something with the information emerges by using standard line algorithm</param>
        /// <param name="start">Minimum value of axial value i.e MinX if not horizontal other wise MinY</param>
        /// <param name="end">Maximum value of axial value i.e MaxX if not horizontal other wise MaxY</param>
        /// <param name="horizontal"></param>
        void ProcessLines(IEnumerable<ILine> lines, FillAction action, int start, int end, bool horizontal);
        #endregion

        #region LOCATION SET
        /// <summary>
        /// Freezes X and Y cordinates from keep changing further in the context of current rendering only.
        /// The Freeze occurs on buffer pen if caller entity is buffer pen from which pixels are read otherwise a buffer to which pixels are written.
        /// </summary>
        /// <param name="caller">Enum to determine whether the buffer pen or buffer taget  to freeze</param>
        /// <param name="value">If True freeze is on be applied otherwise it is off</param>
        void FreezeXY(Entity caller, bool value);

        /// <summary>
        /// Sets X and Y cordinates of a caller entity in the context of current rendering only.
        /// </summary>
        /// <param name="caller">Enum to determine whether the buffer pen or buffer taget  to change</param>
        /// <param name="x">Value of X coordinate</param>
        /// <param name="y"></param>
        void SetXY(Entity caller, int? x, int? y);
        #endregion

        #region CORRECT XY
        /// <summary>
        /// Applies offest to X and Y cordinates of a caller entity in the context of current rendering only.
        /// </summary>
        /// <param name="caller">Enum to determine whether the buffer pen or buffer taget to correct</param>
        /// <param name="x">x offset.</param>
        /// <param name="y">y offset.</param>
        void CorrectXY(Entity caller, ref int x, ref int y);
        #endregion

        #region ROTATION
        /// <summary>
        /// Rotates the caller entity i.e either a buffer pen (If Reader is selected) otherwise a target buffer by a given angle.
        /// </summary>
        /// <param name="caller">Enum to determine whether the buffer pen or target buffer is being rotated</param>
        /// <param name="angle">Angle specification for rotation.</param>
        void RotateTransform(Entity caller, Angle angle);

        /// <summary>
        /// Resets the rotation angle to 0 of the caller entity.
        /// </summary>
        /// <param name="target">Enum indicating whether Reader or Writer is to be reset.</param>
        void ResetTransform(Entity caller);

        /// <summary>
        /// Indicates if a caller entity is in rotated state at the moment.
        /// </summary>
        /// <param name="caller">Enum to determine whether the buffer pen or target buffer is in rotated state</param>
        /// <returns></returns>
        bool IsRotated(Entity caller);

        /// <summary>
        /// Gets transformed X and Y coordinates according to a rotation angle cuurently applied on caller entity
        /// </summary>
        /// <param name="caller">Enum to determine whether the buffer pen or target buffer is being rotated</param>
        /// <param name="val">Position on axis - X cordinate if horizontal otherwise Y.</param>
        /// <param name="axis">Position of axis -Y cordinate if horizontal otherwise X.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical.</param>
        /// <param name="horizontal">Axis orientation - horizontal if true otherwise vertical</param>
        /// <param name="x">Tranformed X coordinate</param>
        /// <param name="y">Tranformed Y coordinate</param>
        void GetRotatedXY(Entity caller, float val, float axis, bool horizontal, out float x, out float y);
        #endregion

        #region LINE
        /// <summary>
        /// Renders a line segment using standard line algorithm between two points specified by x1, y1 and x2, y2.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 before rendering the line segment</param>
        void RenderLine(IBuffer buffer, float x1, float y1, float x2, float y2, IPenContext context, Angle angle = default(Angle));

        /// <summary>
        /// Renders a line segment using standard line algorithm between two start and end points.
        /// </summary>
        /// <param name="buffer">Buffer which to render a line on</param>
        /// <param name="line">A line segment to render</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void RenderLine(IBuffer buffer, ILine line, IPenContext context);
        #endregion

        #region ELLIPSE OR PIE
        /// <summary>
        /// Renders circle or ellipse specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a circle/ellipse on</param>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the circle/ellipse</param>
        void RenderCircleOrEllipse(IBuffer buffer, float x, float y, float width, float height, IPenContext context, Angle angle = default(Angle));
        #endregion

        #region ARC OR PIE
        /// <summary>
        /// Renders an arc or pie specified by the bounding area and angle of rotation if supplied using various option supplied throuh CurveType enum.
        /// </summary>
        /// <param name="buffer">Buffer which to render an arc/pie on</param>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the arc/pie</param>
        /// <param name="type"> Defines the type of curve for example an arc or pie etc. along with other supplimentary options on how to draw it</param>
        void RenderArcOrPie(IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle, IPenContext context,
            Angle angle = default(Angle), CurveType type = CurveType.Pie);
        #endregion

        #region CURVE
        /// <summary>
        /// Renders a curve object.
        /// </summary>
        /// <param name="buffer">Buffer which to render a curve on</param>
        /// <param name="curve">Cureve object to render</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void RenderCurve(IBuffer buffer, ICurve curve, IPenContext context = null);
        #endregion

        #region BEZIER
        /// <summary>
        /// Renders a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier on</param>
        /// <param name="points">Defines perimiter of the bezier</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        void RenderBezier(IBuffer buffer, IEnumerable<float> points, BezierType type, IPenContext context, Angle angle = default(Angle));
        #endregion

        #region BEZIER ARC OR PIE
        /// <summary>
        /// Renders a bezier arc or pie specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a bezier bezier arc/pie on</param>
        /// <param name="x">X cordinate of a bounding area where the bezier arc/pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier arc/pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier arc/pie is to be drawn -> bezier arc/pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier arc/pie is to be drawn ->bezier arc/pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="isArc">Indicates whether or not this to render an arc or pie</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc/pie</param>
        /// <param name="noSweepAngle">Determines whether the end angle has to be sweeped or not. By default End Angle is sweeped, i.e End Angle += Start Angle</param>
        void RenderBezierArcOrPie(IBuffer buffer, float x, float y, float width, float height, float startAngle, float endAngle,
            bool isArc, IPenContext context, Angle angle = default(Angle), bool noSweepAngle = false);
        #endregion

        #region TRINAGLE
        /// <summary>
        /// Renders a trianle formed by three points specified by x1,y1 & x2,y2 & x3,y3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a triangle on</param>
        /// <param name="x1">X corodinate of the first point</param>
        /// <param name="y1">Y corodinate of the first point</param>
        /// <param name="x2">X corodinate of the second point</param>
        /// <param name="y2">Y corodinate of the second point</param>
        /// <param name="x3">X corodinate of the third point</param>
        /// <param name="y3">Y corodinate of the third point</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the traingle</param>
        void RenderTriangle(IBuffer buffer, float x1, float y1, float x2, float y2, float x3, float y3, IPenContext context, Angle angle = default(Angle));
        #endregion

        #region RECTANGLE
        /// <summary>
        /// Renders a rectangle specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rectangle on</param>
        /// <param name="x">X cordinate of the rectangle</param>
        /// <param name="y">Y cordinate of the rectangle</param>
        /// <param name="width">Width of the rectangle/param>
        /// <param name="height">Height the rectangle</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rectangle</param>
        void RenderRectangle(IBuffer buffer, float x, float y, float width, float height, IPenContext context, Angle angle = default(Angle));
        #endregion

        #region ROUNDED BOX
        /// <summary>
        /// Renders a rounded box specified by x, y, width, height parameters and angle of rotation if supplied and a hull convex of circle determined by corner radius at all four corners.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rounded box on</param>
        /// <param name="x">X cordinate of the rounded box</param>
        /// <param name="y">Y cordinate of the rounded box</param>
        /// <param name="width">Width of the rounded box/param>
        /// <param name="height">Height the rounded box</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        void RenderRoundedBox(IBuffer buffer, float x, float y, float width, float height, float cornerRadius, IPenContext context, Angle angle = default(Angle));
        #endregion

        #region RHOMBUS
        /// <summary>
        /// Renders a rhombus specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="x">X cordinate of the bounding rectangle</param>
        /// <param name="y">Y cordinate of the bounding rectangle</param>
        /// <param name="width">Width of the bounding rectangle/param>
        /// <param name="height">Height the bounding rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not zero, it replaces the value of width parameter</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void RenderRhombus(IBuffer buffer, float x, float y, float width, float height, Angle angle, float? deviation, IPenContext context);
        #endregion

        #region TRAPEZIUM
        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line, parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a trapezium on</param>
        /// <param name="baseLine">A line from where the trapezium start</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void RenderTrapezium(IBuffer buffer, ILine baseLine, float parallelLineDeviation, float parallelLineSizeDifference, Angle angle, IPenContext context);
        #endregion

        #region POLYGON
        /// <summary>
        /// Renders a polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="buffer">Buffer which to render a polygon on</param>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        /// <param name="angle">Angle to apply rotation while rendering the polygon</param>
        void RenderPolygon(IBuffer buffer, IEnumerable<float> polyPoints, IPenContext context, Angle angle = default(Angle));
        #endregion

        #region GLYPHS
        /// <summary>
        /// Renders a collection of glyphs.
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="glyphs">A collection of glyphs to render</param>
        /// <param name="pen">A buffer to use while rendering the glyphs</param>
        void RenderGlyphs(IBuffer buffer, IEnumerable<IGlyph> glyphs, IBufferPen pen);

        /// <summary>
        /// Renders a text object which represents a text and a collection of glyphs providing drawing representation of the text. 
        /// </summary>
        /// <param name="buffer">Buffer which to render a rhombus on</param>
        /// <param name="text">A text object to render</param>
        /// <param name="context">A pen context which to create a buffer pen from</param>
        void RenderText(IBuffer buffer, IText text, IPenContext context = null);
        #endregion

        #region STROKING
        /// Get the point join rules of the shape named.
        /// </summary>
        /// <param name="Name">Case sensitive name of shape as used in IRecognizable e.g. "BezierArc".</param>
        /// <returns>Returns Enum description of the Join required.</returns>
        PointJoin GetStrokeJoin(string Name);

        /// <summary>
        /// Returns the rule for joining close points for standard shapes.  !!!!what is too close!!!!
        /// </summary>
        /// <param name="Name">Case sensitive name of shape as used in IRecognizable e.g. "BezierArc".</param>
        /// <returns>True if Points should not be joined for the shape when they are too close.!!!!</returns>
        bool DontJoinPointsIfTooClose(string Name);


        /// <summary>
        /// Gets the AfterStroke rule for the Shape named. So that is can be drawn correctly.
        /// </summary>
        /// <param name="Name">Case sensitive name of shape as used in IRecognizable e.g. "BezierArc".</param>
        /// <returns>AfterStroke Enum for Named shape. </returns>
        AfterStroke GetAfterStroke(string Name);

        /// <summary>
        /// Get the rule for line drawing for the given shape.
        /// </summary>
        /// <param name="Name">Case sensitive name of shape as used in IRecognizable e.g. "BezierArc".</param>
        /// <returns>Returns the LineDraw enum used to decide how line is drawn.</returns>
        LineDraw GetLineDraw(string Name);
       
        void GetLineSkip(string Name, bool willbeFilling, FillMode mode, out LineSkip forData0, out LineSkip forData2);
        
        /// <summary>
        /// Gets the applicable fillmode for a given shape name depending on vakue of stroke
        /// </summary>
        /// <param name="current">Current fill mode</param>
        /// <param name="Name">Name of the shape</param>
        /// <param name="Stroke">Value of stroke</param>
        /// <returns>Compitible fill mode to render the shape</returns>
        FillMode GetFillMode(FillMode current, string Name, float Stroke);

        /// <summary>
        /// Return the swap perimeters state. 
        /// Important for besier where inside and outside can swap due to the orientation of the line.!!!!or a bug!!!!
        /// </summary>
        /// <param name="Name">Case sensitive name of shape as used in IRecognizable e.g. "BezierArc".</param>
        /// <returns>True if perimeters do not need to be swapped.</returns>
        bool NoeedToSwapPerimeters(string Name);
        #endregion
    }

    /// <summary>
    /// Keeps track of the changes made so they can be redrawn.
    /// </summary>
    public interface IUpdateTracker
    {
        /// <summary>
        /// An updatemanager object which is a collection of all invalidated rectangles representing changes made on target buffer.
        /// </summary>
        IUpdateManager PendingUpdates { get; }
    }

    /// <summary>
    /// Holds a collection of invalidated rectangles which represents changes made on target buffer.
    /// </summary>
    public interface IUpdateManager : IEnumerable<IRectangle>, IInvalidater
    {
        /// <summary>
        /// Numbers of rectangles in collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the most recent changes in terms of area.
        /// </summary>
        IRectangle Recent { get; }

        /// <summary>
        /// Gets or sets a flag indication whether or not to suspends automatic removal of any rectangle from the collection after its requirement is not needed. i.e. after screen update is done on an area specified by rectangle
        /// </summary>
        bool SuspendRemoval { get; set; }

        /// <summary>
        /// Cleears the collection. Count becomes zero.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes a specific rectangle from the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Clear(IRectangle item);

        /// <summary>
        /// Tests if a specified rectangle exist in the collecion.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(IRectangle item);

        /// <summary>
        /// Tranfer exisitng rectangles to specified update manager.
        /// </summary>
        /// <param name="other">Other update manager to receive he transfer</param>
        /// <param name="remove">If true it removes the rectangles from the collection otherwise not</param>
        void TransferTo(IUpdateManager other, bool remove = true);

        /// <summary>
        /// Notifies the subscriber when a new area is invalidated in this manager.
        /// </summary>
        event EventHandler<InvalidateEventArgs> Invalidated;
    }

    /// <summary>
    /// Represents an object which has a capability to update the screen display. For example render window.
    /// </summary>
    public interface IUpdatable 
    {
        /// <summary>
        /// Gets or sets a flag indication whether or not to suspends any update attempt.
        /// </summary>
        bool SuspendLayout { get; set; }

        /// <summary>
        /// Submit all data blocks covered by rectangles in update manager collection for update.
        /// </summary>
        void Submit();

        /// <summary>
        /// submit a data block covered by specific rectangle for update.
        /// </summary>
        /// <param name="rectangle"></param>
        void Submit( IRectangle rectangle);

        /// <summary>
        /// Clears the update manager collection so that invalidated rectangles no longer exists to prevent scrren update.
        /// </summary>
        void Discard();

        /// <summary>
        /// Remove a specified rectangle from update manager so that invalidated area no longer exists to prevent scrren update.
        /// </summary>
        /// <param name="rectangle"></param>
        void Discard(IRectangle rectangle);
    }

    /// <summary>
    /// Represents an object which has a capability to upload data to an IUpdateable object. For example render window.
    /// </summary>
    public interface IUploadable : ISize, IStoreable
    {
        /// <summary>
        /// Uploads all data blocks covered by rectangles in update manager collection for update to IUpdateable object.
        /// </summary>
        void Upload();

        /// <summary>
        /// Uploads a data block covered by specified rectangle for update to IUpdateable object.
        /// </summary>
        /// <param name="rectangle"></param>
        void Upload(IRectangle rectangle);

        /// <summary>
        /// Uploads a data block covered by specified rectangle for update to IUpdateable object to an area specified by the destination rectangle.
        /// </summary>
        /// <param name="copyRc"></param>
        /// <param name="destRc"></param>
        void Upload(IRectangle copyRc, IRectangle destRc);

        /// <summary>
        /// Downloads all data blocks covered by rectangles in update manager collection from IUpdateable object back to buffer target.
        /// </summary>
        void Download();

        /// <summary>
        /// Downloads a data block covered by specific rectanglefrom IUpdateable object back to buffer target.
        /// </summary>
        /// <param name="rectangle"></param>
        void Download(IRectangle rectangle);
    }

    /// <summary>
    /// Represents an object whic provides capablilities to invalidate its area so that later it can be updated.
    /// </summary>
    public interface IInvalidater
    {
        /// <summary>
        /// Marks an area as invalidated
        /// </summary>
        /// <param name="rectangle">An area to be invalidated</param>
        void Invalidate(IRectangle rectangle);

        /// <summary>
        /// Marks all areasspecified in collection as invalidated.
        /// </summary>
        /// <param name="rectangles">Areas to be invalidated</param>
        void Invalidate(IEnumerable<IRectangle> rectangles);
    }

    /// <summary>
    /// Not implemented! work in progress!
    /// </summary>
    public interface IFloodFill
    {
        byte this[int index] { get; set; }
        FillAction FillAction { get; }
        Func<float, float, bool> CheckPixel { set; }
        int IndexOf(int val, int axis, bool horizontal);

        void Begin(FillAction action, float x, float y, float w, float h);
        void End();
    }
}
