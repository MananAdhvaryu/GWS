using MnM.GWS;

using System;
using System.Diagnostics;
using static MnM.GWS.Implementation;
#if AdvancedVersion
using MnM.GWS.AdvancedVersion;
#else
using MnM.GWS.StandardVersion;
#endif

namespace Test
{
    public static class Program
    {
        static string desktop = "C://users//maadh//desktop//";
        static IWindow window;
        static bool timerclear;
        static IFillStyle rg = Factory.newFillStyle(Gradient.Central, Colour.Red, Colour.SkyBlue, Colour.LightPink, Colour.Green);
        static IFillStyle rg1 = Factory.newFillStyle(Gradient.Vertical, Colour.Yellow, Colour.Green, Colour.Orange, Colour.Olive);
        static IFillStyle fs = Factory.newFillStyle(Gradient.Central, Colour.Gold, Colour.Orange, Colour.Green, Colour.Teal);
        static IFillStyle fs1 = Factory.newFillStyle(Gradient.Vertical, Colour.Black, Colour.Maroon);
        static IFillStyle moonfs = Factory.newFillStyle(Gradient.BackwardDiagonal, Colour.Red, Colour.Green);
        static IBufferPen timerPen;
        static int timerFillMode;
        static void Main()
        {
            Attach(SdlFactory.Instance);
            Colours.ChangeScheme(0, null, 16, null);
            
            window = Factory.newWindow(width: 1000, height: 900, flags: GwsWindowFlags.Resizable);
            //graphics.Window = window;
            timerPen = rg.Change(Gradient.Horizontal).newBrush(300, 300);
           // var w1 = Factory.newWindow(x: 500, y: 500, width: 300, height: 300);
            //g = Instance.GetCanvas(window.Width, window.Height);
            //window.Transparency = .5f;
            window.Paint += Window_Paint;
            //window.Resized += Window_Resized;
            window.Load += Window_Load;
            //draw(window.Graphics);
            window.KeyPress += Window_KeyPress;
            window.MouseDown += Window_MouseDown;
            //w1.MouseDown += Window_MouseDown;
            //w1.Paint += Window_Paint;
            //window.Capture = true;
            window.Show();
            //w1.Show();
            Implementation.Run();
        }

        private static void Window_Load(object sender, IEventArgs e)
        {
            var timer = Factory.newTimer();
            timer.Interval = 1000;
            timer.Enabled = true;
            //timer.Tick += Timer_Tick;
            //timerclear = true;
            //timer.Start();
        }

        private static void Timer_Tick(object sender, IEventArgs e)
        {
            if (timerclear)
            {
                Renderer.CopySettings(fillMode: (FillMode)(timerFillMode % 6), stroke: 20);
                window.DrawEllipse(300, 300, 200, 200, timerPen);
                window.Submit();
                timerclear = false;
                timerFillMode += 1;
            }
            else
            {
                window.Submit();
                //graphics.Clear(278, 278, 244, 244);
                timerclear = true;
            }
        }

        private static void Window_Resized(object sender, ISizeEventArgs e)
        {
            draw(window);
        }

        private static void Window_MouseDown(object sender, IMouseEventArgs e)
        {
            (sender as IWindow).Title = e.ToString();
        }
        private static void draw(IWindow eGraphics)
        {
            bool? frame = null;

            drawNative(eGraphics, frame);

        }
        private static void Window_Paint(object sender, IPaintEventArgs e)
        {
            draw(window);
        }
        private static void Window_KeyPress(object sender, IKeyPressEventArgs e)
        {
            window.Title = e.KeyChar.ToString();
        }

