using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace _2048
{
    public class Game
    {
        private const int FPS = 60;

        private Canvas canvas;
        private Label title;
        private Label desc1;
        private Label desc2;
        private Board board;

        public Game(Canvas canvas)
        {
            this.canvas = canvas;
            canvas.Background = new SolidColorBrush(Color.FromRgb(251, 248, 240));

            initHeader();
            board = new Board();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += tick;
            timer.Tick += render;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            timer.Start();

            board.setTileAt(0, 0, 2);
        }

        private void initHeader()
        {
            title = new Label();
            title.Content = "2048";
            title.FontSize = 80;
            title.FontFamily = new FontFamily("Arial");
            title.FontWeight = FontWeights.Bold;
            title.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(title, 12);
            Canvas.SetTop(title, 10);

            desc1 = new Label();
            desc1.Content = "Join the numbers and get to the";
            desc1.FontSize = 18;
            desc1.FontFamily = new FontFamily("Arial");
            desc1.FontWeight = FontWeights.Light;
            title.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(desc1, 12);
            Canvas.SetTop(desc1, 123);

            desc2 = new Label();
            desc2.Content = "2048 tile!";
            desc2.FontSize = 18;
            desc2.FontFamily = new FontFamily("Arial");
            desc2.FontWeight = FontWeights.Bold;
            title.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(desc2, 270);
            Canvas.SetTop(desc2, 123);
        }

        /// <summary>
        /// Update all necessary logic, 60 times per second.
        /// </summary>
        private void tick(object sender, EventArgs e)
        {
            board.tick();
        }

        /// <summary>
        /// Redraw all necessary things to the screen, 60 times per second.
        /// </summary>
        private void render(object sender, EventArgs e)
        {
            canvas.Children.Clear();
            canvas.Children.Add(title);
            canvas.Children.Add(desc1);
            canvas.Children.Add(desc2);

            board.render(canvas, 12, 190);
        }
    }
}