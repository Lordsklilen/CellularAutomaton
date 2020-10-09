using CellularAutomaton.Drawing;
using EngineProject;
using EngineProject.DataStructures;
using EngineProject.Engines.DRX;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;
using EngineProject.Templates.GrainTemplates;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        static DispatcherTimer timer;
        static DispatcherTimer Recrystalizationtimer;
        readonly EngineType engineType = EngineType.GrainGrowth;
        int numberOfGrains = 0;
        decimal t;
        decimal tMax;

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
            engine = new EngineComponent();
            t = 0;
            tMax = 0;
            engine.CreateEngine(engineType, width, height);
            timer = new DispatcherTimer();
            Recrystalizationtimer = new DispatcherTimer();
            SetTime();
            timer.Tick += Start_Ticking_timer;
            Recrystalizationtimer.Tick += StartRecrystalization_Ticking_timer;
        }

        void SetTime()
        {
            double.TryParse(FpsCounter.Text, out double result);
            if (result < 1 || result > 999)
                result = 1;
            timer.Interval = TimeSpan.FromMilliseconds((int)(1000.0 / result));
            Recrystalizationtimer.Interval = TimeSpan.FromMilliseconds((int)(1000.0 / result));
        }

        void InitBoard()
        {
            t = 0;
            tMax = 0;
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

        private void StartRecrystalization_Ticking_timer(object sender, EventArgs e)
        {
            decimal.TryParse(dt_DRX_textbox.Text, out decimal dt);
            t += dt;
            var board = engine.NextDRXIteration(t);
            drawingHelper.DrawBoard(board);
            time_label.Content = "Czas: " + t;
            decimal max = board.MaxDensity();
            maxRecVal_label.Content = "MaxVal: " + max.ToString("0.###E+0");
            if (t >= tMax)
            {
                StopRecrystalization_Click(null, null);
            }
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
            t = 0;
            tMax = 0;
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
            drawingHelper.SetSquareAndReload(!drawingHelper.Squares);
            drawingHelper.DrawBoard(engine.Board);
        }

        private void GenrateMonteCarlo(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CalculateMonteCarlo(request);
            drawingHelper.DrawBoard(engine.Board);
        }

        private void ViewEnergy(object sender, RoutedEventArgs e)
        {
            if (drawingHelper.drawingType == DrawingType.DrawEnergy)
                drawingHelper.drawingType = DrawingType.DrawBoard;
            else
                drawingHelper.drawingType = DrawingType.DrawEnergy;
            var request = CreateMonteCarloRequest();
            request.numberOfIterations = 0;
            engine.CalculateMonteCarlo(request);
            drawingHelper.DrawBoard(engine.Board);
        }

        private void CenterPoints_Click(object sender, RoutedEventArgs e)
        {
            drawingHelper.centerPoints = !drawingHelper.centerPoints;
            drawingHelper.DrawBoard(engine.Board);
        }

        private void GenrateDRX(object sender, RoutedEventArgs e)
        {
            var result = engine.CalculateDRX(CreateDRXRequest());
            drawingHelper.DrawBoard(result);
        }

        private void ViewReclystalization(object sender, RoutedEventArgs e)
        {
            if (drawingHelper.drawingType == DrawingType.DrawRecrystalization)
                drawingHelper.drawingType = DrawingType.DrawBoard;
            else
                drawingHelper.drawingType = DrawingType.DrawRecrystalization;
            drawingHelper.DrawBoard(engine.Board);
        }

        private void ViewDensity(object sender, RoutedEventArgs e)
        {
            if (drawingHelper.drawingType == DrawingType.DrawDensity)
                drawingHelper.drawingType = DrawingType.DrawBoard;
            else
                drawingHelper.drawingType = DrawingType.DrawDensity;
            drawingHelper.DrawBoard(engine.Board);
        }

        private void StartRecrystalization_CLick(object sender, RoutedEventArgs e)
        {
            startRecrystalization_btn.IsEnabled = false;
            stopRecrystalization_btn.IsEnabled = true;
            var r = CreateDRXRequest();
            engine.InitializeDRX(r);
            decimal.TryParse(tEntire_DRX_textbox.Text, out decimal tmp);
            tMax += tmp;
            Recrystalizationtimer.Start();
        }

        private void StopRecrystalization_Click(object sender, RoutedEventArgs e)
        {
            startRecrystalization_btn.IsEnabled = true;
            stopRecrystalization_btn.IsEnabled = false;
            Recrystalizationtimer.Stop();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt|All files(*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string text = engine.GetSaveText();
                File.WriteAllText(saveFileDialog.FileName, text);
            }
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

        private DRXRequest CreateDRXRequest()
        {
            var drxRequest = new DRXRequest();

            decimal.TryParse(A_DRX_textbox.Text, out drxRequest.A);
            decimal.TryParse(B_DRX_textbox.Text, out drxRequest.B);
            decimal.TryParse(dt_DRX_textbox.Text, out drxRequest.dt);
            decimal.TryParse(tEntire_DRX_textbox.Text, out drxRequest.tMax);

            return drxRequest;
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


            request.border = Open_Radiobtn.IsChecked ?? false;

            return request;
        }

        private TemplateRequest BuildTemplateRequest()
        {
            var request = new TemplateRequest
            {
                board = engine.Board
            };
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
