using System;
using System.Collections.Generic;
using System.IO;

namespace MnM.GWS
{
    #region ATTACHMENT
    /// <summary>
    /// This is a marker interface. It can be attached to the Implemnetation class of GWS.
    /// </summary>
    public interface IAttachment : IDisposable { }
    #endregion

    #region FACTORY
    public partial interface IFactory : IAttachment
    {
        #region DEFAULT INSTANCES
        /// <summary>
        /// Gets the default font renderer provide by the GWS.
        /// </summary>
        IGlyphRenderer FontRenderer { get; }
        /// <summary>
        /// Retrieves a default disabled pen available GWS.
        /// </summary>
        ISolidPen DisabledPen { get; }
        /// <summary>
        /// Returns a default XAxis line starts from origin and end at (7000, 0)
        /// </summary>
        ILine XAxis { get; }
        /// <summary>
        /// Returns a default YAxis line starts from origin and end at (0, 7000)
        /// </summary>
        ILine YAxis { get; }
        /// <summary>
        /// Returns a default Empty event info object.
        /// </summary>
        EventInfo EventInfoEmpty { get; }
        /// <summary>
        /// Returns a default empty line object.
        /// </summary>
        ILine LineEmpty { get; }
        /// <summary>
        /// Returns a default empty rectangle.
        /// </summary>
        IRectangle RectEmpty { get; }
        /// <summary>
        /// Returns a default empty rectangleF.
        /// </summary>
        IRectangleF RectFEmpty { get; }
        /// Returns a default empty box.
        IBox BoxEmpty { get; }
        /// Returns a default empty boxF.
        IBoxF BoxFEmpty { get; }
        /// Returns a default empty event args.
        EventArgs EventArgsEmpty { get; }
        /// <summary>
        /// Returns a default system font available in GWS.
        /// The font is: UbuntuMono-Regular and is covered under UBUNTU FONT LICENCE Version 1.0.
        /// </summary>
        IFont SystemFont { get; }
        /// <summary>
        /// Returns the default image processor available in GWS for reading and writing image files and memory buffers.
        /// the GWS uses STBIMage internally. For more info on STBImage visit: https://github.com/nothings/stb
        /// </summary>
        IImageProcessor ImageProcessor { get; }
        #endregion

        #region COLOR
        /// <summary>
        /// Creates a new Colour structure with Red, Green, Blue and Alpha components.
        /// </summary>
        /// <param name="r">Red component</param>
        /// <param name="g">Green component</param>
        /// <param name="b">Blue component</param>
        /// <param name="a">Alpha component</param>
        /// <returns></returns>
        IRGBA newColor(byte r, byte g, byte b, byte a = 255);
        #endregion

        #region GRAPHICS
        /// <summary>
        /// Creates a new GWS Graphics object of given width and height with data provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Byte array to copy. Please note that the array will be converted to int[] first.
        /// </param>
        /// <param name="makeCopy">If true then copy the buffer array into internal memory buffer
        /// otherwise set an internal menory buffer referring to the buffer supplied.
        /// </param>
        /// <returns>IGraphics</returns>
        IGraphics newGraphics(int width, int height, byte[] pixels, bool makeCopy = false);

        /// <summary>
        /// Creates a new GWS Graphics object of given width and height with data provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Int array to copy</param>
        /// <param name="makeCopy">If true then copy the buffer array into internal memory buffer
        /// otherwise set an internal menory buffer referring to the buffer supplied.
        /// </param>
        /// <returns>IGraphics</returns>
        IGraphics newGraphics(int width, int height, int[] pixels, bool makeCopy = false);

        /// <summary>
        /// Creates a new GWS Graphics object of given width and height with data provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Pointer data to copy</param>
        /// <param name="bufferLength">Length of the buffer pointer.</param>
        /// <returns>IGraphics</returns>
        IGraphics newGraphics(int width, int height, IntPtr pixels, int bufferLength);

        /// <summary>
        /// Creates a new GWS Graphics object of given width and height.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <returns>IGraphics</returns>
        IGraphics newGraphics(int width, int height);

        /// <summary>
        /// Creates a new GWS Graphics object and fills it with the data received from the disk image file located on a given path.
        /// </summary>
        /// <param name="path">Path of the disk image file to use as a initial source of Graphics object</param>
        /// <returns>IGraphics</returns>
        IGraphics newGraphics(string path);

        /// <summary>
        /// Creates a new GWS Graphics object and fills it with the supplied buffer. 
        /// Actually internal memory buffer is set to refer the buffer supplied.
        /// </summary>
        /// <param name="pixels">Byte array to use as internal memory buffer</param>
        /// <returns>Graphics object</returns>
        IGraphics newGraphics(byte[] pixels);
        #endregion

