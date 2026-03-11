using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПР5
{
    public partial class Form1 : Form
    {
        private double memory = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            txtResult.Text += btn.Text;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            txtResult.Text += "+";
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            txtResult.Text += "-";
        }

        private void btnUmnog_Click(object sender, EventArgs e)
        {
            txtResult.Text += "*";
        }

        private void btnSlesh_Click(object sender, EventArgs e)
        {
            txtResult.Text += "/";
        }

        private void btnToch_Click(object sender, EventArgs e)
        {
            txtResult.Text += ".";
        }

        private void btnZap_Click(object sender, EventArgs e)
        {
            txtResult.Text += ",";
        }

        private void btnCkob1_Click(object sender, EventArgs e)
        {
            txtResult.Text += "(";
        }

        private void btnCkob2_Click(object sender, EventArgs e)
        {
            txtResult.Text += ")";
        }

        private void btnCkobMinus_Click(object sender, EventArgs e)
        {
            txtResult.Text += "(-";
        }

        private void btnRoot_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Sqrt(current);
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnSin_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Sin(current * Math.PI / 180); // градусы в радианы
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnCos_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Cos(current * Math.PI / 180);
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnTan_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Tan(current * Math.PI / 180);
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Log(current);
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnLogg_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Log(current);
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnAbs_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = Math.Abs(current);
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnPi_Click(object sender, EventArgs e)
        {
            txtResult.Text += Math.PI.ToString();
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            txtResult.Text += Math.E.ToString();
        }

        private void btnKvandrat_Click(object sender, EventArgs e)
        {
            try
            {
                double current = string.IsNullOrEmpty(txtResult.Text) ? 0 : Convert.ToDouble(txtResult.Text);
                double result = current * current;
                txtResult.Text = result.ToString();
            }
            catch { txtResult.Text = "Error"; }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Length > 0)
                txtResult.Text = txtResult.Text.Substring(0, txtResult.Text.Length - 1);
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
        }

        private void btnAns_Click(object sender, EventArgs e)
        {
            txtResult.Text += memory.ToString();
        }

        private void Sum_Click(object sender, EventArgs e)
        {
            try
            {
                string expression = txtResult.Text
                    .Replace("sqrt", "Math.Sqrt")
                    .Replace("sin", "Math.Sin")
                    .Replace("cos", "Math.Cos")
                    .Replace("tan", "Math.Tan")
                    .Replace("ln", "Math.Log")
                    .Replace("log", "Math.Log10")
                    .Replace("abs", "Math.Abs")
                    .Replace("^2", "*" + txtResult.Text) // упрощенное возведение в квадрат
                    .Replace(",", ".");

                var result = new System.Data.DataTable().Compute(expression, null);
                memory = Convert.ToDouble(result);
                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = "Error";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn1.Click += Number_Click;
            btn2.Click += Number_Click;
            btn3.Click += Number_Click;
            btn4.Click += Number_Click;
            btn5.Click += Number_Click;
            btn6.Click += Number_Click;
            btn7.Click += Number_Click;
            btn8.Click += Number_Click;
            btn9.Click += Number_Click;
            btn0.Click += Number_Click;
        }

        private void btnSumma_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
        }
    }
}
