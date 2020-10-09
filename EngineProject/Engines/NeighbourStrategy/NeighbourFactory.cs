using EngineProject.DataStructures;


namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourFactory
    {
        public INeighbourStrategy CreateNeighbourComputing(NeighbourStrategyRequest request)
        {
            switch (request.neighbooorhoodType)
            {
                case NeighbooorhoodType.VonNeumann:
                    return new NeighbourVonNeumann();
                case NeighbooorhoodType.Moore:
                    return new NeighbourMoore();
                case NeighbooorhoodType.Pentagonal:
                    return new NeighbourPentagonal();
                case NeighbooorhoodType.Hexagonal:
                    var result = new NeighbourHexagonal
                    {
                        type = request.hexType
                    };
                    return result;
                case NeighbooorhoodType.Radius:
                    var radius = new NeighbourRadius
                    {
                        radius = request.Radius
                    };
                    return radius;
                default:
                    throw new System.Exception("This type of neighboorhood is not recognized");
            }
        }
    }
}
