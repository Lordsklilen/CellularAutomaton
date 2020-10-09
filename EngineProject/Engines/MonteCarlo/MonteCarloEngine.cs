
using EngineProject.DataStructures;
using EngineProject.Engines.NeighbourStrategy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EngineProject.Engines.MonteCarlo
{
    internal class MonteCarloEngine
    {
        private double Kt;
        private NeighbourStrategyRequest strategyRequest;
        private readonly NeighbourFactory factory;
        private INeighbourStrategy neighbourStrategy;
        private bool border;
        private int maxColumn;
        private int maxRow;
        private Board board;
        readonly Random rand = new Random();
        internal MonteCarloEngine(MonteCarloRequest request, INeighbourStrategy strategy)
        {
            factory = new NeighbourFactory();
            Reinstate(request, strategy);
        }

        internal void Reinstate(MonteCarloRequest r, INeighbourStrategy strategy)
        {
            neighbourStrategy = strategy;
            Kt = r.Kt;
            strategyRequest = r.strategyRequest;
            maxColumn = r.maxColumn;
            maxRow = r.maxRow;
            board = r.board;
            border = r.border;
            neighbourStrategy = factory.CreateNeighbourComputing(strategyRequest);
            neighbourStrategy.Initialize(r.board, r.CopyBoard, r.maxRow, r.maxColumn, r.border);
        }

        internal void ChangeStrategy(NeighbourStrategyRequest r)
        {
            neighbourStrategy = factory.CreateNeighbourComputing(r);
            neighbourStrategy.Initialize(board, null, maxRow, maxColumn, r.border);
        }

        private int CalculateEnergy(Board panel, int x, int y)
        {
            var grain = panel.BoardContainer[x][y] as Grain;
            List<Grain> grains = neighbourStrategy.NeighboursGrainCells(grain);
            return neighbourStrategy.GetRecrystalizedAndGrainGrains(grains, grain.RecrystalizedNumber, grain.GetGrainNumber());
        }


        internal Board ReCalculateAllEnergy(Board panel)
        {
            neighbourStrategy.Initialize(panel, null, maxRow, maxColumn, border);
            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    (panel.BoardContainer[i][j] as Grain).E = CalculateEnergy(panel, i, j);
                }
            }
            return panel;
        }

        internal Board NextIterationsEveryCell(Board board, int iterations)
        {
            for (int iter = 0; iter < iterations; iter++)
            {
                var pairs = GeneratePairs();
                pairs = Shuffle(pairs);
                for (int i = 0; i < pairs.Count(); i++)
                {
                    board = NextIteration(board, pairs[i].X, pairs[i].Y);
                }
            }
            ReCalculateAllEnergy(board);
            return board;
        }
        internal Board NextIterations(Board board, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                int x = rand.Next(0, maxRow);
                int y = rand.Next(0, maxColumn);
                board = NextIteration(board, x, y);
            }
            ReCalculateAllEnergy(board);
            return board;
        }

        internal Board NextIteration(Board board, int x, int y)
        {
            var grain = board.BoardContainer[x][y] as Grain;
            List<int> nieghbours = neighbourStrategy.NeighboursGrainNumbers(grain);
            if (nieghbours.Count <= 0)
                return board;

            int EBefore = nieghbours.Count(member => member != grain.GrainNumber);
            int newGrainNumber = nieghbours[rand.Next(0, nieghbours.Count)];
            int EAfter = nieghbours.Count(member => member != newGrainNumber);
            int deltaE = EAfter - EBefore;
            double p = rand.NextDouble();
            double exponenta = Math.Exp(-((deltaE) / (Kt)));

            if (deltaE <= 0)
            {
                (board.BoardContainer[x][y] as Grain).SetGrainNumber(newGrainNumber);
            }
            else if (p < Math.Exp(-((deltaE) / (Kt))))
            {
                (board.BoardContainer[x][y] as Grain).SetGrainNumber(newGrainNumber);
            }
            return board;
        }

        internal List<Point> GeneratePairs()
        {
            List<Point> result = new List<Point>();
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxColumn; j++)
                {
                    result.Add(new Point(i, j));
                }
            }
            return result;
        }

        public List<Point> Shuffle(List<Point> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Point value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
