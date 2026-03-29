using System;
using System.Windows.Forms;

namespace AlarmClock
{
    public partial class AlarmEditDialog : Form
    {
        // Свойства для передачи данных
        public TimeSpan AlarmTime { get; private set; }
        public string Label { get; private set; }
        public bool IsActive { get; private set; }
        public bool RepeatDaily { get; private set; }

        public AlarmEditDialog() : this(DateTime.Now.TimeOfDay, "", true, false)
        {
        }

        public AlarmEditDialog(TimeSpan alarmTime, string label, bool isActive, bool repeatDaily)
        {
            InitializeComponent();

            // Настройка dateTimePicker1 для выбора времени (а не даты)
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;

            // Загружаем данные в элементы
            dateTimePicker1.Value = DateTime.Today.Add(alarmTime);
            txtLabel.Text = label;
            chkActive.Checked = isActive;
            chkRepeatDaily.Checked = repeatDaily;
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            // Сохраняем данные
            AlarmTime = dateTimePicker1.Value.TimeOfDay;
            Label = txtLabel.Text;
            IsActive = chkActive.Checked;
            RepeatDaily = chkRepeatDaily.Checked;

            // Закрываем форму с результатом OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}