﻿
using EngineProject.DataStructures;
using EngineProject.Engines.NeighbourStrategy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineProject.Engines.MonteCarlo
{
    internal class MonteCarloEngine
    {
        private int iterations;
        private double Kt;
        private NeighbourStrategyRequest strategyRequest;
        private NeighbourFactory factory;
        private INeighbourStrategy neighbourStrategy;
        private bool border;
        private int maxColumn;
        private int maxRow;
        private Board board;
        private Board copyBorad;
        Random rand = new Random();
        internal MonteCarloEngine(MonteCarloRequest request, INeighbourStrategy strategy)
        {
            factory = new NeighbourFactory();
            Reinstate(request,strategy);
        }

        //internal ChangeStrategy(INeighbourStrategy strategy) {
        //    neighbourStrategy = strategy;
        //}

        internal void Reinstate(MonteCarloRequest r, INeighbourStrategy strategy)
        {
            neighbourStrategy = strategy;
            iterations = r.numberOfIterations;
            Kt = r.Kt;
            strategyRequest = r.strategyRequest;
            maxColumn = r.maxColumn;
            maxRow = r.maxRow;
            board = r.board;
            copyBorad = r.CopyBoard;
            border = r.border;
            neighbourStrategy = factory.CreateNeighbourComputing(strategyRequest);
            neighbourStrategy.Initialize(r.board, r.CopyBoard, r.maxRow, r.maxColumn, r.border);
        }

        internal void ChangeStrategy(NeighbourStrategyRequest r) {
            neighbourStrategy = factory.CreateNeighbourComputing(r);
            neighbourStrategy.Initialize(board, null, maxRow, maxColumn, r.border);
        }

        private int CalculateEnergy(Board panel, int x, int y)
        {
            var grain = panel.board[x][y] as Grain;
            List<int> nieghbours = neighbourStrategy.NeighboursGrainNumbers(grain);
            return nieghbours.Count(member => member != grain.grainNumber);
        }

        internal Board ReCalculateAllEnergy(Board panel)
        {
            neighbourStrategy.Initialize(panel, null, maxRow, maxColumn, border);
            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    (panel.board[i][j] as Grain).E = CalculateEnergy(panel, i, j);
                }
            }
            return panel;
        }

        internal Board NextIterationsEveryCell(Board board, int iterations)
        {
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < board.Y; i++)
                {
                    for (int j = 0; j < board.X; j++)
                    {
                        board = NextIteration(board, i, j);
                    }
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
            var grain = board.board[x][y] as Grain;
            List<int> nieghbours = neighbourStrategy.NeighboursGrainNumbers(grain);
            if (nieghbours.Count <= 0)
                return board;

            int EBefore = nieghbours.Count(member => member != grain.grainNumber);
            int newGrainNumber = nieghbours[rand.Next(0, nieghbours.Count)];
            int EAfter = nieghbours.Count(member => member != newGrainNumber);
            int deltaE = EAfter - EBefore;
            double p = rand.NextDouble();
            double exponenta = Math.Exp(-((deltaE) / (Kt)));

            if (deltaE <= 0)
            {
                (board.board[x][y] as Grain).SetGrainNumber(newGrainNumber);
            }
            else if (p < Math.Exp(-((deltaE) / (Kt))))
            {
                (board.board[x][y] as Grain).SetGrainNumber(newGrainNumber);
            }
            return board;
        }





    }
}