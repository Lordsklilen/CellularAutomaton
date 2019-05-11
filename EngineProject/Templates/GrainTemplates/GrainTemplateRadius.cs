using EngineProject.DataStructures;
using System;
using System.Diagnostics;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateRadius : IGrainTemplateStrategy
    {
        Random rand = new Random();
        public void GenerateTemplate(TemplateRequest request)
        {
            var numberOfPoints = request.numberOfPoints;
            var panel = request.board;
            if (numberOfPoints < 1 && request.radius> panel.MaxY() || request.radius> panel.MaxX())
                throw new NotImplementedException("Cannot generate template with number of points: " + numberOfPoints.ToString() + " and  Radius: " + request.radius);

            panel.Clear();
            int NumberOfWrongShots = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 1; i <= numberOfPoints; i++)
            {
                if (NumberOfWrongShots > 100000 || stopWatch.ElapsedMilliseconds> 5000)
                    throw new ArgumentOutOfRangeException("Cannot add that many points. Propably number of expected points is to big.");
                int row = rand.Next(0, panel.MaxY());
                int collumn = rand.Next(0, panel.MaxX());
                if (panel.GetGrainNumber(row, collumn) == 0 && FreeSpaceInRadius(request, row, collumn))
                {
                    panel.SetGrainNumber(i, row, collumn);
                }
                else
                {
                    --i;
                    ++NumberOfWrongShots;
                }
            }
            stopWatch.Stop();
        }
        private bool FreeSpaceInRadius(TemplateRequest request, int x, int y)
        {
            var _maxRow = request.board.MaxY();
            var _maxColumn = request.board.MaxX();
            var r = request.radius;
            var panel = request.board;
            for (int i = -r; i <= r; i++)
            {
                for (int j = -r; j <= r; j++)
                {
                    if (Math.Sqrt((Math.Pow(i, 2) + Math.Pow(j, 2))) >= r)
                        continue;
                    int widthId = (i + x) >= 0 ? (i + x) % (_maxRow) : _maxRow - 1;
                    int heightId = (j + y) >= 0 ? (j + y) % (_maxColumn) : _maxColumn - 1;

                    if (panel.GetGrainNumber(widthId, heightId) != 0)
                        return false;
                }
            }
            return true;
        }
    }
}
