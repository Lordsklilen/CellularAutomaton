﻿using EngineProject.DataStructures;
using System;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateRandom : IGrainTemplateStrategy
    {
        Random rand = new Random();
        public void GenerateTemplate(Board panel, int parameter = -1)
        {
            if (parameter < 1)
                throw new NotImplementedException("Cannot generate template with number of points: " + parameter.ToString());
            var numberOfPoints = parameter;
            panel.Clear();
            int NumberOfWrongShots = 0;
            for (int i = 0; i < numberOfPoints; i++)
            {
                if (NumberOfWrongShots > 100000)
                    throw new ArgumentOutOfRangeException("Cannot add that many points. Propably number of expected points is to big.");
                int row = rand.Next(0, panel.MaxY());
                int collumn = rand.Next(0, panel.MaxX());
                if (panel.GetGrainNumber(row,collumn) == 0)
                {
                    panel.SetGrainNumber(i,row, collumn);
                }
                else {
                    --i;
                    ++NumberOfWrongShots;
                }
            }
        }
    }
}