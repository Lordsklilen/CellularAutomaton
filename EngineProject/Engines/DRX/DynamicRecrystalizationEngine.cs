using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines.DRX
{
    internal class DynamicRecrystalizationEngine : IDynamicRecrystalizationEngine
    {
        public IList<DensitySnapshot> TotalDensityList { get; set; }

        private List<Point> borderCoordinates;
        private List<Point> nonBorderCoordinates;
        private Random rand = new Random();
        private double dt;
        private double tMax;
        private double A;
        private double B;
        private double pCritical = 46842668.25;
        //private const double totalCriticalValue = 46842668.25 * 100 * 75;//4215840142323.42;
        private const double borderPropability = 0.8;
        private const double equalDistributionpercentage = 0.3;
        private const double randomPackagePercentage = 0.01;
        public DynamicRecrystalizationEngine(DRXRequest request)
        {
            TotalDensityList = new List<DensitySnapshot>();
            Initialize(request);
        }

        public void Initialize(DRXRequest r)
        {
            if (r.dt <= 0 || r.tMax <=0)
                throw new ArgumentException("time cannot be less or equal 0");
            dt = r.dt;
            tMax = r.tMax;
            A = r.A;
            B = r.B;
        }

        public Board IterateAll(Board board)
        {
            //pCritical = 46842668.25;//totalCriticalValue / (board.X * board.Y);
            for (double t = dt; t <= tMax; t += dt)
            {
                board = NextIteration(board, t);
            }
            return board;
        }

        public Board NextIteration(Board board, double t)
        {
            double ro = CalculateTotalro(t);
            double previousRo;
            if (TotalDensityList.Any() == false)
                previousRo = 0;
            else
                previousRo = TotalDensityList.Last().TotalDensity;
            double dRo = ro - previousRo;
            board = DistributeValues(board, dRo);
            TotalDensityList.Add(new DensitySnapshot { T = t, TotalDensity = ro });
            return board;
        }


        private Board DistributeValues(Board board, double dRo)
        {
            double roPerCell = dRo / (double)(board.X * board.Y);
            double equalDistribution = roPerCell * equalDistributionpercentage;
            double randomDistribution = roPerCell * randomPackagePercentage;
            borderCoordinates = board.GetBorderGrainsCoordinates();
            int bcount = borderCoordinates.Count();
            nonBorderCoordinates = board.GetNonBorderGrainsCoordinates();
            int nbcount = nonBorderCoordinates.Count();



            //DistributeEqualy
            for (int i = 0; i < board.Y; i++)
            {
                for (int j = 0; j < board.X; j++)
                {
                    (board.board[i][j] as Grain).DyslocationDensity += equalDistribution;
                }
            }
            dRo *= 1.0 - equalDistributionpercentage;

            //Distribute randomly
            while (dRo > randomDistribution)
            {
                double p = rand.NextDouble();
                if (p <= borderPropability)
                {
                    var point = borderCoordinates[rand.Next(0, bcount)];
                    var el = (board.board[point.X][point.Y] as Grain);
                    el.DyslocationDensity += randomDistribution;
                    if (el.DyslocationDensity > pCritical)
                    {
                        el.DyslocationDensity = 0;
                        el.IsRecrystallized = true;
                        board.SetNewRecrystalizedNumber(el.X(), el.Y());
                    }
                }
                else
                {
                    var el = nonBorderCoordinates[rand.Next(0, nbcount)];
                    (board.board[el.X][el.Y] as Grain).DyslocationDensity += randomDistribution;
                }
                dRo -= randomDistribution;
            }
            return board;
        }

        private double CalculateTotalro(double t)
        {
            double ro = (A / B) + (1 - A / B) * Math.Exp(-B * t);
            return ro;
        }

        public string GetSaveText()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < TotalDensityList.Count(); i++)
            {
                sb.Append(TotalDensityList[i].T);
                sb.Append(";");
                sb.Append(TotalDensityList[i].TotalDensity);
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
