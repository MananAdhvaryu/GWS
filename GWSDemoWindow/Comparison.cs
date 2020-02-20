// ***********************************************************************
// Assembly         : MnM.GWS
// Author           : Manan Adhvaryu
// Created          : 24-12-2018
//
// Last Modified By : Manan Adhvaryu
// Last Modified On : 31-12-2018
// ***********************************************************************
// <copyright file="GWSDemo.cs" company="eBestow Technocracy Ltd">
//     eBestow Technocracy Ltd & M&M INFO-TECH UK LTD 2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using MnM.GWS;
//using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MnM.GWS.MathExtensions;
using static MnM.GWS.Implementation;
using MnM.GWS.ImageExtensions;

namespace GWSDemoMSFT
{
    partial class Comparison
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMS = new System.Windows.Forms.Label();
            this.cldg = new System.Windows.Forms.ColorDialog();
            this.lblGWS = new System.Windows.Forms.Label();
            this.tblpnl1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpArcPie = new System.Windows.Forms.GroupBox();
            this.lstCurveOption = new System.Windows.Forms.CheckedListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numStroke = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSkew = new System.Windows.Forms.CheckBox();
            this.txtRotCenter = new System.Windows.Forms.TextBox();
            this.numRotate = new System.Windows.Forms.NumericUpDown();
            this.chkCenterRotate = new System.Windows.Forms.CheckBox();
            this.chkRotateImage = new System.Windows.Forms.CheckBox();
            this.grpTrapezium = new System.Windows.Forms.GroupBox();
            this.numSizeDiff = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numSize = new System.Windows.Forms.NumericUpDown();
            this.lblSize = new System.Windows.Forms.Label();
            this.grpPts = new System.Windows.Forms.GroupBox();
            this.btnPlot = new System.Windows.Forms.Button();
            this.txtPts = new System.Windows.Forms.TextBox();
            this.pnlBezier = new System.Windows.Forms.Panel();
            this.cmbBezier = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grpFonts = new System.Windows.Forms.Panel();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.btnOpenFont = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStroke = new System.Windows.Forms.ComboBox();
            this.cmbGradient = new System.Windows.Forms.ComboBox();
            this.lstColors = new System.Windows.Forms.ListBox();
            this.lstKnownColors = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkAA = new System.Windows.Forms.CheckBox();
            this.btnDraw = new System.Windows.Forms.Button();
            this.grpXY = new System.Windows.Forms.GroupBox();
            this.Xlabel = new System.Windows.Forms.Label();
            this.Ylabel = new System.Windows.Forms.Label();
            this.lblH = new System.Windows.Forms.Label();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.pnlArcE = new System.Windows.Forms.Panel();
            this.lblArce = new System.Windows.Forms.Label();
            this.numArcE = new System.Windows.Forms.NumericUpDown();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.widthLabel = new System.Windows.Forms.Label();
            this.pnlArcS = new System.Windows.Forms.Panel();
            this.numArcS = new System.Windows.Forms.NumericUpDown();
            this.lblArcs = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.picGWS = new System.Windows.Forms.PictureBox();
            this.picMS = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pnlRoundRC = new System.Windows.Forms.Panel();
            this.numCornerRadius = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tblpnl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpArcPie.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStroke)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRotate)).BeginInit();
            this.grpTrapezium.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSizeDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).BeginInit();
            this.grpPts.SuspendLayout();
            this.pnlBezier.SuspendLayout();
            this.grpFonts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpXY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).BeginInit();
            this.pnlArcE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArcE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            this.pnlArcS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArcS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGWS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMS)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlRoundRC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCornerRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMS
            // 
            this.lblMS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMS.AutoSize = true;
            this.lblMS.BackColor = System.Drawing.Color.Transparent;
            this.lblMS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblMS.Font = new System.Drawing.Font("Consolas", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMS.Location = new System.Drawing.Point(514, 0);
            this.lblMS.Name = "lblMS";
            this.lblMS.Size = new System.Drawing.Size(505, 20);
            this.lblMS.TabIndex = 3;
            this.lblMS.Text = "MS";
            // 
            // cldg
            // 
            this.cldg.FullOpen = true;
            // 
            // lblGWS
            // 
            this.lblGWS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGWS.AutoSize = true;
            this.lblGWS.BackColor = System.Drawing.Color.Transparent;
            this.lblGWS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblGWS.Font = new System.Drawing.Font("Consolas", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGWS.Location = new System.Drawing.Point(3, 0);
            this.lblGWS.Name = "lblGWS";
            this.lblGWS.Size = new System.Drawing.Size(505, 20);
            this.lblGWS.TabIndex = 4;
            this.lblGWS.Text = "GWS";
            // 
            // tblpnl1
            // 
            this.tblpnl1.AutoSize = true;
            this.tblpnl1.ColumnCount = 1;
            this.tblpnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblpnl1.Controls.Add(this.panel1, 0, 0);
            this.tblpnl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblpnl1.Location = new System.Drawing.Point(0, 0);
            this.tblpnl1.Name = "tblpnl1";
            this.tblpnl1.RowCount = 1;
            this.tblpnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblpnl1.Size = new System.Drawing.Size(1022, 180);
            this.tblpnl1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpPts);
            this.panel1.Controls.Add(this.grpArcPie);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.grpTrapezium);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.chkAA);
            this.panel1.Controls.Add(this.btnDraw);
            this.panel1.Controls.Add(this.grpXY);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 174);
            this.panel1.TabIndex = 3;
            // 
            // grpArcPie
            // 
            this.grpArcPie.Controls.Add(this.lstCurveOption);
            this.grpArcPie.Location = new System.Drawing.Point(799, 13);
            this.grpArcPie.Name = "grpArcPie";
            this.grpArcPie.Size = new System.Drawing.Size(208, 153);
            this.grpArcPie.TabIndex = 74;
            this.grpArcPie.TabStop = false;
            this.grpArcPie.Text = "Curve Option";
            this.grpArcPie.Visible = false;
            // 
            // lstCurveOption
            // 
            this.lstCurveOption.CheckOnClick = true;
            this.lstCurveOption.FormattingEnabled = true;
            this.lstCurveOption.Location = new System.Drawing.Point(9, 26);
            this.lstCurveOption.Name = "lstCurveOption";
            this.lstCurveOption.Size = new System.Drawing.Size(187, 124);
            this.lstCurveOption.TabIndex = 72;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbShape);
            this.groupBox4.Location = new System.Drawing.Point(310, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(136, 45);
            this.groupBox4.TabIndex = 73;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Choose Shape";
            // 
            // cmbShape
            // 
            this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShape.FormattingEnabled = true;
            this.cmbShape.Location = new System.Drawing.Point(4, 19);
            this.cmbShape.Name = "cmbShape";
            this.cmbShape.Size = new System.Drawing.Size(126, 21);
            this.cmbShape.TabIndex = 35;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numStroke);
            this.groupBox3.Location = new System.Drawing.Point(452, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(78, 45);
            this.groupBox3.TabIndex = 71;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stroking";
            // 
            // numStroke
            // 
            this.numStroke.Location = new System.Drawing.Point(6, 23);
            this.numStroke.Name = "numStroke";
            this.numStroke.Size = new System.Drawing.Size(63, 20);
            this.numStroke.TabIndex = 56;
            this.numStroke.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSkew);
            this.groupBox2.Controls.Add(this.txtRotCenter);
            this.groupBox2.Controls.Add(this.numRotate);
            this.groupBox2.Controls.Add(this.chkCenterRotate);
            this.groupBox2.Controls.Add(this.chkRotateImage);
            this.groupBox2.Location = new System.Drawing.Point(310, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 72);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rotation";
            // 
            // chkSkew
            // 
            this.chkSkew.AutoSize = true;
            this.chkSkew.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSkew.Location = new System.Drawing.Point(77, 17);
            this.chkSkew.Name = "chkSkew";
            this.chkSkew.Size = new System.Drawing.Size(55, 19);
            this.chkSkew.TabIndex = 68;
            this.chkSkew.Text = "Skew";
            this.chkSkew.UseVisualStyleBackColor = true;
            // 
            // txtRotCenter
            // 
            this.txtRotCenter.Location = new System.Drawing.Point(133, 42);
            this.txtRotCenter.Name = "txtRotCenter";
            this.txtRotCenter.Size = new System.Drawing.Size(78, 20);
            this.txtRotCenter.TabIndex = 67;
            this.txtRotCenter.Visible = false;
            // 
            // numRotate
            // 
            this.numRotate.Location = new System.Drawing.Point(6, 19);
            this.numRotate.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numRotate.Name = "numRotate";
            this.numRotate.Size = new System.Drawing.Size(61, 20);
            this.numRotate.TabIndex = 53;
            this.numRotate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkCenterRotate
            // 
            this.chkCenterRotate.AutoSize = true;
            this.chkCenterRotate.Checked = true;
            this.chkCenterRotate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCenterRotate.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCenterRotate.Location = new System.Drawing.Point(7, 45);
            this.chkCenterRotate.Name = "chkCenterRotate";
            this.chkCenterRotate.Size = new System.Drawing.Size(78, 19);
            this.chkCenterRotate.TabIndex = 66;
            this.chkCenterRotate.Text = "Centered";
            this.chkCenterRotate.UseVisualStyleBackColor = true;
            // 
            // chkRotateImage
            // 
            this.chkRotateImage.AutoSize = true;
            this.chkRotateImage.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRotateImage.Location = new System.Drawing.Point(138, 18);
            this.chkRotateImage.Name = "chkRotateImage";
            this.chkRotateImage.Size = new System.Drawing.Size(76, 19);
            this.chkRotateImage.TabIndex = 65;
            this.chkRotateImage.Text = "All Image";
            this.chkRotateImage.UseVisualStyleBackColor = true;
            // 
            // grpTrapezium
            // 
            this.grpTrapezium.Controls.Add(this.numSizeDiff);
            this.grpTrapezium.Controls.Add(this.label11);
            this.grpTrapezium.Controls.Add(this.numSize);
            this.grpTrapezium.Controls.Add(this.lblSize);
            this.grpTrapezium.Location = new System.Drawing.Point(6, 108);
            this.grpTrapezium.Name = "grpTrapezium";
            this.grpTrapezium.Size = new System.Drawing.Size(216, 59);
            this.grpTrapezium.TabIndex = 68;
            this.grpTrapezium.TabStop = false;
            this.grpTrapezium.Text = "Trapezium";
            this.grpTrapezium.Visible = false;
            // 
            // numSizeDiff
            // 
            this.numSizeDiff.Location = new System.Drawing.Point(145, 18);
            this.numSizeDiff.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numSizeDiff.Name = "numSizeDiff";
            this.numSizeDiff.Size = new System.Drawing.Size(52, 20);
            this.numSizeDiff.TabIndex = 68;
            this.numSizeDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(104, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 15);
            this.label11.TabIndex = 69;
            this.label11.Text = "Size -";
            // 
            // numSize
            // 
            this.numSize.Location = new System.Drawing.Point(40, 19);
            this.numSize.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numSize.Name = "numSize";
            this.numSize.Size = new System.Drawing.Size(52, 20);
            this.numSize.TabIndex = 66;
            this.numSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSize.Location = new System.Drawing.Point(6, 23);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(28, 15);
            this.lblSize.TabIndex = 67;
            this.lblSize.Text = "Size";
            // 
            // grpPts
            // 
            this.grpPts.Controls.Add(this.btnPlot);
            this.grpPts.Controls.Add(this.txtPts);
            this.grpPts.Controls.Add(this.pnlBezier);
            this.grpPts.Controls.Add(this.grpFonts);
            this.grpPts.Location = new System.Drawing.Point(9, 5);
            this.grpPts.Name = "grpPts";
            this.grpPts.Size = new System.Drawing.Size(293, 99);
            this.grpPts.TabIndex = 43;
            this.grpPts.TabStop = false;
            this.grpPts.Text = "Click Gws Drawing Area to select point";
            this.grpPts.Visible = false;
            // 
            // btnPlot
            // 
            this.btnPlot.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnPlot.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPlot.ForeColor = System.Drawing.Color.Black;
            this.btnPlot.Location = new System.Drawing.Point(262, 16);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(26, 49);
            this.btnPlot.TabIndex = 14;
            this.btnPlot.Text = "6";
            this.btnPlot.UseVisualStyleBackColor = false;
            // 
            // txtPts
            // 
            this.txtPts.Location = new System.Drawing.Point(11, 18);
            this.txtPts.Multiline = true;
            this.txtPts.Name = "txtPts";
            this.txtPts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPts.Size = new System.Drawing.Size(252, 46);
            this.txtPts.TabIndex = 12;
            // 
            // pnlBezier
            // 
            this.pnlBezier.Controls.Add(this.cmbBezier);
            this.pnlBezier.Controls.Add(this.label2);
            this.pnlBezier.Location = new System.Drawing.Point(121, 68);
            this.pnlBezier.Name = "pnlBezier";
            this.pnlBezier.Size = new System.Drawing.Size(167, 27);
            this.pnlBezier.TabIndex = 56;
            this.pnlBezier.Visible = false;
            // 
            // cmbBezier
            // 
            this.cmbBezier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBezier.FormattingEnabled = true;
            this.cmbBezier.Location = new System.Drawing.Point(46, 3);
            this.cmbBezier.Name = "cmbBezier";
            this.cmbBezier.Size = new System.Drawing.Size(118, 21);
            this.cmbBezier.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 29;
            this.label2.Text = "Type";
            // 
            // grpFonts
            // 
            this.grpFonts.Controls.Add(this.numFontSize);
            this.grpFonts.Controls.Add(this.btnOpenFont);
            this.grpFonts.Controls.Add(this.label6);
            this.grpFonts.Controls.Add(this.label10);
            this.grpFonts.Location = new System.Drawing.Point(9, 68);
            this.grpFonts.Name = "grpFonts";
            this.grpFonts.Size = new System.Drawing.Size(274, 27);
            this.grpFonts.TabIndex = 65;
            this.grpFonts.Visible = false;
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(227, 3);
            this.numFontSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(46, 20);
            this.numFontSize.TabIndex = 58;
            this.numFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // btnOpenFont
            // 
            this.btnOpenFont.BackColor = System.Drawing.Color.Black;
            this.btnOpenFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFont.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFont.ForeColor = System.Drawing.Color.White;
            this.btnOpenFont.Location = new System.Drawing.Point(145, 1);
            this.btnOpenFont.Name = "btnOpenFont";
            this.btnOpenFont.Size = new System.Drawing.Size(45, 26);
            this.btnOpenFont.TabIndex = 65;
            this.btnOpenFont.Text = "File";
            this.btnOpenFont.UseVisualStyleBackColor = false;
            this.btnOpenFont.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 29;
            this.label6.Text = "Glyphs";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(198, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 15);
            this.label10.TabIndex = 59;
            this.label10.Text = "Size";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbStroke);
            this.groupBox1.Controls.Add(this.cmbGradient);
            this.groupBox1.Controls.Add(this.lstColors);
            this.groupBox1.Controls.Add(this.lstKnownColors);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(536, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 158);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fill";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 15);
            this.label9.TabIndex = 39;
            this.label9.Text = "Option";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 37;
            this.label3.Text = "Mode";
            // 
            // cmbStroke
            // 
            this.cmbStroke.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStroke.FormattingEnabled = true;
            this.cmbStroke.Location = new System.Drawing.Point(57, 38);
            this.cmbStroke.Name = "cmbStroke";
            this.cmbStroke.Size = new System.Drawing.Size(181, 21);
            this.cmbStroke.TabIndex = 59;
            // 
            // cmbGradient
            // 
            this.cmbGradient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradient.FormattingEnabled = true;
            this.cmbGradient.Location = new System.Drawing.Point(57, 12);
            this.cmbGradient.Name = "cmbGradient";
            this.cmbGradient.Size = new System.Drawing.Size(181, 21);
            this.cmbGradient.TabIndex = 38;
            // 
            // lstColors
            // 
            this.lstColors.AllowDrop = true;
            this.lstColors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstColors.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstColors.FormattingEnabled = true;
            this.lstColors.Location = new System.Drawing.Point(6, 68);
            this.lstColors.Name = "lstColors";
            this.lstColors.Size = new System.Drawing.Size(110, 80);
            this.lstColors.TabIndex = 68;
            // 
            // lstKnownColors
            // 
            this.lstKnownColors.AllowDrop = true;
            this.lstKnownColors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstKnownColors.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstKnownColors.FormattingEnabled = true;
            this.lstKnownColors.Location = new System.Drawing.Point(139, 68);
            this.lstKnownColors.Name = "lstKnownColors";
            this.lstKnownColors.Size = new System.Drawing.Size(99, 80);
            this.lstKnownColors.TabIndex = 69;
            this.lstKnownColors.SelectedIndexChanged += new System.EventHandler(this.lstKnownColors_SelectedIndexChanged);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(316, 134);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(106, 31);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // chkAA
            // 
            this.chkAA.AutoSize = true;
            this.chkAA.Checked = true;
            this.chkAA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAA.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAA.Location = new System.Drawing.Point(230, 150);
            this.chkAA.Name = "chkAA";
            this.chkAA.Size = new System.Drawing.Size(72, 19);
            this.chkAA.TabIndex = 42;
            this.chkAA.Text = "Antialias";
            this.chkAA.UseVisualStyleBackColor = true;
            // 
            // btnDraw
            // 
            this.btnDraw.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDraw.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDraw.ForeColor = System.Drawing.Color.Black;
            this.btnDraw.Location = new System.Drawing.Point(429, 134);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(101, 31);
            this.btnDraw.TabIndex = 8;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = false;
            // 
            // grpXY
            // 
            this.grpXY.Controls.Add(this.pnlArcE);
            this.grpXY.Controls.Add(this.Xlabel);
            this.grpXY.Controls.Add(this.Ylabel);
            this.grpXY.Controls.Add(this.lblH);
            this.grpXY.Controls.Add(this.numX);
            this.grpXY.Controls.Add(this.numY);
            this.grpXY.Controls.Add(this.numW);
            this.grpXY.Controls.Add(this.numH);
            this.grpXY.Controls.Add(this.widthLabel);
            this.grpXY.Controls.Add(this.pnlArcS);
            this.grpXY.Controls.Add(this.pnlRoundRC);
            this.grpXY.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpXY.Location = new System.Drawing.Point(11, 4);
            this.grpXY.Name = "grpXY";
            this.grpXY.Size = new System.Drawing.Size(293, 96);
            this.grpXY.TabIndex = 44;
            this.grpXY.TabStop = false;
            this.grpXY.Text = "Enter Circle parameters";
            // 
            // Xlabel
            // 
            this.Xlabel.AutoSize = true;
            this.Xlabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xlabel.Location = new System.Drawing.Point(13, 17);
            this.Xlabel.Name = "Xlabel";
            this.Xlabel.Size = new System.Drawing.Size(14, 15);
            this.Xlabel.TabIndex = 15;
            this.Xlabel.Text = "X";
            // 
            // Ylabel
            // 
            this.Ylabel.AutoSize = true;
            this.Ylabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ylabel.Location = new System.Drawing.Point(67, 17);
            this.Ylabel.Name = "Ylabel";
            this.Ylabel.Size = new System.Drawing.Size(13, 15);
            this.Ylabel.TabIndex = 19;
            this.Ylabel.Text = "Y";
            // 
            // lblH
            // 
            this.lblH.AutoSize = true;
            this.lblH.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblH.Location = new System.Drawing.Point(239, 17);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(42, 15);
            this.lblH.TabIndex = 20;
            this.lblH.Text = "Height";
            this.lblH.Visible = false;
            // 
            // numX
            // 
            this.numX.Location = new System.Drawing.Point(9, 36);
            this.numX.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(55, 23);
            this.numX.TabIndex = 31;
            this.numX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numX.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numY
            // 
            this.numY.Location = new System.Drawing.Point(70, 35);
            this.numY.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(52, 23);
            this.numY.TabIndex = 32;
            this.numY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numY.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numW
            // 
            this.numW.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numW.Location = new System.Drawing.Point(167, 36);
            this.numW.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numW.Name = "numW";
            this.numW.Size = new System.Drawing.Size(55, 23);
            this.numW.TabIndex = 33;
            this.numW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numW.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // pnlArcE
            // 
            this.pnlArcE.Controls.Add(this.lblArce);
            this.pnlArcE.Controls.Add(this.numArcE);
            this.pnlArcE.Location = new System.Drawing.Point(143, 64);
            this.pnlArcE.Name = "pnlArcE";
            this.pnlArcE.Size = new System.Drawing.Size(142, 27);
            this.pnlArcE.TabIndex = 52;
            this.pnlArcE.Visible = false;
            // 
            // lblArce
            // 
            this.lblArce.AutoSize = true;
            this.lblArce.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArce.Location = new System.Drawing.Point(8, 4);
            this.lblArce.Name = "lblArce";
            this.lblArce.Size = new System.Drawing.Size(60, 15);
            this.lblArce.TabIndex = 49;
            this.lblArce.Text = "End Angle";
            // 
            // numArcE
            // 
            this.numArcE.Location = new System.Drawing.Point(76, 2);
            this.numArcE.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numArcE.Name = "numArcE";
            this.numArcE.Size = new System.Drawing.Size(64, 23);
            this.numArcE.TabIndex = 48;
            this.numArcE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numArcE.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // numH
            // 
            this.numH.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numH.Location = new System.Drawing.Point(228, 37);
            this.numH.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numH.Name = "numH";
            this.numH.Size = new System.Drawing.Size(57, 23);
            this.numH.TabIndex = 34;
            this.numH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numH.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numH.Visible = false;
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.widthLabel.Location = new System.Drawing.Point(186, 17);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(41, 15);
            this.widthLabel.TabIndex = 16;
            this.widthLabel.Text = "Width";
            // 
            // pnlArcS
            // 
            this.pnlArcS.Controls.Add(this.numArcS);
            this.pnlArcS.Controls.Add(this.lblArcs);
            this.pnlArcS.Location = new System.Drawing.Point(9, 63);
            this.pnlArcS.Name = "pnlArcS";
            this.pnlArcS.Size = new System.Drawing.Size(125, 28);
            this.pnlArcS.TabIndex = 52;
            this.pnlArcS.Visible = false;
            // 
            // numArcS
            // 
            this.numArcS.Location = new System.Drawing.Point(71, 2);
            this.numArcS.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numArcS.Name = "numArcS";
            this.numArcS.Size = new System.Drawing.Size(49, 23);
            this.numArcS.TabIndex = 27;
            this.numArcS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArcs
            // 
            this.lblArcs.AutoSize = true;
            this.lblArcs.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArcs.Location = new System.Drawing.Point(4, 7);
            this.lblArcs.Name = "lblArcs";
            this.lblArcs.Size = new System.Drawing.Size(66, 15);
            this.lblArcs.TabIndex = 29;
            this.lblArcs.Text = "Start Angle";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(514, 399);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 45;
            this.label7.Text = "Microsoft";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 399);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 18);
            this.label8.TabIndex = 46;
            this.label8.Text = "MnM GWS";
            // 
            // picGWS
            // 
            this.picGWS.BackColor = System.Drawing.Color.White;
            this.picGWS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picGWS.Location = new System.Drawing.Point(3, 23);
            this.picGWS.Name = "picGWS";
            this.picGWS.Size = new System.Drawing.Size(505, 373);
            this.picGWS.TabIndex = 0;
            this.picGWS.TabStop = false;
            // 
            // picMS
            // 
            this.picMS.BackColor = System.Drawing.Color.White;
            this.picMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picMS.Location = new System.Drawing.Point(514, 23);
            this.picMS.Name = "picMS";
            this.picMS.Size = new System.Drawing.Size(505, 373);
            this.picMS.TabIndex = 1;
            this.picMS.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.picMS, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.picGWS, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblGWS, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblMS, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1022, 419);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tblpnl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(1022, 603);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "ofdlg";
            // 
            // pnlRoundRC
            // 
            this.pnlRoundRC.Controls.Add(this.label1);
            this.pnlRoundRC.Controls.Add(this.numCornerRadius);
            this.pnlRoundRC.Location = new System.Drawing.Point(9, 62);
            this.pnlRoundRC.Name = "pnlRoundRC";
            this.pnlRoundRC.Size = new System.Drawing.Size(274, 29);
            this.pnlRoundRC.TabIndex = 54;
            this.pnlRoundRC.Visible = false;
            // 
            // numCornerRadius
            // 
            this.numCornerRadius.Location = new System.Drawing.Point(219, 3);
            this.numCornerRadius.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numCornerRadius.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCornerRadius.Name = "numCornerRadius";
            this.numCornerRadius.Size = new System.Drawing.Size(57, 23);
            this.numCornerRadius.TabIndex = 50;
            this.numCornerRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numCornerRadius.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(126, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 55;
            this.label1.Text = "Corner Radius";
            // 
            // Comparison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 603);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Comparison";
            this.Text = "GWS vs MS Demo";
            this.tblpnl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpArcPie.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numStroke)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRotate)).EndInit();
            this.grpTrapezium.ResumeLayout(false);
            this.grpTrapezium.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSizeDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).EndInit();
            this.grpPts.ResumeLayout(false);
            this.grpPts.PerformLayout();
            this.pnlBezier.ResumeLayout(false);
            this.pnlBezier.PerformLayout();
            this.grpFonts.ResumeLayout(false);
            this.grpFonts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpXY.ResumeLayout(false);
            this.grpXY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).EndInit();
            this.pnlArcE.ResumeLayout(false);
            this.pnlArcE.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArcE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            this.pnlArcS.ResumeLayout(false);
            this.pnlArcS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArcS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGWS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMS)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlRoundRC.ResumeLayout(false);
            this.pnlRoundRC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCornerRadius)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ColorDialog cldg;
        private System.Windows.Forms.Label lblMS;
        private System.Windows.Forms.Label lblGWS;
        private System.Windows.Forms.TableLayoutPanel tblpnl1;
        private System.Windows.Forms.PictureBox picGWS;
        private System.Windows.Forms.PictureBox picMS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkAA;
        private System.Windows.Forms.ComboBox cmbGradient;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.GroupBox grpXY;
        private System.Windows.Forms.NumericUpDown numX;
        private System.Windows.Forms.Label Ylabel;
        private System.Windows.Forms.Label lblH;
        private System.Windows.Forms.Label Xlabel;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numH;
        private System.Windows.Forms.NumericUpDown numW;
        private System.Windows.Forms.GroupBox grpPts;
        private System.Windows.Forms.TextBox txtPts;
        private System.Windows.Forms.Panel pnlArcE;
        private System.Windows.Forms.Label lblArce;
        private System.Windows.Forms.NumericUpDown numArcE;
        private Panel pnlArcS;
        private NumericUpDown numArcS;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Button btnClear;
        private TableLayoutPanel tableLayoutPanel2;
        private SplitContainer splitContainer1;
        private NumericUpDown numRotate;
        private NumericUpDown numStroke;
        private ComboBox cmbStroke;
        private GroupBox groupBox1;
        private Label label9;
        private Panel pnlBezier;
        private ComboBox cmbBezier;
        private Label label2;
        private Panel grpFonts;
        private Label label10;
        private NumericUpDown numFontSize;
        private Label label6;
        private Button btnOpenFont;
        private OpenFileDialog openFileDialog1;
        private CheckBox chkRotateImage;
        private Label lblSize;
        private NumericUpDown numSize;
        private GroupBox grpTrapezium;
        private NumericUpDown numSizeDiff;
        private Label label11;
        private CheckBox chkCenterRotate;
        private ListBox lstKnownColors;
        private ListBox lstColors;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private GroupBox grpArcPie;
        private CheckedListBox lstCurveOption;
        private GroupBox groupBox4;
        private TextBox txtRotCenter;
        private CheckBox chkSkew;
        private Panel pnlRoundRC;
        private Label label1;
        private NumericUpDown numCornerRadius;
        private Label lblArcs;
        #endregion

        private void lstKnownColors_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }
    }
}

