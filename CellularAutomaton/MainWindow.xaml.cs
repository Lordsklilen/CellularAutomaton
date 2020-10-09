using System.Windows;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Frame.Content = new GrainGrowthPage();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Content = new GrainGrowthPage();
        }
    }
}
