using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;
using System;
using System.Linq;

namespace Greedy
{
	public class ClassForStack
	{
		public int EnergyLeft { get; }
		public List<Point> Path { get; }
		public Point End { get; }
		public int Cost { get; }
		public List<Point> ChestsLeft { get; }

		public ClassForStack(int energyLeft, PathWithCost path, List<Point> chests, Point deleteChest)
		{
			EnergyLeft = energyLeft;
			Cost = path.Cost;
			End = path.End;

			path.Path.RemoveAt(0);
			Path = new List<Point>(path.Path);

			ChestsLeft = new List<Point>(chests);
			ChestsLeft.Remove(deleteChest);
		}

		public ClassForStack(int energyLeft, PathWithCost path, List<Point> prevPath, List<Point> chests, Point deleteChest)
		{
			EnergyLeft = energyLeft;
			//Cost = path.Cost + prevPath.Cost;
			End = path.End;

			Path = new List<Point>(prevPath);
			Path.AddRange(path.Path.Skip(1));
			//Path.Path = Path.Path.Distinct().ToList();

			ChestsLeft = new List<Point>(chests);
			ChestsLeft.Remove(deleteChest);
		}

	}

	public class NotGreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			var res = new ClassForStack(int.MaxValue, new PathWithCost(int.MaxValue, new Point[1] { state.Position }), state.Chests, new Point(-1, -1));
			var stack = new Stack<ClassForStack>();
			var paths = (new DijkstraPathFinder()).GetPathsByDijkstra(state, state.Position, state.Chests).ToList();

			foreach (var path in paths)
				stack.Push(new ClassForStack(state.InitialEnergy - path.Cost, path, state.Chests, path.End));

			while (stack.Count > 0)
			{
				var thisPath = stack.Pop();
				if (thisPath.EnergyLeft <= 0 || thisPath.ChestsLeft.Count == 0)
					return thisPath.Path;
				else
				{
					paths = (new DijkstraPathFinder()).GetPathsByDijkstra(state, thisPath.End, thisPath.ChestsLeft).ToList();
					foreach (var path in paths)
						if (path.Cost < thisPath.EnergyLeft)
							if (thisPath.ChestsLeft.Count == 1)
							//res.Add(new ClassForStack(thisPath.EnergyLeft - path.Cost, path, thisPath.Path, thisPath.ChestsLeft, path.End));
							{
								thisPath.Path.AddRange(path.Path.Skip(1));
								return thisPath.Path;
							}
							else
								stack.Push(new ClassForStack(thisPath.EnergyLeft - path.Cost, path, thisPath.Path, thisPath.ChestsLeft, path.End));
						else if (path.Cost == thisPath.EnergyLeft)
						{
							var temp = new ClassForStack(0, path, thisPath.Path, thisPath.ChestsLeft, path.End);
							if (temp.ChestsLeft.Count() < res.ChestsLeft.Count || temp.ChestsLeft.Count == res.ChestsLeft.Count() && temp.EnergyLeft > res.EnergyLeft)
								res = temp;
						}
				}
			}

			if (res.Path.Count == 1 && res.Path[0] == state.Position)
				return new List<Point>();
			return res.Path;
		}
	}
}