        #region BUFFER
        /// <summary>
        /// Creates a new GWS Buffer object of given width and height with pixels provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// </param>
        /// <returns>IBuffer</returns>
        IBuffer newBuffer(int width, int height);

        /// <summary>
        /// Creates a new GWS Buffer object of given width and height with pixels provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Pointer containing data to use. Please note that the array will be converted to int[] first.
        /// </param>
        /// <param name="makeCopy">If true then copy the buffer array into internal memory buffer
        /// otherwise set an internal menory buffer referring to the pixel pointer supplied.
        /// </param>
        /// <returns>IBuffer</returns>
        unsafe IBuffer newBuffer(int* pixels, int width, int height, bool makeCopy = false);

        /// <summary>
        /// Creates a new GWS Buffer object of given width and height with pixels provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">pixel array containing color data/// </param>
        /// <param name="makeCopy">If true then copy the pixel array into internal memory buffer
        /// otherwise set an internal menory buffer referring to the pixel array supplied.
        /// </param>
        /// <returns>IBuffer</returns>
        IBuffer newBuffer(int[] pixels, int width, int height, bool makeCopy = false);
        #endregion

        #region ELEMENT COLLECTION
        /// <summary>
        /// Creates a new collection of elements
        /// </summary>
        /// <param name="window">Parent Window which is to hold this collection</param>
        /// <returns>IElementCollection</returns>
        IElementCollection newElementCollection(IParent window);
        #endregion

        #region UPDATE MANAGER
        /// <summary>
        /// Creates a new update manager.
        /// This object lets user store and remove invalided areas while performing drawing operation.
        /// </summary>
        /// <returns>IUpdateManger</returns>
        IUpdateManager newUpdateManager();
        #endregion

        #region BUFFER COLLECTION
#if AdvancedVersion
        /// <summary>
        /// Creates a collection to hold buffers to enable user to maintain and use multiple buffers with any parent window and graphics.
        /// </summary>
        /// <returns>IBufferCollection</returns>
        IBufferCollection newBufferCollection();

        /// <summary>
        /// Creates a collection to hold buffers to enable user to maintain and use multiple buffers with any parent window and graphics.
        /// </summary>
        /// <param name="capacity">Initiali capacity of the collection. the default is 4</param>
        /// <returns></returns>
        IBufferCollection newBufferCollection(int capacity);
#endif
        #endregion

        #region RENDERER
        /// <summary>
        /// Creates a new GWS renderer to perform rendering on any buffer.
        /// Only one instance of this object is required by GWS.
        /// If you override the default GWS Factory class, and this method call, 
        /// You need to provide a valid IRenderer implementation.
        /// </summary>
        /// <returns>IRenderer</returns>
        IRenderer newRenderer();
        #endregion

        #region FLOOD FILL
        /// <summary>
        /// Not implemented yet so do not call this method.
        /// </summary>
        /// <returns></returns>
        IFloodFill newFloodFill();
        #endregion

        #region PEN
        /// <summary>
        /// Creates a solid pen of certain width and height fill with specified color.
        /// </summary>
        /// <param name="color">Colour from which this pen is to be created.</param>
        /// <param name="width">Expected width of the pen</param>
        /// <param name="height">Expected height of the pen</param>
        /// <returns></returns>
        ISolidPen newPen(int color, int width, int height);
        #endregion

        #region BRUSH
        /// <summary>
        /// Creates a new brush of certain width and height using specified fill style.
        /// </summary>
        /// <param name="style">Fill style to be used to fill the brush</param>
        /// <param name="width">Expected width of the brush</param>
        /// <param name="height">Expected height of the brush</param>
        /// <returns></returns>
        IBrush newBrush(IFillStyle style, int width, int height);
        #endregion

        #region FILL STYLE
        /// <summary>
        /// Creates a new fill style with specified gradient and values of colors to use.
        /// </summary>
        /// <param name="stops">Colour stops to use while navigating a color spectrum offered by the style</param>
        /// <param name="gradient">Gradient enum determines which gradient style to use</param>
        /// <param name="values">Colours represented by int values</param>
        /// <returns>IFillStyle object</returns>
        IFillStyle newFillStyle(IList<int> stops, Gradient gradient, params int[] values);
        #endregion

