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
            if (percentage < 0.1)
                return InterpolateColor(Color.Orange, Color.Red, percentage * 10.0);
            else if (percentage < 0.2)
                return InterpolateColor(Color.Red, Color.Yellow, (percentage - 0.1) * 10.0);
            else if (percentage < 0.3)
                return InterpolateColor(Color.Yellow, Color.Green, (percentage - 0.2) * 10.0);
            else if (percentage < 0.4)
                return InterpolateColor(Color.Green, Color.Blue, (percentage - 0.3) * 10.0);
            else if (percentage < 0.5)
                return InterpolateColor(Color.Blue, Color.Teal, (percentage - 0.4) * 10.0);
            else if (percentage < 0.6)
                return InterpolateColor(Color.Blue, Color.Purple, (percentage - 0.5) * 10.0);
            else if (percentage < 0.7)
                return InterpolateColor(Color.Purple, Color.Gray, (percentage - 0.6) * 10.0);
            else if (percentage < 0.8)
                return InterpolateColor(Color.Gray, Color.Brown, (percentage - 0.7) * 10.0);
            else if (percentage < 0.9)
                return InterpolateColor(Color.Brown, Color.Black, (percentage - 0.8) * 10.0);
            else
                return InterpolateColor(Color.Black, Color.Pink, (percentage - 0.9) * 10.0);
        }

    }
}