namespace GWSDemoMSFT
{
    public partial class Comparison : Form
    {
        #region variables
        string manualFontPath;
        bool stopDraw = true;
        IGraphics mnmCanvas;
        float x, y, w, h;
        float startA, endA;
        int[] drawPoints;
        Angle rotate;
        MnM.GWS.FillMode pse;
        CurveType curveType = 0;
        static System.Drawing.Font ptFont = new System.Drawing. Font("Verdana", 10);
        System.Drawing.Font msFont;
        IFont gwsFont;

        #endregion

        #region constructors
        static Comparison()
        {
        }
        public Comparison()
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
            
            
            var copts =  Enum.GetValues(typeof(CurveType));
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
            btnDraw.Click += draw;
            btnClear.Click += BtnClear_Click;
            btnPlot.Click += BtnPlot_Click;
            ResizeEnd += GWSDemo_Resize;
            picMS.Paint += PicMS_Paint;
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
            if(lstKnownColors.SelectedIndex!=-1)
                lstColors.Items.Add(lstKnownColors.SelectedItem);
        }

        private void LstKnownColors_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;
            var c = (System.Drawing.Color)(sender as ListBox).Items[e.Index];
            var b =new SolidBrush(c);
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
            var result =  openFileDialog1.ShowDialog(this);
            if(result == DialogResult.OK)
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
                System.Windows.Forms.MessageBox.Show("Provide valid path for font file!");
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
                lblGWS.Text = MnM.GWS.Implementation.BenchMark(() =>
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

        private void PicMS_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (stopDraw)
                return;
            SetDrawingParams(e.Graphics, out System.Drawing.Brush brush);
            var microsoftMethod = SetVoidMethod(e.Graphics, brush);

            if (rotate.Valid)
            {
                e.Graphics.RotateTransform(rotate.Degree);
                e.Graphics.TranslateTransform(rotate.CX, rotate.CY);
            }
            lblMS.Text = MnM.GWS.Implementation.BenchMark(microsoftMethod, out long j, cmbShape.Text + "", BenchmarkUnit.Tick);

            if (rotate.Valid)
            {
                e.Graphics.TranslateTransform(-rotate.CX, -rotate.CY);

                e.Graphics.ResetTransform();
            }
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
            picMS.Refresh();
            stopDraw = true;
        }

