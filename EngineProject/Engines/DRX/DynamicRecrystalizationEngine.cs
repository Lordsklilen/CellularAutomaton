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

        public double CurrentTotalDensity { get; }
        public double PreviousTotalDensity { get; }

        private double currentTime { get; }
        private double borderPropability = 0.8;
        private List<Point> borderPoints;
        private List<Point> NonBorderPoints;
        private double dt;
        private double tMax;
        private double A;
        private double B;

        public DynamicRecrystalizationEngine(DRXRequest request) {
            Initialize(request);
        }

        public void Initialize(DRXRequest r) {
            dt = r.dt;
            tMax = r.tMax;
            A = r.A;
            B = r.B;
        }

        public Board Iterate(Board board) {
            return board;
        }

    }
}
