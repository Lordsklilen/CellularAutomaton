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
        public MainWindow()
        {
            InitializeComponent();
            // initialization
            _engineFacade = new EngineFacade();
            _engineFacade.Create1DCellularAutomation();
        }



        // Event Handling
        private void bt_CLick(object sender, RoutedEventArgs e)
        {
            DrawingHelper drawingHelper = new DrawingHelper(superCanvas, new SolidColorBrush(Colors.Black)); 



            var result = _engineFacade.GetNextIteration();
            drawingHelper.DrawBoard(result);

            MessageBox.Show(result.board[0].ToString());
            //Rectangle rect;
            //rect = new System.Windows.Shapes.Rectangle();
            //rect.Stroke = new SolidColorBrush(Colors.Black);
            //rect.Fill = new SolidColorBrush(Colors.Black);
            //rect.Width = 5;
            //rect.Height = 5;
            //Canvas.SetLeft(rect, 0);
            //Canvas.SetTop(rect, 0);
            //superCanvas.Children.Add(rect);
        }
    }
}
