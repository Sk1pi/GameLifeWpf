using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Drawing;
using System;
using Microsoft.VisualBasic;
using System.Printing;
using System.Windows.Shapes;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace GameLifeWpf
{
    public partial class MainWindow: Window
    {
        private int currentGneeration = 0;
        private WriteableBitmap bitmap;
        private int resolution;
        private DispatcherTimer timer;
        private bool[,] field; // to store information about which cell is which and whether it is a living organism or an empty cell
        private int rows;
        private int columns;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // Інтервал оновлення (наприклад, 100 мс)
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Draw(); // Викликаємо метод малювання при кожному тіку таймера
        }
        private void StartGame()
        {
            if (timer.IsEnabled)
                return;

            currentGneeration = 0;
            Title = $"Current geteration{currentGneeration}";

            Resolution.IsEnabled = false;
            Density.IsEnabled = false;
            resolution = (int)Resolution.Value;
            columns = (int)GameImage.Width / resolution;
            rows = (int)GameImage.Height / resolution;
            field = new bool[columns, rows];

            Random random = new();
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)Density.Value) == 0;
                }
            }

            bitmap = new WriteableBitmap((int)GameImage.Width, (int)GameImage.Height, 96, 96, PixelFormats.Bgra32, null); GameImage.Source = bitmap; // Призначаємо його Image
            GameImage.Source = bitmap;

            timer.Start();
        }

        private void Draw()
        {
            bitmap.Lock();
            unsafe
            {
                int* pixels = (int*)bitmap.BackBuffer;
                int width = bitmap.PixelWidth;
                int height = bitmap.PixelHeight;

                for (int x = 0; x < columns; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        bool life = field[x, y];
                        int color = life ? unchecked((int)0xFFFF0000) : unchecked((int)0xFF000000);

                        for (int dx = 0; dx < resolution; dx++)
                        {
                            for (int dy = 0; dy < resolution; dy++)
                            {
                                int index = (y * resolution + dy) * width + (x * resolution + dx);
                                pixels[index] = color;
                            }
                        }
                    }
                }
            }
            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            bitmap.Unlock();

            UpdateField();
            Title = $"Current geteration: {++currentGneeration}";
        }

        private void UpdateField()
        {
            var nextField = new bool[columns, rows];

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighbours = CountNeighbours(x, y);
                    bool life = field[x, y];

                    if (!life && neighbours == 3)
                        nextField[x, y] = true;
                    else if (life && (neighbours < 2 || neighbours > 3))
                        nextField[x, y] = false;
                    else
                        nextField[x, y] = field[x, y];
                }
            }

            field = nextField;
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int col = (x + i + columns) % columns;
                    int row = (y + j + rows) % rows;

                    if (field[col, row])
                        count++;
                }
            }
            return count;
        }

        private void GameStop()
        {
            if (!timer.IsEnabled)
                return;
            timer.Stop();
            Resolution.IsEnabled = true;
            Density.IsEnabled = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            GameStop();
        }

        private void GameImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer.IsEnabled)
                return;
            Point position = e.GetPosition(GameImage);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var x = (int)position.X / resolution;
                var y = (int)position.Y / resolution;
                field[x, y] = true;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                var x = (int)position.X / resolution;
                var y = (int)position.Y / resolution;
                field[x, y] = false;

            }
        }
    }
}