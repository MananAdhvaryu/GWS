
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MnM.GWS
{
    public static partial class Implementation
    {
        #region VARIABLES - CONSTS
        #region CONSTS
        public const StringComparison NoCase = StringComparison.CurrentCultureIgnoreCase;
        const int MaxAnsiCode = 255;
        public const float BrushOk = -0.5f;
        public const string ImplementedInAdvanceVersionOnly = "Sorry this is only implemented  the Advanced version! For more information - visit www.mnminfotech.co.uk";
        //public static float MouseScale = 1.0f;
        #endregion

        #region EVENT STORAGE
#if Window
        static volatile bool EventsRunning = false;
#endif

        #endregion

        #region BENCHMARKING
        static readonly Stopwatch stopWatch = new Stopwatch();
        #endregion
        #endregion

        #region EXTERNAL LIBRARY NAMES
#if IPHONE
        public const string libSDL ="__Internal";
        public const string libTTF = "__Internal";
        public const string libFT = "__Internal";

#elif LINUX || ANDROID
        public const string libSDL = "libSDL2.so";
        public const string libTTF = "libSDL2_ttf.so";
        public const string libFT = "libfreetype-6.so";

#elif OSX
        public const string libSDL = "libSDL2.dylib";
        public const string libTTF = "libTTF.dylib";
        public const string libFT = "libfreetype-6.dylib";
#else
        public const string libSDL = "SDL2.dll";
        public const string libTTF = "SDL2_ttf.dll";
        public const string libFT = "libfreetype-6.dll";
        //internal const string libFT = "freetype.dll";
#endif
        #endregion

        #region CONSTRUCTORS
        static Implementation()
        {
        }
        #endregion

        #region ATTACH CONTEXT
        /// <summary>
        /// Attach context such as Drawing or Window Context to the Instance.
        /// Order of Attachment should be...
        /// First DrawingContext.
        /// Second WindowContext.
        /// Then rest of the contexts;
        /// </summary>
        /// <param name="attachment"></param>
        public static void Attach(IAttachment attachment)
        {
            if (attachment == null)
                return;

            if (attachment is IFactory)
                Reset(attachment as IFactory);
        }
        static void Reset(IFactory factory)
        {
            if (factory == null)
                return;
            Factory?.Dispose();
#if Window
            Factory = factory as IWindowFactory;
#else
            Factory = factory;
#endif
            Renderer = Factory.newRenderer();
            FloodFill = Factory.newFloodFill();

            FillStyles.Initialize();
        }
        #endregion

        #region PROPERTIES
        public static IRenderer Renderer { get; private set; }
        public static IFloodFill FloodFill { get; private set; }

#if Window
        public static IWindowFactory Factory { get; private set; }
#else
        public static IFactory Factory { get; private set; }
#endif
        #endregion

        #region RUN - QUIT - HALT - RESUME
#if Window
        public static void Run()
        {
            if (EventsRunning)
                return;
            EventsRunning = true;
            while (EventsRunning && Factory.CountOf(ObjType.Window) > 0)
            {
                Factory.PumpEvents();
                while (Factory.PollEvent(out IEvent e))
                {
                    if (Factory.Get(e.ID, out IEventProcessor window, ObjType.Window))
                    {
                        if (!window.ProcessEvent(e))
                            continue;
                    }
                }
                if (Factory.CountOf(ObjType.Timer) > 0)
                {
                    var timers = Factory.GetAll<ITimer>(t => t.Due, ObjType.Timer);
                    foreach (var item in timers)
                        item.FireEvent();
                }
            }
            Quit();
        }
#endif
        public static void Quit()
        {
#if Window
            EventsRunning = false;
#endif
            Factory?.Dispose();
            Factory = null;
            Renderer?.Dispose();
            Renderer = null;
        }
        #endregion

        #region BENCHMARK
        public static string BenchMark(VoidMethod method, out long executionTime, string description = "Method Execution", BenchmarkUnit unit = 0)
        {
            executionTime = 0;
            if (method == null)
                return "No method present";
            stopWatch.Reset();
            stopWatch.Start();
            method();
            stopWatch.Stop();
            switch (unit)
            {
                case BenchmarkUnit.MilliSecond:
                default:
                    executionTime = stopWatch.ElapsedMilliseconds;
                    break;
                case BenchmarkUnit.MicroSecond:
                    executionTime = stopWatch.ElapsedMilliseconds * 1000;
                    break;
                case BenchmarkUnit.Tick:
                    executionTime = stopWatch.ElapsedTicks;
                    break;
                case BenchmarkUnit.Second:
                    executionTime = stopWatch.ElapsedMilliseconds / 1000;
                    break;
            }
            return description + "  takes: " + executionTime + " " + unit.ToString() + "s";
        }
        public static string BenchMark(VoidMethod method, string description = "Method Execution", BenchmarkUnit unit = 0) =>
            BenchMark(method, out long i, description, unit);
        public static string BenchMark<T>(ReturnMethod<T> method, out long executionTime, out T value, string description = "Method Execution", BenchmarkUnit unit = 0)
        {
            stopWatch.Reset();
            stopWatch.Start();
            value = method();
            stopWatch.Stop();
            switch (unit)
            {
                case BenchmarkUnit.MilliSecond:
                default:
                    executionTime = stopWatch.ElapsedMilliseconds;
                    break;
                case BenchmarkUnit.Tick:
                    executionTime = stopWatch.ElapsedTicks;
                    break;
                case BenchmarkUnit.Second:
                    executionTime = stopWatch.ElapsedMilliseconds / 1000;
                    break;
            }
            return description + " takes: " + executionTime + " " + unit.ToString();
        }
        public static T BenchMark<T>(ReturnMethod<T> method, out long executionTime, out string message, string description = "Method Execution", BenchmarkUnit unit = 0)
        {
            stopWatch.Reset();
            stopWatch.Start();
            T value = method();
            stopWatch.Stop();
            switch (unit)
            {
                case BenchmarkUnit.MilliSecond:
                default:
                    executionTime = stopWatch.ElapsedMilliseconds;
                    break;
                case BenchmarkUnit.Tick:
                    executionTime = stopWatch.ElapsedTicks;
                    break;
                case BenchmarkUnit.Second:
                    executionTime = stopWatch.ElapsedMilliseconds / 1000;
                    break;
            }
            message = description + " takes: " + executionTime + " " + unit.ToString();
            return value;
        }
        public static T BenchMark<T>(ReturnMethod<T> method, out string message, string description = "Method Execution", BenchmarkUnit unit = 0)
        {
            long executionTime = 0;
            stopWatch.Reset();
            stopWatch.Start();
            T value = method();
            stopWatch.Stop();
            switch (unit)
            {
                case BenchmarkUnit.MilliSecond:
                default:
                    executionTime = stopWatch.ElapsedMilliseconds;
                    break;
                case BenchmarkUnit.Tick:
                    executionTime = stopWatch.ElapsedTicks;
                    break;
                case BenchmarkUnit.Second:
                    executionTime = stopWatch.ElapsedMilliseconds / 1000;
                    break;
            }
            message = description + " takes: " + executionTime + " " + unit.ToString();
            return value;
        }
        #endregion

        #region FONT
        public static byte[] FontFromFile(string path)
        {
            if (!File.Exists(path))
                return null;
            var name = System.IO.Path.GetFileName(path);
            return File.ReadAllBytes(path);
        }
        #endregion

        #region HASH HELPER
        // Licensed to the.NET Foundation under one or more agreements.
        // The .NET Foundation licenses this file to you under the MIT license.
        // See the LICENSE file  the project root for more information.
        public static int Combine(int h1, int h2)
        {
            // RyuJIT optimizes this to use the ROL instruction
            // Related GitHub pull request: dotnet/coreclr#1830
            uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
            return ((int)rol5 + h1) ^ h2;
        }
        #endregion

        #region STRUCT TO PTR
        public static IntPtr ToPtr<T>(this T structure)
        {
            if (structure == null)
                return IntPtr.Zero;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(structure, ptr, false);
            return ptr;
        }
        public static T ToStruct<T>(this IntPtr ptr)
        {
            return Marshal.PtrToStructure<T>(ptr);
        }
        public static void FreePtr(this IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
        #endregion

        #region UTF8
        public unsafe static IntPtr AllocUTF8(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return IntPtr.Zero;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str + '\0');
            fixed (byte* b = bytes)
            {
                return new IntPtr(b);
            }
        }
        public static byte[] UTF8_ToNative(this string s)
        {
            if (s == null)
                return null;
            return System.Text.Encoding.UTF8.GetBytes(s + '\0');
        }
        public static unsafe string UTF8_ToManaged(this IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero)
                return null;
            byte* ptr = (byte*)s;
            while (*ptr != 0)
                ptr++;

            byte[] bytes = new byte[ptr - (byte*)s];
            Marshal.Copy(s, bytes, 0, bytes.Length);
            string result = System.Text.Encoding.UTF8.GetString(bytes);
            return result;
        }
        public static int ToUnicode(this char character)
        {
            UTF32Encoding encoding = new UTF32Encoding();
            byte[] bytes = encoding.GetBytes(character.ToString().ToCharArray());
            return BitConverter.ToInt32(bytes, 0);
        }
        public static string IntPtrToString(this IntPtr ptr) =>
            Marshal.PtrToStringAnsi(ptr);
        public static bool ContainsUnicodeCharacter(this string input) =>
            input.Any(c => c > MaxAnsiCode);
        #endregion

        #region BLOCK COPY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IRectangle CopyBlock(int srcLen, IRectangle copyRc, int srcW, int destX, int destY, int destW, int destLen, BlockCopy action)
        {
            var newWidth = copyRc.Width;
            var newHeight = copyRc.Height;
            var x = copyRc.X;
            var y = copyRc.Y;
            if (x < 0)
            {
                newWidth += x;
                x = 0;
            }
            if (y < 0)
            {
                newHeight += y;
                y = 0;
            }
            var srcIndex = x + y * srcW;

            if (destX < 0)
                destX = 0;
            if (destY < 0)
                destY = 0;

            var destIndex = destX + destY * destW;

            var copyWidth = Math.Min(newWidth, srcW);
            var copyHeight = Math.Min(newHeight, srcLen / srcW);

            if (srcIndex + copyWidth >= srcLen)
                copyWidth -= (srcIndex + copyWidth - srcLen);

            if (copyWidth <= 0)
                return Factory.RectEmpty;

            if (destIndex + copyWidth >= destLen)
                copyWidth -= (destIndex + copyWidth - destLen);
            if (copyWidth <= 0)
                return Factory.RectEmpty;

            var rc = Factory.newRectangle(destX, destY, copyWidth, copyHeight);

            for (int i = 0; i < copyHeight; i++)
            {
                if (srcIndex + copyWidth >= srcLen)
                    copyWidth -= (srcIndex + copyWidth - srcLen);
                if (copyWidth <= 0)
                    break;

                if (destIndex + copyWidth >= destLen)
                    copyWidth -= (destIndex + copyWidth - destLen);

                if (copyWidth <= 0)
                    break;

                action(srcIndex, destIndex, copyWidth, i);
                srcIndex += srcW;
                destIndex += destW;
            }
            return rc;
        }
        #endregion

        #region COPY TO
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo(this IBuffer pixelSrc, IRectangle copyRc, int[] destination, int destW, int destX, int destY)
        {
            fixed (int* dest = destination)
            {
                _ = pixelSrc.CopyTo(copyRc, (IntPtr)dest, destination.Length, destW, destX, destY);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo(this IBuffer pixelSrc, IBuffer destination, IRectangle copyRc, int destX, int destY)
        {
            _ = pixelSrc.CopyTo(copyRc, destination.Pixels, destination.Length, destination.Width, destX, destY);

        }
        #endregion

        #region COPY MEMORY TO
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyMemoryTo(this IntPtr source, IntPtr dest, IRectangle srcCopyRc, int srcLen, int srcW, int destX, int destY, int destW, int destLen)
        {
            CopyBlock(srcLen, srcCopyRc, srcW, destX, destY, destW, destLen,
                (si, di, w, i) => CopyMemory(source, si, dest, di, w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyMemoryTo(this IBuffer source, IBuffer dest, IRectangle srcCopyRc, int destX, int destY)
        {
            CopyBlock(source.Length, srcCopyRc, source.Width, destX, destY, dest.Width, dest.Length,
                (si, di, w, i) => CopyMemory((int*)source.Pixels, si, (int*)dest.Pixels, di, w));
        }
        #endregion

        #region COPY MEMORY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyMemory(int* src, int srcIndex, int* dst, int destIndex, int length) =>
            Renderer.CopyMemory(src, srcIndex, dst, destIndex, length);
             
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyMemory(int[] source, int srcIndex, int* dst, int destIndex, int length)
        {
            fixed (int* src = source)
            {
                Renderer.CopyMemory(src, srcIndex, dst, destIndex, length);
            }
        }
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyMemory(int* src, int srcIndex, int[] destination, int destIndex, int length)
        {
            fixed (int* dst = destination)
            {
                Renderer.CopyMemory(src, srcIndex, dst, destIndex, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyMemory(IntPtr source, int srcIndex, IntPtr dest, int destIndex, int length)
        {
            Renderer.CopyMemory((int*)source, srcIndex, (int*)dest, destIndex, length);
        }
        #endregion
    }

    #region DELEGATES
    public delegate void VoidMethod();
    public delegate T ReturnMethod<T>();
    public delegate IntPtr GetCurrentContextDelegate();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint TimerCallback(int interval, IntPtr param);
    public delegate void EventNotifier(IEventInfo e);

    public delegate void GlyphFillAction(int val1, int val2, int axis, bool horizontal, float? alpha = null);
    public delegate void FillAction(float val1, float val2, int axis, bool horizontal);

    public delegate void BlockCopy(int sourceIndex, int destinationIndex, int copyLength, int yPosition);
    public delegate void BlockCopyPtr(IntPtr arrayToCopy, int sourceIndex, int destinationIndex, int copyLength, int yPosition);

    //https://web.archive.org/web/20150329101415/https://msdn.microsoft.com/en-us/magazine/cc164015.aspx
    public delegate void TimerEvent(DateTime eventTime, string threadName);
    #endregion
}
