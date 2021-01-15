using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class BfsTask
	{
		public static IEnumerable<Point> GetNeighbours(Point p)
		{
			return new List<Point>() {  new Point(p.X + 1, p.Y), 
										new Point(p.X, p.Y + 1),
										new Point(p.X - 1, p.Y),
										new Point(p.X, p.Y - 1) };
		}

		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
			var gray = new HashSet<Point>();
			var current = new Queue<SinglyLinkedList<Point>>();

			gray.Add(start);
			current.Enqueue(new SinglyLinkedList<Point>(start));

			while (current.Count() != 0)
            {
				var node = current.Dequeue();

				if (chests.Contains(node.Value))
					yield return node;

				foreach (var nextNode in GetNeighbours(node.Value).Where(n => !gray.Contains(n) && map.InBounds(n) && map.Dungeon[n.X, n.Y] == MapCell.Empty))
				{
					gray.Add(nextNode);
					current.Enqueue(new SinglyLinkedList<Point>(nextNode, node));
				}
			}

		}
	}
}