        #region APOINT
        /// <summary>
        /// Creates a new APoint with specified parameters
        /// </summary>
        /// <param name="val">Value of start in axial line - X cordinate if isHorizontal and Y if not</param>
        /// <param name="axis">Axis in axial line - X cordinate if isHorizontal and Y if not</param>
        /// <param name="isHorizontal">Determined orientation of axis line i.e horizontal or vertical</param>
        /// <param name="len">A value of stretch from Value</param>
        /// <returns>IApoint</returns>
        IAPoint newAPoint(float val, int axis, bool isHorizontal, int len = 0);
        /// <summary>
        /// Creates a new APoint with specified parameters
        /// </summary>
        /// <param name="val">Value of start in axial line - X cordinate if isHorizontal and Y if not</param>
        /// <param name="axis">Axis in axial line - X cordinate if isHorizontal and Y if not</param>
        /// <param name="alpha">A value of alpha blend to be used for alpha blending if at all supplied</param>
        /// <param name="isHorizontal">Determined orientation of axis line i.e horizontal or vertical</param>
        /// <param name="len">A value of stretch from Value</param>
        /// <returns>IApoint</returns>
        IAPoint newAPoint(int val, int axis, float? alpha, bool isHorizontal, int len = 0);
        #endregion

        #region DRAW SETTINGS
        /// <summary>
        /// Creates a new draw settings.
        /// </summary>
        /// <returns>IDrawSettings</returns>
        IDrawSettings newDrawSettings();
        #endregion

        #region FONT, GLYPH, TEXT, RENDERER
        /// <summary>
        /// Creates a new Glyph renderer.
        /// </summary>
        /// <returns>IGlyphRenderer</returns>
        IGlyphRenderer newGlyphRenderer();

        /// <summary>
        /// Creates a new font with given parameters.
        /// </summary>
        /// <param name="fontStream">A stream containig font data</param>
        /// <param name="fontSize">Size of the font to be used to create any glyph required.</param>
        /// <returns>IFont object</returns>
        IFont newFont(Stream fontStream, int fontSize);

        /// <summary>
        /// Create a new glyph object from a given slot.
        /// </summary>
        /// <param name="slot">Glyph slot made available by the font object</param>
        /// <returns></returns>
        IGlyph newGlyph(IGlyphSlot slot);

        /// <summary>
        /// Creates a new glyph slot with the given parameters
        /// </summary>
        /// <param name="c">A charact the slot is to represent</param>
        /// <param name="data">A list points which forms a information to create lines and quadratic beziers using the glyph renderer.</param>
        /// <param name="contours">Int array determines how many contours and what is the lenght of each one which defines a group of points to send for bezier processing</param>
        /// <param name="xHeight">Height of the slot</param>
        /// <returns>IGlyphSlot</returns>
        IGlyphSlot newGlyphSlot(char c, IList<PointF> data, int[] contours, float xHeight);

        /// <summary>
        /// Cretes new text object with given parameters.
        /// </summary>
        /// <param name="font">the font object to be used to get glyphs</param>
        /// <param name="text">A text string to process to obtain glyphs collection from font</param>
        /// <param name="destX">X cordinate of destination location where glyphs to be drawn</param>
        /// <param name="destY">X cordinate of destination location where glyphs to be drawn</param>
        /// <param name="drawStyle">A specific drawstyle to use to measure and draw glyphs if desired so</param>
        /// <returns>IText</returns>
        IText newText(IFont font, string text, int destX, int destY, IDrawStyle drawStyle = null);

        /// <summary>
        /// Cretes new text object with given parameters.
        /// </summary>
        /// <param name="text">A list of processed glyphs collection from font</param>
        /// <param name="drawStyle">A specific drawstyle to use to measure and draw glyphs if desired so</param>
        /// <param name="destX">X cordinate of destination location where glyphs to be drawn</param>
        /// <param name="destY">X cordinate of destination location where glyphs to be drawn</param>
        /// <returns></returns>
        IText newText(IList<IGlyph> text, IDrawStyle drawStyle = null, int? destX = null, int? destY = null);
        #endregion

        #region ELLIPSE, ARC, PIE
        /// <summary>
        /// Creates a new circle or ellipse or pie or an arc specified by the bounding area, start and end angles and angle of rotation if supplied.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <param name="angle">Angle to apply rotation while rendering the arc/pie</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="type"> Defines the type of curve for example an arc or pie etc. along with other supplimentary options on how to draw it</param>
        /// <param name="assignID">If true assign an unique id to the object</param>
        /// <returns>ICurve</returns>
        ICurve newCurve(float x, float y, float width, float height, Angle angle = default(Angle), float startAngle = 0, float endAngle = 0, CurveType type = CurveType.Pie, bool assignID = true);

        /// <summary>
        /// Creates a curve replicationg data provided by circle parameter.
        /// </summary>
        /// <param name="circle">A circle whiose identical copy is to be created</param>
        /// <param name="assignID">If true assign an unique id to the object</param>
        /// <returns>ICurve</returns>
        ICurve newCurve(ICircle circle, bool assignID = true);

