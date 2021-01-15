using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rivals
{
	public class RivalsTask
	{
		public static IEnumerable<Point> GetNeighbours(Point p)
		{
			//return new points to check
			return new List<Point>() {  new Point(p.X + 1, p.Y),
										new Point(p.X, p.Y + 1),
										new Point(p.X - 1, p.Y),
										new Point(p.X, p.Y - 1) };
		}

		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var playersSteps = new int[map.Maze.GetLength(0), map.Maze.GetLength(1)];
			var emptyCells = map.Maze.GetLength(0) * map.Maze.GetLength(1);

			for (int i = 0; i < map.Maze.GetLength(0); i++)
				for (int j = 0; j < map.Maze.GetLength(1); j++)
					playersSteps[i, j] = -1;

			var grayCells = new List<Queue<Point>>();
			int steps = 0;

			for (int i = 0; i < map.Players.Length; i++)
			{
				grayCells.Add(new Queue<Point>());
				grayCells[i].Enqueue(map.Players[i]);
				emptyCells--;
				playersSteps[map.Players[i].X, map.Players[i].Y] = steps;
				yield return new OwnedLocation(i, map.Players[i], steps);
			}

			while (grayCells.Any(x => x.Count > 0))
			{
				for (int i = 0; i < map.Players.Length; i++)
				{
					var nodes = new Queue<Point>(grayCells[i]);
					grayCells[i] = new Queue<Point>();
					
					while (nodes.Count > 0)
					{
						var current = nodes.Dequeue();
						if (playersSteps[current.X, current.Y] == -1)
							yield return new OwnedLocation(i, current, steps);

						playersSteps[current.X, current.Y] = steps;
						emptyCells--;

						foreach (var next in GetNeighbours(current).Where(n => map.InBounds(n) &&
																				playersSteps[n.X, n.Y] == -1 &&
																				map.Maze[n.X, n.Y] == MapCell.Empty))
							grayCells[i].Enqueue(next);
					}
				}
				steps++;
			}
		}
	}
}

/*
 var gray = new HashSet<Point>();
			var current = new Queue<SinglyLinkedList<Point>>();

			//visited.Add(start)
			gray.Add(start);
			current.Enqueue(new SinglyLinkedList<Point>(start));

			while (current.Count() != 0)
            {
				var node = current.Dequeue();

				//Check?!
				if (chests.Contains(node.Value))
					yield return node;

				foreach (var nextNode in GetNeighbours(node.Value).Where(n =>  !gray.Contains(n) &&
																		 		map.InBounds(n)  && 
																		 		map.Dungeon[n.X, n.Y] == MapCell.Empty))
				{
					gray.Add(nextNode);
					//add point somewhere
					current.Enqueue(new SinglyLinkedList<Point>(nextNode, node));
				}
			}
//*/