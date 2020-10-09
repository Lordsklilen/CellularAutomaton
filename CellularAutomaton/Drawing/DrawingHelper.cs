using CellularAutomaton.Drawing;
using EngineProject.DataStructures;
using EngineProject.DataStructures.interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CellularAutomaton
{
    public class DrawingHelper
    {
        Graphics g;
        readonly System.Windows.Controls.Image wpfImage;
        Bitmap bitmap;
        readonly BoardTemplateBuilder builder;
        readonly BrushFactory brushFactory;
        readonly int ImageWidth;
        readonly int ImageHeight;
        double elHeight;
        double elWidth;
        int numHeightCells;
        int numWidthCells;
        public DrawingType drawingType;
        public bool net;
        public bool centerPoints = false;
        public bool Squares { get; private set; }

        public DrawingHelper(System.Windows.Controls.Image _img, int numX, int numY, bool net = true, bool squares = false)
        {
            drawingType = DrawingType.DrawBoard;
            wpfImage = _img;
            ImageHeight = (int)wpfImage.Height;
            ImageWidth = (int)wpfImage.Width;
            builder = new BoardTemplateBuilder();
            brushFactory = new BrushFactory();
            this.net = net;
            PrepareToDraw(numX, numY);
            Squares = squares;
        }

        public void PrepareToDraw(int numX, int numY)
        {
            numHeightCells = numY;
            numWidthCells = numX;
            SquaresSize();
        }
        private void SquaresSize()
        {
            elHeight = ImageHeight / (double)(numHeightCells);
            elWidth = ImageWidth / (double)(numWidthCells);
            if (Squares)
            {
                if (elHeight < elWidth)
                    elWidth = elHeight;
                if (elWidth < elHeight)
                    elHeight = elWidth;
            }
        }

        public void SetSquareAndReload(bool _square)
        {
            Squares = _square;
            SquaresSize();
        }

        public void DrawFirstRow(Board board)
        {
            bitmap = new Bitmap(ImageWidth, ImageHeight);
            g = Graphics.FromImage(bitmap);
            foreach (var el in board.BoardContainer[0])
            {
                DrawRectangle(el, (int)(el.Y() * (elWidth)), (int)(el.X() * (elHeight)), board);
            }
            wpfImage.Source = Convert(bitmap);
        }

        public void DrawCenterMassBoard(Board board)
        {
            bitmap = new Bitmap(ImageWidth, ImageHeight);
            g = Graphics.FromImage(bitmap);
            for (int i = 0; i < board.BoardContainer.Length; i++)
            {
                for (int j = 0; j < board.BoardContainer[i].Length; j++)
                {
                    var center = (board.BoardContainer[i][j] as Grain).GetMassCenter();
                    g.FillRectangle(
                           brushFactory.CreateCenterOfMassBrush(),
                           center.X,
                           center.Y,
                           center.X,
                           (float)center.Y
                       );
                }
            }
            wpfImage.Source = Convert(bitmap);
        }

        public void DrawBoard(Board board)
        {
            decimal max = 0;
            decimal min = 0;
            if (drawingType == DrawingType.DrawDensity)
            {
                max = board.MaxDensity();
                min = board.MinDensity();
            }

            bitmap = new Bitmap(ImageWidth, ImageHeight);
            g = Graphics.FromImage(bitmap);
            for (int i = 0; i < board.BoardContainer.Length; i++)
            {
                for (int j = 0; j < board.BoardContainer[i].Length; j++)
                {
                    var el = board.BoardContainer[i][j];
                    int width = (int)(el.Y() * (elWidth));
                    int height = (int)(el.X() * (elHeight));
                    Rectangle rect = Rectangle.FromLTRB(
                        width,
                        height,
                        (int)(net ? ((double)(el.Y() + 1.0) * (elWidth)) - 1.0 : (double)(el.Y() + 1.0) * (elWidth)),
                        (int)(net ? ((double)(el.X() + 1.0) * (elHeight)) - 1.0 : (double)(el.X() + 1.0) * (elHeight))
                        );

                    switch (drawingType)
                    {
                        case DrawingType.DrawEnergy:
                            DrawEnergyRectangle(el, rect);
                            break;
                        case DrawingType.DrawDensity:
                            DrawDensityRectangle(el, rect, min, max);
                            break;
                        case DrawingType.DrawRecrystalization:
                            DrawRecrystalizationRectangle(el, rect);
                            break;
                        case DrawingType.DrawBoard:
                        default:
                            DrawRectangle(el, rect, board);
                            break;
                    }

                    if (centerPoints)
                        DrawCenter(el as Grain);
                }
            }
            wpfImage.Source = Convert(bitmap);
        }

        private void DrawCenter(Grain el)
        {
            var center = el.GetMassCenter();
            g.FillRectangle(
                   brushFactory.CreateCenterOfMassBrush(),
                   (float)(center.X * (elWidth)),
                   (float)(center.Y * (elHeight)),
                   2,
                   2
               );
        }

        private void DrawRectangle(ICell element, int x, int y, Board board)
        {
            Brush brush = (element.GetCellType()) switch
            {
                CellType.Cell => brushFactory.CreateBinaryBrush(element.GetState()),
                CellType.Grain => brushFactory.CreateColorBrush(((Grain)element).GetGrainNumber(), board.MaxNumber()),
                _ => throw new NotSupportedException("Cannot create brush. Cell type not supproted"),
            };
            g.FillRectangle(
                brush,
                x,
                y,
                (int)(net ? elWidth - 1 : elWidth),
               (int)(net ? elHeight - 1 : elHeight)
            );
        }

        private void DrawRectangle(ICell element, Rectangle rectangle, Board board)
        {
            Brush brush;
            switch (element.GetCellType())
            {
                case CellType.Cell:
                    brush = brushFactory.CreateBinaryBrush(element.GetState());
                    break;
                case CellType.Grain:
                    var el = (Grain)element;
                    if (el.IsRecrystallized)
                        brush = brushFactory.CreateRecrystalizationBrush(el.RecrystalizedNumber, board.maxRecrystalizedNumber);
                    else
                        brush = brushFactory.CreateColorBrush(el.GetGrainNumber(), board.MaxNumber());
                    break;
                default:
                    throw new NotSupportedException("Cannot create brush. Cell type not supproted");
            }
            g.FillRectangle(
                brush,
                rectangle
            );
        }

        private void DrawEnergyRectangle(ICell element, Rectangle rectangle)
        {
            g.FillRectangle(
                brushFactory.CreateEnergyBrush((element as Grain).E),
                rectangle
            );
        }

        private void DrawRecrystalizationRectangle(ICell element, Rectangle rectangle)
        {
            g.FillRectangle(
                brushFactory.CreateOnlyRecrystalizationBrush((element as Grain).IsRecrystallized),
                rectangle
            );
        }

        private void DrawDensityRectangle(ICell element, Rectangle rectangle, decimal min, decimal max)
        {
            g.FillRectangle(
                brushFactory.CreateDyslocationBrush((element as Grain).DyslocationDensity, min, max),
                rectangle
            );
        }

        private BitmapImage Convert(Bitmap src)
        {
            var ms = new MemoryStream();
            src.Save(ms, ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
        public static Bitmap BitmapSourceToBitmap2(BitmapSource srs)
        {
            int width = srs.PixelWidth;
            int height = srs.PixelHeight;
            int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
                using var btm = new Bitmap(width, height, stride, PixelFormat.Format1bppIndexed, ptr);
                return new Bitmap(btm);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public System.Drawing.Point GetPosition(int width, int height)
        {
            var result = new System.Drawing.Point
            {
                X = (int)(height / (elHeight)),
                Y = (int)(width / (elWidth))
            };
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
                    throw new Exception(string.Format(@"Template {0} is not recognized", type.ToString()));
            }
        }

    }
}
