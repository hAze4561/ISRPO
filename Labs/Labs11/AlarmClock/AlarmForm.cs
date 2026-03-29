using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AlarmClock
{
    public partial class AlarmForm : Form
    {
        // Подключение к БД
        private string connectionString = @"Data Source=HAZE\SQLEXPRESS;Initial Catalog=AlarmClockDB;Integrated Security=True;";

        // Таймеры
        private Timer tmrClock;
        private Timer tmrAlarm;
        private Timer tmrBlink;

        // Переменные для звонящего будильника
        private int ringingAlarmId = -1;
        private bool blinkState = false;

        public AlarmForm()
        {
            InitializeComponent();
            LoadAlarms();
            StartTimers();

            // Скрываем панель при запуске
            if (pnlRinging != null)
                pnlRinging.Visible = false;
        }

        private void StartTimers()
        {
            // Таймер для часов
            tmrClock = new Timer();
            tmrClock.Interval = 1000;
            tmrClock.Tick += TmrClock_Tick;
            tmrClock.Start();

            // Таймер для проверки будильников
            tmrAlarm = new Timer();
            tmrAlarm.Interval = 1000;
            tmrAlarm.Tick += TmrAlarm_Tick;
            tmrAlarm.Start();

            // Таймер для мигания панели
            tmrBlink = new Timer();
            tmrBlink.Interval = 500;
            tmrBlink.Tick += TmrBlink_Tick;
        }

        private void TmrClock_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblCurrentDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private void TmrAlarm_Tick(object sender, EventArgs e)
        {
            // Если будильник уже звенит, не проверяем новые
            if (ringingAlarmId != -1) return;

            DataTable alarms = GetAlarmsFromDB();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            foreach (DataRow row in alarms.Rows)
            {
                bool isActive = Convert.ToBoolean(row["IsActive"]);
                if (!isActive) continue;

                TimeSpan alarmTime = (TimeSpan)row["AlarmTime"];
                int id = Convert.ToInt32(row["Id"]);
                bool repeatDaily = Convert.ToBoolean(row["RepeatDaily"]);

                // Проверка на срабатывание
                if (Math.Abs((alarmTime - currentTime).TotalSeconds) <= 1)
                {
                    TriggerAlarm(id);
                    break;
                }
            }
        }

        private void TmrBlink_Tick(object sender, EventArgs e)
        {
            if (ringingAlarmId != -1 && pnlRinging != null)
            {
                blinkState = !blinkState;
                pnlRinging.BackColor = blinkState ? Color.Red : Color.DarkRed;
            }
        }

        private void TriggerAlarm(int alarmId)
        {
            ringingAlarmId = alarmId;

            if (pnlRinging != null)
            {
                pnlRinging.Visible = true;
                tmrBlink.Start();
            }

            btnStop.Enabled = true;
            btnSnooze.Enabled = true;

            // Проигрываем звук
            Console.Beep(1000, 1000);

            MessageBox.Show("Будильник сработал!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StopAlarm()
        {
            ringingAlarmId = -1;

            if (pnlRinging != null)
            {
                pnlRinging.Visible = false;
                tmrBlink.Stop();
            }

            btnStop.Enabled = false;
            btnSnooze.Enabled = false;
        }


        private void UpdateAlarmTime(int id, TimeSpan newTime)
        {
            string query = "UPDATE Alarms SET AlarmTime = @AlarmTime WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AlarmTime", newTime);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private DataTable GetAlarmsFromDB()
        {
            DataTable dt = new DataTable();
            string query = "SELECT Id, AlarmTime, Label, IsActive, RepeatDaily, CreatedDate FROM Alarms ORDER BY AlarmTime";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
            }

            return dt;
        }

        private void LoadAlarms()
        {
            DataTable dt = GetAlarmsFromDB();
            dataGridView1.DataSource = dt;

            // Настройка колонок
            if (dataGridView1.Columns.Contains("Id"))
                dataGridView1.Columns["Id"].Visible = false;

            if (dataGridView1.Columns.Contains("AlarmTime"))
                dataGridView1.Columns["AlarmTime"].HeaderText = "Время";

            if (dataGridView1.Columns.Contains("Label"))
                dataGridView1.Columns["Label"].HeaderText = "Название";

            if (dataGridView1.Columns.Contains("IsActive"))
                dataGridView1.Columns["IsActive"].HeaderText = "Активен";

            if (dataGridView1.Columns.Contains("RepeatDaily"))
                dataGridView1.Columns["RepeatDaily"].HeaderText = "Ежедневно";

            if (dataGridView1.Columns.Contains("CreatedDate"))
                dataGridView1.Columns["CreatedDate"].HeaderText = "Дата создания";
        }


        private void AddAlarmToDB(TimeSpan alarmTime, string label, bool isActive, bool repeatDaily)
        {
            string query = @"INSERT INTO Alarms (AlarmTime, Label, IsActive, RepeatDaily, CreatedDate) 
                             VALUES (@AlarmTime, @Label, @IsActive, @RepeatDaily, @CreatedDate)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AlarmTime", alarmTime);
                cmd.Parameters.AddWithValue("@Label", label ?? "");
                cmd.Parameters.AddWithValue("@IsActive", isActive);
                cmd.Parameters.AddWithValue("@RepeatDaily", repeatDaily);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Будильник успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void UpdateAlarmInDB(int id, TimeSpan alarmTime, string label, bool isActive, bool repeatDaily)
        {
            string query = @"UPDATE Alarms SET AlarmTime = @AlarmTime, Label = @Label, 
                             IsActive = @IsActive, RepeatDaily = @RepeatDaily WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@AlarmTime", alarmTime);
                cmd.Parameters.AddWithValue("@Label", label ?? "");
                cmd.Parameters.AddWithValue("@IsActive", isActive);
                cmd.Parameters.AddWithValue("@RepeatDaily", repeatDaily);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Будильник успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteAlarmFromDB(int id)
        {
            string query = "DELETE FROM Alarms WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Будильник успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            StopAlarm();
        }

        private void btnSnooze_Click_1(object sender, EventArgs e)
        {
            if (ringingAlarmId != -1)
            {
                // Откладываем на 5 минут
                DateTime newTime = DateTime.Now.AddMinutes(5);

                // Обновляем время будильника в БД
                UpdateAlarmTime(ringingAlarmId, newTime.TimeOfDay);

                StopAlarm();

                MessageBox.Show($"Будильник отложен на 5 минут. Следующий звонок в {newTime:HH:mm}",
                    "Отложено", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Обновляем список
                LoadAlarms();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            AlarmEditDialog dialog = new AlarmEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                AddAlarmToDB(dialog.AlarmTime, dialog.Label, dialog.IsActive, dialog.RepeatDaily);
                LoadAlarms();
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите будильник для удаления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этот будильник?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
                DeleteAlarmFromDB(id);
                LoadAlarms();
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите будильник для удаления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этот будильник?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

                string query = "DELETE FROM Alarms WHERE Id = @Id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                LoadAlarms();
                MessageBox.Show("Будильник успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}