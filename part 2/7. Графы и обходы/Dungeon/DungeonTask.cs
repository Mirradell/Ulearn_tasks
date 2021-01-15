using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public static class ExtensionTask
	{
		public static IEnumerable<T> ToNormalList<T>(this IEnumerable<SinglyLinkedList<T>> nodes)
		{
			return nodes.SelectMany(x => x.Zip(x.Previous, (a, b) => new List<T>() { a, b })).SelectMany(x => x).Distinct();
		}
	}
	public class DungeonTask
	{
		public static MoveDirection GuessMove(Point from, Point to)
		{
			Point difference = new Point(to.X - from.X, to.Y - from.Y);

			if (difference.X == 0 && difference.Y == 1)
				return MoveDirection.Down;

			if (difference.X == 0 && difference.Y == -1)
				return MoveDirection.Up;

			if (difference.X == 1 && difference.Y == 0)
				return MoveDirection.Right;

			return MoveDirection.Left;
		}

		public static MoveDirection[] FindShortestPath(Map map)
		{
			var start = map.InitialPosition;
			var exitPath = BfsTask.FindPaths(map, start, new Point[1] { map.Exit });
			var pathsToChests = BfsTask.FindPaths(map, start, map.Chests);
			
			var min = int.MaxValue;

			var pathFromStartToChest = new SinglyLinkedList<Point>(map.InitialPosition);
			var pathFromChestToEnd = exitPath;

			if (exitPath.Count() == 0)
				return new MoveDirection[0];

			var prev = map.InitialPosition;
			if (exitPath.ToNormalList().Any(x => map.Chests.Contains(x)))
				return pathFromChestToEnd
								.ToNormalList()
								.Take(pathFromChestToEnd.First().Length - 1)
								.Reverse()
								.Select(x =>
								{
									var move = GuessMove(prev, x);
									prev = x;
									return move;
								})
								.ToArray();

			foreach (var path in pathsToChests)
			{
				var fromChestToEnd = BfsTask.FindPaths(map, path.Value, new Point[1] { map.Exit });
				var fromChestToEndLength = fromChestToEnd.First().Length;
				if (fromChestToEndLength + path.Length - 1 == exitPath.First().Length || fromChestToEndLength + path.Length - 1 < min)
				{
					min = fromChestToEndLength + path.Length - 1;
					pathFromStartToChest = path;
					pathFromChestToEnd = fromChestToEnd;
				}
			}

			if (pathFromChestToEnd == exitPath)
				return pathFromChestToEnd
								.ToNormalList()
								.Take(pathFromChestToEnd.First().Length - 1)
								.Reverse()
								.Select(x =>
								{
									var move = GuessMove(prev, x);
									prev = x;
									return move;
								})
								.ToArray();

			return pathFromChestToEnd
								.ToNormalList()
								.Take(pathFromChestToEnd.First().Length - 1)
								.Concat(pathFromStartToChest
										.Zip(pathFromStartToChest.Previous, (a, b) => new List<Point>() { a, b })
										.SelectMany(x => x)
										.Distinct().Take(pathFromStartToChest.Length - 1))
								.Reverse()
								.Select(x =>
								{
									var move = GuessMove(prev, x);
									prev = x;
									return move;
								})
								.ToArray();
		}
	}
}
