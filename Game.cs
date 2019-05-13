
using System;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Input;

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
        //How many times per second the game will be updated:
        private const int FPS = 60;

        //All display variables.
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

        //All logical variables.
        private Board board;
        private Random random;
        private bool lostGame;
        private int highScore;

        public Game(Canvas canvas)
        {
            this.canvas = canvas;
            canvas.Background = new SolidColorBrush(Color.FromRgb(251, 248, 240));

            //Initialize all visual elements.
            initHeader();

            //Initialize logical variables.
            board = new Board();
            random = new Random();
            lostGame = false;

            //Create a timer to call the tick() and render() methods 60 times per second.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += tick;
            timer.Tick += render;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            //Start the timer.
            timer.Start();

            //Begin the game with two tiles on the board in a random position.
            addRandomTile();
            addRandomTile();
        }

        /// <summary>
        /// Initializes all non-board items of the display.
        /// </summary>
        private void initHeader()
        {
            //Written by Oliver:

            //Game title:
            title = new Label();
            title.Content = "2048";
            title.FontSize = 80;
            title.FontFamily = new FontFamily("Arial");
            title.FontWeight = FontWeights.Bold;
            title.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(title, 12);
            Canvas.SetTop(title, 10);

            //Part 1 of the game description:
            desc1 = new Label();
            desc1.Content = "Join the numbers and get to the";
            desc1.FontSize = 18;
            desc1.FontFamily = new FontFamily("Arial");
            desc1.FontWeight = FontWeights.Light;
            desc1.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(desc1, 12);
            Canvas.SetTop(desc1, 123);

            //Part 2 of the game description:
            desc2 = new Label();
            desc2.Content = "2048 tile!";
            desc2.FontSize = 18;
            desc2.FontFamily = new FontFamily("Arial");
            desc2.FontWeight = FontWeights.Bold;
            desc2.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(desc2, 270);
            Canvas.SetTop(desc2, 123);

            //The label that shows when you lose the game:
            loseLabel = new Label();
            loseLabel.Content = "Game Over!";
            loseLabel.FontSize = 60;
            loseLabel.FontFamily = new FontFamily("Arial");
            loseLabel.FontWeight = FontWeights.Bold;
            loseLabel.Foreground = new SolidColorBrush(Color.FromRgb(118, 110, 102));
            Canvas.SetLeft(loseLabel, 120);
            Canvas.SetTop(loseLabel, 400);

            //The background of the score:
            scoreBackground = new Rectangle();
            scoreBackground.Width = 120;
            scoreBackground.Height = 60;
            scoreBackground.Fill = new SolidColorBrush(Color.FromRgb(182, 170, 158));
            Canvas.SetLeft(scoreBackground, 320);
            Canvas.SetTop(scoreBackground, 5);

            //The background of the high score:
            highScoreBackground = new Rectangle();
            highScoreBackground.Width = 120;
            highScoreBackground.Height = 60;
            highScoreBackground.Fill = new SolidColorBrush(Color.FromRgb(182, 170, 158));
            Canvas.SetLeft(highScoreBackground, 450);
            Canvas.SetTop(highScoreBackground, 5);

            //The rectangle that overlays the board when you lose:
            loseOverlay = new Rectangle();
            loseOverlay.Width = 560;
            loseOverlay.Height = 560;
            loseOverlay.Fill = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));
            Canvas.SetLeft(loseOverlay, 12);
            Canvas.SetTop(loseOverlay, 190);

            //Written by Aidan:

            //The score title:
            scoreLabel = new Label();
            scoreLabel.Content = "SCORE";
            scoreLabel.FontSize = 17;
            scoreLabel.FontFamily = new FontFamily("Arial");
            scoreLabel.FontWeight = FontWeights.Bold;
            scoreLabel.Foreground = Brushes.White;
            Canvas.SetLeft(scoreLabel, 344);
            Canvas.SetTop(scoreLabel, 7);

            //The high score title:
            highScoreLabel = new Label();
            highScoreLabel.Content = "BEST";
            highScoreLabel.FontSize = 17;
            highScoreLabel.FontFamily = new FontFamily("Arial");
            highScoreLabel.FontWeight = FontWeights.Bold;
            highScoreLabel.Foreground = Brushes.White;
            Canvas.SetLeft(highScoreLabel, 481);
            Canvas.SetTop(highScoreLabel, 7);

            //The label that shows the score:
            scoreOutputLabel = new Label();
            scoreOutputLabel.Content = "0";
            scoreOutputLabel.FontSize = 20;
            scoreOutputLabel.FontFamily = new FontFamily("Arial");
            scoreOutputLabel.FontWeight = FontWeights.Bold;
            scoreOutputLabel.Foreground = Brushes.White;
            Canvas.SetLeft(scoreOutputLabel, 369);
            Canvas.SetTop(scoreOutputLabel, 30);

            //Read the high score from a file and store it in highScore
            StreamReader reader = new StreamReader("highscore.txt");
            int.TryParse(reader.ReadLine(), out highScore);

            //The label that shows the high score:
            highScoreOutputLabel = new Label();
            highScoreOutputLabel.Content = highScore.ToString();
            highScoreOutputLabel.FontSize = 20;
            highScoreOutputLabel.FontFamily = new FontFamily("Arial");
            highScoreOutputLabel.FontWeight = FontWeights.Bold;
            highScoreOutputLabel.Foreground = Brushes.White;
            Canvas.SetTop(highScoreOutputLabel, 30);

            reader.Close();

            //The button that starts a new game:
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
                //Update the board even if the game is lost so tiles
                //finish their animations.
                board.tick();
                return;
            }

            //Update all key presses.
            Input.tick();

            //Written by Morghan:

            //The logic for each key press is similar but not similar enough to
            //seperate into a method.
            //Basically, for each key press:
            //1. Visualize the board roatated such that the top is the direction the
            //tiles should move in.
            //2. Loop from the top left to the bottom right.
            //3. If a tile is found, loop from its current position and move it as far 
            //as possible back up the board.
            //4. Combine if possible and if the tile hasn't already combined.

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
            
            //Update all tile animations:
            board.tick();
        }

        private int time = 0;

        /// <summary>
        /// Redraw all necessary things to the screen, 60 times per second.
        /// 
        /// Written by Oliver.
        /// </summary>
        private void render(object sender, EventArgs e)
        {
            //Clear all elements from the canvas:
            canvas.Children.Clear();

            //Add all updated elements to the canvas:
            canvas.Children.Add(title);
            canvas.Children.Add(desc1);
            canvas.Children.Add(desc2);
            canvas.Children.Add(scoreBackground);
            canvas.Children.Add(highScoreBackground);
            canvas.Children.Add(newGame);
            canvas.Children.Add(scoreLabel);
            canvas.Children.Add(highScoreLabel);
            canvas.Children.Add(scoreOutputLabel);
            canvas.Children.Add(highScoreOutputLabel); 

            //Draw the board:
            board.render(canvas, 12, 190);

            //Update the position of the high score label.
            //The property AcutalWidth is not set until a
            //draw pass has already been made, so we need
            //to wait until now to use it.
            if (time >= 0)
            {
                time++;
                Canvas.SetLeft(highScoreOutputLabel, 510 - (highScoreOutputLabel.ActualWidth / 2));

                if (time > 1)
                    time = -1;
            }

            //Only draw the lose overlay if the game has been lost.
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
            //Check every tile on the board.
            for(int x = 0; x < 4; x++)
            {
                for(int y = 0; y < 4; y++)
                {
                    //If there is an available space the game has not
                    //been lost.
                    if (!board.isTileAt(x, y))
                        return false;

                    int currentNum = board.tileNumberAt(x, y);

                    int xMin = x - 1;
                    int xMax = x + 1;
                    int yMin = y - 1;
                    int yMax = y + 1;
                    
                    //If there is a tile on any side of the current tile
                    //equal to it (i.e. it's possible to combine them),
                    //the game has not been lost.
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
            scoreOutputLabel.Content = "0";
            Canvas.SetLeft(scoreOutputLabel, 369);
            Keyboard.ClearFocus();
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
            Canvas.SetLeft(scoreOutputLabel, 380 - (scoreOutputLabel.ActualWidth / 2));

            if (currentScore > highScore)
            {
                highScore = currentScore;
                highScoreOutputLabel.Content = highScore.ToString();
                Canvas.SetLeft(highScoreOutputLabel, 510 - (highScoreOutputLabel.ActualWidth / 2));
            }
        }

        /// <summary>
        /// Save the high score to a file.
        /// </summary>
        public void saveHighScore()
        {
            StreamWriter writer = new StreamWriter("highscore.txt");
            writer.WriteLine(highScore.ToString());
            writer.Flush();
            writer.Close();
        }
    }
}