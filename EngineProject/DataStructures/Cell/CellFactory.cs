using EngineProject.DataStructures.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class CellFactory: ICellFactory
    {
        Random rand = new Random();
        public ICell CreateCell(CellType type, int x, int y) {
            switch (type)
            {
                case CellType.Cell:
                    return new Cell(x,y);
                case CellType.Grain:
                    var grain = new Grain(x, y);
                    grain.CenterOfX = rand.NextDouble();
                    grain.CenterOfY = rand.NextDouble();
                    return grain;
                default:
                    throw new NotSupportedException("This type of cell is not supproted");
            }
        }
    }
}
