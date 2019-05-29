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

        public Brush CreateColorBrush(int number, int max)
        {
            if(number ==0)
                return new SolidBrush(Color.White);
            return new SolidBrush(colorTool.GetColor(number,max,0));
        }

        public Brush CreateEnergyBrush(int energy)
        {
            if (energy == 0)
                return new SolidBrush(Color.White);
            else if (energy == 1)
                return new SolidBrush(Color.Yellow);
            else if (energy == 2)
                return new SolidBrush(Color.Orange);
            else if (energy == 3)
                return new SolidBrush(Color.Red);
            else if (energy == 4)
                return new SolidBrush(Color.Violet);
            else if (energy == 5)
                return new SolidBrush(Color.BlueViolet);
            else if (energy == 6)
                return new SolidBrush(Color.Blue);
            else
                return new SolidBrush(Color.Black);
            throw new NotSupportedException("This type of binary brush is not supprted");
        }

        public Brush CreateEnergyBrushSimple(int energy)
        {
            if (energy == 0)
                return new SolidBrush(Color.White);
            else
                return new SolidBrush(Color.Lime);
            throw new NotSupportedException("This type of binary brush is not supprted");
        }
        public Brush CreateCenterOfMassBrush() {
            return new SolidBrush(Color.Red);
        }

    }
}
