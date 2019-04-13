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
        Brush colorBrush;
        Brush deadBrush;
        int x;
        int y;
        int elHeight;
        int elWidth;
        int numHeightCells;
        int numWidthCells;
        public DrawingHelper(Canvas _canvas, Brush _mainBrush, Brush _deadBrush, int numX, int numY)
        {
            canvas = _canvas;
            colorBrush = _deadBrush;
            deadBrush = _deadBrush;
            y = (int)canvas.ActualHeight;
            x = (int)canvas.ActualWidth;
            PrepareToDraw(numX, numY);
        }
        void PrepareToDraw(int numX, int numY)
        {
            numHeightCells = numY;
            numWidthCells = numX;
            elHeight = y / (numY);
            elWidth = x / (numX);
        }

        public void DrawFirstRow(Board board)
        {
            foreach (var el in board.board.FirstOrDefault())
            {
                DrawRectangle(el, el.y * (elWidth + 1), el.x * (elHeight + 1));
            }
        }

        public void DrawBoard(Board board)
        {
            foreach (var row in board.board)
            {
                foreach (var el in row)
                {
                    DrawRectangle(el, el.y * (elWidth + 1), el.x * (elHeight + 1));
                }
            }

        }
        private void DrawRectangle(Cell element, int x, int y)
        {
            Rectangle rect;
            rect = new Rectangle();
            if (element.state)
            {
                rect.Stroke = colorBrush;
                rect.Fill = colorBrush;
            }
            else
            {
                rect.Stroke = deadBrush;
                rect.Fill = deadBrush;
            }
            rect.Width = elWidth;
            rect.Height = elHeight;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }
    }
}