        /// <summary>
        /// Creates a curve reading data provided by curve parameter then applying specified stroke and fill mode.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="stroke">Stroke value to be used to obtain modified version of curve</param>
        /// <param name="mode">Stroke mode to be used to obtain modified version of curve</param>
        /// <param name="fill">Fill mode to be used to obtain modified version of curve</param>
        /// <param name="assignID">If true assign an unique id to the object</param>
        /// <returns>ICurve</returns>
        ICurve newCurve(ICurve curve, float stroke, StrokeMode mode, FillMode fill, bool assignID = true);
        #endregion

        #region CUTS
        /// <summary>
        /// Creates a new arc cut in order to cut full ellipse or circle to make an arc or pie using specified parameters.
        /// </summary>
        /// <param name="type"> Defines the type of curve for example an arc or pie etc. along with other supplimentary options on how to draw it</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="arcPoint1">Define a point the curve start from</param>
        /// <param name="arcPoint2">Define a point the curve ends to</param>
        /// <param name="centerOfArc">Define a center of arc/pie</param>
        /// <param name="UseArcLine">Define if cut operation is performed using arc line or by calculating an angle of each ellipse points and then compare with the start and end angle</param>
        /// <param name="AttachedCurve">Define if any curve you need to attach in order to create stroked curve</param>
        /// <returns>IArcCut</returns>
        IArcCut newArcCut(CurveType type, float startAngle, float endAngle, PointF arcPoint1, PointF arcPoint2, PointF centerOfArc, bool UseArcLine = true,
            ICurve AttachedCurve = null);
        #endregion

        #region AREA - AREAF - ROUNDED AREA
        /// <summary>
        /// Creates a new box with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <returns>IBox</returns>
        /// 
        IBox newBox(int x, int y, int width, int height);
        /// <summary>
        /// Creates a new box with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <returns>IBox</returns>
        IBoxF newBoxF(float x, float y, float width, float height);

        /// <summary>
        /// Creates a new square with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <returns>ISquare</returns>
        ISquare newSquare(int x, int y, int width);

        /// <summary>
        /// Creates a new square with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <returns>ISquare</returns>
        ISquareF newSquareF(float x, float y, float width);

        /// <summary>
        /// Creates a new rectangle with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <returns>IRectangle</returns>
        IRectangle newRectangle(int x, int y, int width, int height);

        /// <summary>
        /// Creates a new rectangle with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <returns>IRectangle</returns>
        IRectangleF newRectangleF(float x, float y, float width, float height);

        /// <summary>
        /// Creates a new rouded box with specifed parameters.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the circle/ellipse is to be drawn</param>
        /// <param name="width">Width of a bounding area where the circle/ellipse is to be drawn -> circle/ellipse's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the circle is to be drawn ->circle/ellipse's minor Y axis = Height/2</param>
        /// <param name="cornerRadius">Radius of a circle - convex hull of which is to be drawn on each corner</param>
        /// <param name="angle">Angle to apply rotation while rendering the rounded box</param>
        /// <returns>IRectangle</returns>
        IRoundedBox newRoundedBox(float x, float y, float width, float height, float cornerRadius, Angle angle = default(Angle));
        #endregion

        #region LINE
        /// <summary>
        /// Creates a new line segment with points specified by x1, y1 and x2, y2.
        /// </summary>
        /// <param name="x1">X corordinate of start point</param>
        /// <param name="y1">Y corordinate of start point</param>
        /// <param name="x2">X corordinate of end point</param>
        /// <param name="y2">Y corordinate of end point</param>
        /// <param name="angle">Angle to apply rotation on x1, y1, x2, y2 before rendering the line segment</param>
        /// <param name="deviation">Deviates the line segment to create a parallel one away from the original points specified</param>
        /// <param name="assignID">If true assign an unique id to the object</param>
        ILine newLine(float x1, float y1, float x2, float y2, Angle angle = default(Angle), float deviation = 0, bool assignID = false);

        /// <summary>
        /// Creates a new SLine with specified parametrs.
        /// </summary>
        /// <param name="main">Main XLine covers an entire span</param>
        /// <param name="child">Child XLine covers a middle portion within SLine making creating two fragmented line one on left and another on right</param>
        /// <returns>ISLine</returns>
        ISLine newSLine(IXLine main, IXLine child);

        /// <summary>
        /// Creates a new XLine with specified parametrs.
        /// </summary>
        /// <param name="start">Start position on axial line i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position on axial line i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Axis position on axial line i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="isHorizontal">Axis orientation - horizontal if true otherwise vertical</param>
        /// <returns>IXLine</returns>
        IXLine newLine(float start, float end, int axis, bool isHorizontal);

