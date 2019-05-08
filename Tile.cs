using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2048
{
    public class Tile
    {

        private static Dictionary<int, SolidColorBrush> tileColors =
            new Dictionary<int, SolidColorBrush>();

        static Tile()
        {
            tileColors.Add(2, new SolidColorBrush(Color.FromRgb(238, 228, 219)));
            tileColors.Add(4, new SolidColorBrush(Color.FromRgb(238, 223, 200)));
            tileColors.Add(8, new SolidColorBrush(Color.FromRgb(242, 177, 121)));
            tileColors.Add(16, new SolidColorBrush(Color.FromRgb(236, 141, 85)));
            tileColors.Add(32, new SolidColorBrush(Color.FromRgb(247, 123, 96)));
            tileColors.Add(64, new SolidColorBrush(Color.FromRgb(234, 90, 56)));
            tileColors.Add(128, new SolidColorBrush(Color.FromRgb(238, 206, 115)));
            tileColors.Add(256, new SolidColorBrush(Color.FromRgb(242, 210, 75)));
            tileColors.Add(512, new SolidColorBrush(Color.FromRgb(239, 202, 78)));
            tileColors.Add(1024, new SolidColorBrush(Color.FromRgb(227, 186, 20)));
            tileColors.Add(2048, new SolidColorBrush(Color.FromRgb(236, 196, 2)));
            tileColors.Add(4096, new SolidColorBrush(Color.FromRgb(96, 217, 146)));
        }

        private Rectangle colDisplay;
        private Label numDisplay;
        private int labelXAdjust;
        private int labelYAdjust;
        public int boardX;
        public int boardY;
        public int screenX;
        public int screenY;
        public int number;

        private bool moving;
        private int targetX;
        private int targetY;

        public Tile(int x, int y, int number)
        {
            boardX = x;
            boardY = y;
            screenX = (boardX * Board.TILE_SIZE) + ((boardX + 1) * Board.TILE_BORDER);
            screenY = (boardY * Board.TILE_SIZE) + ((boardY + 1) * Board.TILE_BORDER);

            colDisplay = new Rectangle();
            colDisplay.Width = Board.TILE_SIZE;
            colDisplay.Height = Board.TILE_SIZE;
            colDisplay.Fill = tileColors[number];

            this.number = number;
            numDisplay = new Label();
            numDisplay.Content = number.ToString();
            if (number <= 5)
                numDisplay.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            else
                numDisplay.Foreground = new SolidColorBrush(Color.FromRgb(242, 242, 242));

            numDisplay.FontSize = Board.TILE_SIZE / 2;
            if (number > 1000)
                numDisplay.FontSize -= 10;

            labelXAdjust = 0;
            if (number < 10)
                labelXAdjust = 40;
            else if (number < 100)
                labelXAdjust = 23;
            else if (number < 1000)
                labelXAdjust = 6;

            labelYAdjust = 12;
            if (number > 1000)
                labelYAdjust = 20;
        }

        public void moveTo(int x, int y)
        {
            boardX = x;
            boardY = y;
            moving = true;
            targetX = (boardX * Board.TILE_SIZE) + ((boardX + 1) * Board.TILE_BORDER);
            targetY = (boardY * Board.TILE_SIZE) + ((boardY + 1) * Board.TILE_BORDER);
        }

        public void tick()
        {
            if (moving)
            {
                moving = targetX != screenX || targetY != screenY;

                if (targetX > screenX)
                    screenX += 68;
                else if (targetX < screenX)
                    screenX -= 68;

                if (targetY > screenY)
                    screenY += 68;
                else if (targetY < screenY)
                    screenY -= 68;
            }
        }

        public void render(Canvas canvas, int xo, int yo)
        {
            Canvas.SetLeft(colDisplay, screenX + xo);
            Canvas.SetTop(colDisplay, screenY + yo);
            canvas.Children.Add(colDisplay);

            Canvas.SetLeft(numDisplay, screenX + xo + labelXAdjust);
            Canvas.SetTop(numDisplay, screenY + yo + labelYAdjust);
            canvas.Children.Add(numDisplay);
        }
    }
}
