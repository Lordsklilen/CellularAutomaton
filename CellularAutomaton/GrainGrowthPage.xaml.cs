using CellularAutomaton.Drawing;
using EngineProject;
using EngineProject.DataStructures;
using EngineProject.Templates.GrainTemplates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for GrainGrowthPage.xaml
    /// </summary>
    public partial class GrainGrowthPage : Page
    {
        IEngineComponent _engineFacade;
        int width;
        int height;
        DrawingHelper drawingHelper;
        static System.Windows.Forms.Timer timer;
        EngineType engineType = EngineType.GrainGrowth;
        int numberOfGrains = 0;

        public GrainGrowthPage()
        {
            InitializeComponent();
            Initializevariables();
            Loaded += DrawInitial;
            Loaded += InitEvents;
        }
        void DrawInitial(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(img, width, height);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }
        void Initializevariables()
        {
            width = 100;
            height = 75;
            _engineFacade = new EngineComponent(); // TODO DI
            _engineFacade.CreateEngine(engineType, width, height);
            timer = new System.Windows.Forms.Timer();
            SetTime();
            timer.Tick += Start_Ticking_timer;
        }

        void SetTime()
        {
            var result = 1.0;
            double.TryParse(FpsCounter.Text, out result);
            if (result < 1)
                result = 1;
            timer.Interval = (int)(1000.0 / result);
        }
        private void SetTime_timer(object sender, EventArgs e)
        {
            SetTime();
        }


        void InitBoard()
        {
            int.TryParse(widthNumber.Text, out width);
            int.TryParse(iterationNumber.Text, out height);
            if (width < 3)
                width = 3;
            if (height < 3)
                height = 3;

            drawingHelper.PrepareToDraw(width, height);
            _engineFacade.CreateEngine(engineType, width, height);
        }

        void InitEvents(object sender, RoutedEventArgs e)
        {
            widthNumber.TextChanged += DrawAndReload;
            iterationNumber.TextChanged += DrawAndReload;
            FpsCounter.TextChanged += SetTime_timer;
        }
        void DrawAndReload(object sender, RoutedEventArgs e)
        {
            InitBoard();
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }
        private void Start_CLick(object sender, RoutedEventArgs e)
        {
            timer.Start();
            start_btn.IsEnabled = false;
            stopBtn.IsEnabled = true;
        }

        private void Start_Ticking_timer(object sender, EventArgs e)
        {
            _engineFacade.GetNextIteration();
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
            if (_engineFacade.IsFinished())
                Stop_Click(null, null);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            while (!_engineFacade.IsFinished())
            {
                _engineFacade.GetNextIteration();
            }
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            start_btn.IsEnabled = true;
            stopBtn.IsEnabled = false;
            timer.Stop();
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ++numberOfGrains;
            var mousePosition = e.GetPosition(img);
            var x = (int)mousePosition.X;
            var y = (int)mousePosition.Y;
            var position = drawingHelper.GetPosition(x, y);
            _engineFacade.SetGrainNumber(numberOfGrains, position.X, position.Y);
            var result = _engineFacade.GetBoard();
            drawingHelper.DrawBoard(result);
        }

        private void Generate_Template(object sender, RoutedEventArgs e)
        {
            try
            {
                var request = BuildTemplateRequest();
                _engineFacade.GenerateGrainTemplate(request);
                var result = _engineFacade.GetBoard();
                drawingHelper.DrawBoard(result);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private TemplateRequest BuildTemplateRequest() {
            var request = new TemplateRequest();
            request.board = _engineFacade.GetBoard();
            int.TryParse(Random_textBox.Text, out request.numberOfPoints);
            int.TryParse(Radius_textBox.Text, out request.radius);
            if (Radius_RadioBtn.IsChecked ?? false)
                request.type = GrainTemplateType.Radius;
            else if (Random_RadioBtn.IsChecked ?? false)
                request.type = GrainTemplateType.Random;
            else
                request.type = GrainTemplateType.Clear;
            return request;
        }
        private void NeighbourhoodComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("combobox changed");
        }

        private void SetBorderCondition(object sender, RoutedEventArgs e)
        {
            bool OpenBorderCondition  = Open_Radiobtn.IsChecked ?? false;
            _engineFacade.ChangeBorderConditions(OpenBorderCondition);
        }
        
    }
}
