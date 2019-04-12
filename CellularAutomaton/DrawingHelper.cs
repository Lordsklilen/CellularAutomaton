using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CellularAutomaton
{
    public class DrawingHelper
    {
        Canvas canvas;
        Brush solidColorBrush;
        int x;
        int y;
        public DrawingHelper(Canvas _canvas, Brush _solidColorBrush) {
            canvas = _canvas;
            solidColorBrush = _solidColorBrush;
        }

        public void DrawBoard(Board board){
            // TODO
        }
        private void DrawRectangle(int x, int y) {
            Rectangle rect;
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = 5;
            rect.Height = 5;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }
    }
}