        static void drawNative(IWindow graphics, bool? doTest)
        {
            if (graphics == null)
                return;

            //return;
            //var t1 = Instance.BenchMark(()=> mnmCanvas.Clear(Colors.Red));
            var font = Factory.newFont("c:\\windows\\fonts\\tahoma.ttf", 40);
            var drawStyle = new DrawStyle();
            drawStyle.Angle = new Angle(25);
            //var surface = new SdlWindowSurface(window);
            var Path = graphics.Controls;

            //mnmCanvas.ApplyBackground(Geometry.TransparentColor, false);
            //Path.Window.DirectRendering = true;
            graphics.ApplyBackground(moonfs);
            //Path.Window.Update();
            //Path.Window.DirectRendering = false;
            //mnmCanvas.Clear(false);
            //mnmCanvas.Clear(Colors.White);

            //mnmCanvas.Stroke = 12;
            if (doTest == true)
            {
                //var image =  Instance.GetImage(280, 280);
                //var image1 = Instance.GetImage(210, 210);
                //image1.DrawStyle.Fill = false;

                //var temp = Instance.GetCanvas(image);
                //temp.EllipseAt(1, 1, 100, 125);
                ////image.Trapezoid(0, 0, 100, 40, 150, 90, 30, 140, fs);

                //var brush = new Bitmap(200, 200, FillStyle.RedGreen);
                //brush.FillAll();

                //temp = new Canvas(image1);
                //mnmCanvas.Image(brush.Data, brush.Width, brush.Height, 100, 100, 100, 100); 
                //temp.Image(image, 0, 0, 0, 0, 210, 210, false, mode: CopyMode.Front);

                //image1.EllipseAt(25, 25, 100, 100, fs1);
                //temp.Dispose();

                //mnmCanvas.BrushStyle = fs;
                //mnmCanvas.Stroke = 0;
                //API.BenchMark(() => mnmCanvas.DrawEllipseAt(10, 10, 100, 150), "GWS Draw Ellipse operation");
                //mnmCanvas.Fill = false;
                //mnmCanvas.Rhombus(59, 50, 101, 141, 23);

                //mnmCanvas.Pie(new Pixel(200, 300), new Pixel(200, 128));

                //API.BenchMarkMethod(() => mnmCanvas.DrawBezier(160.5f, 500.5f, 200.3f, 280.6f, 150.45f, 230.23f, 250, 460), true, "Draw Bezier");
                //API.BenchMarkMethod(() => mnmCanvas.DrawBezier(300, 100, 300, 200, 400, 200, 400, 100), true, "Draw Bezier");
                //mnmCanvas.DrawBezier(130, 400, 200, 280, 150, 230, 250, 460);

                //API.BenchMarkMethod(() =>
                //    mnmCanvas.Bezier(130.5f, 350.5f, 300.3f, 280.6f, 300.45f, 200.23f, 250, 460, 460, 440, 530, 320), true, "Draw Bezier operation takes");

                //mnmCanvas.DrawRectangle(150, 10, 100, 100, 45, FillStyle.RedGreen);

                //API.BenchMarkMethod(() => mnmCanvas.DrawTriangle(400, 10, 450, 200, 300, 225), true, "Triangle drawing");

                //API.BenchMarkMethod(() => mnmCanvas.DrawTriangle(200, 350, 400, 400, 90, 145), true, "Draw Triangle operation takes");

                //Instance.BenchMark(() => mnmCanvas.DrawTriangle(400, 450, 570, 300, 223, 510,  
                //    rg), "Draw Triangle operation takes");

                //Instance.BenchMark(() => mnmCanvas.DrawTriangle(650, 350, 800, 400, 500, 145, rg),
                //    "Draw Triangle operation takes");

                //Instance.BenchMark(() => mnmCanvas.DrawEllipseAt(100, 95, 150, 150, rg),  "Draw Pie operation takes");

                //Instance.BenchMark(() => mnmCanvas.DrawPieAt(130, 95, 150, 150, 45, 101, rg), "GWS Draw Arc operation");
                //Instance.BenchMark(() => mnmCanvas.DrawPieAt(110, 105, 150, 150, 101, 45, rg), "GWS Draw Pie operation");


                //API.BenchMarkMethod(() => mnmCanvas.Polygon(400, 10, 450, 200, 300, 225), true, "Triangle by Poygon drawing");

                //API.BenchMarkMethod(() => mnmCanvas.Trapezoid(400, 10, 450, 200, 250, 125, 300, 225), true, "Trapezoid drawing");

                //API.BenchMarkMethod(() => mnmCanvas.Polygon(400, 10, 450, 200, 250, 125, 300, 225), true, "Quardilateral by Poygon drawing");

                //mnmCanvas.Quadrilateral(400, 10, 450, 200, 250, 125, 300, 225);

                //mnmCanvas.Box(400, 10, 250, 200, 65);

                //////triple breasted bra
                //mnmCanvas.DrawPolygon(0, 500, 50, 600, 100, 550, 150, 600, 200, 550, 250, 600, 300, 500, 0, 500);

                ////pickachoo1
                //mnmCanvas.DrawPolygon(400, 10 + 300, 450, 200 + 300, 250, 125 + 300, 300, 225 + 300, 67, 176 + 300, 350, 80 + 300);

                ////angry pickachoo1
                //mnmCanvas.DrawPolygon(400, 10, 450, 200, 250, 125, 228, 150, 300, 225, 67, 176, 350, 80);

                ////hot bra
                //mnmCanvas.DrawPolygon(0, 500, 50, 600, 100, 550, 150, 600, 200, 600, 250, 550, 300, 600, 350, 500, 0, 500);


                ////bra
                //mnmCanvas.DrawPolygon(0, 60, 10, 80, 20, 70, 30, 80, 40, 60, 0, 60);

                ////regular polygon
                //mnmCanvas.DrawPolygon(50, 20, 50, 30, 60, 40, 70, 50, 80, 50, 90, 40, 100, 30, 100, 20, 90, 10, 80, 0, 70, 0, 60, 10, 50, 20);

                //////star
                //mnmCanvas.DrawPolygon(10, 0, 20, 50, 30, 0, 0, 30, 40, 30, 10, 0, 10, 0);

                ////phaser1
                //mnmCanvas.DrawPolygon(60, 60, 60, 100, 100, 100, 61, 90, 60, 80, 61, 70, 100, 60, 60, 60);

                ////phaser2 - buggy
                //mnmCanvas.DrawPolygon(60, 60, 60, 120, 100, 100, 61, 110, 60, 90, 61, 70, 100, 80, 60, 60);

                //////bra
                //mnmCanvas.DrawPolygon(0, 60, 10, 80, 20, 70, 30, 80, 40, 60, 0, 60);

                ////big bra
                //mnmCanvas.DrawPolygon(0, 500, 100, 700, 200, 600, 300, 700, 400, 500, 0, 500);

                ////small squarish regular polygon
                //mnmCanvas.DrawPolygon(50, 20, 50, 30, 60, 40, 70, 50, 80, 50, 90, 40, 100, 30, 100, 20, 90, 10, 80, 0, 70, 0, 60, 10, 50, 20);

                //////small buggy phaser
                //g.DrawPolygon(60, 60, 60, 120, 100, 100, 61, 110, 60, 90, 61, 70, 100, 80, 60, 60);

                ///////big buggy big sucking phaser3
                //mnmCanvas.DrawPolygon(500, 500, 500, 940, 665, 720, 501, 830, 500, 720, 501, 610, 720, 720, 500, 500);


                ////big regular poygon
                //mnmCanvas.DrawPolygon(400, 160, 400, 240, 440, 320, 560, 400, 640, 400, 760, 320, 800, 240, 800, 160, 760, 80, 640, 0, 560, 0, 440, 80, 400, 160);

                /////small star
                //mnmCanvas.DrawPolygon(10, 0, 20, 50, 30, 0, 0, 30, 40, 30, 10, 0, 10, 0);

                ////big star
                //mnmCanvas.DrawPolygon(80, 10, 160, 410, 240, 10, 0, 250, 320, 250);



                //Test 6 Interesctions small

                //var pp = Pixel.ToPoints(90, 215, 163, 29, 63, 202, 188, 46, 41, 182, 206, 70, 26, 156, 217, 97, 20,
                //    127, 219, 127, 22, 97, 213, 156, 33, 70, 198, 182, 51, 46, 176, 202, 76, 29, 149, 215, 105,
                //    21, 120, 220, 134, 21, 90, 215);
                //var brush = moonfs.GetBrush(pp.GetPointsArea().Size);

                //window.Title = Instance.BenchMark(() => mnmCanvas.DrawPolygon(moonfs, 90, 215, 163, 29, 63, 202, 188, 46, 41, 182, 206, 70, 26, 156, 217, 97, 20,
                //    127, 219, 127, 22, 97, 213, 156, 33, 70, 198, 182, 51, 46, 176, 202, 76, 29, 149, 215, 105,
                //    21, 120, 220, 134, 21, 90, 215), "Wheel Polygon drawing", BenchmarkUnit.Tick);

                //mnmCanvas.DrawPolygon(0, 0, 10, 5, 15, 10, 10, 15, 15, 20, 10, 25, 15, 30, 10, 35, 15, 40,
                //    10, 45, 15, 50, 10, 55, 15, 60, 10, 65, 15, 70, 10, 75, 15, 80, 10, 85, 15, 90,
                //    10, 95, 15, 100, 10, 105, 15, 110, 10, 115, 15, 120, 10, 125, 15, 130, 10, 135, 15,
                //    140, 10, 145, 15, 150, 10, 155, 15, 160, 10, 165, 15, 170, 10, 175, 15, 180, 10, 185,
                //    15, 190, 10, 195, 15, 200, 10, 205, 15, 210, 10, 215, 15, 220, 10, 225, 0, 0);

                //mnmCanvas.DrawPolygon(206, 124, 26, 37, 217, 96, 20, 66, 219, 66, 22, 96, 213, 37, 33, 124, 15, 119, 10, 114, 15, 109, 10, 104, 15, 99, 10, 94, 15, 89, 10, 84, 15, 79, 10, 74, 15, 69, 10, 64, 15, 59, 10, 54, 15, 49, 10, 44, 15, 39, 10, 34, 15, 29, 10, 24, 0, 19, 0, 190, 220, 190, 206, 124);

                //////Angle Test
                ////mnmCanvas.DrawPolygon(180, 37, 0, 136, 165, 0, 18, 156, 180, 37);
                //mnmCanvas.DrawPolygon(203, 37, 23, 136, 188, 0, 41, 156, 203, 37, 180, 180, 15, 180, 5, 175, 15, 170, 5, 165, 15, 160, 5, 155, 15, 150, 5, 145, 15, 140, 5, 135, 15, 130, 5, 125, 15, 120, 5, 115, 15, 110, 5, 105, 15, 100, 5, 95, 15, 90, 5, 85, 15, 80, 5, 75, 0, 70, 0, 190, 220, 190, 203, 37);

                ////Test 6 Interesctions big
                //window.Title = API.BenchMark(() => mnmCanvas.DrawPolygon(moonfs, 161, 411, 306, 39, 107, 385, 356, 73, 63, 344, 393,
                //     120, 33, 293, 414, 175, 20, 234, 419, 234, 25, 175, 406, 293, 46, 120,
                //     376, 344, 83, 73, 332, 385, 133, 39, 278, 411, 190, 22, 220, 420, 249,
                //     22, 161, 411, 306, 39, 107, 385, 356, 73, 63, 344, 393, 120, 33, 293,
                //     414, 175, 20, 234, 419, 234, 25, 175, 406, 293, 46, 120, 376, 344, 83,
                //     73, 332, 385, 133, 39, 278, 411, 190, 22, 220, 420, 249, 22));


                //7 Mukesh
                //window.Title = Windows.BenchMark(() => mnmCanvas.DrawPolygon(moonfs, 0, 500, 0, 300, 20, 300, 40, 500, 60, 300, 80, 300, 80, 500, 81, 499, 82,
                //    497, 83, 494, 84, 490, 85, 484, 86, 477, 87, 469, 88, 460, 89, 450, 90, 439, 91, 427,
                //    92, 414, 93, 400, 93, 475, 94, 480, 95, 485, 97, 490, 99, 495, 101, 500, 103, 495, 105,
                //    490, 107, 485, 108, 480, 110, 475, 110, 400, 112, 400, 112, 490, 113, 495, 115, 500, 117,
                //    497, 118, 493, 119, 488, 120, 482, 121, 475, 122, 467, 123, 458, 124, 448, 125, 437,
                //    126, 425, 127, 412, 129, 350, 129, 500, 129, 450, 140, 400, 130, 455, 140, 500, 141,
                //    494, 142, 487, 143, 479, 144, 470, 145, 460, 146, 450, 167, 450, 167, 443, 165, 431,
                //    163, 417, 161, 405, 159, 402, 157, 400, 155, 402, 153, 405, 151, 417, 149, 431, 147,
                //    443, 145, 449, 147, 458, 149, 470, 151, 484, 153, 495, 155, 500, 157, 500, 159, 500,
                //    161, 494, 163, 487, 165, 479, 167, 470, 169, 460, 171, 449, 173, 442, 175, 431, 177,
                //    418, 179, 407, 181, 404, 183, 402, 185, 404, 187, 407, 189, 418, 191, 431, 189, 424,
                //    187, 414, 185, 411, 183, 409, 181, 411, 179, 414, 177, 423, 175, 434, 177, 442, 179,
                //    446, 181, 449, 183, 452, 185, 456, 187, 464, 189, 475, 187, 488, 185, 499, 183, 502,
                //    181, 504, 179, 501, 177, 490, 175, 477, 177, 474, 179, 474, 181, 473, 183, 471, 185,
                //    468, 187, 464, 189, 459, 191, 453, 193, 446, 195, 438, 197, 429, 199, 419, 201, 408,
                //    203, 396, 205, 383, 207, 369, 209, 354, 209, 350, 209, 500, 210, 450, 212, 438, 214,
                //    425, 216, 412, 218, 405, 220, 402, 222, 400, 224, 403, 226, 410, 228, 422, 230, 433,
                //    232, 443, 232, 600, 0, 600, 0, 500), unit: BenchmarkUnit.Tick);


                //g.Lines( Color.Red, 400, 10, 450, 200, 250, 125, 300, 225, 67, 176, 350, 80);

                //mnmCanvas.Update();

                //mnmCanvas.BrushStyle.GetBrush(100, 100);
                //var img = Factory.newCanvas(104, 204);
                //img.Path.ApplyBackground(MnM.GWS.Color.White);
                var style = rg.Change(Gradient.Horizontal);
                //img.Path.DrawEllipse(0, 0, 100, 100, style);
                //graphics.Buffers.Add();

                window.Title = Implementation.BenchMark((VoidMethod)(() =>
                {
                    graphics.ApplyBackground(MnM.GWS.Colour.White);
                    //var ellipse = mnmCanvas.AddEllipseAt(0, 0, 100, 100, style);
                    ///////////////////MnMArc(g, 250, 250, 200, 300);
                    for (int i = 0; i <= 720; i++)
                    {
                        //graphics.Buffers.SwithTo(0);
                        var x = 100 + 200 + (int)(Math.Round(200 * Math.Sin(i * Math.PI / 360), 0));
                        var y = 100 + 200 + (int)(Math.Round(200 * Math.Cos(i * Math.PI / 360), 0));
                        
                        //ellipse.Move(x, y);
                        graphics.DrawEllipse(x, y, 400, 400, style);
                        //mnmCanvas.DrawPolygon(rg, x, y, x + 100, y + 25, x, y + 100);
                        //Path.Window.DirectRendering = false;
                        graphics.Submit();
                        //graphics.Buffers.Reset(0);
                        graphics.Submit();
                        //mnmCanvas.DrawImage(img, x, y, submit: true);
                        //break;
                    };
                }), "Shape frame drawing");
                //mnmCanvas.Update();
            }

            if (doTest == false)
            {
                //mnmCanvas.DrawEllipseAt(10, 10, 100, 150);
                //g.Image(image1, x, y, 0, 0, 210, 210, submit: true, mode: CopyMode.Blend);

                //API.BenchMarkMethod(() => g.ArcAt(200, 50, 200, 250, 225, 360, FillStyle.GreenRed),
                //    true, "Draw Arc operation takes");
                //API.BenchMarkMethod(() => g.PieAt(200, 50, 400, 400, 145, 90, FillStyle.GreenRed),
                //    true, "Draw Arc operation takes");

                //API.BenchMarkMethod(() => g.PieAt(200, 350, 400, 400, 90, 145, FillStyle.RedGreen),
                //    true, "Draw Arc operation takes");

                //API.BenchMarkMethod(() => g.Triangle(200, 350, 400, 400, 90, 145, FillStyle.RedGreen),
                //    true, "Draw Triangle operation takes");


                //API.BenchMarkMethod(() => g.Triangle(300, 350, 470, 200, 123, 410, FillStyle.RedGreen),
                //    true, "Draw Triangle operation takes");

                //g.Bezier(130.5f, 300.5f, 200.3f, 280.6f, 150.45f, 230.23f);//, 250, 460, 460, 440, 530, 320),

                //API.BenchMarkMethod(() => 
                //    g.Bezier(130.5f, 350.5f, 300.3f, 280.6f, 300.45f, 200.23f, 250, 460, 460, 440, 530, 320), true, "Draw Bezier operation takes");

                ////diamond
                //API.BenchMarkMethod(() => g.Polygon(280, 420, 600, 820, 920, 420, 760, 260, 440, 260, 280, 420, 440, 420, 760, 420, 600, 820,
                //440, 420, 520, 260, 680, 260, 760, 420, 920, 420, 280, 420));
                ////g.CircleAt(400, 350, 200, FillStyle.GreenRed);
                //MnMEllipse(g, 200, 200, 100, 150, -0, 180);

                //mnmCanvas.Update();

                //var arc = Shape.Arc(225, 225, 250, 205, fill: true, isStartPoint: true,
                //    antiAlias: true, rotation: 0, gradient: Gradient.Central, penWidth: 5, arcStart: 45, arcEnd: 180);

                //arc.(g, Color.Red, Color.Green);

                //g.RingsAt(new Color[] { Gfx.Red, Gfx.Green, Gfx.Navy },
                //    90, 90, 30, true, fill: false, ringCount: 2, radiusShift: -2, moveHoriZontal: 5, moveVertical: 0);

                //g.RingsAt(new Color[] { Gfx.Red, Gfx.Green, Gfx.Blue , Gfx.Brown},
                //    140, 140, 70, true, fill: true, radiusShift: -3, moveHoriZontal: 4, moveVertical: 4);

                //Graphics.WuCurve(g, 300,300, 325,325, Gfx.WuColors())
                //g.FillCircle(200, 200, 20, Gfx.Red, 255);
                //g.Rectangle(Gfx.Red, Gfx.Yellow, 0, 0, 300, 300, mode: GradientMode.Horizontal);
                //g.Rectangle(200, 200, 200, 200, Gfx.Blue);
                //g.Line(2, 2, 183, 323, Gfx.Red, true, true);

                // g.Elipse(100, 100, 25, 45);
                //g.RoundedRectangle(10, 10, 200, 200,  15, Colors.Red);
                //g.FillRectangleRounded(100, 100, 200, 200, 15, Colors.Tan);
                //var size = 40;
                //var sfont = new SdlFont("c:\\windows\\fonts\\calibri.ttf", fontSize: 16);

                //mnmCanvas.DrawText(sfont, 20, 20, "Abcdefghijklmnopqrstuvwxyz", pen: Colors.Red);

                //////sfont.Hint = FontHint.None;
                ////sfont.HasKerning = true;
                window.Title = Implementation.BenchMark((() =>
                {
                    graphics.DrawEllipse(5, 80, 700, 600, rg.Change(Gradient.DiagonalCentral), new Angle(34));
                    var tr = Path.Add( Factory.newTriangle(5, 80, 500, 300, 200, 650), FillStyles.GridBackGround.Change(Gradient.Central));
                    var poly = Path.Add(Factory.newPolygon( 90, 215, 163, 29, 63, 202,
                         188, 46, 41, 182, 206, 70, 26, 156, 217, 97, 20,
                         127, 219, 127, 22, 97, 213, 156, 33, 70, 198, 182, 51,
                         46, 176, 202, 76, 29, 149, 215, 105,
                         21, 120, 220, 134, 21, 90, 215), rg1.Change(Gradient.Central));

                    //tr.Enabled = false;
                    //poly.Visible = false;
                    //mnmCanvas.Erase(poly);
                    //poly.Move(100, 600);
                    var tpz = Path.Add(Factory.newTrapezium(425, 480, 650, 690, 190), rg1.Change(Gradient.Central));
                    //mnmCanvas.Add(tpz, 0, 0, rg1.Change(Gradient.Central));
                    //mnmCanvas.DrawTrapezium(425, 480, 650, 690, 190, rg1.Change(Gradient.Central), 5);

                    //mnmCanvas.DrawText(font, 120, 120, "Method Execution Takes 603 Ticks", rg1.Change(Gradient.Horizontal), drawStyle);

                    var tpztext =  Path.Add(Factory.newText(font, "Trapezium", 580, 300, drawStyle), rg);
                    //tpztext.Move(100, 100);

                    var img = Factory.newGraphics(550, 400);
                    //font.DrawStyle.Angle = null;
                    var text = img.DrawText(font, 0, 0, "Mukesh", Colour.White);
                    img.DrawCircle(200, 200, 200, rg1);
                    //img.SaveAs(desktop + "font", ImageFormat.BMP);
                    //img = img.Scale(.5f, .5f) as IImage;
                    //var glyph = Factory.newScanner(img);
                    //img.Dispose();

                    //mnmCanvas.Draw(glyph, rg1, 100, 600);
                    //mnmCanvas.Clear(100, 100, 500, 700);
                    //mnmCanvas.DrawImage(img, 400, 400);
                    //mnmCanvas.Update();

                    //tpz.Enabled = false;
                    //tpz.Move(40, 40);
                    //tpz.Enabled = false;
                    //tpz.Visible = false;
                    //mnmCanvas.Remove(tpz, false);
                    //mnmCanvas.Update(120, 50, 900, 780);
                    //mnmCanvas.Clear(100, 100, 300, 300);
                }), unit: BenchmarkUnit.MilliSecond);
                //mnmCanvas.Resize(800, 800);
                //mnmCanvas = mnmCanvas.Rotate(Factory.newAngle(3));
                graphics.Submit();
                //graphics.SaveAs(desktop + "canvas", ImageFormat.BMP);

                //var texture = (mnmCanvas as ICanvas).Texture;
                //var t = texture.New(true, 400, 400) as ITexture;
                ////mnmCanvas.Clear();
                //t.Upload(null, null);
                //(mnmCanvas)?.Update(glyph.X, glyph.Y, glyph.Width, glyph.Height);

                //sfont.Dispose();
                //font.Dispose();
                //g.Hint = RenderingHint.Best;
                //mnmCanvas.DrawText(font, text: "Mukesh Adhvaryu",  x: 20, y: 20, fontColor: Colour.Black);
                //mnmCanvas.Update();
                //var sz1 = g.MeasureText(font, "M");
                //var sz = g.MeasureText(font, "Mukesh Adhvaryu");
            }

            if(doTest == null)
            {

                IBufferPen pen= rg.newBrush(100, 100);
                //pen.Stroke = 10;
                Renderer.ReadContext = rg.Change(Gradient.Vertical);
                //Renderer.RotateTransform( Entity.Writer, new Angle(25, 400, 400));

                var poly = Path.Add(Factory.newPolygon(90, 215, 163, 29, 63, 202,
                     188, 46, 41, 182, 206, 70, 26, 156, 217, 97, 20,
                     127, 219, 127, 22, 97, 213, 156, 33, 70, 198, 182, 51,
                     46, 176, 202, 76, 29, 149, 215, 105,
                     21, 120, 220, 134, 21, 90, 215));

                var ellipse = Path.Add(Factory.newEllipse(10, 10, 400, 600, new Angle(50)));

                Renderer.FillMode = FillMode.DrawOutLine;
                Renderer.Stroke = 4;
                Path.Add(Factory.newEllipse(300, 300, 100, 100));

                var tpz = Path.Add(Factory.newTrapezium(425, 480, 650, 690, 190));

                var tpztext = Path.Add(Factory.newText(font, "Trapezium", 580, 300, drawStyle));
                var roundedArea = Path.Add(Factory.newRoundedBox(300, 300, 200, 200, 25, new Angle(15)));

                //var img = Factory.newCanvas(550, 400);
                //var text = img.DrawText(font, 0, 0, "Mukesh", Color.White);

                //mnmCanvas.WriteLine(Factory.newPen(Color.White), 500, 100, 400, 600, 500, true, null);

                //img.DrawCircle(200, 200, 200, rg1);
                //img.SaveAs(desktop + "font", ImageFormat.BMP);

                //img = img.Scale(.5f, .5f) as IImage;
                //var glyph = Factory.newScanner(img);
                //var sw = new Stopwatch();
                //sw.Start();
                //for (int i = 0; i < 1000; i++)
                //{
                //    mnmCanvas.Add(ellipse.Clone() as IEllipse, updateSurface: false);
                //}
                //sw.Stop();
                //window.Title = sw.ElapsedMilliseconds + "";
                //glyph.DrawTo(mnmCanvas, null);
                //mnmCanvas.Draw(glyph, rg1, 100, 600);

                //mnmCanvas.Add(Factory.newEllipse(80, 80, 200, 200));
                //Path.Graphics.SaveAs(@"C:\Users\maadh\Desktop\canvas", ImageFormat.BMP);
                window.Submit();
                //(mnmCanvas as ICanvas).Texture?.DrawEllipse(600, 100, 300, 200, Color.Red);
                //(mnmCanvas as ICanvas).Texture?.DrawTriangle(200, 350, 400, 400, 90, 145, rg);
                //(mnmCanvas as ICanvas).Texture?.Update();

                //window.Canvas.DrawImage(mnmCanvas, 0, 0);
                //window.Canvas.Update();
                //window.Canvas.SaveAs(@"C:\Users\maadh\Desktop\canvas", ImageFormat.BMP);
                //mnmCanvas.SaveAs(@"C:\Users\maadh\Desktop\texture", ImageFormat.BMP);
                //(mnmCanvas as ICanvas).TargetTexture = false;
                //mnmCanvas.SaveAs(@"C:\Users\maadh\Desktop\canvas", ImageFormat.BMP);

                //mnmCanvas.ChangeBackground();
                //mnmCanvas.Hide(poly);
                //window.Sound.Load(@"C:\Users\maadh\Desktop\test.wav");
                //window.Sound.Play();
                // mnmCanvas.Region.ChangeBackground(Color.GrayText);
                //mnmCanvas.Move(poly, 400, 400);
                //path.Remove(poly);
                //path.Surface = (mnmCanvas);
                //path.RefreshAll();
            }
        }

        private static void Popup_Click(object sender, SimplePopupItemEventArgs e)
        {
            //
        }
    }
}
