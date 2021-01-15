using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
    public class MyClass
    {
        public int Hardness { get; }
        public int EnergyLeft { get; }
        public List<Point> Points { get; }
        public Point LastPoint { get; }
        public int ChestsCount { get; }
        public int Index { get; }

        public MyClass(int hardness, int energyLeft, int index, int chests, Point currentPoint, List<Point> previous = null)
        {
            Hardness = hardness;
            EnergyLeft = energyLeft;
            LastPoint = currentPoint;
            Index = index;
            ChestsCount = chests;

            if (previous == null)
                Points = new List<Point>();
            else
                Points = new List<Point>(previous);

            Points.Add(LastPoint);
        }

    }
    public class DijkstraPathFinder
    {
        public static IEnumerable<Point> GetNeighbours(Point p)
        {
            //return new points to check
            return new List<Point>() {  new Point(p.X + 1, p.Y),
                                        new Point(p.X, p.Y + 1),
                                        new Point(p.X - 1, p.Y),
                                        new Point(p.X, p.Y - 1) };
        }

        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var queue = new List<MyClass>();
            queue.Add(new MyClass(state.InitialEnergy, state.InitialEnergy, 0, 0, start));
            var visited = new HashSet<Point>();
            visited.Add(start);

            while (queue.Count > 0)
            {
                var node = queue.OrderBy(x => x.Hardness).ThenBy(x => x.Index).ToList()[0];
                queue.Remove(node);

                if (targets.Contains(node.LastPoint))
                    yield return new PathWithCost((node.Hardness - node.EnergyLeft) / 2, node.Points.ToArray());

                foreach (var nextNode in GetNeighbours(node.LastPoint).Where(n => state.InsideMap(n) &&
                                                                                  !state.IsWallAt(n) &&
                                                                                  !visited.Contains(n)))
                {
                    visited.Add(nextNode);
                    queue.Add(new MyClass(node.Hardness + state.CellCost[nextNode.X, nextNode.Y],
                                              node.EnergyLeft - state.CellCost[nextNode.X, nextNode.Y],
                                              queue.Count,
                                              0, 
                                              nextNode,
                                              node.Points));
                }

            }
        }
    }
}