        /// <summary>
        /// Creates a new XLine with specified parametrs.
        /// </summary>
        /// <param name="start">Start position on axial line i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="end">>End position on axial line i.e X coordinate if horizontal otherwise Y</param>
        /// <param name="axis">Axis position on axial line i.e Y coordinate if horizontal otherwise X</param>
        /// <param name="isHorizontal">Axis orientation - horizontal if true otherwise vertical</param>
        ///<param name="alpha1">Valuee by which blending on start point should happen if at all it is supplied</param>
        ///<param name="alpha2">Value by which blending on end point should happen if at all it is supplied</param>
        /// <returns>IXLine</returns>
        IXLine newLine(int start, int end, int axis, bool isHorizontal, float? alpha1 = null, float? alpha2 = null);
        #endregion

        #region BEZIER ARC - PIE
        /// <summary>
        /// Creates a new bezier arc or pie specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the bezier arc/pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier arc/pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier arc/pie is to be drawn -> bezier arc/pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier arc/pie is to be drawn ->bezier arc/pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc/pie</param>
        IBezierCurve newBezierArc(float x, float y, float width, float height, float startAngle, float endAngle, Angle angle = default(Angle));

        /// <summary>
        /// Creates a new bezier arc or pie specified by the bounding area and angle of rotation if supplied.
        /// </summary>
        /// <param name="x">X cordinate of a bounding area where the bezier arc/pie is to be drawn</param>
        /// <param name="y">Y cordinate of a bounding area where the bezier arc/pie is to be drawn</param>
        /// <param name="width">Width of a bounding area where the bezier arc/pie is to be drawn -> bezier arc/pie's minor X axis = Width/2</param>
        /// <param name="height">Height of a bounding area where the bezier arc/pie is to be drawn ->bezier arc/pie's minor Y axis = Height/2</param>
        /// <param name="startAngle">Start angle from where a curve start</param>
        /// <param name="endAngle">End Angle where a curve stops. If type includes NoSweepAngle option otherwise effective end angle is start angle + end angle</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier arc/pie</param>
        IBezierCurve newBezierPie(float x, float y, float width, float height, float startAngle, float endAngle, Angle angle = default(Angle));
        #endregion

        #region BEZIER
        /// <summary>
        /// Creates a bezier defined by points and specified by type and an angle of rotation if supplied.
        /// </summary>
        /// <param name="points">Defines perimiter of the bezier</param>
        /// <param name="type">BezierType enum determines the type of bezier i.e Cubic - group of 4 points or multiple(group of 4 or 7 or 10 so on...)</param>
        /// <param name="angle">Angle to apply rotation while rendering the bezier</param>
        IBezier newBezier(BezierType type, IList<float> points, IList<PointF> pixels, Angle angle = default(Angle));
        #endregion

        #region RHOMBUS
        /// <summary>
        /// Renders a rhombus specified by x, y, width, height parameters and angle of rotation if supplied.
        /// </summary>
        /// <param name="x">X cordinate of the bounding rectangle</param>
        /// <param name="y">Y cordinate of the bounding rectangle</param>
        /// <param name="width">Width of the bounding rectangle/param>
        /// <param name="height">Height the bounding rectangle</param>
        /// <param name="angle">Angle to apply rotation while rendering the rhombus</param>
        /// <param name="deviation">If not null, it replaces the value of width parameter</param>
        IRhombus newRhombus(float x, float y, float width, float height, Angle angle = default(Angle), float? deviation = null);
        #endregion

        #region TRAPEZIUM
        /// <summary>
        /// Renders a trapezium (defined as per the definition in British English) specified by a base line, parallel line deviation and angle of rotation if supplied.
        /// </summary>
        /// <param name="baseLine">A line from where the trapezium start</param>
        /// <param name="parallelLineDeviation">A deviation from a base line to form a parallel line to construct a trapezium</param>
        /// <param name="parallelLineSizeDifference">A change in parallel line size to tilt the trapezium</param>
        /// <param name="angle">Angle to apply rotation while rendering the trapezium</param>
        /// <returns></returns>
        ITrapezium newTrapezium(ILine baseLine, float parallelLineDeviation, float parallelLineSizeDifference = 0, Angle angle = default(Angle));
        #endregion

        #region POLYGON
        /// <summary>
        /// Creates a new polygon specified by a collection of points and angle of rotation if supplied.
        /// </summary>
        /// <param name="polyPoints">A collection of points which forms perimeter of the polygon an each group of two subsequent values in polypoints forms a point x,y</param>
        /// <param name="angle">Angle to apply rotation while rendering the polygon</param>
        IPolygon newPolygon(IList<PointF> polyPoints, Angle angle = default(Angle));
        #endregion

