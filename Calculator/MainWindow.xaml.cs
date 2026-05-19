using System.Windows;
using Alghoritms;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ExpressionParser parser = new ExpressionParser();
            parser.Event += Parser_Event;
            MessageBox.Show(parser.TryParse("4x-5+=11").ToString());
        }

        private void Parser_Event(string message)
        {
            MessageBox.Show(message);
        }
    }
}