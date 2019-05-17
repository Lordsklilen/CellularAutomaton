using EngineProject.DataStructures;


namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourFactory
    {
        INeighbourStrategy CreateNeighbourComputing(NeighbooorhoodType type) {
            switch (type)
            {
                case NeighbooorhoodType.VonNeumann:
                    break;
                case NeighbooorhoodType.Moore:
                    break;
                case NeighbooorhoodType.Pentagonal:
                    break;
                case NeighbooorhoodType.Hexagonal:
                    break;
                default:
                    throw new System.Exception("This type of neighboorhood is not recognized");
            }
            return null;
        }
    }
}
