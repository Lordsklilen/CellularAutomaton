using EngineProject.DataStructures;
using EngineProject.Engines.DRX;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine
    {
        public Board Panel { get; private set; }
        private readonly int maxRow;
        private readonly int maxColumn;
        private bool OpenBorderCondition = true;
        private readonly bool MCIterateAllCells = true;
        private readonly NeighbourFactory neighbourFactory;
        private MonteCarloEngine MCEngine
        {
            get;
            set;
        }
        private readonly CellType cellType;
        private INeighbourStrategy neighbourStrategy;
        private IDynamicRecrystalizationEngine DRXEngine;

        public bool IsFinished => Panel.finished || Panel.MaxNumber() == 0;
        public GrainGrowthEngine(int width, int height, NeighborhoodType nType = NeighborhoodType.VonNeumann)
        {
            cellType = CellType.Grain;
            Panel = new Board(width, height, cellType);
            maxRow = height;
            maxColumn = width;
            neighbourFactory = new NeighbourFactory();
            NeighbourStrategyRequest request = new NeighbourStrategyRequest() { neighbooorhoodType = nType };
            neighbourStrategy = neighbourFactory.CreateNeighbourComputing(request);
        }

        public Board NextIteration()
        {
            if (Panel.finished)
                return Panel;
            var copyPanel = new Board(Panel)
            {
                finished = true
            };
            neighbourStrategy.Initialize(Panel, copyPanel, maxRow, maxColumn, OpenBorderCondition);
            for (int i = 0; i < Panel.Y; i++)
            {
                var row = Panel.BoardContainer[i];
                foreach (var cell in row)
                {
                    neighbourStrategy.ComputeCell((Grain)cell);
                }
            }
            Panel = MCEngine.ReCalculateAllEnergy(copyPanel);
            return Panel;
        }

        public void SetGrainNumber(int number, int x, int y)
        {
            Panel.SetGrainNumber(number, x, y);
            if (MCEngine != null)
                RecalculateEnergy();
        }

        public void ChangeStrategyType(NeighbourStrategyRequest request)
        {
            OpenBorderCondition = request.border;
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
            if ((neighbourStrategy as NeighbourHexagonal) != null)
                (neighbourStrategy as NeighbourHexagonal).type = type;
        }

        public void CreateMCEngine(MonteCarloRequest request)
        {
            OpenBorderCondition = request.border;
            request.board = Panel;
            request.CopyBoard = new Board(Panel);
            request.maxColumn = maxColumn;
            request.maxRow = maxRow;
            if (MCEngine == null)
                MCEngine = new MonteCarloEngine(request, neighbourStrategy);
            else
                MCEngine.Reinstate(request, neighbourStrategy);
            RecalculateEnergy();
        }

        public Board RecalculateEnergy()
        {
            if (MCEngine != null)
                Panel = MCEngine.ReCalculateAllEnergy(Panel);
            return Panel;
        }

        public void IterateMonteCarlo(int iterations)
        {
            if (MCIterateAllCells)
                MCEngine.NextIterationsEveryCell(Panel, iterations);
            else
                MCEngine.NextIterations(Panel, iterations);
            RecalculateEnergy();
        }

        public Board CalculateDRX(DRXRequest request)
        {
            if (DRXEngine == null)
                DRXEngine = new DynamicRecrystalizationEngine(request, neighbourStrategy);
            else
                DRXEngine.Initialize(request, neighbourStrategy);
            Panel = DRXEngine.IterateAll(Panel);

            return Panel;
        }

        public Board InitializeDRX(DRXRequest request)
        {
            if (DRXEngine == null)
                DRXEngine = new DynamicRecrystalizationEngine(request, neighbourStrategy);
            else
                DRXEngine.Initialize(request, neighbourStrategy);
            return Panel;
        }

        public Board NextDRXIteration(decimal t)
        {
            Panel = DRXEngine.NextIteration(Panel, t);
            if (DRXEngine.IsChaged())
                return RecalculateEnergy();
            return Panel;
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
