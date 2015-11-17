using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphPlotter
{
    public partial class frm_Value : Form
    {
      double tb_start, tb_end, tb_intv;
        public frm_Value(ref double start, ref double end, ref double intv)
        {
           InitializeComponent();


           
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            tb_start = double.Parse(tb_intervall_start.Text);
            tb_end = double.Parse(tb_intervall_start.Text);
            tb_intv = double.Parse(tb_intervall_start.Text);

            Form frm_table = new frm_table(tb_start,tb_end,tb_intv);
            frm_table.Show();

        }
    }
}