        #region TRIANGLE
        /// <summary>
        /// Creates a new trianle formed by three points specified by x1,y1 & x2,y2 & x3,y3 and angle of rotation if supplied.
        /// </summary>
        /// <param name="x1">X corodinate of the first point</param>
        /// <param name="y1">Y corodinate of the first point</param>
        /// <param name="x2">X corodinate of the second point</param>
        /// <param name="y2">Y corodinate of the second point</param>
        /// <param name="x3">X corodinate of the third point</param>
        /// <param name="y3">Y corodinate of the third point</param>
        /// <param name="angle">Angle to apply rotation while rendering the traingle</param>
        ITriangle newTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Angle angle = default(Angle));
        #endregion

        #region TIMER
        /// <summary>
        /// Creates a new Timer with specified interval.
        /// </summary>
        /// <param name="interval">Interval by which tick evebt should get fired repeatedly</param>
        /// <returns>ITimer</returns>
        ITimer newTimer(int interval = 5);
        #endregion

        #region SCANNER
        /// <summary>
        /// Create a new scanner with specified buffer to scan.
        /// </summary>
        /// <param name="buffer">Buffer to scan data from</param>
        /// <returns></returns>
        IScanner newScanner(IBuffer buffer);
        #endregion

        #region POPUP COLLECTION
        /// <summary>
        /// Creates a new popup collection.
        /// </summary>
        /// <param name="target">Target window to host this collection</param>
        /// <returns></returns>
        IPopupCollection newPopupCollection(IParent target);
        #endregion

        #region IMAGE PROCESSOR
        /// <summary>
        /// Creates a new image processor. By default, GWS uses STBImage. For more info on STBImage visit: https://github.com/nothings/stb
        /// </summary>
        /// <returns>IImageProcessor</returns>
        IImageProcessor newImageProcessor();
        #endregion

        #region EVENT ARGS
        /// <summary>
        /// Creates new event args.
        /// </summary>
        /// <returns></returns>
        IEventArgs newEventArgs();
        /// <summary>
        /// Create a new paint event args with specified buffer.
        /// </summary>
        /// <param name="buffer">Buffer which can be used to draw by a method subscribbed to paint event</param>
        /// <returns></returns>
        IPaintEventArgs newPaintEventArgs(IBuffer buffer);
        /// <summary>
        /// Creates a new cancel event args.
        /// </summary>
        /// <returns></returns>
        ICancelEventArgs newCancelEventArgs();
        #endregion

        #region FOR ADVANCED VERSION
#if AdvancedVersion
        #region SIMPLE POPUP
        /// <summary>
        /// Creates a new simple popup using string values converted to simple popup items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        ISimplePopup newSimplePopup(params string[] items);

        /// <summary>
        /// Creates a new simple popup of specified width and height using string values converted to simple popup items.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        ISimplePopup newSimplePopup(int width, int height, params string[] items);
        #endregion
#endif
        #endregion

