﻿using EngineProject.DataStructures;
using System.Drawing;
using System.IO;

namespace CellularAutomaton
{
    public class DrawingHelper
    {
        Graphics g;
        System.Windows.Controls.Image wpfImage;
        Bitmap bitmap;
        Brush colorBrush;
        Brush deadBrush;
        int x;
        int y;
        int elHeight;
        int elWidth;
        int numHeightCells;
        int numWidthCells;
        public DrawingHelper(System.Windows.Controls.Image _img, Brush _mainBrush, Brush _deadBrush, int numX, int numY)
        {
            wpfImage = _img;
            colorBrush = _mainBrush;
            deadBrush = _deadBrush;
            y = (int)wpfImage.Height;
            x = (int)wpfImage.Width;
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
            bitmap = new Bitmap(x, y);
            g = Graphics.FromImage(bitmap);
            foreach (var el in board.board[0])
            {
                DrawRectangle(el, el.y * (elWidth + 1), el.x * (elHeight + 1));
            }
            wpfImage.Source = Convert(bitmap);
        }

        public void DrawBoard(Board board)
        {
            bitmap = new Bitmap(x, y);
            g = Graphics.FromImage(bitmap);
            foreach (var row in board.board)
            {
                foreach (var el in row)
                {
                    DrawRectangle(el, el.y * (elWidth + 1), el.x * (elHeight + 1));
                }
            }
            wpfImage.Source = Convert(bitmap);
        }
        private void DrawRectangle(Cell element, int x, int y)
        {
            if (element.state)
                g.FillRectangle(
                    colorBrush,
                    x,
                    y,
                    elWidth,
                    elHeight
                );
            else
                g.FillRectangle(
                    deadBrush,
                    x,
                    y,
                    elWidth,
                    elHeight
                );
        }

        public System.Windows.Media.Imaging.BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            System.Windows.Media.Imaging.BitmapImage image = new System.Windows.Media.Imaging.BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public Point GetPosition(int x, int y)
        {
            var result = new Point();
            result.Y = x / (numWidthCells);
            result.X = y / (numHeightCells);
            return result;
        }
    }
}
