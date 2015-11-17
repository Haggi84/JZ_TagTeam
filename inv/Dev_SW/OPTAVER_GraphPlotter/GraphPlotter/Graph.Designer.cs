namespace GraphPlotter
{
    partial class Graph
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graph));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnClipboard = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnZoomInX = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOutX = new System.Windows.Forms.ToolStripButton();
            this.btnZoomInY = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOutY = new System.Windows.Forms.ToolStripButton();
            this.btnGrids = new System.Windows.Forms.ToolStripButton();
            this.btnText = new System.Windows.Forms.ToolStripButton();
            this.btnRectangularMode = new System.Windows.Forms.ToolStripButton();
            this.btnPolarMode = new System.Windows.Forms.ToolStripButton();
            this.btnIncreaseSensitivity = new System.Windows.Forms.ToolStripButton();
            this.btnDecreaseSensitivity = new System.Windows.Forms.ToolStripButton();
            this.btnShowOrigin = new System.Windows.Forms.ToolStripButton();
            this.btnDefault = new System.Windows.Forms.ToolStripButton();
            this.lstExpressions = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblPosition = new System.Windows.Forms.Label();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.lblSensitivity = new System.Windows.Forms.Label();
            this.expPlotter = new ExpressionPlotterControl.ExpressionPlotter();
            this.frm_values_insertion = new System.Windows.Forms.Button();
            this.btn_Export_Profile = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnClipboard,
            this.btnZoomIn,
            this.btnZoomOut,
            this.btnZoomInX,
            this.btnZoomOutX,
            this.btnZoomInY,
            this.btnZoomOutY,
            this.btnGrids,
            this.btnText,
            this.btnRectangularMode,
            this.btnPolarMode,
            this.btnIncreaseSensitivity,
            this.btnDecreaseSensitivity,
            this.btnShowOrigin,
            this.btnDefault});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(693, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(36, 36);
            this.btnSave.Text = "toolStripButton1";
            this.btnSave.ToolTipText = "Save graph";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClipboard
            // 
            this.btnClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClipboard.Image = ((System.Drawing.Image)(resources.GetObject("btnClipboard.Image")));
            this.btnClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClipboard.Name = "btnClipboard";
            this.btnClipboard.Size = new System.Drawing.Size(36, 36);
            this.btnClipboard.Text = "toolStripButton1";
            this.btnClipboard.ToolTipText = "Copy graph to clipboard";
            this.btnClipboard.Click += new System.EventHandler(this.btnClipboard_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(36, 36);
            this.btnZoomIn.Text = "Zoom In";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(36, 36);
            this.btnZoomOut.Text = "Zoom Out";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomInX
            // 
            this.btnZoomInX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomInX.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomInX.Image")));
            this.btnZoomInX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomInX.Name = "btnZoomInX";
            this.btnZoomInX.Size = new System.Drawing.Size(36, 36);
            this.btnZoomInX.Text = "Zoom-in X-Axis";
            this.btnZoomInX.Click += new System.EventHandler(this.btnZoomInX_Click);
            // 
            // btnZoomOutX
            // 
            this.btnZoomOutX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOutX.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOutX.Image")));
            this.btnZoomOutX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOutX.Name = "btnZoomOutX";
            this.btnZoomOutX.Size = new System.Drawing.Size(36, 36);
            this.btnZoomOutX.Text = "Zoom-out X-Axis";
            this.btnZoomOutX.Click += new System.EventHandler(this.btnZoomOutX_Click);
            // 
            // btnZoomInY
            // 
            this.btnZoomInY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomInY.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomInY.Image")));
            this.btnZoomInY.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomInY.Name = "btnZoomInY";
            this.btnZoomInY.Size = new System.Drawing.Size(36, 36);
            this.btnZoomInY.Text = "Zoom-in Y-Axis";
            this.btnZoomInY.Click += new System.EventHandler(this.btnZoomInY_Click);
            // 
            // btnZoomOutY
            // 
            this.btnZoomOutY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOutY.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOutY.Image")));
            this.btnZoomOutY.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOutY.Name = "btnZoomOutY";
            this.btnZoomOutY.Size = new System.Drawing.Size(36, 36);
            this.btnZoomOutY.Text = "Zoom-out Y-Axis";
            this.btnZoomOutY.Click += new System.EventHandler(this.btnZoomOutY_Click);
            // 
            // btnGrids
            // 
            this.btnGrids.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGrids.Image = ((System.Drawing.Image)(resources.GetObject("btnGrids.Image")));
            this.btnGrids.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGrids.Name = "btnGrids";
            this.btnGrids.Size = new System.Drawing.Size(36, 36);
            this.btnGrids.Text = "toolStripButton1";
            this.btnGrids.ToolTipText = "Toggle Grids";
            this.btnGrids.Click += new System.EventHandler(this.btnGrids_Click);
            // 
            // btnText
            // 
            this.btnText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnText.Image = ((System.Drawing.Image)(resources.GetObject("btnText.Image")));
            this.btnText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(36, 36);
            this.btnText.Text = "toolStripButton1";
            this.btnText.ToolTipText = "Toggle expressions text";
            this.btnText.Click += new System.EventHandler(this.btnText_Click);
            // 
            // btnRectangularMode
            // 
            this.btnRectangularMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRectangularMode.Image = ((System.Drawing.Image)(resources.GetObject("btnRectangularMode.Image")));
            this.btnRectangularMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRectangularMode.Name = "btnRectangularMode";
            this.btnRectangularMode.Size = new System.Drawing.Size(36, 36);
            this.btnRectangularMode.Text = "Rectangular Mode";
            this.btnRectangularMode.Click += new System.EventHandler(this.btnRectangularMode_Click);
            // 
            // btnPolarMode
            // 
            this.btnPolarMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPolarMode.Image = ((System.Drawing.Image)(resources.GetObject("btnPolarMode.Image")));
            this.btnPolarMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPolarMode.Name = "btnPolarMode";
            this.btnPolarMode.Size = new System.Drawing.Size(36, 36);
            this.btnPolarMode.Text = "Polar Mode";
            this.btnPolarMode.Click += new System.EventHandler(this.btnPolarMode_Click);
            // 
            // btnIncreaseSensitivity
            // 
            this.btnIncreaseSensitivity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnIncreaseSensitivity.Image = ((System.Drawing.Image)(resources.GetObject("btnIncreaseSensitivity.Image")));
            this.btnIncreaseSensitivity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIncreaseSensitivity.Name = "btnIncreaseSensitivity";
            this.btnIncreaseSensitivity.Size = new System.Drawing.Size(36, 36);
            this.btnIncreaseSensitivity.Text = "Increase Sensitivity for Polar mode";
            this.btnIncreaseSensitivity.Click += new System.EventHandler(this.btnIncreaseSensitivity_Click);
            // 
            // btnDecreaseSensitivity
            // 
            this.btnDecreaseSensitivity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDecreaseSensitivity.Image = ((System.Drawing.Image)(resources.GetObject("btnDecreaseSensitivity.Image")));
            this.btnDecreaseSensitivity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDecreaseSensitivity.Name = "btnDecreaseSensitivity";
            this.btnDecreaseSensitivity.Size = new System.Drawing.Size(36, 36);
            this.btnDecreaseSensitivity.Text = "Decrease Sensitivity for Polar mode";
            this.btnDecreaseSensitivity.Click += new System.EventHandler(this.btnDecreaseSensitivity_Click);
            // 
            // btnShowOrigin
            // 
            this.btnShowOrigin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowOrigin.Image = ((System.Drawing.Image)(resources.GetObject("btnShowOrigin.Image")));
            this.btnShowOrigin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowOrigin.Name = "btnShowOrigin";
            this.btnShowOrigin.Size = new System.Drawing.Size(36, 36);
            this.btnShowOrigin.Text = "Return the graph to origin";
            this.btnShowOrigin.Visible = false;
            this.btnShowOrigin.Click += new System.EventHandler(this.btnShowOrigin_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDefault.Image = ((System.Drawing.Image)(resources.GetObject("btnDefault.Image")));
            this.btnDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(36, 36);
            this.btnDefault.Text = "Default graph(reset all scales)";
            this.btnDefault.Visible = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // lstExpressions
            // 
            this.lstExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstExpressions.CheckOnClick = true;
            this.lstExpressions.FormattingEnabled = true;
            this.lstExpressions.Location = new System.Drawing.Point(16, 85);
            this.lstExpressions.Margin = new System.Windows.Forms.Padding(4);
            this.lstExpressions.Name = "lstExpressions";
            this.lstExpressions.Size = new System.Drawing.Size(659, 106);
            this.lstExpressions.TabIndex = 1;
           // this.lstExpressions.SelectedIndexChanged += new System.EventHandler(this.lstExpressions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Select expressions to view";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(576, 54);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 26);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblPosition
            // 
            this.lblPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(76, 778);
            this.lblPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(108, 17);
            this.lblPosition.TabIndex = 23;
            this.lblPosition.Text = "Current position";
            // 
            // btnRight
            // 
            this.btnRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(628, 487);
            this.btnRight.Margin = new System.Windows.Forms.Padding(4);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(48, 44);
            this.btnRight.TabIndex = 18;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(24, 487);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(4);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(48, 44);
            this.btnLeft.TabIndex = 21;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(325, 768);
            this.btnDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(48, 44);
            this.btnDown.TabIndex = 20;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(325, 210);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(48, 44);
            this.btnUp.TabIndex = 22;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // lblSensitivity
            // 
            this.lblSensitivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSensitivity.AutoSize = true;
            this.lblSensitivity.Location = new System.Drawing.Point(457, 778);
            this.lblSensitivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSensitivity.Name = "lblSensitivity";
            this.lblSensitivity.Size = new System.Drawing.Size(106, 17);
            this.lblSensitivity.TabIndex = 24;
            this.lblSensitivity.Text = "Polar sensitivity";
            // 
            // expPlotter
            // 
            this.expPlotter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.expPlotter.DisplayText = true;
            this.expPlotter.DivisionsX = 5;
            this.expPlotter.DivisionsY = 5;
            this.expPlotter.ForwardX = 0D;
            this.expPlotter.ForwardY = 0D;
            this.expPlotter.GraphMode = ExpressionPlotterControl.GraphMode.Rectangular;
            this.expPlotter.Grids = false;
            this.expPlotter.Location = new System.Drawing.Point(80, 262);
            this.expPlotter.Margin = new System.Windows.Forms.Padding(4);
            this.expPlotter.Name = "expPlotter";
            this.expPlotter.PenWidth = 1;
            this.expPlotter.PolarSensitivity = 100;
            this.expPlotter.PrintStepX = 1;
            this.expPlotter.PrintStepY = 1;
            this.expPlotter.ScaleX = 10D;
            this.expPlotter.ScaleY = 10D;
            this.expPlotter.Size = new System.Drawing.Size(540, 498);
            this.expPlotter.TabIndex = 0;
            this.expPlotter.Text = "expressionPlotter1";
            this.expPlotter.Click += new System.EventHandler(this.expPlotter_Click);
            // 
            // frm_values_insertion
            // 
            this.frm_values_insertion.Location = new System.Drawing.Point(460, 54);
            this.frm_values_insertion.Name = "frm_values_insertion";
            this.frm_values_insertion.Size = new System.Drawing.Size(103, 27);
            this.frm_values_insertion.TabIndex = 25;
            this.frm_values_insertion.Text = "Values";
            this.frm_values_insertion.UseVisualStyleBackColor = true;
            this.frm_values_insertion.Click += new System.EventHandler(this.frm_values_insertion_Click);
            // 
            // btn_Export_Profile
            // 
            this.btn_Export_Profile.Location = new System.Drawing.Point(434, 210);
            this.btn_Export_Profile.Name = "btn_Export_Profile";
            this.btn_Export_Profile.Size = new System.Drawing.Size(129, 23);
            this.btn_Export_Profile.TabIndex = 26;
            this.btn_Export_Profile.Text = "Export Profile";
            this.btn_Export_Profile.UseVisualStyleBackColor = true;
            this.btn_Export_Profile.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(576, 211);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(98, 21);
            this.checkBox1.TabIndex = 27;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 830);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btn_Export_Profile);
            this.Controls.Add(this.frm_values_insertion);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lblSensitivity);
            this.Controls.Add(this.expPlotter);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstExpressions);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Graph";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Graph_Load);
            this.Resize += new System.EventHandler(this.Graph_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnZoomInX;
        private System.Windows.Forms.ToolStripButton btnZoomInY;
        private System.Windows.Forms.ToolStripButton btnZoomOutX;
        private System.Windows.Forms.ToolStripButton btnZoomOutY;
        private System.Windows.Forms.ToolStripButton btnShowOrigin;
        private System.Windows.Forms.ToolStripButton btnDefault;
        private System.Windows.Forms.ToolStripButton btnRectangularMode;
        private System.Windows.Forms.ToolStripButton btnPolarMode;
        private System.Windows.Forms.ToolStripButton btnIncreaseSensitivity;
        private System.Windows.Forms.ToolStripButton btnDecreaseSensitivity;
        private System.Windows.Forms.ToolStripButton btnGrids;
        private System.Windows.Forms.CheckedListBox lstExpressions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ToolStripButton btnText;
        private System.Windows.Forms.ToolStripButton btnClipboard;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label lblSensitivity;
        private ExpressionPlotterControl.ExpressionPlotter expPlotter;
        private System.Windows.Forms.Button frm_values_insertion;
        private System.Windows.Forms.Button btn_Export_Profile;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}