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
        public MainWindow()
        {
            InitializeComponent();

            //Create new Game and feed it the canvas.
            //No other init code is necessary.
            new Game(canvas);
        }
    }
}
