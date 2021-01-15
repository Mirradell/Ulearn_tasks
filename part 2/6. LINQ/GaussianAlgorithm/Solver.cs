using System;
using System.Linq;
using System.Collections.Generic;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] items, double[] freeMembers)
        {
            var matrix = new List<List<double>>();
            var free = new List<double>();

            matrix = new List<List<double>>(items.Select(x => x.ToList()));
            free = new List<double>();
            free.AddRange(freeMembers.ToList());

            int shiftColumn = 0;
            for (int i = 0; i < matrix[0].Count; i++)
                if (i + shiftColumn < matrix[0].Count && i < matrix.Count)
                {
                    var mainCoef = matrix[i][i + shiftColumn];
                    if (mainCoef == 0)
                    {
                     //   if (i == matrix.Count - 1)
                       //     continue;

                        var possibleLine = matrix.FindIndex(i, x => x[i + shiftColumn] != 0);
                        if (possibleLine == -1)
                        {
                            shiftColumn++;
                            i--;
                            continue;
                        }
                        var temp = matrix[i];
                        matrix[i] = matrix[possibleLine];
                        matrix[possibleLine] = temp;
                        mainCoef = matrix[i][i + shiftColumn];

                        var t2 = free[i];
                        free[i] = free[possibleLine];
                        free[possibleLine] = t2;
                    }
                    
                    for (int y = 0; y < matrix[0].Count; y++)
                        matrix[i][y] = matrix[i][y] / mainCoef;
                    free[i] = free[i] / mainCoef;

                    for (int x = 0; x < matrix.Count; x++)
                        if (x != i)
                        {
                            var tempCoef = matrix[x][i + shiftColumn];
                            for (int y = 0; y < matrix[0].Count; y++)
                                matrix[x][y] = matrix[x][y] - matrix[i][y] * tempCoef;
                            free[x] = free[x] - free[i] * tempCoef;

                            if (matrix[x].All(z => Math.Abs(z) < 1e-3) && Math.Abs(free[x]) > 1e-3)
                                    throw new NoSolutionException("No solution " + free[x] + " " + x);
                        }
                }

            var res = new List<double>();
            shiftColumn = 0;
            for (int i = 0; i < matrix[0].Count; i++)
                if (i - shiftColumn >= free.Count)
                    res.Add(0);
            else if (matrix[i - shiftColumn][i] == 0)
                {
                    res.Add(0);
                    shiftColumn++;
                }
                else
                    res.Add(free[i - shiftColumn] / matrix[i - shiftColumn][i]);

            return res.ToArray();
        }
    }
}
