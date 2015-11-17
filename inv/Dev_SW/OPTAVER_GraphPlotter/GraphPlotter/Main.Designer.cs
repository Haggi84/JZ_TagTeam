namespace GraphPlotter
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lstExpressions = new System.Windows.Forms.ListBox();
            this.txtExpression = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.cmdPlotGraph = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.startX = new System.Windows.Forms.NumericUpDown();
            this.endX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.endY = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.startY = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.divY = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.divX = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.sensitivity = new System.Windows.Forms.NumericUpDown();
            this.mode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.penWidth = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.startX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // lstExpressions
            // 
            this.lstExpressions.FormattingEnabled = true;
            this.lstExpressions.HorizontalScrollbar = true;
            this.lstExpressions.ItemHeight = 16;
            this.lstExpressions.Location = new System.Drawing.Point(16, 123);
            this.lstExpressions.Margin = new System.Windows.Forms.Padding(4);
            this.lstExpressions.Name = "lstExpressions";
            this.lstExpressions.Size = new System.Drawing.Size(392, 116);
            this.lstExpressions.TabIndex = 2;
            this.lstExpressions.SelectedIndexChanged += new System.EventHandler(this.lstExpressions_SelectedIndexChanged);
            // 
            // txtExpression
            // 
            this.txtExpression.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtExpression.Location = new System.Drawing.Point(16, 65);
            this.txtExpression.Margin = new System.Windows.Forms.Padding(4);
            this.txtExpression.Multiline = false;
            this.txtExpression.Name = "txtExpression";
            this.txtExpression.Size = new System.Drawing.Size(303, 22);
            this.txtExpression.TabIndex = 0;
            this.txtExpression.Text = "";
            this.txtExpression.TextChanged += new System.EventHandler(this.txtExpression_TextChanged);
            this.txtExpression.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtExpression_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Expression";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(328, 62);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(81, 27);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 103);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Expressions to plot";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(16, 247);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(123, 27);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "&Clear List";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(261, 247);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(148, 27);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "&Remove Expression";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // cmdPlotGraph
            // 
            this.cmdPlotGraph.Location = new System.Drawing.Point(144, 471);
            this.cmdPlotGraph.Margin = new System.Windows.Forms.Padding(4);
            this.cmdPlotGraph.Name = "cmdPlotGraph";
            this.cmdPlotGraph.Size = new System.Drawing.Size(123, 27);
            this.cmdPlotGraph.TabIndex = 13;
            this.cmdPlotGraph.Text = "&Plot Graph";
            this.cmdPlotGraph.UseVisualStyleBackColor = true;
            this.cmdPlotGraph.Click += new System.EventHandler(this.cmdPlotGraph_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 303);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "X-axis start";
            // 
            // startX
            // 
            this.startX.DecimalPlaces = 2;
            this.startX.Location = new System.Drawing.Point(117, 298);
            this.startX.Margin = new System.Windows.Forms.Padding(4);
            this.startX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.startX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.startX.Name = "startX";
            this.startX.Size = new System.Drawing.Size(80, 22);
            this.startX.TabIndex = 5;
            this.startX.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // endX
            // 
            this.endX.DecimalPlaces = 2;
            this.endX.Location = new System.Drawing.Point(328, 298);
            this.endX.Margin = new System.Windows.Forms.Padding(4);
            this.endX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.endX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.endX.Name = "endX";
            this.endX.Size = new System.Drawing.Size(80, 22);
            this.endX.TabIndex = 6;
            this.endX.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 303);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "X-axis end";
            // 
            // endY
            // 
            this.endY.DecimalPlaces = 2;
            this.endY.Location = new System.Drawing.Point(328, 329);
            this.endY.Margin = new System.Windows.Forms.Padding(4);
            this.endY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.endY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.endY.Name = "endY";
            this.endY.Size = new System.Drawing.Size(80, 22);
            this.endY.TabIndex = 8;
            this.endY.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 334);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "Y-axis end";
            // 
            // startY
            // 
            this.startY.DecimalPlaces = 2;
            this.startY.Location = new System.Drawing.Point(117, 329);
            this.startY.Margin = new System.Windows.Forms.Padding(4);
            this.startY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.startY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.startY.Name = "startY";
            this.startY.Size = new System.Drawing.Size(80, 22);
            this.startY.TabIndex = 7;
            this.startY.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 334);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Y-axis start";
            // 
            // divY
            // 
            this.divY.Location = new System.Drawing.Point(328, 359);
            this.divY.Margin = new System.Windows.Forms.Padding(4);
            this.divY.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.divY.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.divY.Name = "divY";
            this.divY.Size = new System.Drawing.Size(80, 22);
            this.divY.TabIndex = 10;
            this.divY.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(216, 364);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "Y-axis divisions";
            // 
            // divX
            // 
            this.divX.Location = new System.Drawing.Point(117, 359);
            this.divX.Margin = new System.Windows.Forms.Padding(4);
            this.divX.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.divX.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.divX.Name = "divX";
            this.divX.Size = new System.Drawing.Size(80, 22);
            this.divX.TabIndex = 9;
            this.divX.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 364);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "X-axis divisions";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(216, 395);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 17);
            this.label9.TabIndex = 21;
            this.label9.Text = "Polar Sensitivity";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 430);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "Graph Mode";
            // 
            // sensitivity
            // 
            this.sensitivity.Enabled = false;
            this.sensitivity.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.sensitivity.Location = new System.Drawing.Point(328, 390);
            this.sensitivity.Margin = new System.Windows.Forms.Padding(4);
            this.sensitivity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.sensitivity.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.sensitivity.Name = "sensitivity";
            this.sensitivity.Size = new System.Drawing.Size(80, 22);
            this.sensitivity.TabIndex = 12;
            this.sensitivity.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // mode
            // 
            this.mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mode.FormattingEnabled = true;
            this.mode.Items.AddRange(new object[] {
            "Rectangular",
            "Polar"});
            this.mode.Location = new System.Drawing.Point(117, 425);
            this.mode.Margin = new System.Windows.Forms.Padding(4);
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(291, 24);
            this.mode.TabIndex = 11;
            this.mode.SelectedIndexChanged += new System.EventHandler(this.mode_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(12, 11);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(169, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "Graph Expressions";
            // 
            // penWidth
            // 
            this.penWidth.Location = new System.Drawing.Point(117, 390);
            this.penWidth.Margin = new System.Windows.Forms.Padding(4);
            this.penWidth.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.penWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.penWidth.Name = "penWidth";
            this.penWidth.Size = new System.Drawing.Size(80, 22);
            this.penWidth.TabIndex = 26;
            this.penWidth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 395);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 17);
            this.label12.TabIndex = 27;
            this.label12.Text = "Pen Width";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 517);
            this.Controls.Add(this.penWidth);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.mode);
            this.Controls.Add(this.sensitivity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.divY);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.divX);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.endY);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.startY);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.endX);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.startX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdPlotGraph);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtExpression);
            this.Controls.Add(this.lstExpressions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Graph Plotter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.startX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lstExpressions;
        private System.Windows.Forms.RichTextBox txtExpression;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button cmdPlotGraph;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown startX;
        private System.Windows.Forms.NumericUpDown endX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown endY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown startY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown divY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown divX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown sensitivity;
        private System.Windows.Forms.ComboBox mode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown penWidth;
        private System.Windows.Forms.Label label12;

    }
}

