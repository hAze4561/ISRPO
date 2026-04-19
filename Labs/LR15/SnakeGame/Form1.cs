using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        // Настройки игрового поля
        private const int GridWidth = 20;   // 20 клеток по ширине
        private const int GridHeight = 20;  // 20 клеток по высоте
        private const int CellSize = 25;    // размер клетки в пикселях

        // Компоненты игры
        private List<Point> snake = new List<Point>();
        private Point food;
        private Direction currentDirection;
        private int score = 0;
        private bool isGameOver = false;
        private Timer gameTimer;
        private DateTime gameStartTime;

        // Строка подключения к БД
        private string connectionString = @"Server=HAZE\SQLEXPRESS;Database=SnakeGameDB;Integrated Security=True;";

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public Form1()
        {
            InitializeComponent();

            // Привязываем событие Paint к pictureBox1
            this.pictureBox1.Paint += GamePanel_Paint;

            InitializeGame();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void InitializeGame()
        {
            // Инициализация змейки (3 сегмента)
            snake.Clear();
            snake.Add(new Point(GridWidth / 2, GridHeight / 2));
            snake.Add(new Point(GridWidth / 2 - 1, GridHeight / 2));
            snake.Add(new Point(GridWidth / 2 - 2, GridHeight / 2));

            currentDirection = Direction.Right;
            score = 0;
            isGameOver = false;

            // Вывод счета в ваш lblResult
            if (lblResult != null)
                lblResult.Text = "Счёт: 0";

            // Генерация первой еды
            GenerateFood();

            // Настройка таймера
            if (gameTimer == null)
            {
                gameTimer = new Timer();
                gameTimer.Interval = 150;
                gameTimer.Tick += GameTimer_Tick;
            }

            gameStartTime = DateTime.Now;
            gameTimer.Start();

            // Перерисовка поля
            this.pictureBox1.Invalidate();
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            bool foodOnSnake;

            do
            {
                foodOnSnake = false;
                food = new Point(rand.Next(0, GridWidth), rand.Next(0, GridHeight));

                foreach (Point segment in snake)
                {
                    if (segment == food)
                    {
                        foodOnSnake = true;
                        break;
                    }
                }
            } while (foodOnSnake);
        }

        private void MoveSnake()
        {
            Point head = snake[0];
            Point newHead;

            switch (currentDirection)
            {
                case Direction.Up:
                    newHead = new Point(head.X, head.Y - 1);
                    break;
                case Direction.Down:
                    newHead = new Point(head.X, head.Y + 1);
                    break;
                case Direction.Left:
                    newHead = new Point(head.X - 1, head.Y);
                    break;
                case Direction.Right:
                    newHead = new Point(head.X + 1, head.Y);
                    break;
                default:
                    newHead = head;
                    break;
            }

            snake.Insert(0, newHead);

            if (newHead == food)
            {
                score += 10;
                if (lblResult != null)
                    lblResult.Text = "Счёт: " + score;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        private void CheckCollision()
        {
            Point head = snake[0];

            if (head.X < 0 || head.X >= GridWidth || head.Y < 0 || head.Y >= GridHeight)
            {
                GameOver();
                return;
            }

            for (int i = 1; i < snake.Count; i++)
            {
                if (head == snake[i])
                {
                    GameOver();
                    return;
                }
            }
        }

        private void GameOver()
        {
            if (isGameOver) return;

            isGameOver = true;
            gameTimer.Stop();

            TimeSpan gameDuration = DateTime.Now - gameStartTime;

            // ОКНО 1: "Игра окончена. Ваш счет: X. Игрок. Сохранить / Не сохранять"
            DialogResult saveResult = MessageBox.Show(
                "Ваш счет: " + score + "\n\nИгрок:",
                "Игра окончена",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Если нажали "Сохранить" (Yes)
            if (saveResult == DialogResult.Yes)
            {
                // Запрашиваем имя игрока
                string playerName = ShowInputDialog("Введите имя игрока:", "Сохранение результата", "Игрок");

                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    SaveResultToDatabase(playerName, score, gameDuration);

                    // ОКНО 2: "Успех. Результат сохранен! ОК"
                    MessageBox.Show("Результат сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // ОКНО 2 альтернативное: если выбрали "Не сохранять"
                MessageBox.Show("Результат не был сохранен!", "Сохранение отменено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // ОКНО 3: "Игра окончена. Хотите сыграть еще? Да / Нет"
            DialogResult playAgain = MessageBox.Show(
                "Хотите сыграть еще?",
                "Игра окончена",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (playAgain == DialogResult.Yes)
            {
                StartNewGame();
            }
            else
            {
                pictureBox1.Invalidate();
            }
        }

        private string ShowInputDialog(string text, string caption, string defaultValue)
        {
            Form dialog = new Form();
            dialog.Width = 400;
            dialog.Height = 150;
            dialog.Text = caption;
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;

            Label label = new Label();
            label.Text = text;
            label.Location = new Point(15, 20);
            label.Size = new Size(350, 25);
            label.Font = new Font("Arial", 10);
            dialog.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Text = defaultValue;
            textBox.Location = new Point(15, 50);
            textBox.Size = new Size(350, 25);
            textBox.Font = new Font("Arial", 10);
            dialog.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(230, 85);
            okButton.Size = new Size(60, 25);
            okButton.DialogResult = DialogResult.OK;
            dialog.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location = new Point(295, 85);
            cancelButton.Size = new Size(70, 25);
            cancelButton.DialogResult = DialogResult.Cancel;
            dialog.Controls.Add(cancelButton);

            dialog.AcceptButton = okButton;
            dialog.CancelButton = cancelButton;

            return dialog.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
        }

        private void SaveResultToDatabase(string playerName, int score, TimeSpan duration)
        {
            try
            {
                string query = @"INSERT INTO GameResults (PlayerName, Score, GameDuration, GameDate) 
                                VALUES (@PlayerName, @Score, @GameDuration, @GameDate)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlayerName", playerName);
                        command.Parameters.AddWithValue("@Score", score);
                        command.Parameters.AddWithValue("@GameDuration", duration.Seconds);
                        command.Parameters.AddWithValue("@GameDate", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении в БД:\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartNewGame()
        {
            InitializeGame();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver) return;

            MoveSnake();
            CheckCollision();
            this.pictureBox1.Invalidate();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Белый фон игрового поля
            g.Clear(Color.White);

            // Рисуем серые линии сетки
            Pen gridPen = new Pen(Color.LightGray, 1);

            // Вертикальные линии
            for (int x = 0; x <= GridWidth; x++)
            {
                g.DrawLine(gridPen, x * CellSize, 0, x * CellSize, GridHeight * CellSize);
            }

            // Горизонтальные линии
            for (int y = 0; y <= GridHeight; y++)
            {
                g.DrawLine(gridPen, 0, y * CellSize, GridWidth * CellSize, y * CellSize);
            }

            // Рисуем змейку
            for (int i = 0; i < snake.Count; i++)
            {
                Rectangle rect = new Rectangle(
                    snake[i].X * CellSize,
                    snake[i].Y * CellSize,
                    CellSize - 1,
                    CellSize - 1);

                if (i == 0)
                {
                    // Голова змейки - темно-зеленая
                    g.FillRectangle(Brushes.DarkGreen, rect);
                }
                else
                {
                    // Тело змейки - зеленая
                    g.FillRectangle(Brushes.Green, rect);
                }
                g.DrawRectangle(Pens.DarkGreen, rect);
            }

            // Рисуем еду (красный круг)
            Rectangle foodRect = new Rectangle(
                food.X * CellSize,
                food.Y * CellSize,
                CellSize - 1,
                CellSize - 1);
            g.FillEllipse(Brushes.Red, foodRect);
            g.DrawEllipse(Pens.DarkRed, foodRect);

            // Если игра окончена, выводим сообщение
            if (isGameOver)
            {
                string gameOverText = "GAME OVER";
                Font font = new Font("Arial", 20, FontStyle.Bold);
                SizeF textSize = g.MeasureString(gameOverText, font);

                // Полупрозрачный фон для текста
                g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.White)),
                    (pictureBox1.Width - textSize.Width) / 2 - 10,
                    (pictureBox1.Height - textSize.Height) / 2 - 10,
                    textSize.Width + 20,
                    textSize.Height + 20);

                g.DrawString(gameOverText, font, Brushes.Red,
                    (pictureBox1.Width - textSize.Width) / 2,
                    (pictureBox1.Height - textSize.Height) / 2);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver)
            {
                if (e.KeyCode == Keys.Space)
                    StartNewGame();
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (currentDirection != Direction.Down)
                        currentDirection = Direction.Up;
                    break;
                case Keys.Down:
                    if (currentDirection != Direction.Up)
                        currentDirection = Direction.Down;
                    break;
                case Keys.Left:
                    if (currentDirection != Direction.Right)
                        currentDirection = Direction.Left;
                    break;
                case Keys.Right:
                    if (currentDirection != Direction.Left)
                        currentDirection = Direction.Right;
                    break;
            }
        }
    }
}