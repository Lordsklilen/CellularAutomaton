using EngineProject;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using EngineProject.DataStructures;
using System.Windows.Controls;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for OneDimensionPage.xaml
    /// </summary>
    public partial class OneDimensionPage : Page
    {
        IEngineComponent _engineFacade;
        int width;
        int height;
        DrawingHelper drawingHelper;
        public OneDimensionPage()
        {
            InitializeComponent();
            Initializevariables();
            Loaded += DrawInitialRow;
            Loaded += InitEvents;
        }
        void InitEvents(object sender, RoutedEventArgs e)
        {
            widthNumber.TextChanged += DrawInitialRowAndReload;
            iterationNumber.TextChanged += DrawInitialRowAndReload;
        }
        void Initializevariables()
        {
            width = 100;
            height = 50;
            _engineFacade = new EngineComponent(); // TODO DI
            _engineFacade.CreateEngine(EngineType.OneDimensionEngine, width, height);
        }
        void InitBoard()
        {
            int.TryParse(widthNumber.Text, out width);
            int.TryParse(iterationNumber.Text, out height);
            if (width < 1)
                width = 1;
            if (height < 2)
                height = 2;
            _engineFacade.CreateEngine(EngineType.OneDimensionEngine, width, height);
        }
        void DrawInitialRowAndReload(object sender, RoutedEventArgs e)
        {
            InitBoard();
            DrawInitialRow(sender, e);
        }
        void DrawInitialRow(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(img, new SolidBrush(Color.Black), new SolidBrush(Color.White), width, height);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawFirstRow(result);
        }

        // Event Handling
        private void Iterate_CLick(object sender, RoutedEventArgs e)
        {
            int rule = 90;
            int.TryParse(ruleNumber.Text, out rule);
            _engineFacade.SetRule(rule);
            for (int i = 1; i <= height; i++)
            {
                _engineFacade.GetNextIteration();
            }
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(img);
            var x = (int)mousePosition.X;
            var y = (int)mousePosition.Y;
            var position = drawingHelper.GetPosition(x, y);
            _engineFacade.ChangeCellState((int)position.X, (int)position.Y);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawFirstRow(result);
        }

        private void IterationNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            DrawInitialRow(sender, e);
        }
    }
}
