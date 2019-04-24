using EngineProject;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using EngineProject.DataStructures;

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
            Frame.Content = new OneDimensionPage();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Frame.Content = new OneDimensionPage();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Content = new GameOfLifePage();
        }
    }
}
