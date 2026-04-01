using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MusicPlayer.Models;
using WMPLib;

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        private DatabaseHelper db;
        private List<MusicPlayer.Models.MusicTrack> allTracks;  // Используем полный путь для ясности

        // Windows Media Player компонент
        private WindowsMediaPlayer wmpPlayer;
        private Timer progressTimer;
        private string currentTempFile;

        private MusicPlayer.Models.MusicTrack currentTrack;
        private int currentTrackId = -1;
        private bool isDragging = false;

        public Form1()
        {
            InitializeComponent();
            InitializeWMP();
            InitializeDatabase();
            InitializePlayer();
            LoadTracks();
        }

        private void InitializeWMP()
        {
            wmpPlayer = new WindowsMediaPlayer();
            wmpPlayer.settings.autoStart = false;
            wmpPlayer.PlayStateChange += WmpPlayer_PlayStateChange;
        }

        private void InitializeDatabase()
        {
            db = new DatabaseHelper("HAZE\\SQLEXPRESS", "MusicPlayerDB");

            if (!db.TestConnection())
            {
                MessageBox.Show("Не удалось подключиться к базе данных!\n" +
                    "Проверьте: HAZE\\SQLEXPRESS",
                    "Ошибка подключения",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void InitializePlayer()
        {
            // Настройка DataGridView
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;

            // Настройка ползунков
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = 100;
            hScrollBar1.Value = 0;

            hScrollBar2.Minimum = 0;
            hScrollBar2.Maximum = 100;
            hScrollBar2.Value = 70;

            // Таймер для обновления прогресса
            progressTimer = new Timer();
            progressTimer.Interval = 500;
            progressTimer.Tick += ProgressTimer_Tick;

            button4.Text = "⏸";
        }

        private void LoadTracks()
        {
            try
            {
                allTracks = db.GetAllTracks();
                UpdateDataGridView(allTracks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки треков: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGridView(List<MusicPlayer.Models.MusicTrack> tracks)
        {
            var displayTracks = tracks.Select(t => new
            {
                t.Id,
                t.Title,
                t.Artist,
                Длительность = t.DurationFormatted,
                Прослушиваний = t.PlayCount,
                ДатаДобавления = t.DateAddedFormatted
            }).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = displayTracks;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = (int)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                var track = allTracks.FirstOrDefault(t => t.Id == id);
                if (track != null)
                {
                    DisplayTrackInfo(track);
                }
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = (int)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                PlayTrackById(id);
            }
        }

        private void DisplayTrackInfo(MusicPlayer.Models.MusicTrack track)
        {
            label9.Text = $"Название: {track.Title}";
            label10.Text = $"Исполнитель: {track.Artist}";
            label11.Text = $"Длительность: {track.DurationFormatted}";
            label12.Text = $"Прослушиваний: {track.PlayCount}";
            label13.Text = $"Добавлен: {track.DateAddedFormatted}";

            currentTrack = track;
        }


        private bool AddTrackToDatabase(string filePath)
        {
            try
            {
                var metadata = AudioFileHelper.ReadMetadata(filePath);
                string title = metadata.Item1;
                string artist = metadata.Item2;
                string album = metadata.Item3;
                TimeSpan duration = metadata.Item4;

                byte[] audioData = AudioFileHelper.ReadAudioFile(filePath);

                MusicPlayer.Models.MusicTrack track = new MusicPlayer.Models.MusicTrack
                {
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Duration = duration,
                    FilePath = filePath,
                    PlayCount = 0,
                    DateAdded = DateTime.Now,
                    AudioData = audioData
                };

                db.AddTrack(track);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении файла {Path.GetFileName(filePath)}: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }



        private void PlayTrackById(int id)
        {
            var track = allTracks.FirstOrDefault(t => t.Id == id);
            if (track == null) return;

            currentTrackId = id;
            currentTrack = track;
            DisplayTrackInfo(track);

            try
            {
                StopPlayback();

                byte[] audioData = db.GetAudioData(id);

                if (audioData != null && audioData.Length > 0)
                {
                    currentTempFile = AudioFileHelper.CreateTempFile(audioData);

                    wmpPlayer.URL = currentTempFile;
                    wmpPlayer.controls.play();

                    db.IncrementPlayCount(id);
                    track.PlayCount++;

                    DisplayTrackInfo(track);
                    LoadTracks();

                    progressTimer.Start();
                    button4.Text = "⏸";

                    // Настройка громкости
                    wmpPlayer.settings.volume = hScrollBar2.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Исправленный обработчик - используем явное преобразование типа
        private void WmpPlayer_PlayStateChange(int newState)
        {
            // newState уже int, используем его напрямую
            if (newState == 1 || newState == 8) // Stopped или MediaEnded
            {
                this.Invoke(new Action(() =>
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int currentIndex = dataGridView1.SelectedRows[0].Index;
                        if (currentIndex < dataGridView1.Rows.Count - 1)
                        {
                            dataGridView1.Rows[currentIndex + 1].Selected = true;
                            int nextId = (int)dataGridView1.Rows[currentIndex + 1].Cells["Id"].Value;
                            PlayTrackById(nextId);
                        }
                    }
                }));
            }
        }


        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (wmpPlayer != null && wmpPlayer.currentMedia != null && !isDragging)
            {
                double duration = wmpPlayer.currentMedia.duration;
                double position = wmpPlayer.controls.currentPosition;

                if (duration > 0)
                {
                    hScrollBar1.Value = (int)((position / duration) * 100);
                }
            }
        }

        private void StopPlayback()
        {
            progressTimer?.Stop();

            if (wmpPlayer != null)
            {
                wmpPlayer.controls.stop();
            }

            if (!string.IsNullOrEmpty(currentTempFile) && File.Exists(currentTempFile))
            {
                try
                {
                    File.Delete(currentTempFile);
                }
                catch { }
                currentTempFile = null;
            }

            hScrollBar1.Value = 0;
            button4.Text = "⏸";
        }

        private void ClearTrackInfo()
        {
            label9.Text = "Название: --";
            label10.Text = "Исполнитель: --";
            label11.Text = "Длительность: --";
            label12.Text = "Прослушиваний: --";
            label13.Text = "Добавлен: --";
            currentTrack = null;
            currentTrackId = -1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopPlayback();
            if (wmpPlayer != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wmpPlayer);
                wmpPlayer = null;
            }
        }

        private void btnDobaviti_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Аудио файлы|*.mp3;*.wav|Все файлы|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    int addedCount = 0;

                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        if (AddTrackToDatabase(filePath))
                        {
                            addedCount++;
                        }
                    }

                    LoadTracks();
                    MessageBox.Show($"Добавлено треков: {addedCount}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;

                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этот трек?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    db.DeleteTrack(id);
                    LoadTracks();

                    if (currentTrackId == id)
                    {
                        StopPlayback();
                        ClearTrackInfo();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите трек для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnPoisk_Click_1(object sender, EventArgs e)
        {
            string searchText = txtPoisk.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadTracks();
            }
            else
            {
                var searchResults = db.SearchTracks(searchText);
                UpdateDataGridView(searchResults);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
                PlayTrackById(id);
            }
            else
            {
                MessageBox.Show("Выберите трек для воспроизведения!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (wmpPlayer != null && !string.IsNullOrEmpty(currentTempFile))
            {
                // Преобразуем WMPPlayState в int явно
                int playState = (int)wmpPlayer.playState;

                if (playState == 3) // Playing (воспроизведение)
                {
                    wmpPlayer.controls.pause();
                    progressTimer.Stop();
                    button4.Text = "▶";
                }
                else if (playState == 2) // Paused (пауза)
                {
                    wmpPlayer.controls.play();
                    progressTimer.Start();
                    button4.Text = "⏸";
                }
                else if (playState == 1 || playState == 0) // Stopped или Undefined
                {
                    if (currentTrackId != -1)
                    {
                        PlayTrackById(currentTrackId);
                    }
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int currentIndex = dataGridView1.SelectedRows[0].Index;
                if (currentIndex < dataGridView1.Rows.Count - 1)
                {
                    dataGridView1.Rows[currentIndex + 1].Selected = true;
                    int nextId = (int)dataGridView1.Rows[currentIndex + 1].Cells["Id"].Value;
                    PlayTrackById(nextId);
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int currentIndex = dataGridView1.SelectedRows[0].Index;
                if (currentIndex > 0)
                {
                    dataGridView1.Rows[currentIndex - 1].Selected = true;
                    int prevId = (int)dataGridView1.Rows[currentIndex - 1].Cells["Id"].Value;
                    PlayTrackById(prevId);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (wmpPlayer != null)
            {
                wmpPlayer.controls.currentPosition = 0;
                hScrollBar1.Value = 0;
            }
        }

        private void hScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {
            if (wmpPlayer != null && !isDragging && wmpPlayer.currentMedia != null)
            {
                isDragging = true;
                double duration = wmpPlayer.currentMedia.duration;
                if (duration > 0)
                {
                    wmpPlayer.controls.currentPosition = (hScrollBar1.Value / 100.0) * duration;
                }
                isDragging = false;
            }
        }

        private void hScrollBar2_Scroll_1(object sender, ScrollEventArgs e)
        {
            if (wmpPlayer != null)
            {
                wmpPlayer.settings.volume = hScrollBar2.Value;
            }
        }
    }
}