using CellularAutomaton.Drawing;
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
        public bool centerPoints = true;
        public bool squares { get; private set; }
        public bool energyFocus { get; set; }

        public DrawingHelper(System.Windows.Controls.Image _img, int numX, int numY, bool net = true, bool squares = false)
        {
            energyFocus = false;
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

        public void DrawCenterMassBoard(Board board)
        {
            bitmap = new Bitmap(x, y);
            g = Graphics.FromImage(bitmap);
            for (int i = 0; i < board.board.Length; i++)
            {
                for (int j = 0; j < board.board[i].Length; j++)
                {
                    var center = (board.board[i][j]as Grain).GetMassCenter();
                    g.FillRectangle(
                           brushFactory.CreateCenterOfMassBrush(),
                           (float)center.X,
                           (float)center.Y,
                           (float)center.X,
                           (float)center.Y
                       );
                }
            }
            wpfImage.Source = Convert(bitmap);
        }


        public void DrawBoard(Board board)
        {

            bitmap = new Bitmap(x, y);
            g = Graphics.FromImage(bitmap);
            for (int i = 0; i < board.board.Length; i++)
            {
                for (int j = 0; j < board.board[i].Length; j++)
                {
                    var el = board.board[i][j];
                    int width = (int)((double)el.Y() * (elWidth));
                    int height = (int)((double)el.X() * (elHeight));
                    Rectangle rect = Rectangle.FromLTRB(
                        width,
                        height,
                        (int)(net ? ((double)(el.Y() + 1.0) * (elWidth)) - 1.0 : (double)(el.Y() + 1.0) * (elWidth)),
                        (int)(net ? ((double)(el.X() + 1.0) * (elHeight)) - 1.0 : (double)(el.X() + 1.0) * (elHeight))
                        );
                    if (energyFocus)
                        DrawEnergyRectangle(el, rect, board);
                    else
                        DrawRectangle(el, rect, board);
                    if (centerPoints)
                        DrawCenter(width, height, el as Grain);
                }
            }





            //var el1 = board.board[0][0];
            //var el2 = board.board[0][1];
            var el3 = board.board[1][1];

            //var center1 = (el1 as Grain).GetInsideMassCenter();
            //var center5Main = (el5 as Grain).GetMassCenter();
            //var center5 = (el5 as Grain).GetInsideMassCenter();
            //var w = el5.X() + center5.X;
            //var h = el5.Y() + center5.Y; ;
            //var center4 = (el2 as Grain).GetMassCenter();



            Pen skyBluePen = new Pen(Brushes.DeepSkyBlue);
            //g.DrawLine(skyBluePen,
            //     (float)((double)center3.X * (elWidth)),
            //       (float)((double)center3.Y * (elHeight)),
            //        (float)((double)center4.X * (elWidth)),
            //       (float)((double)center4.Y * (elHeight))
            //       );

            //g.DrawLine(skyBluePen,
            //   (float)((double)(w)* (elWidth)),
            //     (float)((double)(h)* (elHeight)),
            //      (float)((double)center4.X * (elWidth)),
            //     (float)((double)center4.Y * (elHeight))
            //     );



            var center3 = (el3 as Grain).GetMassCenter();
            float centerX = (float)((double)center3.X * (elWidth));
            float centerY = (float)((double)center3.Y * (elHeight));
            g.DrawEllipse(skyBluePen, centerX - (float)elWidth, centerY - (float)elHeight,
                                 (float)(2* elWidth), (float)(2 * elHeight));
            wpfImage.Source = Convert(bitmap);
        }

        private void DrawCenter(int width, int height, Grain el) {
            var center = (el as Grain).GetMassCenter();
            g.FillRectangle(
                   brushFactory.CreateCenterOfMassBrush(),
                   (float)((double)center.X * (elWidth)),
                   (float)((double)center.Y * (elHeight)),
                   2,
                   2
               );

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
        private void DrawRectangle(ICell element, Rectangle rectangle, Board board)
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
                rectangle
            );
        }

        private void DrawEnergyRectangle(ICell element, Rectangle rectangle, Board board)
        {
            g.FillRectangle(
                brushFactory.CreateEnergyBrush((element as Grain).E),
                rectangle
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
