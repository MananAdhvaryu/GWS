// ***********************************************************************
// Assembly         : MnM.GWS
// Author           : Manan Adhvaryu
// Created          : 24-12-2018
//
// Last Modified By : Manan Adhvaryu
// Last Modified On : 31-12-2018
// ***********************************************************************
// <copyright file="Demo.cs" company="eBestow Technocracy Ltd">
//     eBestow Technocracy Ltd & M&M INFO-TECH UK LTD 2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using MnM.GWS;
using MnM.GWS.ImageExtensions;
using MnM.GWS.MathExtensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static MnM.GWS.Implementation;

namespace GWSDemoMSFT
{
    public partial class Demo : Form
    {
        #region variables
        string manualFontPath;
        bool stopDraw = true;
        IGraphics mnmCanvas;
        float x, y, w, h;
        float startA, endA;
        int[] drawPoints;
        Angle rotate, rotate1;
        MnM.GWS.FillMode pse;
        CurveType curveType = 0;
        static System.Drawing.Font ptFont = new System.Drawing.Font("Verdana", 10);
        System.Drawing.Font msFont;
        IFont gwsFont;

        #endregion

        #region constructors
        public Demo()
        {
            InitializeComponent();
            this.Load += this.OnLoad;
        }
        #endregion

        #region OPENING & CLOSING OF FORM
        private void OnLoad(object sender, System.EventArgs e)
        {
            mnmCanvas = Factory.newGraphics(picGWS.Width, picGWS.Height);
            //mnmCanvas.BackgroundStyle = (FillStyle) FillStyles.RedGreen;
            cmbShape.Items.Add("Circle");
            cmbShape.Items.Add("Ellipse");
            cmbShape.Items.Add("Arc");
            cmbShape.Items.Add("Pie");
            cmbShape.Items.Add("BezierArc");
            cmbShape.Items.Add("BezierPie");

            cmbShape.Items.Add("Line");
            cmbShape.Items.Add("Triangle");
            cmbShape.Items.Add("Polygon");
            cmbShape.Items.Add("Square");
            cmbShape.Items.Add("Rectangle");
            cmbShape.Items.Add("RoundedArea");
            cmbShape.Items.Add("Rhombus");
            cmbShape.Items.Add("Trapezium");
            cmbShape.Items.Add("Bezier");
            cmbShape.Items.Add("Glyphs");


            cmbGradient.Items.AddRange(Enum.GetNames(typeof(MnM.GWS.Gradient)));


            cmbBezier.Items.Add(new KeyValuePair<string, BezierType>("Cubic", BezierType.Cubic));
            cmbBezier.Items.Add(new KeyValuePair<string, BezierType>("Multiple", BezierType.Multiple));


            cmbStroke.Items.Add(new KeyValuePair<string, MnM.GWS.FillMode>("Normal", MnM.GWS.FillMode.Original));
            cmbStroke.Items.Add(new KeyValuePair<string, MnM.GWS.FillMode>("Outer", MnM.GWS.FillMode.Outer));
            cmbStroke.Items.Add(new KeyValuePair<string, MnM.GWS.FillMode>("FillOutLine", MnM.GWS.FillMode.FillOutLine));
            cmbStroke.Items.Add(new KeyValuePair<string, MnM.GWS.FillMode>("Inner", MnM.GWS.FillMode.Inner));
            cmbStroke.Items.Add(new KeyValuePair<string, MnM.GWS.FillMode>("DrawOutLine", MnM.GWS.FillMode.DrawOutLine));
            cmbStroke.Items.Add(new KeyValuePair<string, MnM.GWS.FillMode>("ExceptOutLine", MnM.GWS.FillMode.ExceptOutLine));


            cmbBezier.DisplayMember = "Key";
            cmbBezier.SelectedIndex = 0;

            cmbStroke.DisplayMember = "Key";
            cmbStroke.SelectedIndex = 0;

            cmbGradient.SelectedIndex = 1;
            cmbShape.SelectedIndex = 0;


            lstColors.Items.Add(System.Drawing.Color.Red);
            lstColors.Items.Add(System.Drawing.Color.Green);
            lstColors.Items.Add(System.Drawing.Color.DodgerBlue);
            lstColors.Items.Add(System.Drawing.Color.DarkViolet);


            var copts = Enum.GetValues(typeof(CurveType));
            foreach (var item in copts)
            {
                lstCurveOption.Items.Add(item);
            }


            cmbShape.SelectedIndexChanged += changeShape;

            picGWS.MouseClick += PicGWS_MouseClick;
            picGWS.MouseMove += PicGWS_MouseMove;
            cmbGradient.SelectedIndexChanged += draw;

            numArcS.ValueChanged += draw;
            numArcE.ValueChanged += draw;
            numX.ValueChanged += draw;
            numY.ValueChanged += draw;
            numW.ValueChanged += draw;
            numH.ValueChanged += draw;
            numRotate.ValueChanged += draw;
            numStroke.ValueChanged += draw;
            numSize.ValueChanged += draw;
            numSizeDiff.ValueChanged += draw;

            chkAA.CheckedChanged += draw;
            cmbStroke.SelectedIndexChanged += draw;

            chkCenterRotate.Click += ChkCenterRotate_Click;

            var colors = Enum.GetNames(typeof(System.Drawing.KnownColor)).Select(x => (object)System.Drawing.Color.FromName(x)).Reverse().ToArray();
            lstKnownColors.Items.AddRange(colors);
            lstKnownColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            lstColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            lstKnownColors.SelectedIndexChanged += LstKnownColors_SelectedIndexChanged;
            lstKnownColors.DrawItem += LstKnownColors_DrawItem;
            lstColors.DrawItem += LstKnownColors_DrawItem;
            lstColors.DoubleClick += LstColors_DoubleClick;
            numWriteAngle.ValueChanged += draw;
            btnDraw.Click += draw;
            btnClear.Click += BtnClear_Click;
            btnPlot.Click += BtnPlot_Click;
            ResizeEnd += GWSDemo_Resize;
            picGWS.Paint += PicGWS_Paint;
            numFontSize.ValueChanged += NumFontSize_ValueChanged;
            btnOpenFont.Click += BtnOpenFont_Click;
            this.FormClosed += GWSDemo_FormClosed;
        }

