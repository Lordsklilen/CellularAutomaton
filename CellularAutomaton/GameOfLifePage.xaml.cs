using EngineProject;
using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for GameOfLifePage.xaml
    /// </summary>
    public partial class GameOfLifePage : Page
    {
        IEngineComponent _engineFacade;
        int width;
        int height;
        DrawingHelper drawingHelper;
        public GameOfLifePage()
        {
            InitializeComponent();
            Initializevariables();
            Loaded += DrawInitial;
            Loaded += InitEvents;
        }
        void DrawInitial(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(img, new SolidBrush(Color.Black), new SolidBrush(Color.White), width, height);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }
        void Initializevariables()
        {
            width = 100;
            height = 50;
            _engineFacade = new EngineComponent(); // TODO DI
            _engineFacade.CreateEngine(EngineType.GameOfLife, width, height);
        }
        void InitBoard()
        {
            int.TryParse(widthNumber.Text, out width);
            int.TryParse(iterationNumber.Text, out height);
            if (width < 1)
                width = 1;
            if (height < 2)
                height = 2;

            drawingHelper.PrepareToDraw(width, height);
            _engineFacade.CreateEngine(EngineType.GameOfLife, width, height);
        }

        void InitEvents(object sender, RoutedEventArgs e)
        {
            widthNumber.TextChanged += DrawAndReload;
            iterationNumber.TextChanged += DrawAndReload;
        }
        void DrawAndReload(object sender, RoutedEventArgs e)
        {
            InitBoard();
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }
        private void Start_CLick(object sender, RoutedEventArgs e)
        {
            //TODO Ticktock
            _engineFacade.GetNextIteration();
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }

        private void Stop_CLick(object sender, RoutedEventArgs e)
        {
            //TODO Ticktock stop
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(img);
            var x = (int)mousePosition.X;
            var y = (int)mousePosition.Y;
            var position = drawingHelper.GetPosition(x, y);
            _engineFacade.ChangeCellState((int)position.X, (int)position.Y);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
