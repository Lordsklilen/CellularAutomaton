﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineProject.DataStructures;
using EngineProject.Engines.DRX;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition = true;
        private readonly bool MCIterateAllCells = true;
        private NeighbourFactory neighbourFactory;
        private MonteCarloEngine MCEngine;
        private readonly CellType cellType;
        private NeighbooorhoodType neighboursType;
        private INeighbourStrategy neighbourStrategy;
        private HexType hexType;
        private IDynamicRecrystalizationEngine DRXEngine;

        public Board GetBoard() => panel;
        public bool IsFinished() => panel.finished;
        public GrainGrowthEngine(int width, int height, NeighbooorhoodType nType = NeighbooorhoodType.VonNeumann)
        {
            type = EngineType.GrainGrowth;
            cellType = CellType.Grain;
            panel = new Board(width, height, cellType);
            _maxRow = height;
            _maxColumn = width;
            neighboursType = nType;
            neighbourFactory = new NeighbourFactory();
            NeighbourStrategyRequest request = new NeighbourStrategyRequest() { neighbooorhoodType = nType };
            neighbourStrategy = neighbourFactory.CreateNeighbourComputing(request);
        }

        public void NextIteration()
        {
            if (panel.finished)
                return;
            var copyPanel = new Board(panel);
            copyPanel.finished = true;
            neighbourStrategy.Initialize(panel, copyPanel, _maxRow, _maxColumn, OpenBorderCondition);
            Parallel.For(0, panel.Y, i =>
            {
                var row = panel.board[i];
                foreach (var cell in row)
                {
                    neighbourStrategy.ComputeCell((Grain)cell);
                }
            });
            panel = MCEngine.ReCalculateAllEnergy(copyPanel);
        }

        public void ChangeCellState(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SetCellState(int x, int y, bool state)
        {
            throw new NotImplementedException();
        }

        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }

        public void SetGrainNumber(int number, int x, int y)
        {
            panel.SetGrainNumber(number, x, y);
            if (MCEngine != null)
                RecalculateEnergy();

        }

        public void ChangeStrategyType(NeighbourStrategyRequest request)
        {
            OpenBorderCondition = request.border;
            neighboursType = request.neighbooorhoodType;
            this.hexType = request.hexType;
            neighbourStrategy = neighbourFactory.CreateNeighbourComputing(request);
            if (MCEngine != null)
            {
                MCEngine.ChangeStrategy(request);
                RecalculateEnergy();
            }
        }

        public void ChangeBorderConditions(bool state)
        {
            OpenBorderCondition = state;
        }

        public void ChangeHexType(HexType type)
        {
            hexType = type;
            if ((neighbourStrategy as NeighbourHexagonal) != null)
                (neighbourStrategy as NeighbourHexagonal).type = type;
        }

        internal void CreateMCEngine(MonteCarloRequest request)
        {
            OpenBorderCondition = request.border;
            request.board = panel;
            request.CopyBoard = new Board(panel);
            request.maxColumn = _maxColumn;
            request.maxRow = _maxRow;
            if (MCEngine == null)
                MCEngine = new MonteCarloEngine(request, this.neighbourStrategy);
            else
                MCEngine.Reinstate(request, this.neighbourStrategy);
            RecalculateEnergy();
        }

        internal Board RecalculateEnergy()
        {
            if (MCEngine != null)
                panel = MCEngine.ReCalculateAllEnergy(panel);
            return panel;
        }

        internal void IterateMonteCarlo(int iterations)
        {
            if (MCIterateAllCells)
                MCEngine.NextIterationsEveryCell(panel, iterations);
            else
                MCEngine.NextIterations(panel, iterations);
            RecalculateEnergy();
        }

        public Board CalculateDRX(DRXRequest request)
        {
            if (DRXEngine == null)
                DRXEngine = new DynamicRecrystalizationEngine(request, neighbourStrategy);
            else
                DRXEngine.Initialize(request, neighbourStrategy);
            panel = DRXEngine.IterateAll(panel);
            return panel;
        }

        public Board InitializeDRX(DRXRequest request)
        {
            if (DRXEngine == null)
                DRXEngine = new DynamicRecrystalizationEngine(request, neighbourStrategy);
            else
                DRXEngine.Initialize(request, neighbourStrategy);
            return panel;
        }

        public Board NextDRXIteration(decimal t)
        {
            panel = DRXEngine.NextIteration(panel, t);
            return panel;
        }

        public string GetSaveText()
        {
            if (DRXEngine != null)
                return DRXEngine.GetSaveText();
            else
                return "";
        }
    }
}
