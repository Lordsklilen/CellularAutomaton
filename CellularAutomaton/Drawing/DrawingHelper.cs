﻿using CellularAutomaton.Drawing;
using EngineProject.DataStructures;
using EngineProject.DataStructures.interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace CellularAutomaton
{
    public class DrawingHelper
    {
        Graphics g;
        System.Windows.Controls.Image wpfImage;
        Bitmap bitmap;
        BoardTemplateBuilder builder;
        BrushFactory brushFactory;
        int x;
        int y;
        double elHeight;
        double elWidth;
        int numHeightCells;
        int numWidthCells;
        public bool net;
        public bool squares { get ;private set;}

        public DrawingHelper(System.Windows.Controls.Image _img, int numX, int numY, bool net = true, bool squares = false)
        {
            wpfImage = _img;
            y = (int)wpfImage.Height;
            x = (int)wpfImage.Width;
            builder = new BoardTemplateBuilder();
            brushFactory = new BrushFactory();
            this.net = net;
            PrepareToDraw(numX, numY);
            this.squares = squares;
        }

        public void PrepareToDraw(int numX, int numY)
        {
            numHeightCells = numY;
            numWidthCells = numX;
            SquaresSize();
        }
        private void SquaresSize()
        {
            elHeight = (double)y / (double)(numHeightCells);
            elWidth = (double)x / (double)(numWidthCells);
            if (squares)
            {
                if (elHeight < elWidth)
                    elWidth = elHeight;
                if (elWidth < elHeight)
                    elHeight = elWidth;
            }
        }

        public void SetSquareAndReload(bool _square)
        {
            squares = _square;
            SquaresSize();
        }

        public void DrawFirstRow(Board board)
        {
            bitmap = new Bitmap(x, y);
            g = Graphics.FromImage(bitmap);
            foreach (var el in board.board[0])
            {
                DrawRectangle(el, (int)((double)el.Y() * (elWidth)), (int)((double)el.X() * (elHeight)), board);
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
                    DrawRectangle(el, (int)((double)el.Y() * (elWidth)), (int)((double)el.X() * (elHeight)), board);
                }
            }
            wpfImage.Source = Convert(bitmap);
        }

        private void DrawRectangle(ICell element, int x, int y, Board board)
        {
            Brush brush;
            switch (element.GetCellType())
            {
                case CellType.Cell:
                    brush = brushFactory.CreateBinaryBrush(element.GetState());
                    break;
                case CellType.Grain:
                    brush = brushFactory.CreateColorBrush(((Grain)element).GetGrainNumber(), board.MaxNumber());
                    break;
                default:
                    throw new NotSupportedException("Cannot create brush. Cell type not supproted");
            }
            g.FillRectangle(
                brush,
                x,
                y,
                (int)(net ? elWidth - 1 : elWidth),
               (int)(net ? elHeight - 1 : elHeight)
            );
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public Point GetPosition(int width, int height)
        {
            var result = new Point();
            result.X = (int)(((double)height) / (elHeight));
            result.Y = (int)(((double)width) / (elWidth));
            return result;
        }

        public void PrepareTemplate(GOLTemplatesEnum type, Board board)
        {
            switch (type)
            {
                case GOLTemplatesEnum.Clear:
                    builder.BuildClear(board);
                    break;
                case GOLTemplatesEnum.Oscilator:
                    builder.BuildOscilator(board);
                    break;
                case GOLTemplatesEnum.Glider:
                    builder.BuildGlider(board);
                    break;
                case GOLTemplatesEnum.Random:
                    builder.BuildRandom(board);
                    break;
                default:
                    throw new System.Exception(string.Format(@"Template {0} is not recognized", type.ToString()));
            }
        }

    }
}
