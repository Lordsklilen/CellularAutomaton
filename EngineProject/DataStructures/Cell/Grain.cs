using System.Drawing;

namespace EngineProject.DataStructures
{
    public class Grain : Cell
    {
        internal int GrainNumber { get; private set; }
        public double CenterOfX { get; set; }
        public double CenterOfY { get; set; }
        public bool IsRecrystallized { get; set; }
        public int RecrystalizedNumber { get; set; }
        public decimal DyslocationDensity { get; set; }

        public int E { get; set; } = 0;

        public Grain(int x, int y) : base(x, y)
        {
            RecrystalizedNumber = 0;
            DyslocationDensity = 0;
            IsRecrystallized = false;
            GrainNumber = 0;
            type = CellType.Grain;
        }

        public int GetGrainNumber() => GrainNumber;
        public void SetGrainNumber(int number) => GrainNumber = number;
        public Point GetMassCenter() => new Point((int)CenterOfY + y, (int)CenterOfX + x);
        public Point GetInsideMassCenter() => new Point((int)CenterOfY, (int)CenterOfX);

    }
}
