using MnM.GWS.EnumerableExtensions;
using MnM.GWS.EnumExtensions;
using MnM.GWS.MathExtensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static MnM.GWS.Implementation;

namespace MnM.GWS 
{
    public static partial class Geometry
    {
        #region VARAIBLES AND CONSTS
        public const float PI = 3.14159265358979323846264338327950288419716939937510f;
        public const float PIx2 = 2 * 3.14159265358979323846264338327950288419716939937510f;

        public const float Radian = PI / 180f;
        public const float RadianInv = 180f / PI;
        public const float EPSILON = .0001f;
        //const float kaapa = .5522847498307933984022516322796f;
        public const int TransparentColor = 16777215;
        static readonly Dictionary<float, Pair<float, float>> cosSins = new Dictionary<float, Pair<float, float>>(360);

        const float angleEpsilon = 0.001f * Geometry.Radian;     // 0.1% of a degree
        static volatile bool sinCosInitialized;
        static float[] Lookup;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initialise an (angle, Sin, Cosine) lookup table and an index Factorial lookup table.
        /// </summary>
        static Geometry()
        {
            for (float i = -720; i < 721; i += .5f)
                SinCos(i, out float sin, out float cos);
            sinCosInitialized = true;
            Lookup = new float[]
            {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
            355687428096000.0f,
            6402373705728000.0f,
            121645100408832000.0f,
            2432902008176640000.0f,
            51090942171709440000.0f,
            1124000727777607680000.0f,
            25852016738884976640000.0f,
            620448401733239439360000.0f,
            15511210043330985984000000.0f,
            403291461126605635584000000.0f,
            10888869450418352160768000000.0f,
            304888344611713860501504000000.0f,
            8841761993739701954543616000000.0f,
            265252859812191058636308480000000.0f,
            8222838654177922817725562880000000.0f,
            263130836933693530167218012160000000.0f
            };
        }
        #endregion

        #region CROP
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="image">IGraphics object holding an image to crop</param>
        /// <param name="x">x ordinate of top left point in cropped image.</param>
        /// <param name="y">y ordinate of top left point in cropped image.</param>
        /// <param name="w">width of cropped image.</param>
        /// <param name="h">height of cropped image.</param>
        /// <returns>Return the specified rectangle from the orriginal image.</returns>
        public static T GetPotion<T>(this T image, int? x = null, int? y = null, int? w = null, int? h = null) where T : IGraphics
        {
            if (IsCompitibleRC(image.Width, image.Height, x, y, w, h))
                return (image);

            var rc = CompitibleRC(image.Width, image.Height, x, y, w, h);
            if (rc.Width == 0 || rc.Height == 0)
                return image;

            var img = (T)image.ToPen(rc.Width, rc.Height);
            img.DrawImage(image, 0, 0, rc.X, rc.Y, rc.Width, rc.Height);
            return img;
        }
        #endregion

        #region ANIMATED GIF FRAME
        /// <summary>
        /// Load GIF from file.
        /// </summary>
        /// <param name="path">Path of file containing images.</param>
        /// <param name="x">Width of graphic</param>
        /// <param name="y">Height of graphic</param>
        /// <param name="comp">actual color composition</param>
        /// <param name="requiredComposition">Required color composition</param>
        /// <returns>IAnimatedGifFrame containing GIF data.</returns>
        public static IAnimatedGifFrame[] GifFromStream(string path, out int x, out int y, out int comp, int requiredComposition = 4)
        {
            IAnimatedGifFrame[] frames = null;
            using (Stream ms = File.Open(path, FileMode.Open))
            {
                frames = GifFromStream(ms, out x, out y, out comp, requiredComposition);
            }
            return frames;
        }
        /// <summary>
        /// Load GIF from file.
        /// </summary>
        /// <param name="path">Path of file containing images.</param>
        /// <param name="requiredComposition">Required color composition</param>
        /// <returns></returns>
        public static Tuple<IAnimatedGifFrame[], Point, int> GifFromStream(string path, int requiredComposition = 4)
        {
            IAnimatedGifFrame[] frames = null;
            int x, y;
            int comp;
            using (Stream ms = File.Open(path, FileMode.Open))
            {
                frames = GifFromStream(ms, out x, out y, out comp, requiredComposition);
            }
            return Tuple.Create(frames, new Point(x, y), comp);
        }
        /// <summary>
        /// Load GIF from file.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="x">Width of graphic</param>
        /// <param name="y">Height of graphic</param>
        /// <param name="comp">actual color composition</param>
        /// <param name="requiredComposition">Required color composition</param>
        /// <returns></returns>
        public static IAnimatedGifFrame[] GifFromStream(Stream stream, out int x, out int y, out int comp, int requiredComposition) =>
            STBImage.Reader.ReadAnimatedGif(stream, out x, out y, out comp, requiredComposition);
        #endregion