        #endregion

        #region CHANGING SHAPE TYPE
        private void changeShape(object sender, System.EventArgs e)
        {
            //txtPts.Clear();
            stopDraw = true;
            picGWS.Refresh();
            picMS.Refresh();
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
                    if (cmbShape.Text == "Arc" || cmbShape.Text=="Pie")
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
                    if (cmbShape.Text == "RoundedArea")
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

                    grpPts.Visible = true;
                    grpXY.Visible = false;
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
            picMS.Refresh();
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

            VoidMethod gwsMethod = null;

            Renderer.ReadContext = fStyle;
            var stroke = (float)numStroke.Value;
            Renderer.CopySettings(pse, stroke, StrokeMode.Middle, chkAA.Checked ? LineDraw.AA : LineDraw.NonAA);
            //mnmCanvas.ApplyBackground(fStyle);
            curveType = 0;
            foreach (var item in lstCurveOption.CheckedItems)
            {
                curveType |= (CurveType)item;
            }
            IDrawStyle ds = new DrawStyle();
            ds.Angle = rotate;
            ds.LineHeight = (gwsFont?.Info?.LineHeight ?? 0).Ceiling();

            switch (cmbShape.Text)
            {
                case "Line":
                    gwsMethod = () => mnmCanvas.DrawLines(true, rotate, drawPoints);
                    break;
                case "Trapezium":
                    gwsMethod = () => mnmCanvas.DrawTrapezium(
                        new float[] {drawPoints[0], drawPoints[1], drawPoints[2], drawPoints[3],
                        (float) numSize.Value, (float)numSizeDiff.Value} , null, rotate);
                    break;

                case "Circle":
                    if (drawPoints?.Length >= 4)
                    {
                        gwsMethod = () => mnmCanvas.DrawCircle(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), null, rotate);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawEllipse(x, y, w, w, null, rotate);

                    break;
                case "Ellipse":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawEllipse(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), null, rotate);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawEllipse(x, y, w, h, null, rotate);
                    break;
                case "Arc":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawArc(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), null, rotate, curveType);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawArc(x, y, w, h, startA, endA, null, rotate, curveType);
                    break;
                case "Pie":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawPie(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), null, rotate, curveType);
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawPie(x, y, w, h, startA, endA, null, rotate, curveType);
                    break;

                case "BezierArc":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawBezierArc(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), null, rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawBezierArc(x, y, w, h, startA, endA, null, rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    break;
                case "BezierPie":
                    if (drawPoints?.Length >= 6)
                    {
                        gwsMethod = () => mnmCanvas.DrawBezierPie(new MnM.GWS.PointF(drawPoints[0], drawPoints[1]),
                            new MnM.GWS.PointF(drawPoints[2], drawPoints[3]), new MnM.GWS.PointF(drawPoints[4], drawPoints[5]), null, rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    }
                    else
                        gwsMethod = () => mnmCanvas.DrawBezierPie(x, y, w, h, startA, endA, null, rotate, curveType.HasFlag(CurveType.NoSweepAngle));
                    break;

                case "Square":
                    gwsMethod = () => mnmCanvas.DrawRhombus(x, y, w, w, rotate);
                    break;
                case "Rectangle":
                    gwsMethod = () => mnmCanvas.DrawRectangle(x, y, w, h, null, rotate);
                    break;

                case "RoundedArea":
                    gwsMethod = () => mnmCanvas.DrawRoundedBox(x, y, w, h, (float)numCornerRadius.Value, rotate);
                    break;

                case "Rhombus":
                    gwsMethod = () => mnmCanvas.DrawRhombus(x, y, w, h, rotate);

                    break;
                case "Triangle":
                    if (drawPoints == null || drawPoints.Length < 6)
                        return null;
                    gwsMethod = () => mnmCanvas.DrawTriangle(drawPoints[0], drawPoints[1], 
                        drawPoints[2], drawPoints[3], drawPoints[4], drawPoints[5], null ,rotate);
                    break;

                case "Polygon":
                    if (drawPoints == null)
                        return null;
                    gwsMethod = () => mnmCanvas.DrawPolygon(rotate, drawPoints);
                    break;
                case "Bezier":
                    var type = cmbBezier.SelectedIndex != -1 ?
                        ((KeyValuePair<string, BezierType>)cmbBezier.SelectedItem).Value : BezierType.Cubic;
                    if (type.HasFlag(BezierType.Multiple))
                    {
                        if (drawPoints == null || drawPoints.Length < 6)
                            return null;
                        gwsMethod = () => mnmCanvas.DrawBezier(type, rotate, drawPoints);

                    }
                    else
                    {
                        if (drawPoints == null)
                            return null; 
                        gwsMethod = () => mnmCanvas.DrawBezier(type, rotate, drawPoints);
                    }
                    break;
                case "Glyphs":
                    gwsMethod = () => mnmCanvas.DrawText(gwsFont, 130, 130 + gwsFont.Size, txtPts.Text, ds);
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
                rotate = new Angle((float)numRotate.Value,x, y, chkSkew.Checked);

            }

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
                            var dist = Math.Sqrt(MathHelper.Sqr( drawPoints[0] - drawPoints[2]) + MathHelper.Sqr( drawPoints[1] - drawPoints[3]));
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
        VoidMethod SetVoidMethod(System.Drawing.Graphics msGraphics, System.Drawing.Brush brush)
        {
            VoidMethod microsoftMethod = null;
            var stroke = (float)numStroke.Value;
            //determine shape drawing methods
            if (mnmCanvas == null)
                return null;

            switch (cmbShape.Text)
            {
                case "Line":
                    if (drawPoints == null)
                        return null;
                    microsoftMethod = () => msGraphics.DrawLine(new Pen(brush, stroke), drawPoints[0], drawPoints[1], drawPoints[2], drawPoints[3]);
                    break;
                case "Circle":
                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillEllipse(brush, x, y, w, w);
                    else
                        microsoftMethod = () => msGraphics.DrawEllipse(new Pen(brush, stroke), x, y, w, w);

                    break;
                case "Ellipse":
                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillEllipse(brush, x, y, w, h);
                    else
                        microsoftMethod = () => msGraphics.DrawEllipse(new Pen(brush, stroke), x, y, w, h);
                    break;
                case "Arc":
                case "BezierArc":
                    microsoftMethod = () => msGraphics.DrawArc(new Pen(brush, stroke), x, y, w, h, startA, endA);
                    break;
                case "Pie":
                case "BezierPie":
                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillPie(brush, x, y, w, h, startA, endA);
                    else
                        microsoftMethod = () => msGraphics.DrawPie(new Pen(brush, stroke), x, y, w, h, startA, endA); break;
                case "Square":
                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillRectangle(brush, x, y, w, w);
                    else
                        microsoftMethod = () => msGraphics.DrawRectangle(new Pen(brush, stroke), x, y, w, w);
                    break;
                case "Rectangle":
                    var loc = new System.Drawing.PointF(x, y);

                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillRectangle(brush, new System.Drawing.RectangleF(loc, new System.Drawing.SizeF(w, h)));
                    else
                    {
                        microsoftMethod = () => msGraphics.DrawRectangle(new Pen(brush), x, y, w, h);
                    }
                    break;
                case "Triangle":
                    if (drawPoints == null || drawPoints.Length < 6)
                        return null;
                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillPolygon(brush, Program.ToPointsF(drawPoints));
                    else
                        microsoftMethod = () => msGraphics.DrawPolygon(new Pen(brush, stroke), Program.ToPointsF(drawPoints));
                    break;

                case "Polygon":
                    if (drawPoints == null)
                        return null;
                    if (pse == MnM.GWS.FillMode.Outer || pse == MnM.GWS.FillMode.Original)
                        microsoftMethod = () => msGraphics.FillPolygon(brush, Program.ToPointsF(drawPoints));
                    else
                        microsoftMethod = () => msGraphics.DrawPolygon(new Pen(brush, stroke), Program.ToPointsF(drawPoints));
                    break;
                case "Bezier":
                    var type = cmbBezier.SelectedIndex != -1 ?
                        ((KeyValuePair<string, BezierType>)cmbBezier.SelectedItem).Value : BezierType.Cubic;
                    if (type.HasFlag(BezierType.Multiple))
                    {
                        if (drawPoints == null || drawPoints.Length < 6)
                            return null;
                        microsoftMethod = () => msGraphics.DrawBeziers(new Pen(brush, stroke), drawPoints.ToPointsF(true));

                    }
                    else
                    {
                        if (drawPoints == null || drawPoints.Length < 8)
                            return null;
                        microsoftMethod = () => msGraphics.DrawBezier(new Pen(brush, stroke),
                            drawPoints[0], drawPoints[1], drawPoints[2], drawPoints[3], drawPoints[4], drawPoints[5], drawPoints[6], drawPoints[7]);
                    }
                    break;
                case "Glyphs":
                    microsoftMethod = () => msGraphics.DrawString( txtPts.Text, msFont, brush, new System.Drawing.PointF(10, 10));

                    break;
            }
            return microsoftMethod;
        }
        void SetDrawingParams(System.Drawing.Graphics msGraphics, out System.Drawing. Brush brush)
        {
            msGraphics.SmoothingMode = chkAA.Checked ? 
                SmoothingMode.AntiAlias : SmoothingMode.Default;
            Enum.TryParse(cmbGradient.SelectedItem + "", true, out LinearGradientMode msgrad);
            //create an instance of the Microst gradient fill brush 

            object[] all = null;
            if(lstColors.Items.Count>0)
            {
                all = new object[lstColors.Items.Count];
                lstColors.Items.CopyTo(all, 0);
            }

            if(all==null)
            {
                all = new object[] { System.Drawing.Color.Red, System.Drawing.Color.Black };
            }
            var colors = all.Select(x => (System.Drawing.Color)x).ToList();


            var lbrush = new LinearGradientBrush(new System.Drawing.RectangleF(x, y, w + 1, h + 1),
                colors[0], colors[colors.Count-1], msgrad);
            brush = lbrush;
        }
        
        private void setColor(object sender, System.EventArgs e)
        {
            cldg.ShowDialog();
            (sender as System.Windows.Forms.Button).BackColor = cldg.Color;
        }
        #endregion
    }
}
