using MnM.GWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#if AdvancedVersion
using MnM.GWS.AdvancedVersion;
#else
using MnM.GWS.StandardVersion;
#endif

namespace GWSDemoMSFT
{
    static class Program
    {
        internal const BenchmarkUnit Unit = BenchmarkUnit.MilliSecond;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Implementation.Attach(NativeFactory.Instance);
#if Compare
            Application.Run(new Comparison());
#else
            System.Windows.Forms.Application.Run(new Demo());
#endif
        }

        internal static System.Drawing.PointF[] ToPointsF(this IEnumerable<int> xyPairs, bool formultipleBezier = false)
        {
            List<System.Drawing.PointF> points;
            var len = xyPairs.Count();
            if (formultipleBezier)
            {
                var i = len % 3;
                if (i != 1)
                {
                    len -= i;
                    len += 1;
                }
            }
            var previous = -1;

            points = new List<System.Drawing.PointF>(len);
            foreach (var item in xyPairs)
            {
                if (previous == -1)
                    previous = item;
                else
                {
                    points.Add(new System.Drawing.PointF(previous, item));
                    previous = -1;
                }
            }
            return points.ToArray();
        }
        internal static System.Drawing.PointF[] ToPointsF(this IEnumerable<float> xyPairs, bool formultipleBezier = false)
        {
            List<System.Drawing.PointF> points;
            var len = xyPairs.Count();
            var previous = -1f;

            points = new List<System.Drawing.PointF>(len);
            foreach (var item in xyPairs)
            {
                if (previous == -1)
                    previous = item;
                else
                {
                    points.Add(new System.Drawing.PointF(previous, item));
                    previous = -1;
                }
            }
            return points.ToArray();
        }

        internal static System.Drawing.PointF[] ToPointsF(params int[] xyPairs) =>
            ToPointsF(xyPairs as IEnumerable<int>);
        internal static System.Drawing.PointF[] ToPointsF(params float[] xyPairs) =>
            ToPointsF(xyPairs as IEnumerable<float>);

        internal static System.Drawing.PointF[] ToPointsF(bool formultipleBezier, params int[] xyPairs) =>
            ToPointsF(xyPairs as IEnumerable<int>, formultipleBezier);
        internal static System.Drawing.PointF[] ToPointsF(bool formultipleBezier, params float[] xyPairs) =>
            ToPointsF(xyPairs as IEnumerable<float>, formultipleBezier);
    }
}
