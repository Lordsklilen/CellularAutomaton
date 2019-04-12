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
        int elHeight;
        int elWidth;
        int numHeightCells;
        int numWidthCells;
        public DrawingHelper(Canvas _canvas, Brush _solidColorBrush,int numX, int numY) {
            canvas = _canvas;
            solidColorBrush = _solidColorBrush;
            x = (int)canvas.ActualHeight;
            y = (int)canvas.ActualWidth;
            numHeightCells = numX;
            numWidthCells = numY;
            elHeight = x / (numX + 1);
            elWidth = y / (numY + 1);
        }

        public void DrawBoard(Board board){
            foreach (var row in board.board) {
                foreach (var el in row) {
                    DrawRectangle(el.x*(elWidth + 1), el.y*(elHeight+1));
                }
            }

        }
        private void DrawRectangle(int x, int y) {
            Rectangle rect;
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = elWidth;
            rect.Height = elHeight;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }
    }
}
