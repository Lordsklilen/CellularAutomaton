using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Drawing
{
    public class BrushFactory
    {
        ColorTool colorTool = new ColorTool();

        public Brush CreateBinaryBrush(bool state)
        {
            switch (state)
            {
                case true:
                    return new SolidBrush(Color.Black);
                case false:
                    return new SolidBrush(Color.White);
                default:
                    throw new NotSupportedException("This type of binary brush is not supprted");
            }
        }

        public Brush CreateColorBrush(int number, int max, int min)
        {
            return new SolidBrush(colorTool.GetColor(number,max,min));
        }
    }
}
