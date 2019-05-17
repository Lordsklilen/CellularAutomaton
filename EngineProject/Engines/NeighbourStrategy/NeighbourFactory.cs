using EngineProject.DataStructures;


namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourFactory
    {
        public INeighbourStrategy CreateNeighbourComputing(NeighbooorhoodType type, HexType hexType = HexType.Left)
        {
            switch (type)
            {
                case NeighbooorhoodType.VonNeumann:
                    return new NeighbourVonNeumann();
                case NeighbooorhoodType.Moore:
                    return new NeighbourMoore();
                case NeighbooorhoodType.Pentagonal:
                    return new NeighbourPentagonal();
                case NeighbooorhoodType.Hexagonal:
                    var result = new NeighbourHexagonal();
                    result.type = hexType;
                    return result;
                case NeighbooorhoodType.Radius:
                default:
                    throw new System.Exception("This type of neighboorhood is not recognized");
            }
        }
    }
}