        #region MISC
        /// <summary>
        /// Creates a new animated gif class.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        IAnimatedGifFrame newAnimatedGifFrame(byte[] data, int delay);
        #endregion
    }
    partial interface IFactory
    {
        #region COUNT OF
        /// <summary>
        /// Returns a count of specified object type in Factory's object store.
        /// </summary>
        /// <param name="type">ObjType enum to specify count for any of the available storage units for GWS object types</param>
        /// <returns></returns>
        int CountOf(ObjType type);

        /// <summary>
        /// Returns a count of specified object type in Factory's object store using filter applied through condition specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">A condition to filter objects before counting</param>
        /// <param name="type">ObjType enum to specify count for any of the available storage units for GWS object types</param>
        /// <returns></returns>
        int CountOf<T>(Predicate<T> condition, ObjType type);
        #endregion

        #region NEW ID
        /// <summary>
        /// Returns unique ID for a given type.
        /// </summary>
        /// <param name="objType">Type of object unique id is sought for</param>
        /// <returns></returns>
        string NewID(Type objType);
        /// <summary>
        /// Returns unique ID for a given object.
        /// </summary>
        /// <param name="o">Object for which unique id is sought for</param>
        /// <returns></returns>
        string NewID(object o);
        /// <summary>
        /// Returns unique ID for a given objType which is a class name usually.
        /// </summary>
        /// <param name="objType">Class name usually</param>
        /// <returns></returns>
        string NewID(string objType);
        #endregion

        #region CONTAINS
        /// <summary>
        /// Tests if the specified key exists in a storage unit assigned for specified type.
        /// </summary>
        /// <param name="key">Key to search storage for</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        /// <returns></returns>
        bool Contains(string key, ObjType type);
        #endregion

        #region REPLACE
        /// <summary>
        /// Replace a current version of object with specified one. Search is performed using id of object.
        /// This mkaes sure updated version of the instance is held in GWS storage unit for a particular type.
        /// </summary>
        /// <param name="obj">Storeable object which has to be the replaced value of whatever is stored in storage unit now</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        void Replace(IStoreable obj, ObjType type);
        #endregion

        #region ADD - REMOVE
        /// <summary>
        /// Adds object to the specified storage unit in factory.
        /// </summary>
        /// <param name="obj">Object to store</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        void Add(IStoreable obj, ObjType type);

        /// <summary>
        /// Removes object specified from the specified store unit in factory.
        /// </summary>
        /// <param name="obj">Object to remove</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        void Remove(IStoreable obj, ObjType type);

        /// <summary>
        /// Removes object after finding it using key as id from the specified storage unit in factory.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        void Remove(string id, ObjType type);
        #endregion

        #region GET SINGLE
        /// <summary>
        /// Gets the exisiting object with agiven key from the storage unit of specified type from factory. 
        /// </summary>
        /// <typeparam name="T">Any IStoreble object</typeparam>
        /// <param name="key">Key by which object will be identified in the storage</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        /// <returns></returns>
        T Get<T>(string key, ObjType type) where T : IStoreable;

        /// <summary>
        /// Gets the exisiting object with agiven key from the storage unit of specified type from factory. 
        /// </summary>
        /// <typeparam name="T">Any IStoreble object</typeparam>
        /// <param name="key"></param>
        /// <param name="obj">Object returned if found</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        /// <returns>True if found otherwise false</returns>
        bool Get<T>(string key, out T obj, ObjType type) where T : IStoreable;

        /// <summary>
        /// Gets the first exisiting object which satisfies a condition specified from the storage unit of specified type of factory. 
        /// </summary>
        /// <typeparam name="T">Any IStoreble object</typeparam>
        /// <param name="condition">A condition which must be satisfied by objects to get qualified</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        /// <returns></returns>
        T Get<T>(Predicate<T> condition, ObjType type) where T : IStoreable;
        #endregion

        #region GET ALL
        /// <summary>
        /// Gets the exisiting object which satisfy a condition specified from the storage unit of specified type from factory. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">A condition which must be satisfied by objects to get qualified</param>
        /// <param name="type">ObjType enum to specify any of the available storage units for GWS object types</param>
        /// <returns>collection of qualified objects</returns>
        IEnumerable<T> GetAll<T>(Predicate<T> condition, ObjType type) where T : IStoreable;
        #endregion
    }
    #endregion

    #region WINDOW FACTORY