        #region COMPITIBLE RC
        /// <summary>
        /// Returns True if image width and height match the Rectangle width and height with (x,y)=(0,0) or the rectangle has not beem defined yet.
        /// </summary>
        /// <param name="imgW">Image width.</param>
        /// <param name="imgH">Image height.</param>
        /// <param name="x">Top Left x ordinate (0 or null).</param>
        /// <param name="y">Top Left y ordinate (0 or null).</param>
        /// <param name="width">Rectangle width (imgW or null).</param>
        /// <param name="height">Rectangle height (imgH or null).</param>
        /// <returns></returns>
        public static bool IsCompitibleRC(int imgW, int imgH, int? x = null, int? y = null, int? width = null, int? height = null)
        {
            return (
                (x == 0 || x == null) &&
                (y == 0 || y == null) &&
                (width == imgW || width == 0 || width == null) &&
                (height == imgH || height == 0 || height == null));
        }
        /// <summary>
        /// Returns an IRectangle that is compatible with the one required.
        /// </summary>
        /// <param name="sW">Width reuired</param>
        /// <param name="sH">Height Required</param>
        /// <param name="x0">Proposed X position if any.</param>
        /// <param name="y0">Proposed Y position if any.</param>
        /// <param name="w0">Proposed width if any.</param>
        /// <param name="h0">Prioposed height if any.</param>
        /// <returns>Returns a rexctangle compatible with the sW and sH using the provided parameters (if any) or and empty Irectangle object.</returns>
        public static IRectangle CompitibleRC(int sW, int sH, int? x0 = null, int? y0 = null, int? w0 = null, int? h0 = null)
        {
            var x = x0 ?? 0;
            var y = y0 ?? 0;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            var w = Math.Min(w0 ?? sW, sW);
            var h = Math.Min(h0 ?? sH, sH);
            if (h < 0 || w < 0)
                return Factory.RectEmpty;


            var r = Factory.newRectangle(x, y, w, h);

            int rt = r.Right, bt = r.Bottom;
            if (r.Right > sW)
                rt = sW;
            if (r.Bottom > sH)
                bt = sH;
            return Factory.newRectangle(r.X, r.Y, rt - r.X, bt - r.Y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sz">Size object defining rectangle.</param>
        /// <param name="x0">Proposed X position if any.</param>
        /// <param name="y0">Proposed Y position if any.</param>
        /// <param name="w0">Proposed width if any.</param>
        /// <param name="h0">Prioposed height if any.</param>
        /// <returns>Returns a rexctangle compatible with the specified ISize object using the provided parameters (if any) or and empty Irectangle object.</returns>
        public static IRectangle CompitibleRC(this ISize sz, int? x0 = null, int? y0 = null, int? w0 = null, int? h0 = null)
        {
            if (sz is ISolidPen)
            {
                var wh = Math.Max(w0 ?? sz.Width, sz.Width);
                var ht = Math.Max(h0 ?? sz.Height, sz.Height);
                return CompitibleRC(wh, ht, x0, y0);
            }
            else
            {
                return CompitibleRC(sz.Width, sz.Height, x0, y0, w0, h0);
            }
        }
        #endregion

        #region RESIZE DATA
        /// <summary>
        /// Resizes Array the image is stored in returning: a truncated image, an image with transparent border to right and bottom or an image that is transparent..
        /// </summary>
        /// <param name="source">Original image array</param>
        /// <param name="newWidth">Required Width.</param>
        /// <param name="newHeight">Required Height.</param>
        /// <param name="oldWidth">Original Width.</param>
        /// <param name="oldHeight">Original Height.</param>
        /// <param name="clear">If true a trasparrent image is returned in the new size.</param>
        /// <returns>Returns an int array of transparrent colour or the image truncated/padded to fit the new rectangle.</returns>
        public static int[] ResizedData(int[] source, int newWidth, int newHeight, ref int oldWidth, ref int oldHeight, bool clear = false)
        {
            int[] result;
            if (clear)
            {
                oldWidth = newWidth;
                oldHeight = newHeight;
                result = TransparentColor.Repeat(newWidth * newHeight);
                return result;
            }
            var area = Factory.newRectangle(0, 0, newWidth, newHeight);
            result = TransparentColor.Repeat(area.Width * area.Height);
            CopyBlock(source.Length, area, oldWidth, 0, 0, newWidth, result.Length,
                (si, di, w, i) => Array.Copy(source, si, result, di, w));

            oldWidth = newWidth;
            oldHeight = newHeight;

            return result;
        }

        /// <summary>
        /// Resizes an usafe Memory Array the image is stored in returning: a truncated image, an image with transparent border to right and bottom or an image that is transparent..
        /// </summary>
        /// <param name="source">Pointer to original image array</param>
        /// <param name="srcLen">Length of int array.</param>
        /// <param name="newWidth">Required Width.</param>
        /// <param name="newHeight">Required Height.</param>
        /// <param name="oldWidth">Original Width.</param>
        /// <param name="oldHeight">Original Height.</param>
        /// <param name="clear">If true a trasparrent image is returned in the new size.</param>
        /// <returns>Returns pointer to int array of transparrent colour or the image truncated/padded to fit the new rectangle.</returns>
        public static unsafe int* ResizedData(int* source, int srcLen, int newWidth, int newHeight, ref int oldWidth, ref int oldHeight, bool clear = false)
        {
            int[] result;

            if (clear)
            {
                oldWidth = newWidth;
                oldHeight = newHeight;
                result = TransparentColor.Repeat(newWidth * newHeight);
                goto mks;
            }

            var src = (int*)source;
            var area = Factory.newRectangle(0, 0, newWidth, newHeight);
            result = TransparentColor.Repeat(area.Width * area.Height);
            CopyBlock(srcLen, area, oldWidth, 0, 0, newWidth, result.Length,
                (si, di, w, i) => CopyMemory(src, si, result, di, w));

            oldWidth = newWidth;
            oldHeight = newHeight;

        mks:
            fixed (int* p = result)
            {
                return p;
            }
        }

        /// <summary>
        /// Resizes IBuffer Array the image is stored in returning IBuffer containing: a truncated image, an image with transparent border to right and bottom or an image that is transparent..
        /// </summary>
        /// <param name="source">Original image array</param>
        /// <param name="newWidth">Required Width.</param>
        /// <param name="newHeight">Required Height.</param>
        /// <param name="clear">If true a trasparrent image is returned in the new size.</param>
        /// <param name="clear"></param>
        /// <returns>Returns IBuffer object containing either an array of transparrent colour or the image truncated/padded to fit the new rectangle.</returns>
        public static unsafe IBuffer ResizedData(this IBuffer source, int newWidth, int newHeight, bool clear = false)
        {
            var oldWidth = source.Width;
            var oldHeight = source.Height;
            int[] result;

            if (clear)
            {
                oldWidth = newWidth;
                oldHeight = newHeight;
                result = TransparentColor.Repeat(newWidth * newHeight);
                goto mks;
            }

            var src = (int*)source.Pixels;
            var area = Factory.newRectangle(0, 0, newWidth, newHeight);
            result = TransparentColor.Repeat(area.Width * area.Height);
            CopyBlock(source.Length, area, oldWidth, 0, 0, newWidth, result.Length,
                (si, di, w, i) => CopyMemory(src, si, result, di, w));

            oldWidth = newWidth;
            oldHeight = newHeight;

        mks:
            return Factory.newBuffer(result, oldWidth, oldHeight);
        }
        #endregion

        #region IMAGE READ
        /// <summary>
        /// Read Image from file and return the image with width and height data.
        /// </summary>
        /// <param name="path">File path of image.</param>
        /// <returns>Returns a Pair containing the image byte array and its width and height.</returns>
        public static Pair<byte[], int, int> ReadImage(string path)
        {
            return Factory.ImageProcessor.Reader.Read(path);
        }
        /// <summary>
        /// Read Image and return the image with width and height data.
        /// </summary>
        /// <param name="data">Byte array representing image.</param>
        /// <returns>Returns a Pair containing the image byte array and its width and height.</returns>
        public static Pair<byte[], int, int> ReadImage(byte[] data)
        {
            return Factory.ImageProcessor.Reader.Read(data);
        }
        #endregion
    }

    //ANGLE HELPER
    partial class Geometry
    {
        #region ROTATE
        /// <summary>
        /// Rotates a point on the shape around the center of its rectangular bounds.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <param name="x">x position of point on the shape.</param>
        /// <param name="y">y position of point on the shape.</param>
        /// <param name="CX">x position of the centre of the rectangle containing the shape.</param>
        /// <param name="CY">y position of the center of the rectangle containing the shape.</param>
        /// <param name="newX">The x position of the rotated point.</param>
        /// <param name="newY">The y position of the rotated point.</param>
        /// <param name="negative">True for clockwise rotation.</param>
        public static void Rotate(float angle, float x, float y, float CX, float CY, out float newX, out float newY, bool negative = false)
        {
            if (angle == 0)
            {
                newX = x;
                newY = y;
                return;
            }
            x -= CX;
            y -= CY;

            float sin, cos;
            if (!negative)
                SinCos(angle, out sin, out cos);
            else
                SinCos(-angle, out sin, out cos);

            newX = x * cos - y * sin;
            newY = x * sin + y * cos;

            newX += CX;
            newY += CY;
        }
        #endregion

        #region ROTATE 180
        /// <summary>
        /// Rotates a point on the shape 180 degrees around the center of its rectangular bounds.
        /// </summary>
        /// <param name="x">x position of point on the shape.</param>
        /// <param name="y">y position of point on the shape.</param>
        /// <param name="height"></param>
        /// <param name="newX">Rotated x co-ordinate of point.</param>
        /// <param name="newY">Rotated y co-ordinate of point.</param>
        public static void Rotate180(int x, int y, int height, out int newX, out int newY)
        {
            y -= height;
            if (y < 0)
                y = Math.Abs(y);//!!!!If the Rectangle centre is 0 then You get strange results. The image is folded on to itself about y = 0!!!!
            newX = x;
            newY = y;
        }
        #endregion

        #region FLIP
        /// <summary>
        /// Flip along the x axis at the centre of the rectangle containing the shape.
        /// </summary>
        /// <param name="y">Point to be flipped (x is unchanged).</param>
        /// <param name="height">Height of rectangle.</param>
        /// <returns>New value of points y co-ordinate.</returns>
        public static int FlipVertical(int y, int height)
        {
            y -= height;
            if (y < 0)
                y = Math.Abs(y);
            return y;
        }
        /// <summary>
        /// Flip along the y axis at the centre of the rectangle containing the shape.
        /// </summary>
        /// <param name="x">Point to be flipped (y is unchanged).</param>
        /// <param name="width">Width of rectangle.</param>
        /// <returns>New value of points x co-ordinate.</returns>
        public static int FlipHorizontal(int x, int width)
        {
            x -= width;
            if (x < 0)
                x = Math.Abs(x);
            return x;
        }
        #endregion

        #region SIN COS
        /// <summary>
        /// Adds The angle and its Sin and Cisine to CoSins and returns the Sin and Cosine.
        /// </summary>
        /// <param name="angle">Angle in degrees for lookup/calculation.</param>
        /// <param name="sin">Returns Sin of angle.</param>
        /// <param name="cos">Returns Cosine of angle,</param>
        public static void SinCos(float angle, out float sin, out float cos)
        {
            if (cosSins.ContainsKey(angle))
            {
                var p = cosSins[angle];
                cos = p.Item1;
                sin = p.Item2;
                return;
            }

            float radians = angle * Geometry.Radian;
            radians = (float)Math.IEEERemainder(radians, Geometry.PI * 2);
            if (radians > -angleEpsilon && radians < angleEpsilon)
            {
                // Exact case for zero rotation.
                cos = 1;
                sin = 0;
            }
            else if (radians > Math.PI / 2 - angleEpsilon && radians < Math.PI / 2 + angleEpsilon)
            {
                // Exact case for 90 degree rotation.
                cos = 0;
                sin = 1;
            }
            else if (radians < -Math.PI + angleEpsilon || radians > Math.PI - angleEpsilon)
            {
                // Exact case for 180 degree rotation.
                cos = -1;
                sin = 0;
            }
            else if (radians > -Math.PI / 2 - angleEpsilon && radians < -Math.PI / 2 + angleEpsilon)
            {
                // Exact case for 270 degree rotation.
                cos = 0;
                sin = -1;
            }
            else
            {
                // Arbitrary rotation.
                cos = (float)Math.Cos(radians);
                sin = (float)Math.Sin(radians);
            }

            if (!sinCosInitialized)
                cosSins[angle] = Pair.Create(cos, sin);
        }
        #endregion

        #region GET ANGLE
        /// <summary>
        /// Find the angle of a point in relation to a central point ( of the enclosing rectangle).
        /// </summary>
        /// <param name="x">x position of point.</param>
        /// <param name="y">y position of point.</param>
        /// <param name="cx">x position of rectangles center.</param>
        /// <param name="cy">y position of rectangles center.</param>
        /// <param name="inDegree">Returns degrees if true and radians otherwise.</param>
        /// <param name="convertToPositiveIfNegative">Returns Positive only values if true.</param>
        /// <returns>Angle of point in relation to the centre of the Rectangle.</returns>
        public static float GetAngle(float x, float y, float cx, float cy, bool inDegree = true, bool convertToPositiveIfNegative = true)
        {
            float angle = (float)Math.Atan2((y - cy), (x - cx));

            if (inDegree)
            {
                angle *= Geometry.RadianInv;
                if (angle < 0 && convertToPositiveIfNegative)
                    angle += 360;
                return angle;
            }

            if (angle < 0 && convertToPositiveIfNegative)
                angle += Geometry.PIx2;
            return angle;
        }
        /// <summary>
        /// Returns the angle between a point and a central point with horizontal as 0.
        /// </summary>
        /// <param name="p">First point.</param>
        /// <param name="c">Second point (typically the center of rectangle containing shape).</param>
        /// <param name="inDegree">Returns degrees if true.</param>
        /// <param name="convertToPositiveIfNegative">Returns positive values only if true.</param>
        /// <returns>Returns the angle betweeen points.</returns>
        public static float GetAngle(this PointF p, PointF c, bool inDegree = true, bool convertToPositiveIfNegative = true) =>
            GetAngle(p.X, p.Y, c.X, c.Y, inDegree, convertToPositiveIfNegative);
        /// <summary>
        /// Returns the angle between a point and a central point with horizontal as 0.
        /// </summary>
        /// <param name="x">x position of point of interest.</param>
        /// <param name="y">y position of point of interest.</param>
        /// <param name="c">Second point (typically the center of rectangle containing shape).</param>
        /// <param name="inDegree">Returns degrees if true.</param>
        /// <param name="convertToPositiveIfNegative">Returns positive values only if true.</param>
        /// <returns>Returns the angle betweeen points.</returns>
        public static float GetAngle(float x, float y, PointF c, bool inDegree = true, bool convertToPositiveIfNegative = true) =>
            GetAngle(x, y, c.X, c.Y, inDegree, convertToPositiveIfNegative);
        #endregion
    }

    //AREA HELPER
    partial class Geometry
    {
        #region UNION
        /// <summary>
        /// Return a IRectangle object defining an area containing the areas of two specified rectangles.
        /// </summary>
        /// <param name="areaG1">First rectangle to be merged.</param>
        /// <param name="areaG2">Second rectangle to be merged.</param>
        /// <returns>IRectangle with area containing the orriginal rectangles.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRectangle Hybrid(this IRectangle areaG1, IRectangle areaG2)
        {
            var x = Math.Min(areaG1.X, areaG2.X);
            var y = Math.Min(areaG1.Y, areaG2.Y);
            var r = Math.Max(areaG1.Right, areaG2.Right);
            var b = Math.Max(areaG1.Bottom, areaG2.Bottom);
            return Factory.RectangleFromLTRB(x, y, r, b);
        }
        /// <summary>
        /// Return a IRectangleF object defining an area containing the areas of two specified rectangles.
        /// </summary>
        /// <param name="areaG1">First rectangle to be merged.</param>
        /// <param name="areaG2">Second rectangle to be merged.</param>
        /// <returns>IRectangleF with area containing the orriginal rectangles.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRectangleF Hybrid(this IRectangleF areaG1, IRectangleF areaG2)
        {
            var x = Math.Min(areaG1.X, areaG2.X);
            var y = Math.Min(areaG1.Y, areaG2.Y);

            var r = Math.Max(areaG1.Right, areaG2.Right);
            var b = Math.Max(areaG1.Bottom, areaG2.Bottom);
            return Factory.RectangleFFromLTRB(x, y, r, b);
        }
        /// <summary>
        /// Creates a IRectangleF which defines a rectangle containing the two areas defined by the bounds on the two list of points supplied..
        /// </summary>
        /// <param name="c1">List of points defining one area.</param>
        /// <param name="c2">List of point defining a second area.</param>
        /// <returns>IRectangleF defining a rectangle that contains both sets of points.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRectangleF HybridBounds(this IEnumerable<PointF> c1, IEnumerable<PointF> c2) =>
            c1.ToArea().Hybrid(c2.ToArea());

        #endregion

        #region GET STROKE AREAS
        /// <summary>
        /// Returns 2 IRectangles that define the area of the inside or outside of the outline with stroke applied. 
        /// </summary>
        /// <param name="x">x position of Top Left of rectangle containing the curve without stroke.</param>
        /// <param name="y">y position of Top Left of rectangle containing the curve without stroke.</param>
        /// <param name="w">Width of rectangle containing the curve without stroke.</param>
        /// <param name="h">Width  of rectangle containing the curve without stroke.</param>
        /// <param name="outer">IRectangle returned with dimensions that contain the Outer edge of the Stroke.</param>
        /// <param name="inner">IRectangle returned with dimensions that contain the Inner edge of the Stroke.</param>
        /// <param name="stroke">Stroke size.</param>
        /// <param name="mode">Enum defining stroke position in relation to the curve without stroke.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetStrokeAreas(float x, float y, float w, float h, out IRectangleF outer, out IRectangleF inner, float stroke, StrokeMode mode = StrokeMode.Middle)
        {
            var r = x + w;
            var b = y + h;

            switch (mode)
            {
                case StrokeMode.Middle:
                default:
                    outer = Factory.RectangleFFromLTRB(x - stroke / 2f, y - stroke / 2f, r + stroke, b + stroke);
                    inner = Factory.RectangleFFromLTRB(x + stroke / 2f, y + stroke / 2f, r, b);
                    break;
                case StrokeMode.Outer:
                    outer = Factory.newRectangleF(x - stroke, y - stroke, r + stroke, b + stroke);
                    inner = outer;
                    break;
                case StrokeMode.Inner:
                    inner = Factory.newRectangleF(x + stroke, y + stroke, r - stroke, b - stroke);
                    outer = inner;
                    break;
            }
        }

        /// <summary>
        /// Returns 2 IRectangleFs that define the area of the inside or outside of the outline with stroke applied. 
        /// </summary>
        /// <param name="area">Area occupied by the shape.</param>
        /// <param name="outerArea">IRectangleF returned with dimensions that contain the Outer edge of the Stroke.</param>
        /// <param name="innerArea">IRectangleF returned with dimensions that contain the Inner edge of the Stroke.</param>
        /// <param name="stroke">Stroke size.</param>
        /// <param name="mode">Enum defining stroke position in relation to the curve without stroke.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetStrokeAreas(this IRectangle area, out IRectangleF outerArea, out IRectangleF innerArea, float stroke, StrokeMode mode = StrokeMode.Middle) =>
            GetStrokeAreas(area.X, area.Y, area.Width, area.Height, out outerArea, out innerArea, stroke, mode);

        /// <summary>
        /// Returns 2 IRectangleFs that define the area of the inside or outside of the outline with stroke applied. 
        /// </summary>
        /// <param name="area">Area occupied by the shape.</param>
        /// <param name="outerArea">IRectangleF returned with dimensions that contain the Outer edge of the Stroke.</param>
        /// <param name="innerArea">IRectangleF returned with dimensions that contain the Inner edge of the Stroke.</param>
        /// <param name="stroke">Stroke size.</param>
        /// <param name="mode">Enum defining stroke position in relation to the curve without stroke.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetStrokeAreas(this IRectangleF area, out IRectangleF outerArea, out IRectangleF innerArea, float stroke, StrokeMode mode = StrokeMode.Middle) =>
            GetStrokeAreas(area.X, area.Y, area.Width, area.Height, out outerArea, out innerArea, stroke, mode);
        #endregion

        #region CHANGE
        /// <summary>
        /// Convert IRectangle with integer dimensions to IRectangleF with floating point dimension.
        /// </summary>
        /// <param name="r">IRectangle to convert.</param>
        /// <returns></returns>
        public static IRectangleF ToRectangleF(this IRectangle r) =>
            Factory.newRectangleF(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// !!!! depricate?!!!!
        /// Creates new IRectangle with same dimensions as given Integer version.
        /// </summary>
        /// <param name="r">IRectangle to convert</param>
        /// <returns></returns>
        public static IRectangleF ToAreaF(this IRectangle r) =>
            Factory.newRectangleF(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Converts IBoxF to IBox.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding up.
        /// </summary>
        /// <param name="r">IBoxF to convert</param>
        /// <returns>New IBox with integer dimensions.</returns>
        public static IBox Ceiling(this IBoxF r) =>
            Factory.newBox(r.X.Ceiling(), r.Y.Ceiling(), r.Width.Ceiling(), r.Height.Ceiling());
        /// <summary>
        /// Converts IBoxF to IBox.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding to nearest Integer.
        /// </summary>
        /// <param name="r">IBoxF to convert</param>
        /// <returns>New IBox with integer dimensions.</returns>
        public static IBox Round(this IBoxF r) =>
            Factory.newBox(r.X.Round(), r.Y.Round(), r.Width.Round(), r.Height.Round());
        /// <summary>
        /// Converts IBoxF to IBox.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding down.
        /// </summary>
        /// <param name="r">IBoxF to convert</param>
        /// <returns>New IBox with integer dimensions.</returns>
        public static IBox Floor(this IBoxF r) =>
            Factory.newBox((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);

        /// <summary>
        /// Converts IBoxF to IBox.
        /// Converts contained rectangle by rounding up the co-ordinates of the Top left corner and rounding down the width and height.
        /// </summary>
        /// <param name="r">IBoxF to convert</param>
        /// <returns>New IBox with integer dimensions.</returns>
        public static IBox Shrink(this IBoxF r) =>
            Factory.newBox(r.X.Ceiling(), r.Y.Ceiling(), (int)r.Width, (int)r.Height);

        /// <summary>
        /// Converts IBoxF to IBox.
        /// Converts contained rectangle by rounding down the co-ordinates of the Top left corner and rounding up the width and height.
        /// </summary>
        /// <param name="r">IBoxF to convert</param>
        /// <returns>New IBox with integer dimensions.</returns>
        public static IBox Expand(this IBoxF r) =>
           Factory.newBox((int)r.X, (int)r.Y, r.Width.Ceiling(), r.Height.Ceiling());

        /// <summary>
        /// Converts IRectngleF to IRectangle.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding up.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <returns>New IRectangle with integer dimensions.</returns>
        public static IRectangle Ceiling(this IRectangleF r) =>
            Factory.newRectangle(r.X.Ceiling(), r.Y.Ceiling(), r.Width.Ceiling(), r.Height.Ceiling());
        /// <summary>
        /// Converts IRectngleF to IRectangle.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding to nearest integer.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <returns>New IRectangle with integer dimensions.</returns>
        public static IRectangle Round(this IRectangleF r) =>
            Factory.newRectangle(r.X.Round(), r.Y.Round(), r.Width.Round(), r.Height.Round());
        /// <summary>
        /// Converts IRectngleF to IRectangle.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding down.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <returns>New IRectangle with integer dimensions.</returns>
        public static IRectangle Floor(this IRectangleF r) =>
            Factory.newRectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);

        /// <summary>
        /// Converts IRectngleF to IRectangle.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding up the (x,y) of the top left corner 
        /// and rounding down the Height and Width.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <returns>New IRectangle with integer dimensions.</returns>
        public static IRectangle Shrink(this IRectangleF r) =>
            Factory.newRectangle(r.X.Ceiling(), r.Y.Ceiling(), (int)r.Width, (int)r.Height);

        /// <summary>
        /// Converts IRectngleF to IRectangle.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding down the (x,y) of the top left corner 
        /// and rounding up the Height and Width.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <returns>New IRectangle with integer dimensions.</returns>
        public static IRectangle Expand(this IRectangleF r) =>
           Factory.newRectangle((int)r.X, (int)r.Y, r.Width.Ceiling(), r.Height.Ceiling());


        /// <summary>
        /// Returns integer dimensions for the rectangle in IRectangleF provided.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding up.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <param name="x">x position of Top Left corner rounded up to an Integer..</param>
        /// <param name="y">y position of the Top Left corner rounded up to an Integer..</param>
        /// <param name="w">Width rounded up to an Integer.</param>
        /// <param name="h">Height rounded up to an Integer.</param>
        public static void Ceiling(this IRectangleF r, out int x, out int y, out int w, out int h)
        {
            x = r.X.Ceiling();
            y = r.Y.Ceiling();
            w = r.Width.Ceiling();
            h = r.Height.Ceiling();
        }
        /// <summary>
        /// Returns integer dimensions for the rectangle in IRectangleF provided.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding to nearest integer.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <param name="x">x position of Top Left corner rounded to the nearest Integer.</param>
        /// <param name="y">y position of the Top Left corner rounded to the nearest Integer.</param>
        /// <param name="w">Width rounded to the nearest Integer.</param>
        /// <param name="h">Height rounded to the nearest Integer.</param>
        public static void Round(this IRectangleF r, out int x, out int y, out int w, out int h)
        {
            x = r.X.Round();
            y = r.Y.Round();
            w = r.Width.Round();
            h = r.Height.Round();
        }
        /// <summary>
        /// Returns integer dimensions for the rectangle in IRectangleF provided.
        /// Converts contained rectangle with floating point dimensions to integer dimensions by rounding down.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <param name="x">x position of Top Left corner rounded down.</param>
        /// <param name="y">y position of the Top Left corner rounded down.</param>
        /// <param name="w">Width rounded down.</param>
        /// <param name="h">Height rounded down.</param>
        public static void Floor(this IRectangleF r, out int x, out int y, out int w, out int h)
        {
            x = r.X.Floor();
            y = r.Y.Floor();
            w = r.Width.Floor();
            h = r.Height.Floor();
        }
        /// <summary>
        /// Returns integer dimensions for the rectangle in IRectangleF provided.
        /// Converts contained rectangle's (x,y), of top left corner, to integer values by rounding up.
        /// Converts width and height to integer dimensions by rounding down.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <param name="x">x position of Top Left corner rounded up.</param>
        /// <param name="y">y position of the Top Left corner rounded up.</param>
        /// <param name="w">Width rounded down.</param>
        /// <param name="h">Height rounded down.</param>
        public static void Shrink(this IRectangleF r, out int x, out int y, out int w, out int h)
        {
            x = r.X.Ceiling();
            y = r.Y.Ceiling();
            w = r.Width.Floor();
            h = r.Height.Floor();
        }
        /// <summary>
        /// Returns integer dimensions for the rectangle in IRectangleF provided.
        /// Converts contained rectangle's (x,y), of top left corner, to integer values by rounding down.
        /// Converts width and height to integer dimensions by rounding up.
        /// </summary>
        /// <param name="r">IRectangleF to convert</param>
        /// <param name="x">x position of Top Left corner rounded down.</param>
        /// <param name="y">y position of the Top Left corner rounded down.</param>
        /// <param name="w">Width rounded up.</param>
        /// <param name="h">Height rounded up.</param>
        public static void Expand(this IRectangleF r, out int x, out int y, out int w, out int h)
        {
            x = r.X.Floor();
            y = r.Y.Floor();
            w = r.Width.Ceiling();
            h = r.Height.Ceiling();
        }
        /// <summary>
        /// Returns the floating point dimensions of the rectangle defined by IRectangleF.
        /// !!!!bounds do something special!!!!
        /// </summary>
        /// <param name="rc">IRectangleF to report on.</param>
        /// <param name="x">x position of Top Left corner.</param>
        /// <param name="y">y position of Top Left corner.</param>
        /// <param name="r">x position of bottom right corner.</param>
        /// <param name="b">y position of bottom right corner.</param>
        public static void Get(this IRectangleF rc, out float x, out float y, out float r, out float b)
        {
            x = rc.X;
            y = rc.Y;
            r = rc.Right;
            b = rc.Bounds.Bottom;
        }
        #endregion

        #region Center
        public static PointF Center(this IRectangleF r) =>
            new PointF(r.X + r.Width / 2f, r.Y + r.Height / 2f);
        public static PointF Center(this IRectangle r) =>
            new PointF(r.X + r.Width / 2f, r.Y + r.Height / 2f);
        #endregion

        #region HAS
        public static bool Has(this IRectangle rect, Point p, int checkRightUpto = -1, int checkBottomUpto = -1)
        {
            if (p == null)
                return false;
            return Has(rect, p.X, p.Y, checkRightUpto, checkBottomUpto);
        }
        public static bool Has(this IRectangle rect, int x = -1, int y = -1, int checkRightUpto = -1, int checkBottomUpto = -1)
        {
            if (rect.Width == 0 || rect.Height == 0) return false;
            if (x == -1 && y == -1)
            {
                x = 0;
                y = 0;
            }
            else
            {
                x = x == -1 ? rect.X : x;
                y = y == -1 ? rect.Y : y;
            }
            if (checkRightUpto == -1)
                checkRightUpto = rect.Width;
            if (checkBottomUpto == -1)
                checkBottomUpto = rect.Height;
            return x >= rect.X && x <= rect.X +
                checkRightUpto && y >= rect.Y &&
                y <= rect.Y + checkBottomUpto;
        }
        public static bool Has(this IRectangleF rect, Point p, int checkRightUpto = -1, int checkBottomUpto = -1)
        {
            if (p == null)
                return false;
            return Has(rect, p.X, p.Y, checkRightUpto, checkBottomUpto);
        }
        public static bool Has(this IRectangleF rect, float x = -1, float y = -1, float checkRightUpto = -1, float checkBottomUpto = -1)
        {
            if (rect.Width == 0 || rect.Height == 0) return false;
            if (x == -1 && y == -1)
            {
                x = 0;
                y = 0;
            }
            else
            {
                x = x == -1 ? rect.X : x;
                y = y == -1 ? rect.Y : y;
            }
            if (checkRightUpto == -1)
                checkRightUpto = rect.Width;
            if (checkBottomUpto == -1)
                checkBottomUpto = rect.Height;
            return x >= rect.X && x <= rect.X +
                checkRightUpto && y >= rect.Y &&
                y <= rect.Y + checkBottomUpto;
        }
        public static bool Has(this IRectangleF rect, PointF p, int checkRightUpto = -1, int checkBottomUpto = -1)
        {
            if (p == null)
                return false;
            return Has(rect, p.X, p.Y, checkRightUpto, checkBottomUpto);
        }
        #endregion

        #region PROXIMITY - INTERSECT
        public static bool Proximity(this IRectangle rect, Point p, out Point distancePoint)
        {
            bool ok = (Has(rect, p));
            if (ok)
                distancePoint = new Point(rect.X + rect.Width - p.X, rect.Y + rect.Height - p.Y);
            else
                distancePoint = new Point();
            return ok;
        }
        public static bool Intersects(this IRectangle rect, IRectangle other)
        {
            bool xOverlap = MathHelper.IsWithIn(other.X, other.Right, rect.X) ||
                 MathHelper.IsWithIn(rect.X, rect.Right, other.X);

            bool yOverlap = MathHelper.IsWithIn(other.Y, other.Bottom, rect.Y) ||
                 MathHelper.IsWithIn(rect.Y, rect.Bottom, other.Y);
            return xOverlap && yOverlap;
        }
        public static bool Proximity(this IRectangleF rect, PointF p, out PointF distancePoint)
        {
            bool ok = (Has(rect, p));
            if (ok)
                distancePoint = new PointF(rect.X + rect.Width - p.X, rect.Y + rect.Height - p.Y);
            else
                distancePoint = new PointF();
            return ok;
        }
        public static bool Intersects(this IRectangleF rect, IRectangleF other)
        {
            bool xOverlap = MathHelper.IsWithIn(other.X, (other.X + other.Width), rect.X) ||
                 MathHelper.IsWithIn(rect.X, (rect.X + rect.Width), other.X);

            bool yOverlap = MathHelper.IsWithIn(other.Y, (other.Y + other.Height), rect.Y) ||
                 MathHelper.IsWithIn(rect.Y, (rect.Y + rect.Height), other.Y);
            return xOverlap && yOverlap;
        }
        #endregion

        #region CONTAINS OTHER RECT
        public static bool Contains(this IRectangle parent, IRectangle child)
        {
            return child.X >= parent.X && child.Y >= parent.Y && child.Right <= parent.Right && child.Bottom <= parent.Bottom;
        }
        public static bool Contains(this IRectangle parent, IRectangleF child)
        {
            return child.X >= parent.X && child.Y >= parent.Y && child.Right <= parent.Right && child.Bottom <= parent.Bottom;
        }
        public static bool Contains(this IRectangleF parent, IRectangleF child)
        {
            return child.X >= parent.X && child.Y >= parent.Y && child.Right <= parent.Right && child.Bottom <= parent.Bottom;
        }
        public static bool Contains(this IRectangleF parent, IRectangle child)
        {
            return child.X >= parent.X && child.Y >= parent.Y && child.Right <= parent.Right && child.Bottom <= parent.Bottom;
        }
        #endregion
    }

    //CURVE HELPER
    partial class Geometry
    {
        #region GET ROTATED ELLIPSE DATA
        public static void GetDataAt(this CurveType Option, ref ICollection<float> List1, ref ICollection<float> List2,
              EllipseData ei, IArcCut ai, float position, bool horizontal, bool forOutLinesOnly, out int axis1, out int axis2)
        {
            bool Empty = Option == (CurveType.Full);
            Func<float, float, bool, bool> Contains;

            if (!Empty && ai?.IsEmpty == false)
                Contains = ai.Contains;
            else
                Contains = (v, a, h) => true;

            float v1, v2, v3, v4;
            List1.Clear();
            List2.Clear();

            GetRotatedEllipseDataAt(position, horizontal, ei.A, ei.B, ei.C, out float val1, out float val2);



            v1 = horizontal ? ei.Cx + val1 : ei.Cy - val1;
            v2 = horizontal ? ei.Cx + val2 : ei.Cy - val2;
            v3 = horizontal ? ei.Cx - val1 : ei.Cy + val1;
            v4 = horizontal ? ei.Cx - val2 : ei.Cy + val2;
            var a1 = horizontal ? ei.Cy - position : ei.Cx + position;
            var a2 = horizontal ? ei.Cy + position : ei.Cx - position;

            MathHelper.Order(ref v1, ref v2);
            MathHelper.Order(ref v3, ref v4);

            bool only2 = position > (horizontal ? ei.YEnd : ei.XEnd);
            only2 = only2 && forOutLinesOnly;
            float p1, p2;

            if (only2)
            {
                if (horizontal)
                {
                    p1 = ei.RightYGreater ? v2 : v1;
                    p2 = ei.RightYGreater ? v3 : v4;
                }
                else
                {
                    p1 = ei.TopXGreater ? v1 : v2;
                    p2 = ei.TopXGreater ? v4 : v3;

                }
                AddValSafe(p1, a1, horizontal, ref List1, Contains);
                AddValSafe(p2, a2, horizontal, ref List2, Contains);
            }
            else
            {
                AddValSafe(v1, a1, horizontal, ref List1, Contains);
                AddValSafe(v2, a1, horizontal, ref List1, Contains);
                AddValSafe(v3, a2, horizontal, ref List2, Contains);
                AddValSafe(v4, a2, horizontal, ref List2, Contains);
            }

            if (Empty)
                goto mks;

            if (forOutLinesOnly)
                goto mks;

            if (ai != null && !ai.IsEmpty)
            {
                Option.AddValsSafe(a1, horizontal, ref List1, ai);
                Option.AddValsSafe(a2, horizontal, ref List2, ai);
            }

        mks:

            axis1 = a1.Round();
            axis2 = a2.Round();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRotatedEllipseDataAt(float position, bool horizontal, float A, float B, float C, out float v1, out float v2)
        {
            v1 = v2 = float.NaN;
            var evSqr = MathHelper.Sqr(position);
            var mult = horizontal ? C : A;
            var oMult = horizontal ? A : C;
            float a1 = (mult * evSqr) - 1;
            float b1 = B * position;

            double disc = (b1 * b1) - (4 * oMult * a1);
            if (disc < 0)
                disc = 0;

            double b2 = -b1 / (2 * oMult);
            double d2 = Math.Sqrt(disc) / (2 * oMult);
            v1 = (float)(b2 + d2);
            v2 = (float)(b2 - d2);

            MathHelper.Order(ref v1, ref v2);
            return disc == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddValsSafe(this CurveType Option, float axis, bool horizontal,
            ref ICollection<float> list, ILine ArcLine, ILine Line1, ILine Line2, ref IList<ILine> Extra)
        {
            if (Option.IsClosedArc())
            {
                ArcLine.AddValSafe(axis, horizontal, ref list);
                ArcLine.AddValSafe(axis, horizontal, ref list);
            }
            else if (Option.IsArc())
            {
                if (Extra != null)
                {
                    foreach (var line in Extra)
                    {
                        line.AddValSafe(axis, horizontal, ref list);
                        line.AddValSafe(axis, horizontal, ref list);
                    }
                }
            }
            else if (Option.IsPie())
            {
                Line1.AddValSafe(axis, horizontal, ref list);
                Line2.AddValSafe(axis, horizontal, ref list);

                Line1.HandleAxisLine(axis, horizontal, ref list);
                Line2.HandleAxisLine(axis, horizontal, ref list);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddValsSafe(this CurveType Option, float axis, bool horizontal, ref ICollection<float> list, IArcCut ai)
        {
            if (ai == null)
                return;

            if (Option.IsClosedArc())
            {
                ai.ArcLine.AddValSafe(axis, horizontal, ref list);
                ai.ArcLine.AddValSafe(axis, horizontal, ref list);
            }
            else if (Option.IsArc())
            {
                if (ai.Extra != null)
                {
                    foreach (var line in ai.Extra)
                    {
                        line.AddValSafe(axis, horizontal, ref list);
                        line.AddValSafe(axis, horizontal, ref list);
                    }
                }
            }
            else if (Option.IsPie())
            {
                ai.Line1.AddValSafe(axis, horizontal, ref list);
                ai.Line2.AddValSafe(axis, horizontal, ref list);

                ai.Line1.HandleAxisLine(axis, horizontal, ref list);
                ai.Line2.HandleAxisLine(axis, horizontal, ref list);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddValSafe(float val, float axis, bool horizontal, ref ICollection<float> list, Func<float, float, bool, bool> CurveCondition)
        {
            if (!CurveCondition(val, axis, horizontal))
                return;
            list.Add(val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRotatedPieInfo(this EllipseData Data, float StartAngle, float EndAngle, out float SX2, out float SY2, out float EX2,
            out float EY2, out float SM, out float EM, out bool Small)
        {
            float angle = 0;

            if (Data.Angle.Valid)
                angle = -Data.Angle.Degree;

            var cos = (float)Math.Cos(angle * Geometry.Radian);
            var sin = (float)Math.Sin(angle * Geometry.Radian);

            var arcStart = -StartAngle;
            var arcEnd = -EndAngle;
            if (arcEnd > arcStart)
                MathHelper.Swap(ref arcStart, ref arcEnd);

            Small = Math.Abs(arcStart - arcEnd) < 180;

            float sinS, cosS, sinE, cosE;
            //AngleHelper.SinCos(arcStart, out sinS, out cosS);
            //AngleHelper.SinCos(arcEnd, out sinE, out cosE);

            sinS = (float)Math.Sin(arcStart * Geometry.Radian);
            cosS = (float)Math.Cos(arcStart * Geometry.Radian);
            sinE = (float)Math.Sin(arcEnd * Geometry.Radian);
            cosE = (float)Math.Cos(arcEnd * Geometry.Radian);


            var yS = (Data.Rx * cosS * sin + Data.Ry * sinS * cos).Round();
            SX2 = Data.Rx * cosS * cos - Data.Ry * sinS * sin;

            var yE = (Data.Rx * cosE * sin + Data.Ry * sinE * cos).Round();
            EX2 = Data.Rx * cosE * cos - Data.Ry * sinE * sin;

            int y0, y1;
            y0 = (int)Data.YMax;
            y1 = -y0;
            float v1, v2;

            if (yS >= y0 || yS <= y1)
                SY2 = Data.Rx * cosS * sin + Data.Ry * sinS * cos;
            else
            {
                GetRotatedEllipseDataAt(yS, true, Data.A, Data.B, Data.C, out v1, out v2);
                if (Math.Abs(SX2 - v1) > Math.Abs(SX2 - v2))
                    SX2 = v2;
                else
                    SX2 = v1;
                SY2 = yS;
            }

            if (yE >= y0 || yE <= y1)
                EY2 = Data.Rx * cosE * sin + Data.Ry * sinE * cos;
            else
            {
                GetRotatedEllipseDataAt(yE, true, Data.A, Data.B, Data.C, out v1, out v2);
                if (Math.Abs(EX2 - v1) > Math.Abs(EX2 - v2))
                    EX2 = v2;
                else
                    EX2 = v1;
                EY2 = yE;
            }
            SM = (SY2) / (SX2);
            EM = (EY2) / (EX2);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //static IPointF GetCompitiblePoint(IPointF p, float A, float B, float Cx, float Cy, float C, float yStart, float yEnd, float xStart, float xEnd)
        //{
        //    float v1, v2, v3, v4;
        //    int position;
        //    IPointF p1, p2, p3, p4;
        //    p1 = p2 = p3 = p4 = null;

        //    bool horizontal =
        //        MathHelper.IsWithIn(yStart, yEnd, Cy - p.Y) ||
        //        MathHelper.IsWithIn(yStart, yEnd, Cy + p.Y);

        //    if (!horizontal)
        //    {
        //        var inX =
        //            MathHelper.IsWithIn(xStart, xEnd, p.X - Cx) ||
        //            MathHelper.IsWithIn(xStart, xEnd, Cx - p.X);

        //        if (!inX)
        //            return p;
        //    }

        //    position = horizontal ? (int)(Cy - p.Y) : (int)(Cx - p.X);
        //    if (position < 0)
        //        position = horizontal ? (int)(Cy + p.Y) : (int)(p.X - Cx);


        //    var ok = GetDataAt(position, horizontal, A, B, C, out float val1, out float val2);
        //    if (!ok)
        //        return p;

        //    v1 = horizontal ? Cx + val1 : Cy - val1;
        //    v2 = horizontal ? Cx + val2 : Cy - val2;
        //    v3 = horizontal ? Cx - val1 : Cy + val1;
        //    v4 = horizontal ? Cx - val2 : Cy + val2;
        //    var axis = horizontal ? Cy - position : Cx + position;
        //    var axis1 = horizontal ? Cy + position : Cx - position;

        //    MathHelper.Order(ref v1, ref v2);
        //    MathHelper.Order(ref v3, ref v4);

        //    p1 = new PointF(horizontal ? v1 : axis, horizontal ? axis : v1);
        //    p2 = new PointF(horizontal ? v2 : axis, horizontal ? axis : v2);
        //    p3 = new PointF(horizontal ? v3 : axis1, horizontal ? axis1 : v3);
        //    p4 = new PointF(horizontal ? v4 : axis1, horizontal ? axis1 : v4);

        //    var d1 = p1.DistanceSquared(p);
        //    var d2 = p2.DistanceSquared(p);
        //    var d3 = p3.DistanceSquared(p);
        //    var d4 = p4.DistanceSquared(p);

        //    var dl1 = d1 < d2 ? d1 : d2;
        //    var dl2 = d3 < d4 ? d3 : d4;

        //    var m = d1 < d2 ? p1 : p2;
        //    var n = d3 < d4 ? p3 : p4;
        //    return dl1 < dl2 ? m : n;
        //}
        #endregion

        #region GET ELLIPSE POINT/S
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF GetEllipsePoint(float angle, float CX, float CY, float RadiusX, float RadiusY, Angle rotation, bool pieAngle)
        {
            GetEllipsePoint(angle, CX, CY, RadiusX, RadiusY, out float x, out float y, rotation, pieAngle);
            return new PointF(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetEllipsePoint(float angle, float CX, float CY, float RadiusX, float RadiusY, out float x, out float y, Angle rotation, bool pieAngle)
        {
            SinCos(angle, out float sin, out float cos);
            float cosrx, sinry, cs, RxRy = RadiusX * RadiusY;

            if (RadiusX == RadiusY || !pieAngle)
            {
                cosrx = RadiusX * cos;
                sinry = RadiusY * sin;
            }
            else
            {
                cs = (float)Math.Sqrt(MathHelper.Sqr(RadiusY * cos) + MathHelper.Sqr(RadiusX * sin));
                cosrx = (RxRy * cos) / cs;
                sinry = (RxRy * sin) / cs;
            }
            x = CX + cosrx;
            y = CY + sinry;

            if (rotation.Valid)
                rotation.Rotate(x, y, out x, out y);
        }

        public static PointF GetEllipsePoint(this IEllipse e, float angle, bool pieAngle) =>
            GetEllipsePoint(angle, e.Data.Cx, e.Data.Cy, e.Data.Rx, e.Data.Ry, e.Data.Angle, pieAngle);

        public static PointF GetEllipsePoint(this IEllipse e, float angle, bool pieAngle, Angle userAngle) =>
            GetEllipsePoint(angle, e.Data.Cx, e.Data.Cy, e.Data.Rx, e.Data.Ry, userAngle, pieAngle);
        public static PointF[] GetEllipsePoints(bool WMajor, float Cx, float Cy, float Rx, float Ry, bool pieAngle = false, Angle angle = default(Angle))
        {
            var points = new PointF[361];
            float x, y;
             
            GetEllipsePoint(0, Cx, Cy, Rx, Ry, out x, out y, angle, pieAngle);
            points[0] = new PointF(x, y);

            GetEllipsePoint(90, Cx, Cy, Rx, Ry, out x, out y, angle, pieAngle);
            points[90] = new PointF(x, y);

            GetEllipsePoint(180, Cx, Cy, Rx, Ry, out x, out y, angle, pieAngle);
            points[180] = new PointF(x, y);

            GetEllipsePoint(360, Cx, Cy, Rx, Ry, out x, out y, angle, pieAngle);
            points[360] = new PointF(x, y);

            int step;
            for (int i = 1; i < 360; i += step)
            {
                step = GetCurveStep(WMajor, i, Rx, Ry);
                GetEllipsePoint(i, Cx, Cy, Rx, Ry, out x, out y, angle, pieAngle);
                points[i] = new PointF(x, y);
            }
            return points;
        }

        public static PointF[] GetArcPoints(ref float startAngle, ref float endAngle, bool addCenter, bool WMajor, float Cx, float Cy, float Rx, float Ry,
             Angle angle = default(Angle), bool negativeMotion = false)
        {
            if (startAngle < 0)
                startAngle += 360;
            if (endAngle < 0)
                endAngle += 360;

            MathHelper.Order(ref startAngle, ref endAngle);

            //if (negativeMotion)
            //{
            //    startAngle += 360;
            //}
            //MathHelper.Order(ref startAngle, ref endAngle);

            var center = angle.Rotate(Cx, Cy);

            var points = new PointF[362];
            if (addCenter)
                points[0] = center;

            var first = GetEllipsePoint(startAngle, Cx, Cy, Rx, Ry, angle, true);
            var last = GetEllipsePoint(endAngle, Cx, Cy, Rx, Ry, angle, true);
            var line = Factory.newLine(first, last);
            Func<float, float, bool> condition;

            if (negativeMotion)
                condition = (m, n) => !line.IsGreaterThan(m, n);
            else
                condition = (m, n) => !line.IsLessThan(m, n);

            //var one = GetAngle(first, center).Round();
            //if(one!=0)
            //    points[one] = first;

            float x, y;
            int step;

            for (int i = 1; i <= 360; i += step)
            {
                step = GetCurveStep(WMajor, i, Rx, Ry);
                GetEllipsePoint(i, Cx, Cy, Rx, Ry, out x, out y, angle, false);
                if (condition(x, y))
                {
                    points[i] = (new PointF(x, y));
                }
            }

            GetEllipsePoint(90, Cx, Cy, Rx, Ry, out x, out y, angle, false);
            if (condition(x, y))
                points[90] = (new PointF(x, y));

            GetEllipsePoint(180, Cx, Cy, Rx, Ry, out x, out y, angle, false);
            if (condition(x, y))
                points[180] = (new PointF(x, y));

            GetEllipsePoint(270, Cx, Cy, Rx, Ry, out x, out y, angle, false);
            if (condition(x, y))
                points[270] = (new PointF(x, y));

            //GetEllipsePoint(360, Cx, Cy, Rx, Ry, out x, out y, angle, false);
            //if (condition(x, y))
            //    points[360] = (new XYf(x, y));

            //var final = GetAngle(last, center).Round();
            //if (final != 0)
            //    points[final] = last;

            return points;
        }

        #endregion

        #region GET CUBIC BEZIER 4 POINTS
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<PointF> GetBezier4Points(float x, float y, float rx, float ry, float startAngle, float endAngle)
        {
            /* center */
            var cx = x + rx;
            var cy = y + ry;

            startAngle *= Geometry.Radian;
            endAngle *= Geometry.Radian;

            var ssin = (float)Math.Sin(startAngle);
            var scos = (float)Math.Cos(startAngle);
            var esin = (float)Math.Sin(endAngle);
            var ecos = (float)Math.Cos(endAngle);

            /* adjust angles for ellipses */
            var alpha = (float)Math.Atan2(rx * ssin, ry * scos);
            var beta = (float)Math.Atan2(rx * esin, ry * ecos);


            if (Math.Abs(beta - alpha) > Geometry.PI)
            {
                if (beta > alpha)
                    beta -= 2 * Geometry.PI;
                else
                    alpha -= 2 * Geometry.PI;
            }

            var delta = (beta - alpha) / 2f;

            var bcp = (float)(4f / 3 * (1 - Math.Cos(delta)) / Math.Sin(delta));


            /* starting point */
            float x1 = cx + rx * scos;
            float y1 = cy + ry * ssin;

            var x2 = cx + rx * (scos - bcp * ssin);
            var y2 = cy + ry * (ssin + bcp * scos);

            var x3 = cx + rx * (ecos + bcp * esin);
            var y3 = cy + ry * (esin - bcp * ecos);

            var x4 = cx + rx * ecos;
            var y4 = cy + ry * esin;

            return EnumerableHelper.ToIEnumerable(x1, y1, x2, y2, x3, y3, x4, y4).ToPoints();
        }
        #endregion

        #region INTERCEPTION
        public static bool EllipseInterception(PointF p1, PointF p2, float x, float y, float width, float height,
            out PointF iPt1, out PointF iPt2, Angle angle = default(Angle), bool ignoreAngle = false)
        {
            var Rx = width / 2f; 
            var Ry = height / 2f;
            return EllipseInterception(Rx, Ry, x + Rx, y + Rx, p1, p2, out iPt1, out iPt2, angle, ignoreAngle);
        }

        /// <summary>
        /// Source Credit:http://csharphelper.com/blog/2017/08/calculate-where-a-line-segment-and-an-ellipse-intersect-in-c/
        /// </summary>
        /// <param name="Rx"></param>
        /// <param name="Ry"></param>
        /// <param name="Cx"></param>
        /// <param name="Cy"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="iPt1"></param>
        /// <param name="iPt2"></param>
        /// <param name="angle"></param>
        /// <param name="ignoreAngle"></param>
        /// <returns></returns>
        public static bool EllipseInterception(float Rx, float Ry, float Cx, float Cy, PointF p1, PointF p2, out PointF iPt1, out PointF iPt2,
            Angle angle = default(Angle), bool ignoreAngle = false)
        {
            iPt1 = iPt2 = PointF.Empty;
            // If the ellipse or line segment are empty, return no intersections.
            if ((Rx == 0) || (Ry == 0) ||
                ((p1.X == p2.X) && (p1.Y == p2.Y)))
                return false;

            var x1 = p1.X;
            var x2 = p2.X;
            var y1 = p1.Y;
            var y2 = p2.Y;

            if (!ignoreAngle && angle.Valid)
            {
                angle.Rotate(x1, y1, out x1, out y1, true);
                angle.Rotate(x2, y2, out x2, out y2, true);
            }

            // Translate so the ellipse is centered at the origin.
            x1 -= Cx;
            y1 -= Cy;
            x2 -= Cx;
            y2 -= Cy;

            var dx = x2 - x1;
            var dy = y2 - y1;
            var dxSqr = MathHelper.Sqr(dx);
            var dySqr = MathHelper.Sqr(dy);
            var rxSqr = MathHelper.Sqr(Rx);
            var rySqr = MathHelper.Sqr(Ry);

            // Calculate the quadratic parameters.
            float A = dxSqr / rxSqr +
                      dySqr / rySqr;
            float B = 2 * x1 * dx / rxSqr + 2 * y1 * dy / rySqr;
            float C = x1 * x1 / rxSqr + y1 * y1 / rySqr - 1f;

            // Calculate the discriminant.
            float discriminant = B * B - 4 * A * C;
            float t1 = -1, t2 = -1;

            bool intercepts = false;
            if (discriminant == 0)
            {
                // One real solution.
                t1 = (-B / 2 / A);
            }
            else if (discriminant > 0)
            {
                // Two real solutions.
                t1 = ((float)((-B + Math.Sqrt(discriminant)) / 2 / A));
                t2 = ((float)((-B - Math.Sqrt(discriminant)) / 2 / A));
            }
            if (t1 >= 0)
            {
                float ix1 = p1.X + (x2 - x1) * t1 + Cx;
                float iy1 = p1.Y + (y2 - y1) * t1 + Cy;
                iPt1 = new PointF(ix1, iy1);

                intercepts = true;
            }
            if (t2 >= 0f)
            {
                float ix1 = x1 + (x2 - x1) * t2 + Cx;
                float iy1 = y1 + (y2 - y1) * t2 + Cy;
                iPt2 = new PointF(ix1, iy1);

                intercepts = true;
            }
            if (!ignoreAngle && intercepts && angle.Valid)
            {
                iPt1 = angle.Rotate(iPt1);
                iPt2 = angle.Rotate(iPt2);
            }
            return intercepts;
        }

        public static bool EllipseInterception(this IEllipse e, PointF p1, PointF p2, out PointF iPt1, out PointF iPt2, bool ignoreAngle = false) =>
            e.Data.EllipseInterception(p1, p2, out iPt1, out iPt2, ignoreAngle);

        public static bool EllipseInterception(this EllipseData e, PointF p1, PointF p2, out PointF iPt1, out PointF iPt2, bool ignoreAngle = false) =>
            EllipseInterception(e.Rx, e.Ry, e.Cx, e.Cy, p1, p2, out iPt1, out iPt2, e.Angle, ignoreAngle);
        #endregion

        #region CHECK CURVE STEP
        static int GetCurveStep(bool WMajor, float angle, float Rx, float Ry)
        {
            if (angle > 360)
                angle %= 360;

            if (WMajor)
            {
                if (
                    MathHelper.IsWithIn(0, 23, angle) ||
                    MathHelper.IsWithIn(359, 359 - 23, angle) ||
                    MathHelper.IsWithIn(180 - 23, 180 + 23, angle))
                {
                    if (Rx < 20)
                        return 1;
                    else if (Rx > 200)
                        return 3;
                    return 4;
                }
            }
            else
            {
                if (
                    MathHelper.IsWithIn(90 - 23, 90 + 23, angle) ||
                    MathHelper.IsWithIn(270 - 23, 270 + 23, angle))
                {
                    if (Ry < 20)
                        return 1;
                    else if (Ry > 200)
                        return 3;
                    return 4;
                }
            }
            return Rx > 200 || Ry > 200 ? 3 : 6;
        }
        #endregion

        #region GET BEZIER POINTS
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBezierPoints(int multiplier, ref List<PointF> points, ICollection<PointF> pts)
        {
            IList<float> dataPoints = new float[pts.Count * 2];
            int g = 0;

            foreach (var item in pts)
            {
                dataPoints[g++] = item.X;
                dataPoints[g++] = item.Y;
            }
            GetBezierPoints(multiplier, dataPoints, ref points);
        }
        static float Bernstein(int n, int i, double t)
        {
            float basis;
            float ti; /* t^i */
            float tni; /* (1 - t)^i */

            if (t == 0.0 && i == 0)
                ti = 1.0f;
            else
                ti = (float)Math.Pow(t, i);

            if (n == i && t == 1.0f)
                tni = 1.0f;
            else
                tni = (float)Math.Pow((1 - t), (n - i));

            //Bernstein basis
            n = Math.Min(Math.Max(n, 0), 32);
            i = Math.Min(Math.Max(i, 0), 32);
            var ni = n - i;
            ni = Math.Min(Math.Max(n - i, 0), 32);
            basis = (Lookup[n] / (Lookup[i] * Lookup[n - i])) * ti * tni;
            return basis;
        }
        static void GetBezierPoints(int multiplier, IList<float> dataPoints, ref List<PointF> result)
        {
            var cpts = dataPoints.Count * multiplier;
            int npts = (dataPoints.Count) / 2;
            int icount, jcount;
            double step, t;
            var p = new float[cpts * 2];

            icount = 0;
            t = 0;
            step = 1.0 / (cpts - 1);

            for (var i = 0; i != cpts; i++)
            {
                if ((1.0 - t) < 5e-6)
                    t = 1.0;

                jcount = 0;
                p[icount] = 0.0f;
                p[icount + 1] = 0.0f;
                for (int j = 0; j != npts; j++)
                {
                    var basis = Bernstein(npts - 1, j, t);
                    var x = basis * dataPoints[jcount];
                    var y = basis * dataPoints[jcount + 1];

                    p[icount] += x;
                    p[icount + 1] += y;

                    jcount = jcount + 2;
                }

                icount += 2;
                t += step;
            }
            result.AddRange(p.ToPoints());
        }
        #endregion
    }

    //LINE HELPER
    partial class Geometry
    {
        #region VARAIBLES & CONSTS
        public const float LineEPSILON = .05f;
        #endregion

        #region CONTAINS
        public static bool ContainsY(this ILine l, float y) =>
            y >= l.MinY && y <= l.MaxY;
        public static bool ContainsX(this ILine l, float x) =>
            x >= l.MinX && x <= l.MaxX;
        #endregion

        #region ROTATE
        public static ILine Rotate(this ILine l, Angle angle) =>
            Factory.newLine(l.Start.X, l.Start.Y, l.End.X, l.End.Y, angle);
        public static ILine Rotate(this IXLine l, float angle) =>
            Factory.newLine(l, new Angle(angle));
        #endregion

        #region OFFSET & SHRINK
        public static ILine Offset(this ILine l, float offsetX, float offsetY) =>
            Factory.newLine(l.Start.X + offsetX, l.Start.Y + offsetY, l.End.X + offsetX, l.End.Y + offsetY);
        public static ILine Resize(this ILine l, float unit, bool byX = false)
        {
            float x1, y1, x2, y2;
            if (byX)
            {
                if (l.IsHorizontal)
                    return Factory.newLine(l.MinX - unit, l.Y1, l.MaxX + unit, l.Y1);

                x1 = l.MinX - unit;
                y1 = l.M * x1 + l.C;
                x2 = l.MaxX + unit;
                y2 = l.M * x2 + l.C;
                return Factory.newLine(x1, y1, x2, y2);
            }
            else
            {
                if (l.IsVertical)
                    return Factory.newLine(l.X1, l.MinY - unit, l.X1, l.MaxY + unit);

                y1 = l.MinY - unit;
                x1 = (y1 - l.C) / l.M;
                y2 = l.MaxY + unit;
                x2 = (y2 - l.C) / l.M;
                return Factory.newLine(x1, y1, x2, y2);
            }
        }
        #endregion

        #region MOVE
        public static ILine Move(this ILine l, float move)
        {
            var dist = (float)Math.Sqrt(l.DX * l.DX + l.DY * l.DY);
            var x = l.X1 + l.DX / dist * move;
            var y = l.Y1 + l.DY / dist * move;
            return Factory.newLine(x, y, l.End);
        }
        public static PointF MoveLine(PointF p, PointF q, float move)
        {
            var DX = q.X - p.X;
            var DY = q.Y - p.Y;
            var dist = (float)Math.Sqrt(DX * DX + DY * DY);
            var x = p.X + DX / dist * move;
            var y = p.Y + DY / dist * move;
            return new PointF(x, y);
        }
        #endregion

        #region PARALLEL
        /// <summary>
        /// Source Inspiration: https://stackoverflow.com/questions/2825412/draw-a-parallel-line Krumelur's answer
        /// </summary>
        /// <param name="lX1"></param>
        /// <param name="lY1"></param>
        /// <param name="lX2"></param>
        /// <param name="lY2"></param>
        /// <param name="deviation"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ParallelLine(float lX1, float lY1, float lX2, float lY2, float deviation, out float x1, out float y1, out float x2, out float y2)
        {
            x1 = lX1;
            y1 = lY1;
            x2 = lX2;
            y2 = lY2;

            if (deviation == 0)
                return;

            var w = x1 - x2;
            var h = y2 - y1;

            float length = (float)Math.Sqrt(MathHelper.Sqr(w) + MathHelper.Sqr(h));

            if (length == 0)
                length = 1;

            var dy = (deviation * h / length);
            var dx = (deviation * w / length);

            x1 += dy;
            x2 += dy;
            y1 += dx;
            y2 += dx;
        }
        public static ILine ParallelAt(this ILine l, float x, float y, bool oppositeDirection = false)
        {
            var dx = l.DX;
            var dy = l.DY;
            if (oppositeDirection)
            {
                dx = -dx;
                dy = -dy;
            }
            return Factory.newLine(x + dx, y + dy, x, y);
        }
        #endregion

        #region INTERSECTION
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this ILine first, ILine second, out PointF P, out bool isParallel)
        {
            bool ok = LineIntersection(first.Start, first.End, second.Start, second.End, out P, out isParallel);
            return ok;
        }
        /// <summary>
        /// Source Credit:https://rosettacode.org/wiki/Find_the_intersection_of_two_lines#C.23
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="e1"></param>
        /// <param name="s2"></param>
        /// <param name="e2"></param>
        /// <param name="P"></param>
        /// <param name="parallel"></param>
        /// <returns></returns>
        public static bool LineIntersection(PointF s1, PointF e1, PointF s2, PointF e2, out PointF P, out bool parallel)
        {
            parallel = false;
            float a1 = e1.Y - s1.Y;
            float b1 = s1.X - e1.X;
            float c1 = a1 * s1.X + b1 * s1.Y;

            float a2 = e2.Y - s2.Y;
            float b2 = s2.X - e2.X;
            float c2 = a2 * s2.X + b2 * s2.Y;

            float delta = a1 * b2 - a2 * b1;
            if (delta == 0)
            {
                parallel = true;
                P = new PointF(float.NaN, float.NaN);
                return false;
            }

            P = new PointF((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
            return true;
        }
        #endregion

        #region TO AREA
        public static IBoxF ToArea(this IEnumerable<ILine> lines)
        {

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = 0;
            float maxY = 0;

            foreach (var n in lines)
            {
                if (n == null)
                    continue;
                var x = Math.Min(n.Start.X, n.End.X);
                var y = Math.Min(n.Start.Y, n.End.Y);
                var r = Math.Max(n.Start.X, n.End.X);
                var b = Math.Max(n.Start.Y, n.End.Y);

                if (x < minX)
                    minX = x;

                if (y < minY)
                    minY = y;

                if (maxX < r)
                    maxX = r;

                if (maxY < b)
                    maxY = b;
            }

            if (minX == float.MaxValue || minY == float.MaxValue)
                return Factory.newBoxF(0, 0, 0, 0);

            return Factory.BoxFFromLTRB(minX, minY, maxX, maxY);
        }
        #endregion

        #region LINE DATA
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TrapeziumData GetTrapeziumData(this ILine line, float stroke, Angle angle = default(Angle), StrokeMode mode = StrokeMode.Middle,
                float parallelLineSizeDifference = 0)
        {
            if (float.IsNaN(line.M) || float.IsNaN(line.C))
            {
                return TrapeziumData.Empty;
            }


            ILine first = line.Clone();
            ILine second = line.Clone();

            if (mode == StrokeMode.Middle)
            {
                first = Factory.newLine(line, -stroke / 2f);
                second = Factory.newLine(line, stroke / 2f);
            }
            else if (mode == StrokeMode.Outer)
                second = Factory.newLine(line, stroke);
            else if (mode == StrokeMode.Inner)
                second = Factory.newLine(line, -stroke);

            var bounds = first.HybridBounds(second);
            angle = angle.AssignCenter(bounds);

            if (angle.Skew)
            {
                angle = new Angle(angle.Degree, angle.CX, angle.CY, angle.Skew, false);
                first = first.Rotate(angle);
                second = second.Rotate(angle);
            }
            else
            {
                angle = new Angle(angle.Degree, angle.CX, angle.CY, angle.Skew, true);
                stroke = mode == StrokeMode.Inner ? -stroke : stroke;
                first = first.Rotate(angle);
                second = Factory.newLine(first, stroke);
            }

            if (parallelLineSizeDifference != 0)
            {
                if (parallelLineSizeDifference >= stroke)
                    parallelLineSizeDifference = stroke - 1;
                second = first.Steep ? second.Offset(0, parallelLineSizeDifference) :
                    second.Offset(parallelLineSizeDifference, 0);

                bounds = first.HybridBounds(second);
            }

            var points = new PointF[] { first.Start, first.End, second.End, second.Start };
            return new TrapeziumData(points, angle, bounds);
        }

        public static bool GetDrawablePoints(this ILine l, out float x1, out float y1, out float x2, out float y2)
        {
            x1 = l.X1;
            y1 = l.Y1;
            x2 = l.X2;
            y2 = l.Y2;
            return GetDrawableLinePoints(ref x1, ref y1, ref x2, ref y2, l.M, l.C);
        }

        public static bool GetDrawableLinePoints(ref float x1, ref float y1, ref float x2, ref float y2,
            out float DeltaX, out float DeltaY, out float m, out float c)
        {
            DeltaX = DeltaY = m = c = 0;

            if (!RoundLineCoordinates(ref x1, ref y1, ref x2, ref y2, 4))
                return false;

            DeltaX = x2 - x1;
            DeltaY = y2 - y1;
            m = c = 0;
            m = DeltaY;
            if (DeltaX != 0)
                m /= DeltaX;
            c = y1 - m * x1;
            return GetDrawableLinePoints(ref x1, ref y1, ref x2, ref y2, m, c);
        }

        public static bool GetDrawableLinePoints(ref float x1, ref float y1, ref float x2, ref float y2,
            float m, float c)
        {
            if (!RoundLineCoordinates(ref x1, ref y1, ref x2, ref y2, 4))
                return false;

            if (x1 < 0)
            {
                x1 = 0;
                y1 = c;
                //if (y1 > y2)
                //    return false;
            }
            else if (x2 < 0)
            {
                x2 = 0;
                y2 = c;
                //if (y2 > y1)
                //    return false;
            }
            if (y1 < 0)
            {
                y1 = 0;
                x1 = (-c) / m;
                //if (x1 > x2)
                //    return false;
            }
            if (y2 < 0)
            {
                y2 = 0;
                x2 = (-c) / m;
                //if (x2 > x1)
                //    return false;
            }
            return true;
        }

        public static bool RoundLineCoordinates(ref float x1, ref float y1, ref float x2, ref float y2, int digits = 4)
        {
            if (float.IsNaN(x1) || float.IsNaN(y1) || float.IsNaN(x2) || float.IsNaN(y2) || ((x1 < 0 && x2 < 0) || (y1 < 0 && y2 < 0)))
                return false;
            if (x1 == x2)
            {
                x1 = x2 = x1.Round();
                return true;
            }
            //if (y1 == y2)
            //{
            //    y1 = y2 = y1.Floor();
            //    return true;
            //}
            x1 = (float)Math.Round(x1, digits);
            y1 = (float)Math.Round(y1, digits);
            x2 = (float)Math.Round(x2, digits);
            y2 = (float)Math.Round(y2, digits);
            return true;
        }
        #endregion

        #region ANGLE BETWEEN 2 LINES
        public static Angle AngleFrom(this ILine l1, ILine l2)
        {
            float theta1 = (float)Math.Atan2(l1.DY, l1.DX);
            float theta2 = (float)Math.Atan2(l2.DY, l2.DX);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            return new Angle(angle, l1.Bounds);
        }
        public static Angle AngleFromVerticalCounterPart(this ILine l)
        {
            float theta1 = (float)Math.Atan2(l.DY, l.DX);
            float theta2 = (float)Math.Atan2(l.DY, 0);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            return new Angle(angle, l.Bounds);
        }
        public static Angle AngleFromHorizontalCounterPart(this ILine l)
        {
            float theta1 = (float)Math.Atan2(l.DY, l.DX);
            float theta2 = (float)Math.Atan2(0, l.DX);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            return new Angle(angle, l.Bounds);
        }
        #endregion

        #region > OR < THAN POINT
        public static bool IsGreaterThan(this ILine l, float x, float y)
        {
            var c1 = (l.DX) * (y - l.Y1);
            var c2 = (l.DY) * (x - l.X1);
            //if (Math.Abs(c1 - c2) < 0.01f)
            //    return true;
            return c1 < c2;
        }
        public static bool IsLessThan(this ILine l, float x, float y)
        {
            var c1 = (l.DX) * (y - l.Y1);
            var c2 = (l.DY) * (x - l.X1);
            //if (Math.Abs(c1 - c2) < 0.01f)
            //    return true;
            return c1 > c2;
        }
        #endregion

        #region ADD VAL SAFE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddValSafe(this ILine l, float axis, bool horizontal, ref ICollection<float> list)
        {

            if (!l.IsValid || l.IsPoint)
                return;

            if (horizontal)
            {
                if (l.IsHorizontal)
                {
                    return;
                }

                if (axis < l.MinY || axis > l.MaxY)
                    return;
                //if (axis <= l.MinY.Round() || axis >= l.MaxY.Round())
                //    return;
                var x = ((axis - l.C) / l.M).RoundF(4);
                list.Add(x);
            }
            else
            {
                if (l.IsVertical)
                    return;

                //if (axis <= l.MinX.Round() || axis >= l.MaxX.Round())
                //    return;
                list.Add(l.M * axis + l.C);
            }
            return;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HandleAxisLine(this ILine l, float axis, bool horizontal, ref ICollection<float> list)
        {
            if (list.Count % 2 == 0)
                return;

            if (horizontal)
            {
                if (!l.IsHorizontal)
                    return;

                if (l.Y1.Round() == axis)
                    list.Add(l.X1);

            }
            else
            {
                if (!l.IsVertical)
                    return;
                if (l.X1.Round() == axis)
                    list.Add(l.Y1);
            }
        }
        #endregion

        #region DISTANCE BETWEEN PARALLEL LINES

        #endregion
    }

    //POINT HELPER
    partial class Geometry
    {
        #region TO AREA
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRectangle ToArea(this IEnumerable<Point> points)
        {
            points.MinMaxX(out Point minX, out Point maxX);
            points.MinMaxY(out Point minY, out Point maxY);

            var x1 = minX.X;
            var y1 = minY.Y;

            var x2 = maxX.X;
            var y2 = maxY.Y;

            var width = Math.Abs(x2 - x1);
            var height = Math.Abs(y2 - y1);

            return Factory.newRectangle(x1, y1, ++width, ++height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRectangleF ToArea(this IEnumerable<PointF> points)
        {
            points.MinMaxX(out PointF minX, out PointF maxX);
            points.MinMaxY(out PointF minY, out PointF maxY);

            var x1 = minX.X;
            var y1 = minY.Y;

            var x2 = maxX.X;
            var y2 = maxY.Y;

            var width = Math.Abs(x2 - x1);
            var height = Math.Abs(y2 - y1);

            return Factory.newRectangleF(x1, y1, ++width, ++height);
        }
        #endregion

        #region MIN MAX - AVG OF COLLECTION
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMaxY(this IEnumerable<Point> collection, out Point min, out Point max)
        {
            min = max = Point.Empty;
            foreach (var p in collection)
            {
                if (!p.Valid)
                    continue;
                if (!min.Valid)
                    min = new Point(p);
                else if (p.Y < min.Y)
                    min = new Point(p);

                if (!max.Valid)
                    max = new Point(p);
                else if (p.Y > max.Y)
                    max = new Point(p);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMaxY(this IEnumerable<PointF> collection, out PointF min, out PointF max)
        {
            min = max = PointF.Empty;
            foreach (var p in collection)
            {
                if (!p.Valid)
                    continue;
                if (!min.Valid)
                    min = new PointF(p);
                else if (p.Y < min.Y)
                    min = new PointF(p);

                if (!max.Valid)
                    max = new PointF(p);
                else if (p.Y > max.Y)
                    max = new PointF(p);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMaxX(this IEnumerable<Point> collection, out Point min, out Point max)
        {
            min = max = Point.Empty;
            foreach (var p in collection)
            {
                if (!p.Valid)
                    continue;
                if (!min.Valid)
                    min = new Point(p);
                else if (p.X < min.X)
                    min = new Point(p);

                if (!max.Valid)
                    max = new Point(p);
                else if (p.X > max.X)
                    max = new Point(p);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMaxX(this IEnumerable<PointF> collection, out PointF min, out PointF max)
        {
            min = max = PointF.Empty;
            foreach (var p in collection)
            {
                if (!p.Valid)
                    continue;
                if (!min.Valid)
                    min = new PointF(p);
                else if (p.X < min.X)
                    min = new PointF(p);

                if (!max.Valid)
                    max = new PointF(p);
                else if (p.X > max.X)
                    max = new PointF(p);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF AvgXY(this IEnumerable<PointF> collection)
        {
            float x = 0, y = 0;
            int i = -1;

            foreach (var p in collection)
            {
                if (!p.Valid)
                    continue;
                ++i;
                x += p.X;
                y += p.Y;
            }
            return new PointF(x / i, y / i);
        }
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF AvgXY(this IEnumerable<Point> collection)
        {
            float x = 0, y = 0;
            int i = -1;

            foreach (var p in collection)
            {
                if (!p.Valid)
                    continue;
                ++i;
                x += p.X;
                y += p.Y;
            }
            return new PointF(x / i, y / i);
        }
        #endregion

        #region ROTATION
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<PointF> Rotate(this IEnumerable<PointF> Source, Angle angle)
        {
            if (angle.Valid)
                return Source.Select(x => angle.Rotate(x)).ToArray();

            if (Source is IList<PointF>)
                return Source as IList<PointF>;
            return Source.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<PointF> Rotate(this IEnumerable<Point> Source, Angle angle)
        {
            if (angle.Valid)
                return Source.Select(x => angle.Rotate(x)).ToArray();
            return Source.Select(x => new PointF(x)).ToArray();
        }
        #endregion

        #region ANGLE BETWEEN 2 Points
        public static Angle AngleFromVerticalCounterPart(this PointF p, PointF q)
        {
            var DY = p.Y - q.Y;
            var DX = p.X - q.X;
            float theta1 = (float)Math.Atan2(DY, DX);
            float theta2 = (float)Math.Atan2(DY, 0);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            var Bounds = Factory.newRectangleF(p, q);
            return new Angle(angle, Bounds);
        }
        public static Angle AngleFromHorizontalCounterPart(this PointF p, PointF q)
        {
            var DY = p.Y - q.Y;
            var DX = p.X - q.X;
            float theta1 = (float)Math.Atan2(DY, DX);
            float theta2 = (float)Math.Atan2(0, DX);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            var Bounds = Factory.newRectangleF(p, q);
            return new Angle(angle, Bounds);
        }

        public static Angle AngleFromVerticalCounterPart(this Point p, Point q)
        {
            var DY = p.Y - q.Y;
            var DX = p.X - q.X;
            float theta1 = (float)Math.Atan2(DY, DX);
            float theta2 = (float)Math.Atan2(DY, 0);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            var Bounds = Factory.newRectangleF(p, q);
            return new Angle(angle, Bounds);
        }
        public static Angle AngleFromHorizontalCounterPart(this Point p, Point q)
        {
            var DY = p.Y - q.Y;
            var DX = p.X - q.X;
            float theta1 = (float)Math.Atan2(DY, DX);
            float theta2 = (float)Math.Atan2(0, DX);
            var angle = Math.Abs(theta1 - theta2) * RadianInv;
            var Bounds = Factory.newRectangleF(p, q);
            return new Angle(angle, Bounds);
        }
        #endregion

        #region TO POINTS
        public static List<PointF> ToPointsF(this IEnumerable<int> xyPairs)
        {
            if (xyPairs == null)
                return new List<PointF>();
            List<PointF> points;
            var len = xyPairs.Count();
            var previous = -1;
            points = new List<PointF>(len);
            foreach (var item in xyPairs)
            {
                if (previous == -1)
                    previous = item;
                else
                {
                    points.Add(new PointF(previous, item));
                    previous = -1;
                }
            }
            return points;
        }
        public static List<PointF> ToPoints(this IEnumerable<float> xyPairs)
        {
            if (xyPairs == null)
                return new List<PointF>();

            List<PointF> points;
            var len = xyPairs.Count();
            var previous = -1f;

            points = new List<PointF>(len);
            foreach (var item in xyPairs)
            {
                if (previous == -1)
                    previous = item;
                else
                {
                    points.Add(new PointF(previous, item));
                    previous = -1;
                }
            }

            return points;
        }
        #endregion

        #region GET CENTER
        public static PointF FindCenterFrom(this PointF p, PointF q)
        {
            var cx = Math.Min(p.X, q.X) + Math.Abs(q.X - p.X) / 2;
            var cy = Math.Min(p.Y, q.Y) + Math.Abs(q.Y - p.Y) / 2;
            return new PointF(cx, cy);
        }
        public static PointF FindCenterFrom(this Point p, Point q)
        {
            var cx = Math.Min(p.X, q.X) + Math.Abs(q.X - p.X) / 2f;
            var cy = Math.Min(p.Y, q.Y) + Math.Abs(q.Y - p.Y) / 2f;
            return new PointF(cx, cy);
        }
        #endregion

        #region TO LINES
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<ILine> ToLines(this IEnumerable<PointF> data, PointJoin join, float stroke = 0, Angle angle = default(Angle))
        {
            int count;
            if (data is ICollection<PointF>)
                count = (data as ICollection<PointF>).Count;
            else if (data is IReadOnlyList)
                count = (data as IReadOnlyList).Count;
            else
                count = data.Count();

            var lines = new List<ILine>(count / 2 + 1);
            bool connectEach = join.HasFlag(PointJoin.ConnectEach);
            bool unique = join.HasFlag(PointJoin.NoRepeat);
            bool joinEnds = join.HasFlag(PointJoin.ConnectEnds);
            bool noTooClose = join.HasFlag(PointJoin.AvoidTooClose);

            PointF p0, p1, first;
            p0 = p1 = first = PointF.Empty;

            foreach (var item in data)
            {
                if (!item.Valid)
                    continue;

                p1 = item;

                if (!first.Valid)
                    first = p1;

                if (!p0.Valid)
                {
                    p0 = p1;
                    continue;
                }
                if (unique && p1.Equals(p0))
                    continue;

                if (noTooClose)
                {
                    var distance = MathHelper.Sqr(p0.X - p1.X) + MathHelper.Sqr(p0.Y - p1.Y);
                    if (distance < 1.5f)
                        continue;
                }
                lines.Add(Factory.newLine(p0, p1, angle, stroke));

                if (connectEach)
                    p0 = p1;
                else
                    p0 = PointF.Empty;
            }

            if (connectEach && joinEnds && !Equals(p1, first))
            {
                var line = Factory.newLine(p1, first, angle, stroke);
                lines.Add(line);
            }

            if (join.HasFlag(PointJoin.RemoveLast))
                lines.RemoveAt(lines.Count - 1);

            return lines;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<ILine> ToLines(this IEnumerable<Point> data, PointJoin join, float stroke = 0, Angle angle = default(Angle))
        {
            int count;
            if (data is ICollection<Point>)
                count = (data as ICollection<Point>).Count;
            else if (data is IReadOnlyList)
                count = (data as IReadOnlyList).Count;
            else
                count = data.Count();

            var lines = new List<ILine>(count / 2 + 1);
            bool connectEach = join.HasFlag(PointJoin.ConnectEach);
            bool unique = join.HasFlag(PointJoin.NoRepeat);
            bool joinEnds = join.HasFlag(PointJoin.ConnectEnds);
            bool noTooClose = join.HasFlag(PointJoin.AvoidTooClose);

            Point p0, p1, first;
            p0 = p1 = first = Point.Empty;

            foreach (var item in data)
            {
                if (!item.Valid)
                    continue;

                p1 = item;

                if (!first.Valid)
                    first = p1;

                if (!p0.Valid)
                {
                    p0 = p1;
                    continue;
                }
                if (unique && p1.Equals(p0))
                    continue;

                if (noTooClose)
                {
                    var distance = MathHelper.Sqr(p0.X - p1.X) + MathHelper.Sqr(p0.Y - p1.Y);
                    if (distance < 1.5f)
                        continue;
                }
                lines.Add(Factory.newLine(p0, p1, angle, stroke));

                if (connectEach)
                    p0 = p1;
                else
                    p0 = Point.Empty;
            }

            if (connectEach && joinEnds && !Equals(p1, first))
            {
                var line = Factory.newLine(p1, first, angle, stroke);
                lines.Add(line);
            }

            if (join.HasFlag(PointJoin.RemoveLast))
                lines.RemoveAt(lines.Count - 1);

            return lines;
        }
        #endregion

        #region STROKING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pair<IList<PointF>, IList<PointF>> StrokePoints(this IEnumerable<PointF> points, string shapeType, float stroke, StrokeMode mode)
        {
            var afterStroke = Renderer.GetAfterStroke(shapeType);
            var join = Renderer.GetStrokeJoin(shapeType);

            IList<PointF> outerP;
            IList<PointF> innerP;

            bool reset1st = afterStroke.HasFlag(AfterStroke.Reset1st);
            if (mode == StrokeMode.Middle)
            {
                outerP = StrokePoints(shapeType, points, ((stroke) / 2f), join, reset1st);
                innerP = StrokePoints(shapeType, points, -((stroke) / 2f), join, reset1st);
            }
            else if (mode == StrokeMode.Inner)
            {
                innerP = StrokePoints(shapeType, points, -stroke, join, reset1st);
                outerP = points.ToArray();
            }
            else
            {
                outerP = StrokePoints(shapeType, points, stroke, join, reset1st);
                innerP = points.ToArray();
            }
            return Pair.Create(outerP, innerP);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<PointF> StrokePoints(string shapeType, IEnumerable<PointF> source, float stroke,
            PointJoin join = 0, bool reset0 = true, Angle angle = default(Angle))
        {
            var lines = ToLines(source, PointJoin.ConnectEach | join | PointJoin.NoRepeat, stroke);
            if (lines.Count == 1)
                return new PointF[] { lines[0].Start, lines[0].End };

            List<PointF> points = new List<PointF>(lines.Count / 2);
            ILine first = null, last = null;
            PointF P;
            bool isParallel;

            foreach (var l in lines)
            {
                if (l == null)
                    continue;

                if (first == null)
                {
                    first = l;
                    last = l;
                    points.Add(last.Start);
                    continue;
                }

                if (last.Intersects(l, out P, out isParallel))
                    points.Add(P);

                else if (isParallel)
                    points.Add(last.Start);

                last = l;
            }

            if (reset0 && first != null && last != null)
            {
                if (last.Intersects(first, out P, out isParallel))
                    points[0] = P;
                else if (isParallel)
                    points.Add(last.Start);
            }
            else if (!reset0 && last != null)
            {
                points.Add(last.End);
            }

            return points;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetLines(string Name, IList<PointF> Outer, IList<PointF> Inner, float stroke,
            out IList<ILine> outerLines, out IList<ILine> innerLines, bool avoidTooClose = false, bool dontCloseEnds = false)
        {
            var join = PointJoin.ConnectEach | Renderer.GetStrokeJoin(Name);
            if (avoidTooClose)
                join |= PointJoin.AvoidTooClose;

            if (stroke == 0)
            {
                outerLines = Geometry.ToLines(Outer, join);
                innerLines = null;
                return;
            }
            bool close = Renderer.GetAfterStroke(Name).HasFlag(AfterStroke.JoinEnds);

            outerLines = Geometry.ToLines(Outer, join);
            innerLines = Geometry.ToLines(Inner, join);

            if (close && !dontCloseEnds)
            {
                var close1 = Factory.newLine(outerLines[0].Start, innerLines[0].Start);
                var close2 = Factory.newLine(outerLines[outerLines.Count - 1].End, innerLines[innerLines.Count - 1].End);
                innerLines.Add(close1);
                outerLines.Add(close2);
            }
            if (Renderer.NoeedToSwapPerimeters(Name))
                MathHelper.Swap(ref outerLines, ref innerLines);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pair<IList<PointF>, IList<PointF>> StrokePoints(this IEnumerable<Point> points, string shapeType, float stroke, StrokeMode mode)
        {
            var afterStroke = Renderer.GetAfterStroke(shapeType);
            var join = Renderer.GetStrokeJoin(shapeType);

            IList<PointF> outerP;
            IList<PointF> innerP;

            bool reset1st = afterStroke.HasFlag(AfterStroke.Reset1st);
            if (mode == StrokeMode.Middle)
            {
                outerP = StrokePoints(shapeType, points, ((stroke) / 2f), join, reset1st);
                innerP = StrokePoints(shapeType, points, -((stroke) / 2f), join, reset1st);
            }
            else if (mode == StrokeMode.Inner)
            {
                innerP = StrokePoints(shapeType, points, -stroke, join, reset1st);
                outerP = points.Select(x => new PointF(x)).ToArray();
            }
            else
            {
                outerP = StrokePoints(shapeType, points, stroke, join, reset1st);
                innerP = points.Select(x => new PointF(x)).ToArray();
            }
            return Pair.Create(outerP, innerP);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<PointF> StrokePoints(string shapeType, IEnumerable<Point> source, float stroke,
            PointJoin join = 0, bool reset0 = true, Angle angle = default(Angle))
        {
            var lines = ToLines(source, PointJoin.ConnectEach | join | PointJoin.NoRepeat, stroke);
            if (lines.Count == 1)
                return new PointF[] { lines[0].Start, lines[0].End };

            List<PointF> points = new List<PointF>(lines.Count / 2);
            ILine first = null, last = null;
            PointF P;
            bool isParallel;

            foreach (var l in lines)
            {
                if (l == null)
                    continue;

                if (first == null)
                {
                    first = l;
                    last = l;
                    points.Add(last.Start);
                    continue;
                }

                if (last.Intersects(l, out P, out isParallel))
                    points.Add(P);

                else if (isParallel)
                    points.Add(last.Start);

                last = l;
            }

            if (reset0 && first != null && last != null)
            {
                if (last.Intersects(first, out P, out isParallel))
                    points[0] = P;
                else if (isParallel)
                    points.Add(last.Start);
            }
            else if (!reset0 && last != null)
            {
                points.Add(last.End);
            }

            return points;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetLines(string Name, IList<Point> Outer, IList<Point> Inner, float stroke,
            out IList<ILine> outerLines, out IList<ILine> innerLines, bool avoidTooClose = false, bool dontCloseEnds = false)
        {
            var join = PointJoin.ConnectEach | Renderer.GetStrokeJoin(Name);
            if (avoidTooClose)
                join |= PointJoin.AvoidTooClose;

            if (stroke == 0)
            {
                outerLines = Geometry.ToLines(Outer, join);
                innerLines = null;
                return;
            }
            bool close = Renderer.GetAfterStroke(Name).HasFlag(AfterStroke.JoinEnds);

            outerLines = Geometry.ToLines(Outer, join);
            innerLines = Geometry.ToLines(Inner, join);

            if (close && !dontCloseEnds)
            {
                var close1 = Factory.newLine(outerLines[0].Start, innerLines[0].Start);
                var close2 = Factory.newLine(outerLines[outerLines.Count - 1].End, innerLines[innerLines.Count - 1].End);
                innerLines.Add(close1);
                outerLines.Add(close2);
            }
            if (Renderer.NoeedToSwapPerimeters(Name))
                MathHelper.Swap(ref outerLines, ref innerLines);
        }
        #endregion

        #region INTERPOLATE
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static bool Interpolate(this IPixelReader reader, float val, float axis, bool horizontal, int srcColor, out int index, out int color)
        //{
        //    index = -1;
        //    color = 0;
        //    var angle = reader.Angle;
        //    var  x = horizontal ? val : axis;
        //    var  y = horizontal ? axis : val;
        //    x -= angle.CX;
        //    y = angle.CY - y;

        //    var fPolarAngle = -angle.Radian;
        //    SinCos(fPolarAngle, out float sin, out float cos);

        //    var fDistance = (float)Math.Sqrt(x * x + y * y);

        //    var fTrueX = (float)(fDistance * Math.Cos(fPolarAngle));
        //    var fTrueY = (float)(fDistance * Math.Sin(fPolarAngle));

        //    // convert Cartesian to raster
        //    fTrueX = fTrueX + angle.CX;
        //    fTrueY = angle.CY - fTrueY;

        //    var iFloorX = (int)(Math.Floor(fTrueX));
        //    var iFloorY = (int)(Math.Floor(fTrueY));
        //    var iCeilingX = (int)(Math.Ceiling(fTrueX));
        //    var iCeilingY = (int)(Math.Ceiling(fTrueY));

        //    // check bounds
        //    if (iFloorX < 0 || iCeilingX < 0 || iFloorY < 0 ||
        //        iCeilingY < 0) 
        //        return false;

        //    var fDeltaX = fTrueX - iFloorX;
        //    var fDeltaY = fTrueY - iFloorY;

        //    var clrTopLeft = reader.ReadPixel(iFloorY * reader.Width + iFloorX);
        //    var clrTopRight = oldp[iFloorY * oldw + iCeilingX];
        //    var clrBottomLeft = oldp[iCeilingY * oldw + iFloorX];
        //    var clrBottomRight = oldp[iCeilingY * oldw + iCeilingX];

        //    newp[i * newWidth + j] = Colors.Blend(clrTopLeft, clrTopRight, clrBottomLeft, clrBottomRight, fDeltaX, fDeltaY);

        //}

        #endregion
    }
}

