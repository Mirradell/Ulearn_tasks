// Вставьте сюда финальное содержимое файла GreedyPathFinder.cs

using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;
using System;

namespace Greedy
{
	public class GreedyPathFinder : IPathFinder
	{

		public List<Point> FindPathToCompleteGoal(State state)
		{
			var res = new List<Point>();
			if (state.Goal > state.Chests.Count)
				return res;

			var energyLeft = state.Energy;
			if (state.Chests.Contains(state.Position))
			{
				state.Chests.Remove(state.Position);
			}//*/

			DijkstraPathFinder pathFinder = new DijkstraPathFinder();
			var paths = pathFinder.GetPathsByDijkstra(state, state.Position, state.Chests);

			var pathToWin = new PathWithCost(-1, new Point[0]);

			for (int i = 0; i < state.Goal; i++)
			{
				if (energyLeft == 0)
				{
					//Console.WriteLine(res.Count);
					break;
				}
				pathToWin = new PathWithCost(-1, new Point[0]);
				foreach (var path in paths)
					if (path.Cost <= energyLeft)// && path.Cost > 1)
												//if (path.Cost > 1)
					{
						pathToWin = path;
						break;
					}
				if (pathToWin.Cost != -1 && pathToWin.Path.Count > 1 && state.Chests.Count > 0)
				{
					pathToWin.Path.Remove(pathToWin.Start);

					res.AddRange(pathToWin.Path);
					energyLeft -= pathToWin.Cost;

					state.Chests.Remove(pathToWin.End);
					paths = pathFinder.GetPathsByDijkstra(state, pathToWin.End, state.Chests);
				}
			}

			return res;
		}
	}
}
