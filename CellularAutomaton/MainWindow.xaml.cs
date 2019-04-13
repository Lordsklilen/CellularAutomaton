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
        DrawingHelper drawingHelper;
        public MainWindow()
        {
            InitializeComponent();
            Initializevariables();
            Loaded += DrawInitialRow;
        }
        void Initializevariables()
        {
            width = 50;
            height = 25;
            _engineFacade = new EngineFacade();
            _engineFacade.Create1DCellularAutomation(width, height);
        }
        void DrawInitialRow(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(superCanvas, new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.White), width, height);
            var result = _engineFacade.GetNextIteration();
            drawingHelper.DrawFirstRow(result);
        }

        // Event Handling
        private void bt_CLick(object sender, RoutedEventArgs e)
        {
            var result = _engineFacade.GetNextIteration();
            drawingHelper.DrawBoard(result);
        }
    }
}
