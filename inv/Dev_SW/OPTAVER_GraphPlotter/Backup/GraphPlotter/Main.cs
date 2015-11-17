
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ExpressionPlotterControl;

namespace GraphPlotter
{
    public partial class Main : Form
    {
        Graph form;
        Color[] colorLevels = { Color.Red,  Color.Green, Color.Blue,  
            Color.Purple, Color.Brown, Color.Orange, Color.Chocolate, 
            Color.Maroon, Color.Navy, Color.YellowGreen };
        string[] strFunctions ={ "abs", "sin", "cos", "tan", "sec", "cosec", "cot", "arcsin", 
            "arccos", "arctan", "exp", "ln", "log", "antilog", "sqrt", "sinh", "cosh", "tanh", 
            "arcsinh", "arccosh", "arctanh" };

        public Main()
        {
            InitializeComponent();
        }

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            this.mode.SelectedIndex = 0;
            this.sensitivity.Enabled = false;

            this.lstExpressions.Items.Add("tanh(x)");
            this.lstExpressions.Items.Add("3*sin(x)*cos(4*x)");
            this.lstExpressions.Items.Add("-ln(abs(x))");
            this.lstExpressions.Items.Add("x*sin(4*x/3)");
            this.lstExpressions.Items.Add("x^2-4");
            this.lstExpressions.Items.Add("x-3");
            //this.lstExpressions.Items.Add("5*(sin(x)+sin(3*x)/3+sin(5*x)/5+sin(7*x)/7+sin(9*x)/9+sin(11*x)/11+sin(13*x)/13)");
        }

        private void txtExpression_TextChanged(object sender, EventArgs e)
        {
            int cursorPosition = this.txtExpression.SelectionStart;
            WriteText(this.txtExpression.Text);
            this.txtExpression.SelectionStart = cursorPosition;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddExpression();
            this.lstExpressions.SelectedIndex = -1;
            this.lstExpressions.Refresh();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lstExpressions.Items.Clear();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = this.lstExpressions.SelectedIndex;
            this.lstExpressions.Items.Remove(this.lstExpressions.SelectedItem);
            if (index == this.lstExpressions.Items.Count)
                index--;
            if (index != -1)
                this.lstExpressions.SelectedIndex = index;
        }

        private void cmdPlotGraph_Click(object sender, EventArgs e)
        {
            if (form == null || form.IsDisposed)
            {
                form = new Graph();
                form.Show();
            }

            form.SetRange((Double)startX.Value, (Double)endX.Value, (Double)startY.Value, (Double)endY.Value);
            form.SetDivisions((int)this.divX.Value, (int)this.divY.Value);
            form.SetPenWidth((int)this.penWidth.Value);

            if (this.mode.SelectedItem.ToString() == "Polar")
                form.SetMode(GraphMode.Polar, (int)this.sensitivity.Value);
            else
                form.SetMode(GraphMode.Rectangular, 50);

            form.RemoveAllExpressions();
            for (int i = 0; i < lstExpressions.Items.Count; i++)
            {
                form.AddExpression((string)lstExpressions.Items[i], colorLevels[i % colorLevels.Length]);
            }

            form.Refresh();
            form.Activate();
        }

        private void mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.mode.SelectedIndex == 1)
                this.sensitivity.Enabled = true;
            else
                this.sensitivity.Enabled = false;
        }

        private void txtExpression_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && txtExpression.Text.Length > 0)
                AddExpression();

            //if a letter is found
            if (char.IsLetter(e.KeyChar))
            {
                int cursorPos = this.txtExpression.SelectionStart;
                if (cursorPos > 0)
                {
                    //if the previous char is a digit, add a *
                    if (char.IsDigit(this.txtExpression.Text[cursorPos - 1]))
                    {
                        this.txtExpression.Text = this.txtExpression.Text.Insert(cursorPos, "*" + e.KeyChar);
                        cursorPos += 2;
                        this.txtExpression.SelectionStart = cursorPos;
                        e.Handled = true;
                    }
                    //if a function is formed, add a "("
                    else
                    {
                        string text = string.Empty;
                        int i = cursorPos - 1;
                        while (i >= 0)
                        {
                            if (!char.IsLetter(this.txtExpression.Text[i]))
                                break;
                            i--;
                        }
                        i++;
                        //now i is the index where last text is started
                        if (i < cursorPos)
                            text = this.txtExpression.Text.Substring(i, cursorPos - i) + e.KeyChar;
                        if (IsFunction(text))
                        {
                            this.txtExpression.Text = this.txtExpression.Text.Insert(cursorPos, e.KeyChar.ToString() + "(");
                            cursorPos += 2;
                            this.txtExpression.SelectionStart = cursorPos;
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void lstExpressions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstExpressions.SelectedIndex != -1)
                this.txtExpression.Text = this.lstExpressions.Items[this.lstExpressions.SelectedIndex].ToString();
        }

        #endregion

        #region Helper Functions
        //this functions handles coloring of expressions
        private void WriteText(string text)
        {
            int colorIndex = 0;
            this.txtExpression.Text = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(')
                {
                    colorIndex++;
                    if (colorIndex == colorLevels.Length)
                        colorIndex = 0;
                    txtExpression.SelectionColor = colorLevels[colorIndex];
                }

                this.txtExpression.AppendText(text[i].ToString());

                if (text[i] == ')')
                {
                    colorIndex--;
                    if (colorIndex < 0)
                        colorIndex = colorLevels.Length - 1;
                    txtExpression.SelectionColor = colorLevels[colorIndex];
                }
            }
        }

        private bool IsFunction(string text)
        {
            for (int i = 0; i < strFunctions.Length; i++)
                if (string.Compare(text, strFunctions[i], true) == 0)
                    return true;
            return false;
        }

        private void AddExpression()
        {
            if (this.txtExpression.Text.Length == 0)
                return;
            this.txtExpression.Text = CompleteParenthesis(this.txtExpression.Text);
            string expText = this.txtExpression.Text;
            IEvaluatable exp = new Expression(expText);
            if (!exp.IsValid)
            {
                if (MessageBox.Show("The expression entered does not seem to be valid, do you still want to add it?", "ValidationError", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            this.lstExpressions.Items.Add(expText);
            this.txtExpression.Text = string.Empty;
        }
        private string CompleteParenthesis(string exp)
        {
            int leftBracket = 0;
            int rightBracket = 0;
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '(')
                    leftBracket++;
                else if (exp[i] == ')')
                    rightBracket++;
            }
            exp = exp.PadRight(exp.Length + leftBracket - rightBracket, ')');
            return exp;
        }

        #endregion

    }
}
