using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace _2048
{
    /// <summary>
    /// The class that describes a tile on the board.
    /// 
    /// Written by Oliver.
    /// </summary>
    public class Tile
    {

        //Store all tile colours according to their proper number.
        private static Dictionary<int, SolidColorBrush> tileColors =
            new Dictionary<int, SolidColorBrush>();

        //Set all tile colours to their proper value.
        //(values obtained from https://play2048.co/)
        static Tile()
        {
            tileColors.Add(2,    new SolidColorBrush(Color.FromRgb( 238, 228, 219 )));
            tileColors.Add(4,    new SolidColorBrush(Color.FromRgb( 238, 223, 200 )));
            tileColors.Add(8,    new SolidColorBrush(Color.FromRgb( 242, 177, 121 )));
            tileColors.Add(16,   new SolidColorBrush(Color.FromRgb( 236, 141, 85  )));
            tileColors.Add(32,   new SolidColorBrush(Color.FromRgb( 247, 123, 96  )));
            tileColors.Add(64,   new SolidColorBrush(Color.FromRgb( 234,  90, 56  )));
            tileColors.Add(128,  new SolidColorBrush(Color.FromRgb( 238, 206, 115 )));
            tileColors.Add(256,  new SolidColorBrush(Color.FromRgb( 242, 210, 75  )));
            tileColors.Add(512,  new SolidColorBrush(Color.FromRgb( 239, 202, 78  )));
            tileColors.Add(1024, new SolidColorBrush(Color.FromRgb( 227, 186, 20  )));
            tileColors.Add(2048, new SolidColorBrush(Color.FromRgb( 236, 196, 2   )));
        }

        //Display variables.
        private Rectangle colDisplay;
        private Label numDisplay;
        private int labelXAdjust;
        private int labelYAdjust;
        public int boardX;
        public int boardY;
        public int screenX;
        public int screenY;
        public int number;

        //Animation variables.
        private bool moving;
        private int targetX;
        private int targetY;

        public Tile(int x, int y, int number)
        {
            //Set position on board.
            boardX = x;
            boardY = y;
            //Calculate position on screen.
            screenX = (boardX * Board.TILE_SIZE) + ((boardX + 1) * Board.TILE_BORDER);
            screenY = (boardY * Board.TILE_SIZE) + ((boardY + 1) * Board.TILE_BORDER);

            //Create tile background with appropriate colour.
            colDisplay = new Rectangle();
            colDisplay.Width = Board.TILE_SIZE;
            colDisplay.Height = Board.TILE_SIZE;
            if (tileColors.ContainsKey(number))
                colDisplay.Fill = tileColors[number];
            else
                colDisplay.Fill = Brushes.Black;

            //Create number label.
            this.number = number;
            numDisplay = new Label();
            numDisplay.Content = number.ToString();
            numDisplay.FontWeight = FontWeights.Bold;

            //2 and 4 have a different text colour than all other numbers.
            if (number <= 5)
                numDisplay.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            else
                numDisplay.Foreground = new SolidColorBrush(Color.FromRgb(242, 242, 242));

            //Numbers over 1000 need to be smaller to fit in the tile.
            numDisplay.FontSize = Board.TILE_SIZE / 2;
            if (number > 1000)
                numDisplay.FontSize -= 10;

            //Horizontally adjust the label based on number of digits to
            //keep it centred on the tile.
            labelXAdjust = 0;
            if (number < 10)
                labelXAdjust = 40;
            else if (number < 100)
                labelXAdjust = 23;
            else if (number < 1000)
                labelXAdjust = 6;

            //Vertically adjust the label to keep it centred on the tile.
            labelYAdjust = 12;
            if (number > 1000)
                labelYAdjust = 20;
        }

        /// <summary>
        /// Move the tile to a certain position on the screen.
        /// </summary>
        /// <param name="x">Destination x</param>
        /// <param name="y">Destination y</param>
        public void moveTo(int x, int y)
        {
            boardX = x;
            boardY = y;
            moving = true;
            targetX = (boardX * Board.TILE_SIZE) + ((boardX + 1) * Board.TILE_BORDER);
            targetY = (boardY * Board.TILE_SIZE) + ((boardY + 1) * Board.TILE_BORDER);
        }

        /// <summary>
        /// Update position according to whether or not tile
        /// should be moving.
        /// </summary>
        public void tick()
        {
            //While moving, adjust position to be closer to target position.
            if (moving)
            {
                moving = targetX != screenX || targetY != screenY;
                
                //It takes 2 ticks for a tile to move 1 position on the board.
                int moveSpeed = (Board.TILE_SIZE + Board.TILE_BORDER) / 2;

                if (targetX > screenX)
                    screenX += moveSpeed;
                else if (targetX < screenX)
                    screenX -= moveSpeed;

                if (targetY > screenY)
                    screenY += moveSpeed;
                else if (targetY < screenY)
                    screenY -= moveSpeed;
            }
        }

        /// <summary>
        /// Add both background and text to given canvas.
        /// </summary>
        /// <param name="canvas">Canvas to draw to.</param>
        /// <param name="xo"></param>
        /// <param name="yo"></param>
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