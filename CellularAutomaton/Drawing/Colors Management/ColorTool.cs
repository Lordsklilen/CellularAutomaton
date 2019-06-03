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
                return InterpolateColor(Color.SteelBlue, Color.Violet, percentage * 10.0);
            else if (percentage < 0.2)
                return InterpolateColor(Color.Violet, Color.Magenta, (percentage - 0.1) * 10.0);
            else if (percentage < 0.3)
                return InterpolateColor(Color.Magenta, Color.Green, (percentage - 0.2) * 10.0);
            else if (percentage < 0.4)
                return InterpolateColor(Color.Green, Color.Blue, (percentage - 0.3) * 10.0);
            else if (percentage < 0.5)
                return InterpolateColor(Color.Blue, Color.Lime, (percentage - 0.4) * 10.0);
            else if (percentage < 0.6)
                return InterpolateColor(Color.Lime, Color.Purple, (percentage - 0.5) * 10.0);
            else if (percentage < 0.7)
                return InterpolateColor(Color.Purple, Color.Gray, (percentage - 0.6) * 10.0);
            else if (percentage < 0.8)
                return InterpolateColor(Color.Gray, Color.Teal, (percentage - 0.7) * 10.0);
            else if (percentage < 0.9)
                return InterpolateColor(Color.Teal, Color.Ivory, (percentage - 0.8) * 10.0);
            else
                return InterpolateColor(Color.Ivory, Color.Olive, (percentage - 0.9) * 10.0);
        }

        public Color GetDensityColors(decimal elNumber, decimal max, decimal min)
        {
            var percentage = (elNumber - min) / (max - min);
            if (percentage < 0.1m)
                return InterpolateColor(Color.LightYellow, Color.Yellow,(double)percentage* 10);
            if (percentage < 0.9m)
                return InterpolateColor(Color.Yellow, Color.Orange, ((double)percentage - 0.1) * 10.0 / 8.0);
            else
                return InterpolateColor(Color.Orange, Color.Red, ((double)percentage - 2.0 / 3.0) * 3);
        }

        public Color GetRecrystalizationColors(double elNumber, double max)
        {
            var percentage = (elNumber - 0) / (max - 0);
            if (percentage < 0.3)
                return InterpolateColor(Color.Orange, Color.OrangeRed, (double)percentage * 10/3.0);
            else if (percentage < 0.5)
                return InterpolateColor(Color.OrangeRed, Color.Red, ((double)percentage - 0.3) * 10.0 / 2.0);
            else if (percentage < 0.7)
                return InterpolateColor(Color.Red, Color.DarkRed, ((double)percentage - 0.5) * 10 / 2.0);
            else
                return InterpolateColor(Color.DarkRed, Color.Black, ((double)percentage - 0.7) * 10 / 3.0);
        }

    }
}
