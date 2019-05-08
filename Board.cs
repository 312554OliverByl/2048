
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace _2048
{
    public class Board
    {
        public static readonly int TILE_BORDER = 16;
        public static readonly int TILE_SIZE = 120;

        private Canvas background;
        public int width;
        public int height;
        public Tile[] tiles;

        public Board()
        {
            width = 4;
            height = 4;
            tiles = new Tile[width * height];

            initBackground();
        }

        /// <summary>
        /// Initialize the board's background layer.
        /// </summary>
        private void initBackground()
        {
            background = new Canvas();

            Rectangle bottomLayer = new Rectangle();
            bottomLayer.Width = 560;
            bottomLayer.Height = 560;
            bottomLayer.Fill = new SolidColorBrush(Color.FromRgb(182, 170, 158));
            background.Children.Add(bottomLayer);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Rectangle tileBackground = new Rectangle();
                    tileBackground.Width = TILE_SIZE;
                    tileBackground.Height = TILE_SIZE;
                    tileBackground.Fill = new SolidColorBrush(Color.FromRgb(204, 192, 180));
                    Canvas.SetLeft(tileBackground, (x * TILE_SIZE) + ((x + 1) * TILE_BORDER));
                    Canvas.SetTop(tileBackground, (y * TILE_SIZE) + ((y + 1) * TILE_BORDER));
                    background.Children.Add(tileBackground);
                }
            }
        }

        /// <summary>
        /// Tests to see if a tile exists at a certain position.
        /// </summary>
        /// <param name="x">X position of tile on board.</param>
        /// <param name="y">Y position of tile on board.</param>
        /// <returns>If a tile exists.</returns>
        public bool isTileAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return false;
            return tiles[x + y * width] != null;
        }

        /// <summary>
        /// Deletes the tile at a certain position.
        /// </summary>
        /// <param name="x">X position of tile on board.</param>
        /// <param name="y">Y position of tile on board.</param>
        public void deleteTileAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return;
            tiles[x + y * width] = null;
        }

        /// <summary>
        /// Sets the value of a tile at a certain position.
        /// </summary>
        /// <param name="x">X position of tile on board.</param>
        /// <param name="y">Y position of tile on board.</param>
        /// <param name="number">Number for tile to display. 
        /// Must be between 0 and 5000 (non inclusive) and a power of 2. </param>
        public void setTileAt(int x, int y, int number)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return;
            tiles[x + y * width] = new Tile(x, y, number);
        }

        /// <summary>
        /// Get the display number of a tile at a certain position.
        /// </summary>
        /// <param name="x">X position of tile on board.</param>
        /// <param name="y">Y position of tile on board.</param>
        /// <returns>Number of tile.</returns>
        public int tileNumberAt(int x, int y)
        {
            return tiles[x + y * width].number;
        }

        /// <summary>
        /// Move a tile from it's current location to a new location.
        /// Will only succeed if new location is empty.
        /// </summary>
        /// <param name="fromX">Beginning X position</param>
        /// <param name="fromY">Beginning Y position</param>
        /// <param name="toX">Destination X position</param>
        /// <param name="toY">Destination Y position</param>
        public void moveTile(int fromX, int fromY, int toX, int toY)
        {
            if (isTileAt(fromX, fromY) && !isTileAt(toX, toY))
            {
                int fromPos = fromX + fromY * width;
                int toPos = toX + toY * width;

                tiles[toPos] = tiles[fromPos];
                tiles[fromPos] = null;
                tiles[toPos].moveTo(toX, toY);
            }
        }

        /// <summary>
        /// Update board logic.
        /// </summary>
        public void tick()
        {
            foreach (Tile t in tiles)
                if (t != null)
                    t.tick();
        }

        /// <summary>
        /// Update board on screen.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void render(Canvas canvas, int x, int y)
        {
            Canvas.SetLeft(background, x);
            Canvas.SetTop(background, y);
            canvas.Children.Add(background);

            foreach (Tile t in tiles)
                if (t != null)
                    t.render(canvas, x, y);
        }
       
    }
}