using CellularAutomaton.Drawing;
using EngineProject;
using EngineProject.DataStructures;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;
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
        IEngineComponent engine;
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
        void Initializevariables()
        {
            width = 100;
            height = 75;
            engine = new EngineComponent(); // TODO DI
            engine.CreateEngine(engineType, width, height);
            timer = new System.Windows.Forms.Timer();
            SetTime();
            timer.Tick += Start_Ticking_timer;
        }
        void SetTime()
        {
            var result = 1.0;
            double.TryParse(FpsCounter.Text, out result);
            if (result < 1 || result > 999)
                result = 1;
            timer.Interval = (int)(1000.0 / result);
        }
        void InitBoard()
        {
            int.TryParse(widthNumber.Text, out width);
            int.TryParse(iterationNumber.Text, out height);
            if (width < 3 || width > 800)
                width = 3;
            if (height < 3 || height > 600)
                height = 3;

            drawingHelper.PrepareToDraw(width, height);
            engine.CreateEngine(engineType, width, height);
        }

        private void SetTime_timer(object sender, EventArgs e)
        {
            SetTime();
        }
        void DrawInitial(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(img, width, height, true);
            drawingHelper.DrawBoard(engine.Board);
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
            drawingHelper.DrawBoard(engine.Board);
        }
        private void Start_CLick(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CalculateEnergy(request);
            timer.Start();
            start_btn.IsEnabled = false;
            stopBtn.IsEnabled = true;
        }
        private void Start_Ticking_timer(object sender, EventArgs e)
        {
            engine.GetNextIteration();

            drawingHelper.DrawBoard(engine.Board);
            if (engine.IsFinished)
                Stop_Click(null, null);
        }
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CalculateEnergy(request);
            while (!engine.IsFinished)
            {
                engine.GetNextIteration();
            }
            drawingHelper.DrawBoard(engine.Board);
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            start_btn.IsEnabled = true;
            stopBtn.IsEnabled = false;
            timer.Stop();
        }
        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {

            var mousePosition = e.GetPosition(img);
            var x = (int)mousePosition.X;
            var y = (int)mousePosition.Y;
            var position = drawingHelper.GetPosition(x, y);
            if (engine.Board.GetGrainNumber(position.X, position.Y) == 0)
            {
                ++numberOfGrains;
                engine.SetGrainNumber(numberOfGrains, position.X, position.Y);
                drawingHelper.DrawBoard(engine.Board);
            }
        }
        private void Generate_Template(object sender, RoutedEventArgs e)
        {
            try
            {
                var request = BuildTemplateRequest();
                engine.GenerateGrainTemplate(request);
                drawingHelper.DrawBoard(engine.Board);
            }
            catch (Exception ex)
            {
                drawingHelper.DrawBoard(engine.Board);
                MessageBox.Show(ex.Message);
            }
        }
        private void ChangeNeighbourStrategy(object sender, RoutedEventArgs e)
        {
            if ((neighboour_comboBox.SelectedValue as ComboBoxItem).Content == null || LeftHexOptions_radioBtn == null)
                return;
            LeftHexOptions_radioBtn.IsEnabled = false;
            RightHexOptions_radioBtn.IsEnabled = false;
            RandomHexOptions_radioBtn.IsEnabled = false;

            var request = CreateNeighbourhoodRequest();
            engine.ChangeNeighboroodType(request);
            drawingHelper.DrawBoard(engine.Board);

        }
        private void SetBorderCondition(object sender, RoutedEventArgs e)
        {
            bool OpenBorderCondition = Open_Radiobtn.IsChecked ?? false;
            engine.ChangeBorderConditions(OpenBorderCondition);
        }
        private void OnOffborder_Click(object sender, RoutedEventArgs e)
        {
            drawingHelper.net = !drawingHelper.net;
            drawingHelper.DrawBoard(engine.Board);
        }
        private void Squares_Click(object sender, RoutedEventArgs e)
        {
            drawingHelper.SetSquareAndReload(!drawingHelper.squares);
            drawingHelper.DrawBoard(engine.Board);
        }

        void GenrateMonteCarlo(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CalculateMonteCarlo(request);
            drawingHelper.DrawBoard(engine.Board);
        }

        void ViewEnergy(object sender, RoutedEventArgs e)
        {
            drawingHelper.energyFocus = !drawingHelper.energyFocus;
            var request = CreateMonteCarloRequest();
            request.numberOfIterations = 0;
            engine.CalculateMonteCarlo(request);
            drawingHelper.DrawBoard(engine.Board);
        }


        private MonteCarloRequest CreateMonteCarloRequest()
        {
            var mcRequest = new MonteCarloRequest();
            int.TryParse(MCNumberOfPoints_textbox.Text, out mcRequest.numberOfIterations);
            double.TryParse(MCtemperature_textbox.Text, out mcRequest.Kt);
            if (mcRequest.Kt <= 0)
            {
                mcRequest.Kt = 0.1;
                MCtemperature_textbox.Text = "0,1";
            }
            if (mcRequest.Kt > 6)
            {
                mcRequest.Kt = 6;
                MCtemperature_textbox.Text = "6";
            }
            if (mcRequest.numberOfIterations <= 0)
            {
                mcRequest.numberOfIterations = 1;
                MCNumberOfPoints_textbox.Text = "1";
            }
            mcRequest.border = Open_Radiobtn.IsChecked ?? false;
            mcRequest.strategyRequest = CreateNeighbourhoodRequest();
            return mcRequest;
        }
        private NeighbourStrategyRequest CreateNeighbourhoodRequest()
        {
            var request = new NeighbourStrategyRequest();

            switch ((neighboour_comboBox.SelectedValue as ComboBoxItem).Content.ToString())
            {
                case "Moore":
                    request.neighbooorhoodType = NeighbooorhoodType.Moore;
                    break;
                case "Pentagonal":
                    request.neighbooorhoodType = NeighbooorhoodType.Pentagonal;
                    RandomHexOptions_radioBtn.IsChecked = true;
                    break;
                case "Hexagonal":
                    request.neighbooorhoodType = NeighbooorhoodType.Hexagonal;
                    LeftHexOptions_radioBtn.IsEnabled = true;
                    RightHexOptions_radioBtn.IsEnabled = true;
                    RandomHexOptions_radioBtn.IsEnabled = true;
                    break;
                case "Radius":
                    request.neighbooorhoodType = NeighbooorhoodType.Radius;
                    break;
                case "Von Neumann":
                default:
                    request.neighbooorhoodType = NeighbooorhoodType.VonNeumann;
                    break;
            }

            if (LeftHexOptions_radioBtn.IsChecked ?? false) request.hexType = HexType.Left;
            else if (RightHexOptions_radioBtn.IsChecked ?? false) request.hexType = HexType.Right;
            else
            {
                request.hexType = HexType.Random;
            }

            double.TryParse(RadiusNeighbour_textbox.Text, out request.Radius);
            if (request.Radius < 1.0)
            {
                request.Radius = 1.0;
            }
            return request;
        }
        private TemplateRequest BuildTemplateRequest()
        {
            var request = new TemplateRequest();
            request.board = engine.Board;
            int.TryParse(Random_textBox.Text, out request.numberOfPoints);
            int.TryParse(Radius_textBox.Text, out request.radius);
            int.TryParse(Xhomogenious_textbox.Text, out request.x);
            int.TryParse(Yhomogenious_textbox.Text, out request.y);
            if (Radius_RadioBtn.IsChecked ?? false)
                request.type = GrainTemplateType.Radius;
            else if (Random_RadioBtn.IsChecked ?? false)
                request.type = GrainTemplateType.Random;
            else if (Homogenius_RadioBtn.IsChecked ?? false)
                request.type = GrainTemplateType.Homogeneous;
            else
                request.type = GrainTemplateType.Clear;
            return request;
        }

    }
}