        private void ChkCenterRotate_Click(object sender, System.EventArgs e)
        {
            if (chkCenterRotate.Checked)
            {
                txtRotCenter.Clear();
                txtRotCenter.Visible = false;
                chkCenterRotate.Text = "On Center";
            }
            else
            {
                txtRotCenter.Visible = true;
                chkCenterRotate.Text = "Center XY: ";
            }
        }

        private void LstColors_DoubleClick(object sender, System.EventArgs e)
        {
            lstColors.Items.Remove(lstColors.SelectedItem);

        }

        private void LstKnownColors_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lstKnownColors.SelectedIndex != -1)
                lstColors.Items.Add(lstKnownColors.SelectedItem);
        }

        private void LstKnownColors_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;
            var c = (System.Drawing.Color)(sender as ListBox).Items[e.Index];
            var b = new SolidBrush(c);
            e.Graphics.FillRectangle(new SolidBrush(c), e.Bounds);
            //e.Graphics.DrawString(c.Name, lstKnownColors.Font, b, 0, 0);
        }
        private void GWSDemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            MnM.GWS.Implementation.Quit();
        }
        #endregion

        #region FONT HANDLING
        private void BtnOpenFont_Click(object sender, System.EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                manualFontPath = openFileDialog1.FileName;
                setFont(this, e);
            }
        }

        private void NumFontSize_ValueChanged(object sender, System.EventArgs e)
        {
            setFont(sender, e);
            if (gwsFont != null)
                draw(sender, e);
        }
        void setFont(object sender, System.EventArgs e)
        {

            if (manualFontPath != null)
            {
                msFont = new System.Drawing.Font(manualFontPath, (float)numFontSize.Value);
                gwsFont = Factory.newFont(manualFontPath, (int)numFontSize.Value);
                if (gwsFont == null)
                    System.Windows.Forms.MessageBox.Show("This font can not be read!");
                return;
            }
            else
                gwsFont = Factory.SystemFont;
        }
        #endregion

        #region FORM RESIZE - REFRESH
        private void GWSDemo_Resize(object sender, System.EventArgs e)
        {
            mnmCanvas = Factory.newGraphics(picGWS.Width, picGWS.Height);
            draw(this, e);
        }
        private void PicGWS_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (mnmCanvas == null)
                return;
            if (stopDraw)
                return;
            if (chkRotateImage.Checked)
            {
                lblGWS.Text = Implementation.BenchMark(() =>
                {
                    var img = mnmCanvas.RotatedCopy(new Angle((float)numRotate.Value));
                    uploadToMS(img, e.Graphics);
                }, unit: Program.Unit);
                return;
            }
            mnmCanvas.ApplyBackground(MnM.GWS.Colour.White);
            
            SetDrawingParams();
            var gwsMethod = SetVoidMethod();
            lblGWS.Text = MnM.GWS.Implementation.BenchMark(gwsMethod, out long i, cmbShape.Text + "", Program.Unit);
            uploadToMS(mnmCanvas, e.Graphics);
        }

        void uploadToMS(IGraphics img, System.Drawing.Graphics g)
        {
            //g.Clear(Color.White);
            var b = new System.Drawing.Bitmap(img.Width, img.Height, img.Width * 4,
                   System.Drawing.Imaging.PixelFormat.Format32bppArgb, img.Pixels);
            g.DrawImage(b, System.Drawing.Point.Empty);
        }

        /// <summary>
        /// This method draws comparative drawing on two picture boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void draw(object sender, System.EventArgs e)
        {
            stopDraw = false;

            picGWS.Refresh();
            stopDraw = true;
        }

        #endregion

        #region CHANGING SHAPE TYPE
        private void changeShape(object sender, System.EventArgs e)
        {
            //txtPts.Clear();
            stopDraw = true;
            picGWS.Refresh();
            pnlArcS.Visible = false;
            pnlArcE.Visible = false;
            pnlBezier.Visible = false;
            grpFonts.Visible = false;
            grpXY.Visible = false;
            grpTrapezium.Visible = false;
            grpPts.Visible = false;
            grpArcPie.Visible = false;
            grpXY.Text = "Enter " + cmbShape.Text + " paramters";
            var shape = cmbShape.SelectedItem + "";
            btnOpenFont.Visible = false;
            pnlRoundRC.Visible = false;

            switch (shape)
            {
                case "Trapezium":
                    grpTrapezium.Visible = true;
                    grpTrapezium.BringToFront();
                    break;
                case "Circle":
                case "Ellipse":
                    grpXY.Visible = true;
                    grpPts.Visible = false;
                    lblArcs.Text = "Angle";
                    if (cmbShape.Text == "Circle")
                        numH.Visible = false;
                    else
                        numH.Visible = true;
                    lblH.Visible = numH.Visible;
                    break;
                case "Arc":
                case "Pie":
                case "BezierPie":
                case "BezierArc":
                case "Square":
                case "Rectangle":
                case "Rhombus":
                case "RoundedArea":
                    if (cmbShape.Text == "Arc" || cmbShape.Text == "Pie")
                    {
                        grpArcPie.Visible = true;
                    }

                    grpXY.Visible = true;
                    grpPts.Visible = false;
                    if (cmbShape.Text == "Sqaure")
                        numH.Visible = false;
                    else
                    {
                        numH.Visible = true;
                        if (cmbShape.Text == "Arc" || cmbShape.Text == "Pie" || cmbShape.Text == "BezierArc" || cmbShape.Text == "BezierPie")
                        {
                            lblArcs.Text = "Arc Start";
                            pnlArcS.Visible = true;
                            pnlArcE.Visible = true;
                        }
                    }
                    lblH.Visible = numH.Visible;
                    if(cmbShape.Text == "RoundedArea")
                    {
                        pnlRoundRC.Visible = true;
                    }
                    break;
                case "Triangle":
                case "Polygon":
                case "Bezier":
                case "Line":
                    pnlArcS.Visible = false;
                    pnlArcE.Visible = false;

                    grpXY.Visible = false;
                    grpPts.Visible = true;
                    if (shape == "Bezier")
                        pnlBezier.Visible = true;
                    txtPts.Clear();
                    break;
                case "Glyphs":
                    grpPts.Visible = true;
                    grpXY.Visible = false;
                    grpFonts.Visible = true;
                    grpFonts.BringToFront();
                    btnOpenFont.Visible = true;
                    break;
                default:

                    break;
            }
            stopDraw = false;
        }
        #endregion

        #region MISC
        private void BtnPlot_Click(object sender, System.EventArgs e)
        {
            if (txtPts.Text.Length == 0)
                return;

            if (txtPts.Text.Substring(0, 1) == ",")
                txtPts.Text = txtPts.Text.Substring(1);
            var points = txtPts.Text.Split(',').Select(p => Convert.ToInt32(p)).ToArray();
            int j = 0;
            using (var g = picGWS.CreateGraphics())
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var x = points[i];
                    var y = points[++i];
                    g.DrawString((j++) + "", ptFont, Brushes.Red, x, y);
                    //g.DrawRectangle(Pens.Red, points[i], points[++i], 2, 2);
                }
            }

        }

        private void BtnClear_Click(object sender, System.EventArgs e)
        {
            txtPts.Clear();
            picGWS.Refresh();
        }

        private void PicGWS_MouseMove(object sender, MouseEventArgs e)
        {
            lblGWS.Text = e.X + "," + e.Y + ",";
        }
        private void ArcRadius(int aX, int aY, int bX, int bY, out float width, out float height, out Angle sa, out Angle ea)
        {
            var cx = (aX - bX) / 2;
            var cy = (aY - bY) / 2;

            width = (float)Math.Sqrt(MathHelper.Sqr(aX - (aX - bX) / 2) + MathHelper.Sqr(aY - (aY - bY) / 2)) * 2;
            height = width;

            var startAngle = Geometry.GetAngle(aX, aY, cx, cy);
            var endAngle = Geometry.GetAngle(bX, bY, cx, cy);
            sa = new Angle(startAngle, cx, cy);
            ea = new Angle(endAngle, cx, cy);
        }

        private void tblpnl1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void lblGWS_Click(object sender, System.EventArgs e)
        {

        }

        private void PicGWS_MouseClick(object sender, MouseEventArgs e)
        {
            txtPts.Text += "," + e.X + "," + e.Y;
            using (var g = picGWS.CreateGraphics())
            {
                g.DrawRectangle(Pens.Red, e.X, e.Y, 2, 2);
            }
        }
        #endregion

        #region SETTING DRAWING PARAMS AND APPROPRIATE METHOD
        VoidMethod SetVoidMethod()
        {
            Enum.TryParse(cmbGradient.SelectedItem + "", true, out MnM.GWS.Gradient grad);
            IFillStyle fStyle;
            if (lstColors.Items.Count > 0)
            {
                object[] all = new object[lstColors.Items.Count];
                lstColors.Items.CopyTo(all, 0);
                fStyle = Factory.newFillStyle(grad, all.Select(x => ((System.Drawing.Color)x).ToArgb()).ToArray());
            }
            else
            {
                fStyle = Factory.newFillStyle(grad, MnM.GWS.Colour.Silver);
            }

            Renderer.ReadContext = fStyle;

            VoidMethod gwsMethod = null;
            var stroke = (float)numStroke.Value;
            Renderer.CopySettings(pse, stroke, StrokeMode.Middle, chkAA.Checked ? LineDraw.AA : LineDraw.NonAA);

            mnmCanvas.ApplyBackground(MnM.GWS.Colour.White);

            if (rotate1.Valid)
                Renderer.RotateTransform(Entity.Buffer, rotate1);
            else
                Renderer.ResetTransform(Entity.Buffer);

            curveType = 0;
            foreach (var item in lstCurveOption.CheckedItems)
            {
                curveType |= (CurveType)item;
            }
            IDrawStyle ds = new DrawStyle();
            ds.Angle = rotate;
            var font = gwsFont ?? Factory.SystemFont;
            ds.LineHeight = (font?.Info?.LineHeight ?? 0).Ceiling();

            switch (cmbShape.Text)
            {
                case "Line":
                    if (drawPoints == null || drawPoints.Length < 4)
                    {
                        System.Windows.Forms.MessageBox.Show("Please provide 2 coordinates to create a line by clicking 2 points on GWS picture box!");
                        return null;
                    }
                    gwsMethod = () => mnmCanvas.DrawLines(true, rotate, drawPoints);
                    break;
                case "Trapezium":
                    if (drawPoints == null || drawPoints.Length < 4)
                    {
                        System.Windows.Forms.MessageBox.Show("Please provide 2 coordinates to create a line by clicking 2 points on GWS picture box!");
                        return null;
                    }
                    gwsMethod = () => mnmCanvas.DrawTrapezium(
                        new float[] {drawPoints[0], drawPoints[1], drawPoints[2], drawPoints[3],
                        (float) numSize.Value, (float)numSizeDiff.Value}, rotate);
                    break;

                case "Circle":
                    if (drawPoints?.Length >= 4)
                    {
                        gwsMethod = () => mnmCanvas.DrawCircle(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), rotate);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawEllipse(x, y, w, w, rotate);

                    break;
                case "Ellipse":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawEllipse(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), rotate);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawEllipse(x, y, w, h, rotate);
                    break;
                case "Arc":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawArc(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), rotate, curveType | CurveType.Arc);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawArc(x, y, w, h, startA, endA, rotate, curveType | CurveType.Arc);
                    break;
                case "Pie":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawPie(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), rotate, curveType | CurveType.Pie);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawPie(x, y, w, h, startA, endA, rotate, curveType | CurveType.Pie);
                    break;

                case "BezierArc":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawBezierArc(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawBezierArc(x, y, w, h, startA, endA, rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    break;
                case "BezierPie":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawBezierPie(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawBezierPie(x, y, w, h, startA, endA, rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    break;

                case "Square":
                    gwsMethod = () => mnmCanvas.DrawRhombus(x, y, w, w, rotate);
                    break;
                case "Rectangle":
                    gwsMethod = () => mnmCanvas.DrawRectangle(x, y, w, h, rotate);
                    break;

                case "RoundedArea":
                    gwsMethod = () => mnmCanvas.DrawRoundedBox(x, y, w, h, (float)numCornerRadius.Value, rotate);
                    break;

                case "Rhombus":
                    gwsMethod = () => mnmCanvas.DrawRhombus(x, y, w, h, rotate);

                    break;
                case "Triangle":
                    if (drawPoints == null || drawPoints.Length < 6)
                    {
                        System.Windows.Forms.MessageBox.Show("Please provide 3 coordinates to create a triangle by clicking 3 points on GWS picture box!");
                        return null;
                    }
                    gwsMethod = () => mnmCanvas.DrawTriangle(drawPoints[0], drawPoints[1],
                        drawPoints[2], drawPoints[3], drawPoints[4], drawPoints[5], rotate);
                    break;

                case "Polygon":
                    if (drawPoints == null)
                        return null;
                    gwsMethod = () => mnmCanvas.DrawPolygon(rotate, drawPoints.Select(p => p + 0f).ToArray());
                    break;
                case "Bezier":
                    var type = cmbBezier.SelectedIndex != -1 ?
                        ((KeyValuePair<string, BezierType>)cmbBezier.SelectedItem).Value : BezierType.Cubic;
                    if (type.HasFlag(BezierType.Multiple))
                    {
                        if (drawPoints == null || drawPoints.Length < 6)
                            return null;
                        gwsMethod = () => mnmCanvas.DrawBezier(type, rotate, drawPoints.Select(p => (float)p).ToArray());

                    }
                    else
                    {
                        if (drawPoints == null || drawPoints.Length < 3)
                        {
                            System.Windows.Forms.MessageBox.Show("Please provide at least 3 coordinates to create a bezier by clicking 3 points on GWS picture box!");
                            return null;
                        }
                        gwsMethod = () => mnmCanvas.DrawBezier(type, rotate, drawPoints.Select(p => (float)p).ToArray());
                    }
                    break;
                case "Glyphs":
                    gwsMethod = () => mnmCanvas.DrawText(font, 130, 130 + font.Size, txtPts.Text, ds);
                    break;
            }
            return gwsMethod;
        }
        void SetDrawingParams()
        {
            drawPoints = null;
            //mnmCanvas.AntiAlias = chkAA.Checked;
            pse = cmbStroke.SelectedIndex != -1 ?
                ((KeyValuePair<string, MnM.GWS.FillMode>)cmbStroke.SelectedItem).Value : MnM.GWS.FillMode.FillOutLine;

            //get the user input values for width, height, x & y coordinates.
            x = (int)numX.Value;
            y = (int)numY.Value;
            w = (int)numW.Value;
            h = (int)numH.Value;

            //get the start and sweep angle for arc/pie drawing
            startA = (float)numArcS.Value;
            endA = (float)numArcE.Value;

            if (chkCenterRotate.Checked)
                rotate = new Angle((float)numRotate.Value, chkSkew.Checked);
            else
            {
                float x = 0;
                float y = 0;
                if (!string.IsNullOrEmpty(txtRotCenter.Text))
                {
                    try
                    {
                        var arr = txtRotCenter.Text.Split(',');
                        if (arr.Length > 0)
                        {
                            if (float.TryParse(arr[0], out float p))
                                x = p;
                        }
                        if (arr.Length > 1)
                        {
                            if (float.TryParse(arr[1], out float p))
                                y = p;
                        }
                    }
                    catch { }
                }
                rotate = new Angle((float)numRotate.Value, x, y, chkSkew.Checked);

            }
            if (numWriteAngle.Value != 0)
            {
                if (!string.IsNullOrEmpty(txtRotCenter1.Text))
                {
                    try
                    {
                        var arr = txtRotCenter1.Text.Split(',');
                        if (arr.Length > 0)
                        {
                            if (float.TryParse(arr[0], out float p))
                                x = p;
                        }
                        if (arr.Length > 1)
                        {
                            if (float.TryParse(arr[1], out float p))
                                y = p;
                        }
                    }
                    catch { }
                }
            }
            rotate1 = new Angle((float)numWriteAngle.Value, x, y);

            //get the GWS gradient fill mode 

            //for Triangle, Bezier, Polygon etc. - get the points selected by the user
            IRectangleF rc;
            var shape = cmbShape.SelectedItem + "";
            if (shape == "Glyphs")
            {
                return;
            }
            if (txtPts.Text.Length > 1)
            {
                if (txtPts.Text.Substring(0, 1) == ",")
                    txtPts.Text = txtPts.Text.Substring(1);
                drawPoints = txtPts.Text.Split(',').Select(p => Convert.ToInt32(p)).ToArray();
                switch (shape)
                {
                    case "Arc":
                    case "Pie":
                    case "BezierArc":
                    case "BezierPie":
                        if (drawPoints.Length < 4)
                            goto exit;
                        float rx, ry, cx, cy;
                        ArcRadius(drawPoints[0], drawPoints[1], drawPoints[2],
                            drawPoints[3], out rx, out ry, out Angle sa, out Angle ea);

                        startA = sa.Degree;
                        endA = ea.Degree;
                        w = (int)rx * 2;
                        h = (int)ry * 2;
                        x = (int)(sa.CX - rx);
                        y = (int)(sa.CY - ry);
                        goto mks;
                    case "Line":
                    case "Trapezium":
                        if (drawPoints.Length < 4)
                            goto exit;
                        rc = Geometry.ToPointsF(drawPoints).ToArea();
                        x = rc.X;
                        y = rc.Y;
                        w = rc.Width;
                        h = rc.Height;
                        goto mks;
                    case "Sqaure":
                    case "Rectangle":
                    case "Rhombus":

                        if (drawPoints.Length < 2)
                            goto exit;
                        var wd = Math.Abs(drawPoints[0] - drawPoints[2]);
                        var hd = Math.Abs(drawPoints[1] - drawPoints[2]);
                        if (drawPoints[0] < drawPoints[2])
                        {
                            x = drawPoints[0];
                            y = drawPoints[1];
                        }
                        else
                        {
                            x = drawPoints[2];
                            y = drawPoints[3];
                        }
                        if (wd < 3 || hd < 3)
                        {
                            var dist = Math.Sqrt(MathHelper.Sqr(drawPoints[0] - drawPoints[2]) + MathHelper.Sqr(drawPoints[1] - drawPoints[3]));
                            w = dist.Round();
                            h = dist.Round();
                        }
                        else
                        {
                            w = Math.Abs(drawPoints[2] - drawPoints[0]);
                            h = Math.Abs(drawPoints[3] - drawPoints[1]);
                        }
                        goto mks;
                    case "Triangle":
                    case "Polygon":
                    case "Bezier":
                        if (drawPoints.Length < 3)
                            goto exit;
                        rc = Geometry.ToPointsF(drawPoints).ToArea();
                        w = rc.Width;
                        h = rc.Height;

                        goto mks;
                }
            exit:
                txtPts.Clear();
            mks:
                return;
            }
        }
        #endregion
    }
}
