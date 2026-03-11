using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПР4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ProcessText(int shift)
        {
            if (string.IsNullOrEmpty(txtVvod.Text)) return;

            string text = txtVvod.Text;
            bool isRussian = comboBox1.Text == "Русский";
            int size = isRussian ? 32 : 26;
            StringBuilder result = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char first = isRussian ? (char.IsUpper(c) ? 'А' : 'а') :
                                           (char.IsUpper(c) ? 'A' : 'a');
                    char shifted = (char)(first + (c - first + shift + size) % size);
                    result.Append(shifted);
                }
                else
                    result.Append(c);
            }

            lblResult.Text = result.ToString();
        }

        private void btnShivr_Click(object sender, EventArgs e)
        {
            ProcessText(int.Parse(txtSdvig.Text));
        }

        private void btnRashivr_Click(object sender, EventArgs e)
        {
            ProcessText(-int.Parse(txtSdvig.Text));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtVvod.Text = "";
            lblResult.Text = "";
            txtSdvig.Text = "3";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Русский");
            comboBox1.Items.Add("English");
            comboBox1.SelectedIndex = 0;
            txtSdvig.Text = "3";
        }
    }
}
