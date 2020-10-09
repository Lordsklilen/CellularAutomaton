using EngineProject.DataStructures;


namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourFactory
    {
        public INeighbourStrategy CreateNeighbourComputing(NeighbourStrategyRequest request)
        {
            switch (request.neighbooorhoodType)
            {
                case NeighborhoodType.VonNeumann:
                    return new NeighbourVonNeumann();
                case NeighborhoodType.Moore:
                    return new NeighbourMoore();
                case NeighborhoodType.Pentagonal:
                    return new NeighbourPentagonal();
                case NeighborhoodType.Hexagonal:
                    var result = new NeighbourHexagonal
                    {
                        type = request.hexType
                    };
                    return result;
                case NeighborhoodType.Radius:
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
