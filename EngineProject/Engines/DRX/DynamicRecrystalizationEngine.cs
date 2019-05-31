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

        private List<Point> borderPoints;
        private List<Point> NonBorderPoints;
        private Random rand = new Random();
        private double dt;
        private double tMax;
        private double A;
        private double B;
        private const double totalCriticalValue = 4215840142323.42;
        private const double borderPropability = 0.8;
        private const double equalDistributionpercentage = 0.3;
        private const double randomPackagePercentage = 0.001;

        public DynamicRecrystalizationEngine(DRXRequest request)
        {
            Initialize(request);
        }

        public void Initialize(DRXRequest r)
        {
            dt = r.dt;
            tMax = r.tMax;
            A = r.A;
            B = r.B;
            TotalDensityList = new List<DensitySnapshot>();
        }

        public Board Iterate(Board board)
        {
            for (double t = dt; t <= tMax; t += dt)
            {
                double ro = CalculateTotalro(t);
                double previousRo;
                if (TotalDensityList.Any() == false)
                    previousRo = 0;
                else
                    previousRo = TotalDensityList.Last().TotalDensity;
                double dRo = ro - previousRo;
                board = DistributeValues(board,dRo);
                TotalDensityList.Add(new DensitySnapshot {T=t,TotalDensity= ro });
            }
            return board;
        }

        private Board DistributeValues(Board board,double dRo) {
            double roPerCell = dRo / (double)(board.X * board.Y);
            //DistributeEqualy
            double equalDistribution = roPerCell * equalDistributionpercentage;
            for (int i = 0; i < board.Y; i++) {
                for (int j = 0; j < board.X; j++) {
                    (board.board[i][j] as Grain).DyslocationDensity += equalDistribution;
                }
            }
            dRo *= 1.0 - equalDistributionpercentage;

            //Distribute randomly
            double randomDistribution = roPerCell * randomPackagePercentage;
            List<Point> borderCoordinates = board.GetBorderGrainsCoordinates();
            int bcount = borderCoordinates.Count();
            List<Point> nonBorderCoordinates = board.GetNonBorderGrainsCoordinates();
            int nbcount = nonBorderCoordinates.Count();
            while (dRo > randomDistribution)
            {
                double p = rand.NextDouble();
                if (p <= borderPropability)
                {
                    var el = borderCoordinates[rand.Next(0, bcount)];
                    (board.board[el.X][el.Y] as Grain).DyslocationDensity += randomDistribution;
                }
                else {
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

    }
}
