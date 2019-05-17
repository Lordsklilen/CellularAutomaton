using EngineProject.DataStructures;


namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourFactory
    {
        public INeighbourStrategy CreateNeighbourComputing(NeighbooorhoodType type) {
            switch (type)
            {
                case NeighbooorhoodType.VonNeumann:
                    return new NeighbourVonNeumann();
                case NeighbooorhoodType.Moore:
                    return new NeighbourMoore();
                case NeighbooorhoodType.Pentagonal:
                case NeighbooorhoodType.Hexagonal:
                default:
                    throw new System.Exception("This type of neighboorhood is not recognized");
            }
        }
    }
}
