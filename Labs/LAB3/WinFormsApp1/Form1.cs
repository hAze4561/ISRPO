namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Создание случайного ноера билета через Random
            Random random = new Random();
            string ticket = random.Next(100000, 1000000).ToString();

            // Вывод номера билета
            lblResult.Text = ticket;

            // Расчет сумм цифр
            int sum1 = (ticket[0] - '0') + (ticket[1] - '0') + (ticket[2] - '0');
            int sum2 = (ticket[3] - '0') + (ticket[4] - '0') + (ticket[5] - '0');

            // Проверка и вывод результат
            if (sum1 == sum2)
            {
                lblResult.Text += " - СЧАСТЛИВЫЙ!";
                lblResult.ForeColor = Color.Green;
            }
            else
            {
                {
                    lblResult.Text += " - обычный!";
                    lblResult.ForeColor = Color.Red;
                }
            }
        }
    }
}
