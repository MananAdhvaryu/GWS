using MnM.GWS.MathExtensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MnM.GWS
{
    public abstract partial class GwsFactory : IFactory
    {
        #region VARIABLES
        static ILine xAxis, yAxis;
        static ISolidPen disabledPen;
        static EventInfo dfEventInfo;
        static IGlyphRenderer dfRenderer;
        static ILine dfLine;
        static IRectangle dfRect;
        static IBoxF dfAreaF;
        static IRectangleF dfRectF;

        static IBox dfArea;
        static EventArgs dfEventArgs;
        static IFont sysFont;
        readonly static string sysFontpath;
        static IImageProcessor imageProcessor;
        #endregion

        #region CONSTRUCTORS
        static GwsFactory()
        {
            sysFontpath = AppContext.BaseDirectory + "UbuntuMono-Regular.ttf";
        }
        protected GwsFactory()
        {
            var vals = Enum.GetValues(typeof(ObjType));
            foreach (var item in vals)
                Objects.Add((int)item, new Dictionary<string, IStoreable>(5));
        }
        #endregion

        #region PROPERTIES
        public IGlyphRenderer FontRenderer
        {
            get
            {
                if (dfRenderer == null)
                    dfRenderer = newGlyphRenderer();
                return dfRenderer;
            }
        }
        public ISolidPen DisabledPen
        {
            get
            {
                if (disabledPen == null)
                    disabledPen = newPen(Colour.Silver, 3000, 3000);
                return disabledPen;
            }
        }
        public ILine XAxis
        {
            get
            {
                if (xAxis == null)
                    xAxis = newLine(0, 0, 7000, 0);
                return xAxis;
            }
        }
        public ILine YAxis
        {
            get
            {
                if (yAxis == null)
                    yAxis = newLine(0, 0, 0, 7000);
                return xAxis;
            }
        }
        public EventInfo EventInfoEmpty
        {
            get
            {
                if (dfEventInfo == null)
                    dfEventInfo = new EventInfo(null, null, 0);
                return dfEventInfo;
            }
        }
        public ILine LineEmpty
        {
            get
            {
                if (dfLine == null)
                    dfLine = newLine(0, 0, 0, 0);
                return dfLine;
            }
        }
        public IRectangle RectEmpty
        {
            get
            {
                if (dfRect == null)
                    dfRect = newRectangle(0, 0, 0, 0);
                return dfRect;
            }
        }
        public IRectangleF RectFEmpty
        {
            get
            {
                if (dfRectF == null)
                    dfRectF = newRectangleF(0, 0, 0, 0);
                return dfRectF;
            }
        }
        public IBox BoxEmpty
        {
            get
            {
                if (dfArea == null)
                    dfAreaF = newBoxF(0, 0, 0, 0);
                return dfArea;
            }
        }
        public IBoxF BoxFEmpty
        {
            get
            {
                if (dfAreaF == null)
                    dfAreaF = newBoxF(0, 0, 0, 0);
                return dfAreaF;
            }
        }
        public EventArgs EventArgsEmpty
        {
            get
            {
                if (dfEventArgs == null)
                    dfEventArgs = new EventArgs();
                return dfEventArgs;
            }
        }
        public IFont SystemFont
        {
            get
            {
                if (sysFont == null)
                    sysFont = this.newFont(sysFontpath, 12);
                return sysFont;
            }
        }
        public IImageProcessor ImageProcessor
        {
            get
            {
                if(imageProcessor == null)
                    imageProcessor = newImageProcessor();
                return imageProcessor;
            }
        }
        #endregion

        #region COLOR
        public virtual IRGBA newColor(byte r, byte g, byte b, byte a = 255) =>
            new RGBA(r, g, b, a);
        #endregion

        #region CANVAS
        public virtual IGraphics newGraphics( int width,  int height, byte[] buffer,  bool copy = false) =>
            new Graphics(width, height, buffer, copy);
        public virtual IGraphics newGraphics( int width,  int height, int[] buffer,  bool copy = false) =>
            new Graphics(width, height, buffer, copy);
        public virtual IGraphics newGraphics( int width,  int height, IntPtr buffer,  int bufferLength) =>
            new Graphics(width, height, buffer, bufferLength);

        public IGraphics newGraphics( int width,  int height) =>
            newGraphics(width, height, default(int[]));
        public IGraphics newGraphics( string path)
        {
            var tuple = Geometry.ReadImage(path);
            return newGraphics(tuple.Item2, tuple.Item3, tuple.Item1);
        }
        public IGraphics newGraphics(byte[] buffer)
        {
            var tuple = Geometry.ReadImage(buffer);
            return newGraphics(tuple.Item2, tuple.Item3, tuple.Item1);
        }
        #endregion

        #region BUFFER
        public virtual IBuffer newBuffer(int width,  int height) =>
            new Buffer(width, height);
        public virtual unsafe  IBuffer newBuffer( int* pixels,  int width,  int height, bool makeCopy = false) =>
            new Buffer(pixels, width, height);
        public virtual IBuffer newBuffer( int[] pixels,  int width,  int height, bool makeCopy = false) =>
            new Buffer(pixels, width, height);
        #endregion

        #region PATH
        public abstract IElementCollection newElementCollection(IParent window);
        #endregion

        #region UPDATE MANAGER
        public virtual IUpdateManager newUpdateManager() =>
            new UpdateManager();
        #endregion

        #region BUFFER COLLECTION
#if AdvancedVersion
        public abstract IBufferCollection newBufferCollection();
        public abstract IBufferCollection newBufferCollection(int capacity);
#endif
        #endregion

        #region RENDERER
        public abstract IRenderer newRenderer();
        #endregion

        #region FLOOD FILL
        public virtual IFloodFill newFloodFill() =>
            new FloodFill();
        #endregion
        
        #region PEN
        public virtual ISolidPen newPen( int color,  int width,  int height) =>
            SolidPen.CreateInstance(color, width, height);
        #endregion

        #region BRUSH
        public virtual IBrush newBrush( IFillStyle style,  int width,  int height) =>
            Brush.CreateInstance(style ?? FillStyles.Black, width, height);
        #endregion

        #region FILLSTYLE
        public virtual IFillStyle newFillStyle( IList<int> stops,  Gradient g, params int[] values) =>
            new FillStyle(values, g, stops);
        #endregion

        #region APOINT
        public virtual IAPoint newAPoint( float val,  int axis,  bool isHorizontal,  int len = 0) =>
            new APoint(val, axis, isHorizontal, len);
        public virtual IAPoint newAPoint( int val,  int axis,  float? alpha,  bool isHorizontal,  int len = 0) =>
            new APoint(val, axis, alpha, isHorizontal, len);

        #endregion

        #region DRAW SETTINGS
        public virtual IDrawSettings newDrawSettings() =>
            DrawSetter.Create();
        #endregion

        #region FONT - GLYPH - GLYPHSLOT - TEXT - RENDERER
        public virtual IFont newFont(Stream fontStream,  int fontSize) =>
            new Font(fontStream, fontSize);

        public virtual IGlyph newGlyph(IGlyphSlot slot) =>
            new Glyph(slot);
        public virtual IText newText(IFont font,  string text,  int destX,  int destY, IDrawStyle drawStyle = null) =>
            new Text(font, text, destX, destY, drawStyle);
        public virtual IText newText(IList<IGlyph> glyphs, IDrawStyle drawStyle = null,  int? destX = null,  int? destY = null) =>
            new Text(glyphs, drawStyle, destX, destY);

        public virtual IGlyphSlot newGlyphSlot( char c,  IList<PointF> data, int[] contours,  float xHeight) =>
            new GlyphSlot(c, data, contours, xHeight);
        public virtual IGlyphRenderer newGlyphRenderer() => new GlyphRenderer();
        #endregion

        #region ELLIPSE - ARC - PIE
        public virtual ICurve newCurve( float x,  float y,  float width,  float height,
              Angle angle = default(Angle),  float StartAngle = 0,  float EndAngle = 0,  CurveType type = CurveType.Pie,  bool assignID = true) =>
            new Curve(x, y, width, height, angle, StartAngle, EndAngle, type, assignID);

        public virtual ICurve newCurve( ICircle circle,  bool assignID = true) =>
            new Curve(circle, assignID);

        public virtual ICurve newCurve( ICurve curve,  float stroke,  StrokeMode mode,  FillMode fill,  bool assignID = true) =>
            new Curve(curve, stroke, mode, fill, assignID);
        #endregion

        #region CUTS
        public virtual IArcCut newArcCut(CurveType type, float startAngle, float endAngle, PointF arcPoint1,  PointF arcPoint2,  PointF centerOfArc,
             bool UseArcLine = true,  ICurve AttachedCurve = null) =>
            new ArcCut(type, startAngle, endAngle, arcPoint1, arcPoint2, centerOfArc, UseArcLine, AttachedCurve);
        #endregion

        #region AREA - AREAF - ROUNDED AREA
        public virtual IBox newBox( int x,  int y,  int width,  int height) =>
            new Box(x, y, width, height);
        public virtual ISquare newSquare( int x,  int y,  int width)=>
            new Box(x, y, width, width);
        public virtual ISquareF newSquareF( float x,  float y,  float width) =>
            new BoxF(x, y, width, width);
        public virtual IRectangle newRectangle( int x,  int y,  int width,  int height) =>
            new Rect(x, y, width, height);
        public virtual IRectangleF newRectangleF( float x,  float y,  float width,  float height) =>
            new RectF(x, y, width, height);
        
        public virtual IBoxF newBoxF( float x,  float y,  float width,  float height) =>
            new BoxF(x, y, width, height);
        public virtual IRoundedBox newRoundedBox( float x,  float y,  float width,  float height,  float cornerRadius,  Angle angle = default(Angle)) =>
            new RoundedBox(x, y, width, height, cornerRadius, angle);
        #endregion

        #region LINES
        public virtual ILine newLine( float x1,  float y1,  float x2,  float y2,  Angle angle = default(Angle),  float deviation = 0, bool assignID = false) =>
            new Line(x1, y1, x2, y2, angle, deviation, assignID);

        public virtual ISLine newSLine(IXLine main, IXLine child) =>
            new SLine(main, child);

        public virtual IXLine newLine( float val1,  float val2,  int axis,  bool isHorizontal) =>
            new XLine(val1, val2, axis, isHorizontal);
        public virtual IXLine newLine( int val1,  int val2,  int axis,  bool isHorizontal,  float? alpha1 = null,  float? alpha2 = null) =>
            new XLine(val1, val2, axis, isHorizontal, alpha1, alpha2);
        #endregion

        #region BEZIER ARC - PIE
        public virtual IBezierCurve newBezierArc(float x, float y, float width, float height, float startA, float endA, Angle angle = default(Angle)) =>
            new BezierCurve(x, y, width, height, startA, endA, true, angle);
        public virtual IBezierCurve newBezierPie(float x, float y, float width, float height, float startA, float endA, Angle angle = default(Angle)) =>
             new BezierCurve(x, y, width, height, startA, endA, false, angle);
        #endregion

        #region BEZIER
        public virtual IBezier newBezier(BezierType type, IList<float> points, IList<PointF> pixels, Angle angle = default(Angle)) =>
            new Bezier(type, points, pixels, angle);
        #endregion

        #region TRAPEZIUM
        public virtual ITrapezium newTrapezium(ILine first, float parallelLineDeviation, float parallelLineSizeDifference = 0, Angle angle = default(Angle)) =>
            new Trapezium(first, parallelLineDeviation, parallelLineSizeDifference, angle);
        #endregion

        #region RHOMBUS
        public virtual IRhombus newRhombus(float x, float y, float width, float height, Angle angle = default(Angle), float? deviation = null) =>
            new Rhombus(x, y, width, height, angle, deviation);
        #endregion
        
        #region POLYGON
        public virtual IPolygon newPolygon(IList<PointF> points, Angle angle = default(Angle)) =>
            new Polygon(points, angle);
        #endregion

        #region TRINAGLE
        public virtual ITriangle newTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Angle angle = default(Angle)) =>
            new Triangle(x1, y1, x2, y2, x3, y3);
        #endregion

        #region TIMER
        public virtual ITimer newTimer(int interval = 5) =>
            new Timer(interval);
        #endregion

        #region SCANNER
        public IScanner newScanner(IBuffer surface) =>
            new Scanner(surface);
        #endregion

        #region POPUP COLLECTION
        public virtual IPopupCollection newPopupCollection(IParent target) =>
            new PopupCollection(target);
        #endregion

        #region IMAGE PROCESSOR
        public virtual IImageProcessor newImageProcessor() => 
            new StbImageProcessor();
        #endregion

        #region EVENT ARGS
        public virtual IEventArgs newEventArgs() => EventArgs.Empty;
        public virtual IPaintEventArgs newPaintEventArgs(IBuffer buffer) =>
            new PaintEventArgs(buffer);
        public virtual ICancelEventArgs newCancelEventArgs() =>
            new CancelEventArgs();
        #endregion

        #region MISC
        public virtual IAnimatedGifFrame newAnimatedGifFrame(byte[] data, int delay) =>
            STBImage.GetAnimatedGifFrame(data, delay);
        #endregion

        #region DISPOSE
        public void Dispose()
        {
            xAxis = null;
            yAxis = null;
            disabledPen?.Dispose();
            disabledPen = null;
            dfEventInfo = null;
            dfRenderer?.Dispose();
            dfRenderer = null;
            dfLine = null;
            dfRect = null;
            dfAreaF = null;
            dfArea = null;
            dfEventArgs = null;
            sysFont = null;
            imageProcessor?.Dispose();
            imageProcessor = null;
            IsClosing = true;
            lock (Sync)
            {
                foreach (var dict in Objects.Values)
                {
                    foreach (var item in dict.Values.OfType<IDisposable>())
                        item.Dispose();
                    dict.Clear();
                }
                Objects.Clear();
                newIDs.Clear();
            }
            Dispose2();
        }
        protected virtual void Dispose2() { }
        #endregion
    }

    //WAREHOUSE ACCESS METHODS
    partial class GwsFactory
    {
        #region VARIABLES
        readonly Dictionary<int, Dictionary<string, IStoreable>> Objects =
            new Dictionary<int, Dictionary<string, IStoreable>>(4);

        static readonly object Sync = new object();
        static readonly Dictionary<string, int> newIDs = new Dictionary<string, int>(100);
        bool IsClosing;
        #endregion

        #region COUNT OF
        public int CountOf(ObjType type) =>
            Objects[(int)type].Count;
        public int CountOf<T>(Predicate<T> condition, ObjType type)
        {
            if (IsClosing)
                return 0;
            var dict = Objects[(int)type];

            lock (Sync)
            {
                if (condition != null)
                    return dict.Values.OfType<T>().Where(x => condition(x)).Count();
                return dict.Values.OfType<T>().Count();
            }
        }
        #endregion

        #region NEW ID
        public string NewID(Type objType)
        {
            if (objType == null)
                return null;
            var name = objType.FullName;
            if (!newIDs.ContainsKey(name))
                newIDs.Add(name, 0);

            var newID = ++newIDs[name];
            newIDs[name] = newID;
            return name + newID;
        }
        public string NewID(object o)
        {
            if (o == null)
                return null;
            return NewID(o.GetType());
        }
        public string NewID(string objType)
        {
            if (objType == null)
                return null;
            var name = objType;
            if (!newIDs.ContainsKey(name))
                newIDs.Add(name, 0);

            var newID = ++newIDs[name];
            newIDs[name] = newID;
            return name + newID;
        }
        #endregion

        #region CONTAINS
        public bool Contains(string key, ObjType type)
        {
            return Objects[(int)type].ContainsKey(key);
        }
        #endregion

        #region REPLACE
        public void Replace(IStoreable obj, ObjType type)
        {
            if (IsClosing || string.IsNullOrEmpty(obj.ID))
                return;
            var dict = Objects[(int)type];
            if (dict.ContainsKey(obj.ID))
                dict.Remove(obj.ID);

            dict.Add(obj.ID, obj);
        }
        #endregion

        #region ADD - REMOVE
        public void Add(IStoreable obj, ObjType type)
        {
            if (IsClosing || string.IsNullOrEmpty(obj.ID))
                return;
            var dict = Objects[(int)type];
            if (dict.ContainsKey(obj.ID))
                return;
            dict.Add(obj.ID, obj);
        }
        public void Remove(IStoreable obj, ObjType type)
        {
            if (IsClosing || string.IsNullOrEmpty(obj.ID))
                return;
            var dict = Objects[(int)type];
            if (dict.ContainsKey(obj.ID))
                dict.Remove(obj.ID);
        }
        public void Remove(string id, ObjType type)
        {
            if (IsClosing || string.IsNullOrEmpty(id))
                return;
            var dict = Objects[(int)type];
            if (dict.ContainsKey(id))
                dict.Remove(id);
        }
        #endregion

        #region GET SINGLE
        public T Get<T>(string key, ObjType type) where T : IStoreable
        {
            if (IsClosing || string.IsNullOrEmpty(key))
                return default(T);

            var dict = Objects[(int)type];
            if (!dict.ContainsKey(key))
                return default(T);
            var o = dict[key];
            if (o is T)
                return (T)o;
            return default(T);
        }
        public bool Get<T>(string key, out T obj, ObjType type) where T : IStoreable
        {
            obj = Get<T>(key, type);
            if (obj == null || obj.Equals(default(T)))
                return false;
            return true;
        }
        public T Get<T>(Predicate<T> condition, ObjType type) where T : IStoreable
        {
            if (IsClosing)
                return default(T);
            var dict = Objects[(int)type];

            return dict.Values.OfType<T>().FirstOrDefault(x => condition(x));
        }
        #endregion

        #region GET ALL
        public IEnumerable<T> GetAll<T>(Predicate<T> condition, ObjType type) where T : IStoreable
        {
            if (IsClosing)
                return new T[0];
            var dict = Objects[(int)type];

            lock (Sync)
            {
                return dict.Values.OfType<T>().Where(x => condition(x));
            }
        }
        #endregion
    }

    //AVAILABLE ONLY IN CHIP2
    partial class GwsFactory
    {
#if AdvancedVersion
        public abstract ISimplePopup newSimplePopup(params string[] items);
        public abstract ISimplePopup newSimplePopup(int width, int height, params string[] items);
#endif
    }
}