#if Window
    public interface IWindowFactory : IFactory
    {
        #region PROPERTIES
        /// <summary>
        /// Gets the primary scrren available with system default resoultion in the operating system.
        /// </summary>
        IScreen PrimaryScreen { get; }
        /// <summary>
        /// Gets the default window creatio flags from the operating system.
        /// </summary>
        int DefaultWinFlag { get; }
        /// <summary>
        /// Gets the flags required to create full screen desktop.
        /// </summary>
        int FullScreenWinFlag { get; }
        /// <summary>
        /// Returns all the availble scrrens with possible resoultion provided by operating system.
        /// </summary>
        IScreens AvailableScreens { get; }

        /// <summary>
        /// Gets the array of available pixelformats offered by the operating system.
        /// </summary>
        uint[] PixelFormats { get; }
        /// <summary>
        /// Gets default primary pixel format offered by the operating system.
        /// </summary>
        uint PixelFormat { get; }
        /// <summary>
        /// Indicates if a connection to by the operating system is established or not.
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        /// Gets the type of underlying operating system.
        /// </summary>
        OS OS { get; }
        /// <summary>
        /// Gets the latest error occured while interacting with by the operating system.
        /// </summary>
        string LastError { get; }
        #endregion

        #region WINDOW
        /// <summary>
        /// Creates a new window with specified parameters.
        /// </summary>
        /// <param name="title">Title of the window</param>
        /// <param name="width">Width of the window</param>
        /// <param name="height">Height of the window</param>
        /// <param name="x">X coordinate of location of the window</param>
        /// <param name="y">Y coordinate of location of the window</param>
        /// <param name="flags">GwsWindowFlags to create a certain kind of window i.e fullscrren, resizeable etc.</param>
        /// <param name="display"></param>
        /// <param name="isSingleThreaded">Specify if your window will be single or multi threaded here.</param>
        /// <param name="renderFlags">Define flags to create renderer whenever requested.</param>
        /// <returns></returns>
        IWindow newWindow(string title = null, int? width = null, int? height = null,
            int? x = null, int? y = null, GwsWindowFlags? flags = null, IScreen display = null,
            bool isSingleThreaded = true, RendererFlags? renderFlags = null);
        /// <summary>
        /// Create a new window from an existing window by refercing the exisitng window's handle.
        /// </summary>
        /// <param name="externalWindow">Pointer to external window</param>
        /// <returns></returns>
        IWindow newWindowFrom(IntPtr externalWindow);
        #endregion

        #region GET WINDOW ID
        /// <summary>
        /// Gets the unique window id associated with the window.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        int GetWindowID(IntPtr window);
        #endregion

        #region SAVE IMAGE AS BITMAP
        /// <summary>
        /// Saves specified buffer data as an image file on disk on specified file path.
        /// </summary>
        /// <param name="image">Buffer data which is to be saved</param>
        /// <param name="file">Path of a file where data is to be saved</param>
        /// <returns></returns>
        bool SaveAsBitmap(IBufferData image, string file);
        #endregion

        #region CURSOR TYPE ENUM CONVERSION
        /// <summary>
        /// Converts GWS cursor types to native operating system's cursor types.
        /// </summary>
        /// <param name="cursorType"></param>
        /// <returns></returns>
        int ConvertToSystemCursorID(CursorType cursorType);
        #endregion

        #region SURFACE
        /// <summary>
        /// Creates a new GWS Surface object of given width and height with data provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Byte array to copy. Please note that the array will be converted to int[] first.
        /// <returns>ISurface</returns>
        ISurface newSurface(int width, int height, byte[] pixels);
        /// <summary>
        /// Creates a new GWS Surface object of given width and height with data provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Int array to copy. 
        /// <returns>ISurface</returns>
        ISurface newSurface(int width, int height, int[] pixels = null);
        /// <summary>
        /// Creates a new GWS Graphics object of given width and height with data provided by buffer.
        /// </summary>
        /// <param name="width">Required width</param>
        /// <param name="height">Requred height</param>
        /// <param name="pixels">Pointer data to copy</param>
        /// <param name="bufferLength">Length of the pixel pointer.</param>
        /// <returns>ISurface</returns>
        ISurface newSurface(int width, int height, IntPtr pixels, int bufferLength);
        #endregion

        #region WINDOW SURFACE
        /// <summary>
        /// Creates a new surface instance from a given wondow.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        IWindowSurface newSurface(IRenderWindow window);
        #endregion

        #region TEXTURE
        /// <summary>
        /// Crates a new texture from a given window.
        /// </summary>
        /// <param name="window">Window from which texture is to be created</param>
        /// <param name="w">Width of the texture</param>
        /// <param name="h">Height of the texture</param>
        /// <param name="isPrimary">Define if its a primary one for the window</param>
        /// <returns></returns>
        ITexture newTexture(IRenderWindow window, int? w = null, int? h = null, bool isPrimary = false);

        /// <summary>
        /// Crates a new texture from a given window. Then copies dat from given buffer.
        /// </summary>
        /// <param name="window">Window from which texture is to be created</param>
        /// <param name="source">Buffer source to copy data from onto surface</param>
        /// <param name="isPrimary">Define if its a primary one for the window</param>
        /// <returns></returns>
        ITexture newTexture(IRenderWindow window, IBuffer source, bool isPrimary = false);
        #endregion

        #region SET CURSOR POSITION
        /// <summary>
        /// Sets window's cusor's position to specified x and y coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the location where cursor should be placed</param>
        /// <param name="x">Y coordinate of the location where cursor should be placed</param>
        void SetCursorPos(int x, int y);
        #endregion

        #region MISC
        /// <summary>
        /// Disables the existing scrren saver of the operating system.
        /// </summary>
        void DisableScreenSaver();
        #endregion

        #region PUSH, PUMP, POLL EVENTS
        /// <summary>
        /// Push the specified event to the active window.
        /// </summary>
        /// <param name="e">Event to push on</param>
        void PushEvent(IEvent e);
        /// <summary>
        /// Instructs Window manager to start pumping events to eligble window.
        /// </summary>
        void PumpEvents();
        /// <summary>
        /// Gives current event happeed on active window.
        /// </summary>
        /// <param name="e">Event which is just happened</param>
        /// <returns></returns>
        bool PollEvent(out IEvent e);
        #endregion

        #region WAV PLAYER
        /// <summary>
        /// Createa new Wav player.
        /// </summary>
        /// <returns></returns>
        ISound newWavPlayer();
        #endregion
    }
#endif
    #endregion

    #region VIRTUAL FACTORY
#if VCSupport
    public interface IVirtualFactory : IAttachment
    {

    }
#endif
    #endregion

    #region IMAGE PROCESSING FACTORY
    /// <summary>
    /// Image proccessing unit of GWS.
    /// </summary>
    public interface IImageProcessor : IAttachment
    {
        /// <summary>
        /// Reader which is capable of reading image data.
        /// </summary>
        IImageReader Reader { get; }
        /// <summary>
        /// Write which is capable of writing any buffer data to a disk image file.
        /// </summary>
        IImageWriter Writer { get; }
    }
    #endregion
}
