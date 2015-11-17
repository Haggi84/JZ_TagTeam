using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GraphPlotter

{
    public partial class frm_table : Form
    {


        /// <summary>
	    /// Contains column names.
	    /// </summary>
	    List<string> _names = new List<string>();

	    /// <summary>
	    /// Contains column data arrays.
	    /// </summary>
        List<double[]> _dataArray = new List<double[]>();



        public frm_table(double start, double end, double intv)
        {
            InitializeComponent();
                       
            

            /* #####################################################################
             * ###########################   Beispiel  #############################
             * #####################################################################
            
            
                   
                    _names.Add("Cat");
                    // Three numbers of cat data.
                    _dataArray.Add(new double[]
	                {
		            1.0,
		            2.2,
		            3.4
	                });

                    // Another example column.
                    _names.Add("Dog");
                    // Add three numbers of dog data.
                    _dataArray.Add(new double[]
	                {
		            3.3,
		            5.0,
		            7.0
	                });
                    //// Render the DataGridView.
                    dataGridView1.DataSource = GetResultsTable();
                     *
             * 
             * #####################################################################
             * ############################  Beispiel  #############################
             * #####################################################################
            */

           
            
            
            
            //_names.Add("Cat");
            //// Three numbers of cat data.
            //_dataArray.Add(new double[]
            //        {
            //        1.0,
            //        2.2,
            //        3.4
            //        });


        }

        
        private void frm_table_Load(object sender, EventArgs e)
        {

            //DataTable dt = new DataTable();
            //dt.Clear();
            //dt.Columns.Add("Name");
            //dt.Columns.Add("Marks");
            //DataRow _ravi = dt.NewRow();
            //_ravi["Name"] = "ravi";
            //_ravi["Marks"] = "500";
            //dt.Rows.Add(_ravi);

           

        }

        public bool Calculate_Table(double start, double end, double intv)
        {

            try
            {
                //lstExpressions.Select();
                //double x = 1;
                //double[] y;
                //y=expPlotter.GetValues(x);

                return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }


        public DataTable GetResultsTable()
        {
            // Create the output table.
            DataTable d = new DataTable();

            // Loop through all process names.
            for (int i = 0; i < this._dataArray.Count; i++)
            {
                // The current process name.
                string name = this._names[i];

                // Add the program name to our columns.
                d.Columns.Add(name);

                // Add all of the memory numbers to an object list.
                List<object> objectNumbers = new List<object>();

                // Put every column's numbers in this List.
                foreach (double number in this._dataArray[i])
                {
                    objectNumbers.Add((object)number);
                }

                // Keep adding rows until we have enough.
                while (d.Rows.Count < objectNumbers.Count)
                {
                    d.Rows.Add();
                }

                // Add each item to the cells in the column.
                for (int a = 0; a < objectNumbers.Count; a++)
                {
                    d.Rows[a][i] = objectNumbers[a];
                }
            }
            return d;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
