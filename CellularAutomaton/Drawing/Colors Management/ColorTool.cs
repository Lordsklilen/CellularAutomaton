using System.Drawing;

namespace CellularAutomaton.Drawing
{
    public class ColorTool
    {
        private Color InterpolateColor(Color a, Color b, double t)
        {
            return Color.FromArgb
            (
                a.A + (int)((b.A - a.A) * t),
                a.R + (int)((b.R - a.R) * t),
                a.G + (int)((b.G - a.G) * t),
                a.B + (int)((b.B - a.B) * t)
            );
        }

        public Color GetColor(double elNumber, double max, double min)
        {
            var percentage = (elNumber - min) / (max - min);
            if (percentage < 0.25)
                return InterpolateColor(Color.Purple, Color.Blue, percentage * 4.0);
            else if (percentage < 0.5)
                return InterpolateColor(Color.Blue, Color.Yellow, (percentage - 0.25) * 4.0);
            else if (percentage < 0.75)
                return InterpolateColor(Color.Yellow, Color.Orange, (percentage - 0.5) * 4.0);
            else
                return InterpolateColor(Color.Orange, Color.Red, (percentage - 0.75) * 4.0);
        }

    }
}
