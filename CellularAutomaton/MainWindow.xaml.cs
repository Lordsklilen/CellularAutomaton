using EngineProject;
using System.Windows;
using System.Windows.Input;
using System.Drawing;

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
            drawingHelper = new DrawingHelper(img, new SolidBrush(Color.Black), new SolidBrush(Color.White), width, height);
            var result = _engineFacade.GetNextIteration();
            drawingHelper.DrawFirstRow(result);
        }

        // Event Handling
        private void bt_CLick(object sender, RoutedEventArgs e)
        {
            var result = _engineFacade.GetNextIteration();
            drawingHelper.DrawBoard(result);
        }

        // change 
        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(img);
            var x = (int)mousePosition.X;
            var y = (int)mousePosition.Y;
            var position = drawingHelper.GetPosition(x, y);
            _engineFacade.SetCellState((int)position.X, (int)position.Y, true);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }
    }
}
