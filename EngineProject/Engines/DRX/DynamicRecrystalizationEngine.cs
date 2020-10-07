using EngineProject.DataStructures;
using EngineProject.Engines.NeighbourStrategy;
using System;
using System.Collections.Concurrent;
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
        private ConcurrentBag<Grain> PreviousChanges;
        private List<Point> nonBorderCoordinates;
        private ConcurrentBag<Grain> Changes;
        private readonly Random rand = new Random();
        private decimal dt;
        private decimal tMax;
        private decimal A;
        private decimal B;
        private readonly decimal pCritical = 46842668.25m;
        //private const double totalCriticalValue = 46842668.25 * 100 * 75;//4215840142323.42;
        private const double borderPropability = 0.8;
        private const decimal equalDistributionpercentage = 0.3m;
        private const decimal randomPackagePercentage = 0.05m;
        private INeighbourStrategy strategy;


        public bool IsChaged() => !Changes.IsEmpty;
        public DynamicRecrystalizationEngine(DRXRequest request, INeighbourStrategy strategy)
        {
            PreviousChanges = new ConcurrentBag<Grain>();
            TotalDensityList = new List<DensitySnapshot>();
            Initialize(request, strategy);
        }

        public void Initialize(DRXRequest r, INeighbourStrategy strategy)
        {
            if (r.dt <= 0 || r.tMax <= 0)
                throw new ArgumentException("time cannot be less or equal 0");

            this.strategy = strategy;
            dt = r.dt;
            tMax = r.tMax;
            A = r.A;
            B = r.B;
        }

        public Board IterateAll(Board board)
        {
            for (decimal t = dt; t <= tMax; t += dt)
            {
                board = NextIteration(board, t);
            }
            return board;
        }

        public Board NextIteration(Board board, decimal t)
        {
            Changes = new ConcurrentBag<Grain>();
            decimal dRo = CalculateRo(t);
            board = DistributeValues(board, dRo);
            board = Recrystalize(board);
            board = Growth(board);
            return board;
        }

        private decimal CalculateRo(decimal t)
        {
            decimal ro = CalculateTotalro(t);
            decimal previousRo;
            if (TotalDensityList.Any() == false)
                previousRo = 0;
            else
                previousRo = TotalDensityList.Last().TotalDensity;
            TotalDensityList.Add(new DensitySnapshot { T = t, TotalDensity = ro });
            return ro - previousRo;
        }

        private Board Recrystalize(Board board)
        {
            Parallel.For(0, board.Y, i =>
            {
                for (int j = 0; j < board.X; j++)
                {
                    var el = (board.board[i][j] as Grain);
                    if (el.DyslocationDensity > pCritical)
                    {
                        el.DyslocationDensity = 0;
                        el.IsRecrystallized = true;
                        board.SetNewRecrystalizedNumber(el.X(), el.Y());
                        Changes.Add(el);
                    }
                }
            });
            return board;
        }

        private Board Growth(Board board)
        {
            if (PreviousChanges.Any())
            {
                strategy.Initialize(board, null, board.Y, board.X, false);
                for (int i = 0; i < board.Y; i++)
                {
                    for (int j = 0; j < board.X; j++)
                    {
                        var el = (board.board[i][j] as Grain);
                        var neighbours = strategy.NeighboursGrainCells(el);
                        var recrystalizedGrain = RecrystalizedGrain(neighbours);
                        if (recrystalizedGrain != null && MaxNeighboursValue(neighbours) < el.DyslocationDensity)
                        {
                            el.DyslocationDensity = 0;
                            el.IsRecrystallized = true;
                            (board.board[el.X()][el.Y()] as Grain).RecrystalizedNumber = recrystalizedGrain.RecrystalizedNumber;
                            Changes.Add(el);
                        }
                    }
                }
            }
            PreviousChanges = Changes;
            return board;
        }

        private Board DistributeValues(Board board, decimal dRo)
        {
            decimal roPerCell = dRo / (decimal)(board.X * board.Y);
            decimal equalDistribution = roPerCell * equalDistributionpercentage;
            decimal randomDistribution = roPerCell * randomPackagePercentage;
            borderCoordinates = board.GetBorderGrainsCoordinates();
            int bcount = borderCoordinates.Count();
            nonBorderCoordinates = board.GetNonBorderGrainsCoordinates();
            int nbcount = nonBorderCoordinates.Count();

            //Distribute equaly
            Parallel.For(0, board.Y, i =>
            {
                for (int j = 0; j < board.X; j++)
                {
                    (board.board[i][j] as Grain).DyslocationDensity += equalDistribution;
                }
            });

            dRo *= 1.0m - equalDistributionpercentage;

            //Distribute randomly
            int iterationCount = (int)(dRo / randomDistribution);
            for (int i = 0; i < iterationCount; i++)
            {
                double p = rand.NextDouble();
                if (p <= borderPropability && bcount > 0)
                {
                    var point = borderCoordinates[rand.Next(0, bcount)];
                    var el = (board.board[point.X][point.Y] as Grain);
                    el.DyslocationDensity += randomDistribution;
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

        private decimal MaxNeighboursValue(List<Grain> grains)
        {
            return grains.Max(x => x.DyslocationDensity);
        }

        private Grain RecrystalizedGrain(List<Grain> grains)
        {
            return grains.FirstOrDefault(x => PreviousChanges.Any(y => x.x == y.x && x.y == y.y));
        }

        private decimal CalculateTotalro(decimal t)
        {
            decimal ro = (A / B) + (1 - A / B) * (decimal)Math.Exp((double)(-B * t));
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
