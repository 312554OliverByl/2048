
using System;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace _2048
{
    /// <summary>
    /// Class containing core game logic.
    /// 
    /// Written by Oliver, Morghan, and Aidan.
    /// If an entire method was written by one person,
    /// their name will be in the method description.
    /// Otherwise, their names will be substituted in
    /// the method where appropriate.
    /// </summary>
    public class Game
    {
        private const int FPS = 60;

        private Canvas canvas;
        private Label title;
        private Label desc1;
        private Label desc2;
        private Label scoreLabel;
        private Label highScoreLabel;
        private Label scoreOutputLabel;
        private Label highScoreOutputLabel;
        private Label loseLabel;
        private Rectangle scoreBackground;
        private Rectangle highScoreBackground;
        private Rectangle loseOverlay;
        private Button newGame;

        private Board board;
        private Random random;
        private bool lostGame;

        public Game(Canvas canvas)
        {
            this.canvas = canvas;
            canvas.Background = new SolidColorBrush(Color.FromRgb(251, 248, 240));

            initHeader();
            board = new Board();
            random = new Random();
            lostGame = false;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += tick;
            timer.Tick += render;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            timer.Start();

            addRandomTile();
            addRandomTile();
        }

        /// <summary>
        /// Initializes all non-board items of the display.
        /// </summary>
        private void initHeader()
        {
            //Written by Oliver:
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
            desc1.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(desc1, 12);
            Canvas.SetTop(desc1, 123);

            desc2 = new Label();
            desc2.Content = "2048 tile!";
            desc2.FontSize = 18;
            desc2.FontFamily = new FontFamily("Arial");
            desc2.FontWeight = FontWeights.Bold;
            desc2.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(desc2, 270);
            Canvas.SetTop(desc2, 123);

            loseLabel = new Label();
            loseLabel.Content = "Game Over!";
            loseLabel.FontSize = 60;
            loseLabel.FontFamily = new FontFamily("Arial");
            loseLabel.FontWeight = FontWeights.Bold;
            loseLabel.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(loseLabel, 120);
            Canvas.SetTop(loseLabel, 400);

            scoreBackground = new Rectangle();
            scoreBackground.Width = 120;
            scoreBackground.Height = 60;
            scoreBackground.Fill = new SolidColorBrush(Color.FromRgb(182, 170, 158));
            Canvas.SetLeft(scoreBackground, 320);
            Canvas.SetTop(scoreBackground, 5);

            highScoreBackground = new Rectangle();
            highScoreBackground.Width = 120;
            highScoreBackground.Height = 60;
            highScoreBackground.Fill = new SolidColorBrush(Color.FromRgb(182, 170, 158));
            Canvas.SetLeft(highScoreBackground, 450);
            Canvas.SetTop(highScoreBackground, 5);

            loseOverlay = new Rectangle();
            loseOverlay.Width = 560;
            loseOverlay.Height = 560;
            loseOverlay.Fill = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
            Canvas.SetLeft(loseOverlay, 12);
            Canvas.SetTop(loseOverlay, 190);

            //Written by Aidan:
            scoreLabel = new Label();
            scoreLabel.Content = "SCORE";
            scoreLabel.FontSize = 17;
            scoreLabel.FontFamily = new FontFamily("Arial");
            scoreLabel.FontWeight = FontWeights.Bold;
            scoreLabel.Foreground = Brushes.White;
            Canvas.SetLeft(scoreLabel, 344);
            Canvas.SetTop(scoreLabel, 7);

            highScoreLabel = new Label();
            highScoreLabel.Content = "BEST";
            highScoreLabel.FontSize = 17;
            highScoreLabel.FontFamily = new FontFamily("Arial");
            highScoreLabel.FontWeight = FontWeights.Bold;
            highScoreLabel.Foreground = Brushes.White;
            Canvas.SetLeft(highScoreLabel, 481);
            Canvas.SetTop(highScoreLabel, 7);

            scoreOutputLabel = new Label();
            scoreOutputLabel.Content = "0";
            scoreOutputLabel.FontSize = 20;
            scoreOutputLabel.FontFamily = new FontFamily("Arial");
            scoreOutputLabel.FontWeight = FontWeights.Bold;
            scoreOutputLabel.Foreground = Brushes.White;
            Canvas.SetLeft(scoreOutputLabel, 369);
            Canvas.SetTop(scoreOutputLabel, 30);

            newGame = new Button();
            newGame.Content = "New Game";
            newGame.FontSize = 16;
            newGame.FontFamily = new FontFamily("Arial");
            newGame.FontWeight = FontWeights.Bold;
            newGame.Foreground = Brushes.White;
            newGame.Width = 120;
            newGame.Height = 40;
            newGame.Background = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            newGame.Click += onNewGameClick;
            Canvas.SetLeft(newGame, 450);
            Canvas.SetTop(newGame, 115);
        }

        /// <summary>
        /// Update all necessary logic, 60 times per second.
        /// </summary>
        private void tick(object sender, EventArgs e)
        {
            //Written by Oliver:
            if (lostGame)
            {
                board.tick();
                return;
            }

            Input.tick();

            //Written by Morghan:
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
                                    addToScore(board.tileNumberAt(x, d) * 2);
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

                lostGame = checkIfLost();
            }
            if (Input.wasKeyPressed(Input.DOWN))
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
                                else if (board.tileNumberAt(x, d) == board.tileNumberAt(x, d + 1) && !hasCombined)
                                {
                                    board.setTileAt(x, d + 1, board.tileNumberAt(x, d) * 2);
                                    addToScore(board.tileNumberAt(x, d) * 2);
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

                lostGame = checkIfLost();
            }
            if (Input.wasKeyPressed(Input.LEFT))
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
                                else if (board.tileNumberAt(d, y) == board.tileNumberAt(d - 1, y) && !hasCombined)
                                {
                                    board.setTileAt(d - 1, y, board.tileNumberAt(d, y) * 2);
                                    addToScore(board.tileNumberAt(d, y) * 2);
                                    board.deleteTileAt(d, y);
                                    hasCombined = true;
                                    addtile = true;
                                }
                            }
                        }
                }
                if (addtile)
                    addRandomTile();

                lostGame = checkIfLost();
            }
            if (Input.wasKeyPressed(Input.RIGHT))
            {
                bool addtile = false;
                for (int x = 2; x > -1; x--)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (board.isTileAt(x, y))
                        {
                            bool hasCombined = false;
                            for (int d = x; d < 3; d++)
                            {
                                if (!board.isTileAt(d + 1, y))
                                {
                                    board.moveTile(d, y, d + 1, y);
                                    addtile = true;
                                }
                                else if (board.tileNumberAt(d, y) == board.tileNumberAt(d + 1, y) && !hasCombined)
                                {
                                    board.setTileAt(d + 1, y, board.tileNumberAt(d, y) * 2);
                                    addToScore(board.tileNumberAt(d, y) * 2);
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

                lostGame = checkIfLost();
            }
            
            board.tick();
        }

        /// <summary>
        /// Redraw all necessary things to the screen, 60 times per second.
        /// 
        /// Written by Oliver.
        /// </summary>
        private void render(object sender, EventArgs e)
        {
            canvas.Children.Clear();
            canvas.Children.Add(title);
            canvas.Children.Add(desc1);
            canvas.Children.Add(desc2);
            canvas.Children.Add(scoreBackground);
            canvas.Children.Add(highScoreBackground);
            canvas.Children.Add(newGame);
            canvas.Children.Add(scoreLabel);
            canvas.Children.Add(highScoreLabel);
            canvas.Children.Add(scoreOutputLabel);

            board.render(canvas, 12, 190);

            if (!lostGame)
                return;

            canvas.Children.Add(loseOverlay);
            canvas.Children.Add(loseLabel);
        }

        /// <summary>
        /// Adds a tile to the board in a random position.
        /// 
        /// Written by Morghan.
        /// </summary>
        private void addRandomTile()
        {
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

            int TileX = -1;
            int TileY = -1;

            int attempts = 0;
            do
            {
                TileX = random.Next(4);
                TileY = random.Next(4);

                if (attempts++ >= 16)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            if (!board.isTileAt(x, y))
                            {
                                board.setTileAt(x, y, TileNumber);
                            }   
                        }
                    }
                    return;
                }
            } while (board.isTileAt(TileX, TileY));

            board.setTileAt(TileX, TileY, TileNumber);
        }

        /// <summary>
        /// Checks to see if the game has been lost.
        /// 
        /// Written by Oliver and Morghan.
        /// </summary>
        /// <returns>If the game is lost.</returns>
        private bool checkIfLost()
        {
            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    if (!board.isTileAt(x, y))
                        return false;

                    int currentNum = board.tileNumberAt(x, y);

                    int xMin = x - 1;
                    int xMax = x + 1;
                    int yMin = y - 1;
                    int yMax = y + 1;
                    
                    if (currentNum == board.tileNumberAt(xMin, y))
                        return false;
                    if (currentNum == board.tileNumberAt(xMax, y))
                        return false;
                    if (currentNum == board.tileNumberAt(x, yMin))
                        return false;
                    if (currentNum == board.tileNumberAt(x, yMax))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Logic for "New Game" button press.
        /// 
        /// Written by Aidan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onNewGameClick(object sender, RoutedEventArgs e)
        {
            lostGame = false;
            board.reset();
            addRandomTile();
            addRandomTile();
        }

        /// <summary>
        /// Adds to the current score.
        /// 
        /// Written by Aidan and Morghan.
        /// </summary>
        /// <param name="add">Amount to add.</param>
        private void addToScore(int add)
        {
            int currentScore;
            int.TryParse(scoreOutputLabel.Content.ToString(), out currentScore);
            currentScore += add;
            scoreOutputLabel.Content = currentScore.ToString();
            //Need to get score to reset
            //Centre the score
        }

    }

}