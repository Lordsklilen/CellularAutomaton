using EngineProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EngineFacade _engineFacade;
        int width;
        int height;
        public MainWindow()
        {
            InitializeComponent();
            // initialization
            width = 50;
            height = 100;
            _engineFacade = new EngineFacade();
            _engineFacade.Create1DCellularAutomation(width, height);
        }



        // Event Handling
        private void bt_CLick(object sender, RoutedEventArgs e)
        {
            DrawingHelper drawingHelper = new DrawingHelper(superCanvas, new SolidColorBrush(Colors.Black), width, height); 
            var result = _engineFacade.GetNextIteration();
            drawingHelper.DrawBoard(result);

          
        }
    }
}
