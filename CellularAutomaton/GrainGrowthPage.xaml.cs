using CellularAutomaton.Drawing;
using EngineProject;
using EngineProject.DataStructures;
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
        void DrawInitial(object sender, RoutedEventArgs e)
        {
            drawingHelper = new DrawingHelper(img, width, height, true);
            drawingHelper.DrawBoard(engine.Board);
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
        private void SetTime_timer(object sender, EventArgs e)
        {
            SetTime();
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
        private void NeighbourhoodComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if ((neighboour_comboBox.SelectedValue as ComboBoxItem).Content == null)
                return;
            NeighbooorhoodType type;
            HexType hexType;
            switch ((neighboour_comboBox.SelectedValue as ComboBoxItem).Content.ToString())
            {
                case "Moore":
                    type = NeighbooorhoodType.Moore;
                    break;
                case "Pentagonal":
                    type = NeighbooorhoodType.Pentagonal;
                    break;
                case "Hexagonal":
                    type = NeighbooorhoodType.Hexagonal;
                    break;
                case "Von Neumann":
                default:
                    type = NeighbooorhoodType.VonNeumann;
                    break;
            }

            if (LeftHexOptions_radioBtn.IsChecked ?? false) hexType = HexType.Left;
            else if (RightHexOptions_radioBtn.IsChecked ?? false) hexType = HexType.Right;
            else hexType = HexType.Random;

            engine.ChangeNeighbooroodType(type,hexType);
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

        private void Send_hexData(object sender, RoutedEventArgs e)
        {
            if (LeftHexOptions_radioBtn.Content == null)
                return;
            if (LeftHexOptions_radioBtn.IsChecked ?? false)
                engine.ChangeHexType(HexType.Left);
            else if (RightHexOptions_radioBtn.IsChecked ?? false)
                engine.ChangeHexType(HexType.Right);
            else if (RandomHexOptions_radioBtn.IsChecked ?? false)
                engine.ChangeHexType(HexType.Random);
        }
    }
}
