/* Unit 4 Group Project
 * May 17, 2019
 * Oliver Byl, Morghan Kiverago, Aidan Hobman
 */
using System.Windows;

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();

            //Create new Game and feed it the canvas.
            //No other init code is necessary.
            game = new Game(canvas);
        }

        /// <summary>
        /// Runs when the window closes.
        /// Saves the high score to a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            game.saveHighScore();
        }
    }
}
