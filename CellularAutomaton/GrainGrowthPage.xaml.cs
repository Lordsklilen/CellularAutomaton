using CellularAutomaton.Drawing;
using EngineProject.DataStructures;
using EngineProject.Engines.DRX;
using EngineProject.Engines.Engines;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;
using EngineProject.Templates.GrainTemplates;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CellularAutomaton
{
    /// <summary>
    /// Interaction logic for GrainGrowthPage.xaml
    /// </summary>
    public partial class GrainGrowthPage : Page
    {
        int width = 100;
        int height = 75;
        int numberOfGrains = 0;
        decimal t = 0;
        decimal tMax = 0;
        GrainGrowthEngine engine = new GrainGrowthEngine(100, 75);
        readonly GrainTemplateFactory templateFactory = new GrainTemplateFactory();
        DrawingHelper drawingHelper;

        public GrainGrowthPage()
        {
            InitializeComponent();
            Loaded += DrawInitial;
            Loaded += InitEvents;
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
            engine = new GrainGrowthEngine(width, height);
        }

        void DrawInitial(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(img, width, height, true);
            drawingHelper.DrawBoard(engine.Panel);
        }

        void InitEvents(object sender, RoutedEventArgs e)
        {
            widthNumber.TextChanged += DrawAndReload;
            iterationNumber.TextChanged += DrawAndReload;
        }

        void DrawAndReload(object sender, RoutedEventArgs e)
        {
            InitBoard();
            drawingHelper.DrawBoard(engine.Panel);
        }

        private async void Start_CLick(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CreateMCEngine(request);
            start_btn.IsEnabled = false;
            stopBtn.IsEnabled = true;
            while (!engine.IsFinished && stopBtn.IsEnabled)
            {
                await Task.Run(() =>
                {
                    engine.NextIteration();
                });
                drawingHelper.DrawBoard(engine.Panel);
            }
            start_btn.IsEnabled = true;
            stopBtn.IsEnabled = false;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CreateMCEngine(request);
            while (!engine.IsFinished)
            {
                engine.NextIteration();
            }
            drawingHelper.DrawBoard(engine.Panel);
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {

            var mousePosition = e.GetPosition(img);
            var x = (int)mousePosition.X;
            var y = (int)mousePosition.Y;
            var position = drawingHelper.GetPosition(x, y);
            if (engine.Panel.GetGrainNumber(position.X, position.Y) == 0)
            {
                ++numberOfGrains;
                engine.SetGrainNumber(numberOfGrains, position.X, position.Y);

                drawingHelper.DrawBoard(engine.Panel);
            }
        }

        private void Generate_Template(object sender, RoutedEventArgs e)
        {
            t = 0;
            tMax = 0;
            try
            {
                var request = BuildTemplateRequest();
                var template = templateFactory.CreateTemplate(request.type);
                template.GenerateTemplate(request);

                engine.RecalculateEnergy();
                drawingHelper.DrawBoard(engine.Panel);
            }
            catch (Exception ex)
            {
                drawingHelper.DrawBoard(engine.Panel);
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
            engine.ChangeStrategyType(request);
            drawingHelper.DrawBoard(engine.Panel);

        }

        private void SetBorderCondition(object sender, RoutedEventArgs e)
        {
            bool OpenBorderCondition = Open_Radiobtn.IsChecked ?? false;
            engine.ChangeBorderConditions(OpenBorderCondition);
        }

        private void OnOffborder_Click(object sender, RoutedEventArgs e)
        {
            drawingHelper.net = !drawingHelper.net;
            drawingHelper.DrawBoard(engine.Panel);
        }

        private void Squares_Click(object sender, RoutedEventArgs e)
        {
            drawingHelper.SetSquareAndReload(!drawingHelper.Squares);
            drawingHelper.DrawBoard(engine.Panel);
        }

        private void GenrateMonteCarlo(object sender, RoutedEventArgs e)
        {
            var request = CreateMonteCarloRequest();
            engine.CreateMCEngine(request);
            engine.IterateMonteCarlo(request.numberOfIterations);
            drawingHelper.DrawBoard(engine.Panel);
        }

        private void ViewEnergy(object sender, RoutedEventArgs e)
        {
            if (drawingHelper.drawingType == DrawingType.DrawEnergy)
                drawingHelper.drawingType = DrawingType.DrawBoard;
            else
                drawingHelper.drawingType = DrawingType.DrawEnergy;
            var request = CreateMonteCarloRequest();
            request.numberOfIterations = 0;
            engine.CreateMCEngine(request);
            engine.IterateMonteCarlo(request.numberOfIterations);
            drawingHelper.DrawBoard(engine.Panel);
        }

        private void CenterPoints_Click(object sender, RoutedEventArgs e)
        {
            drawingHelper.centerPoints = !drawingHelper.centerPoints;
            drawingHelper.DrawBoard(engine.Panel);
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
            drawingHelper.DrawBoard(engine.Panel);
        }

        private void ViewDensity(object sender, RoutedEventArgs e)
        {
            if (drawingHelper.drawingType == DrawingType.DrawDensity)
                drawingHelper.drawingType = DrawingType.DrawBoard;
            else
                drawingHelper.drawingType = DrawingType.DrawDensity;
            drawingHelper.DrawBoard(engine.Panel);
        }

        private async void StartRecrystalization_CLick(object sender, RoutedEventArgs e)
        {
            startRecrystalization_btn.IsEnabled = false;
            stopRecrystalization_btn.IsEnabled = true;
            var r = CreateDRXRequest();
            engine.InitializeDRX(r);
            decimal.TryParse(tEntire_DRX_textbox.Text, out decimal tmp);
            tMax += tmp;
            decimal.TryParse(dt_DRX_textbox.Text, out decimal dt);
            do
            {
                Board panel = new Board(1, 1);
                await Task.Run(() =>
                {
                    t += dt;
                    panel = engine.NextDRXIteration(t);
                });
                drawingHelper.DrawBoard(panel);
                time_label.Content = "Czas: " + t;
                decimal max = panel.MaxDensity();
                maxRecVal_label.Content = "MaxVal: " + max.ToString("0.###E+0");
            }
            while (t <= tMax && stopRecrystalization_btn.IsEnabled);
            startRecrystalization_btn.IsEnabled = true;
            stopRecrystalization_btn.IsEnabled = false;
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
                    request.neighbooorhoodType = NeighborhoodType.Moore;
                    break;
                case "Pentagonal":
                    request.neighbooorhoodType = NeighborhoodType.Pentagonal;
                    RandomHexOptions_radioBtn.IsChecked = true;
                    break;
                case "Hexagonal":
                    request.neighbooorhoodType = NeighborhoodType.Hexagonal;
                    LeftHexOptions_radioBtn.IsEnabled = true;
                    RightHexOptions_radioBtn.IsEnabled = true;
                    RandomHexOptions_radioBtn.IsEnabled = true;
                    break;
                case "Radius":
                    request.neighbooorhoodType = NeighborhoodType.Radius;
                    break;
                case "Von Neumann":
                default:
                    request.neighbooorhoodType = NeighborhoodType.VonNeumann;
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
                board = engine.Panel
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

        private void StopRecrystalization_btn_Click(object sender, RoutedEventArgs e)
        {
            stopRecrystalization_btn.IsEnabled = false;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            stopBtn.IsEnabled = false;
        }
    }
}
