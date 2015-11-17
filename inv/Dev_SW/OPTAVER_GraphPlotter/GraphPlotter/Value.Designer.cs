namespace GraphPlotter
{
    partial class frm_Value
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
            this.tb_intervall_start = new System.Windows.Forms.TextBox();
            this.tb_intervall_end = new System.Windows.Forms.TextBox();
            this.tb_inervall_step = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_intervall_start
            // 
            this.tb_intervall_start.Location = new System.Drawing.Point(70, 21);
            this.tb_intervall_start.Name = "tb_intervall_start";
            this.tb_intervall_start.Size = new System.Drawing.Size(126, 22);
            this.tb_intervall_start.TabIndex = 0;
            // 
            // tb_intervall_end
            // 
            this.tb_intervall_end.Location = new System.Drawing.Point(70, 49);
            this.tb_intervall_end.Name = "tb_intervall_end";
            this.tb_intervall_end.Size = new System.Drawing.Size(126, 22);
            this.tb_intervall_end.TabIndex = 1;
            // 
            // tb_inervall_step
            // 
            this.tb_inervall_step.Location = new System.Drawing.Point(70, 77);
            this.tb_inervall_step.Name = "tb_inervall_step";
            this.tb_inervall_step.Size = new System.Drawing.Size(126, 22);
            this.tb_inervall_step.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "End";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Step";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Crimson;
            this.button1.Location = new System.Drawing.Point(203, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 76);
            this.button1.TabIndex = 6;
            this.button1.Text = "Calculate Result";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frm_Value
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 120);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_inervall_step);
            this.Controls.Add(this.tb_intervall_end);
            this.Controls.Add(this.tb_intervall_start);
            this.Name = "frm_Value";
            this.Text = "Insert Values";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_intervall_start;
        private System.Windows.Forms.TextBox tb_intervall_end;
        private System.Windows.Forms.TextBox tb_inervall_step;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}