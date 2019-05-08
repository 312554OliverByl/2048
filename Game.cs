
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
        private Random random;

        public Game(Canvas canvas)
        {
            this.canvas = canvas;
            canvas.Background = new SolidColorBrush(Color.FromRgb(251, 248, 240));

            initHeader();
            board = new Board();
            random = new Random();  
          ;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += tick;
            timer.Tick += render;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            timer.Start();

            //board.setTileAt(0, 0, 2);
            
            int TileRandomNumber = random.Next(0, 10);
            int TileNumber;
            if(TileRandomNumber <=9)
            {
                TileNumber = 2;
            }
            else
            {
                TileNumber = 4;
            }
            int Tile1PositionX = random.Next(4);
            int Tile1PositionY = random.Next(4);
            board.setTileAt(Tile1PositionX, Tile1PositionY, TileNumber);
            int Tile2PositionX, Tile2PositionY;
            do
            {
                 Tile2PositionX = random.Next(4);
                 Tile2PositionY = random.Next(4);
                
            } while (Tile1PositionX == Tile2PositionX && Tile1PositionY == Tile2PositionY);
            {
                board.setTileAt(Tile2PositionX, Tile2PositionY, TileNumber);
            }

            
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
            Input.tick();

            if (Input.wasKeyPressed(Input.UP))
            {
                bool addTile = false;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 1; y < 4; y++)
                    {
                        //tile at x,y
                        if (board.isTileAt(x, y))
                        {
                            bool hasCombined = false;
                            for (int d = y; d > 0; d--)
                            {
                                if (!board.isTileAt(x, d - 1))
                                {
                                    board.moveTile(x, d, x, d - 1);
                                    addTile = true;
                                }
                                else if (board.tileNumberAt(x, d) == board.tileNumberAt(x, d - 1) && !hasCombined)
                                {
                                    board.setTileAt(x, d - 1, board.tileNumberAt(x, d) * 2);
                                    board.deleteTileAt(x, d);
                                    hasCombined = true;
                                    addTile = true;
                                }
                            }
                        }
                    }
                }
                if (addTile)
                    addRandomTile();
            }
            if(Input.wasKeyPressed(Input.DOWN))
            {
                bool addtile = false;
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 2; y > -1; y--)
                    {
                        if (board.isTileAt(x, y))
                        {
                            bool hasCombined = false;
                            for (int d = y; d < 3; d++)
                            {
                                if (!board.isTileAt(x, d + 1))
                                {
                                    board.moveTile(x, d, x, d + 1);
                                    addtile = true;
                                }
                                else if(board.tileNumberAt(x, d) == board.tileNumberAt(x,d + 1) && !hasCombined)
                                {
                                    board.setTileAt(x, d + 1, board.tileNumberAt(x, d) * 2);
                                    board.deleteTileAt(x, d);
                                    hasCombined = true;
                                        addtile = true;
                                }
                            }
                        }
                    }
                }
                if (addtile)
                    addRandomTile();
            }
            if(Input.wasKeyPressed(Input.LEFT))
            {
                bool addtile = false;
                for (int x = 1; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                        if (board.isTileAt(x, y))
                        {
                            bool hasCombined = false;
                        
                            for (int d = x; d > 0; d--)
                            {
                                if (!board.isTileAt(d - 1, y))
                                {
                                    board.moveTile(d, y, d - 1, y);
                                    addtile = true;
                                }
                                else if (board.tileNumberAt(d, y) == board.tileNumberAt(d - 1, y) && !hasCombined )
                                {
                                    board.setTileAt(d - 1, y, board.tileNumberAt(d, y) * 2);
                                    board.deleteTileAt(d, y);
                                    hasCombined = true;
                                    addtile = true;
                                }
                            }
                        }
                }
                if (addtile)
                    addRandomTile();
            }
            if(Input.wasKeyPressed(Input.RIGHT))
            {
                bool addtile = false;
                for (int x = 2; x > -1; x--)
                    {
                    for (int y = 0; y < 4; y++)
                    {
                        if (board.isTileAt (x, y))
                        {
                            bool hasCombined = false;
                            for(int d = x; d < 3; d++)
                            {
                                if (!board.isTileAt(d + 1, y))
                                {
                                    board.moveTile(d, y, d + 1, y);
                                    addtile = true;
                                }
                                else if (board.tileNumberAt(d, y) == board.tileNumberAt(d + 1, y) && !hasCombined)
                                    {
                                    board.setTileAt(d + 1, y, board.tileNumberAt(d, y) * 2);
                                    board.deleteTileAt(d, y);
                                    hasCombined = true;
                                        addtile = true;
                                }
                                
                            }
                        }
                        
                    }
                }
                if (addtile)
                    addRandomTile();
            }

            /*if(Keyboard.IsKeyToggled(Key.Down) || Keyboard.IsKeyToggled(Key.Up) || Keyboard.IsKeyToggled(Key.Left) || Keyboard.IsKeyToggled(Key.Right))
            {
                Random random = new Random();
                int TileRandomNumber = random.Next(0, 10);
                int TileNumber;
                if (TileRandomNumber <= 9)
                {
                    TileNumber = 2;
                }
                else
                {
                    TileNumber = 4;
                }
                int Tile1PositionX = random.Next(4);
                int Tile1PositionY = random.Next(4);
                board.setTileAt(Tile1PositionX, Tile1PositionY, TileNumber);
            }*/
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
        private void addRandomTile()
        {
            

            int tileNumber = 2;
            int chance = random.Next(10);
            if (chance == 1)
                tileNumber = 4;
            int tileX = -1;
            int tileY = -1;
            int attempts = 0;
            do
            {
                tileX = random.Next(4);
                tileY = random.Next(4);
                if (attempts++ == 16)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            if (!board.isTileAt(x, y))
                            {
                                board.setTileAt(x, y, tileNumber);
                                return;
                            }
                        }
                        }
                        return;
                    }
                } while (board.isTileAt(tileX, tileY)) ;
                board.setTileAt(tileX, tileY, tileNumber);
            
            
        }
    }
}