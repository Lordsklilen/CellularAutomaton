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
        public ICell CreateCell(CellType type, int x, int y) {
            switch (type)
            {
                case CellType.Cell:
                    return new Cell(x,y);
                case CellType.Grain:
                    return new Grain(x, y);
                default:
                    throw new NotSupportedException("This type of cell is not supproted");
            }
        }
    }
}
