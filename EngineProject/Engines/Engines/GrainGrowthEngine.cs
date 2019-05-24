using System;
using System.Collections.Generic;
using System.Linq;
using EngineProject.DataStructures;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private CellType cellType;
        private NeighbooorhoodType neighboursType;
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition = true;
        private NeighbourFactory neighbourFactory;
        private INeighbourStrategy neighbourStrategy;
        private HexType hexType;
        private MonteCarloEngine MCEngine;

        public Board GetBoard() => panel;
        public bool IsFinished() => panel.IsFinished();
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
            if (panel.MaxNumber() == 1)
                return;
            var copyPanel = new Board(_maxColumn, _maxRow, cellType);
            copyPanel.finished = true;
            neighbourStrategy.Initialize(panel, copyPanel, _maxRow, _maxColumn, OpenBorderCondition);
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    neighbourStrategy.ComputeCell((Grain)cell);
                }
            }
            panel = copyPanel;
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
        }
        public void ChangeStrategyType(NeighbourStrategyRequest request)
        {
            neighboursType = request.neighbooorhoodType;
            this.hexType = request.hexType;
            neighbourStrategy = neighbourFactory.CreateNeighbourComputing(request);
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
            MCEngine = new MonteCarloEngine(request);
        }
        internal Board RecalculateEnergy() {
            panel = MCEngine.ReCalculateAllEnergy(panel);
            return panel;
        }

    }
}
