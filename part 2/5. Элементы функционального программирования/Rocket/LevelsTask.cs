using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		static readonly Physics standardPhysics = new Physics();

		private static Level GetNewLevel(string name, Rocket rocket, Vector target, Gravity gravity)
		{
			return new Level(name, rocket, target, gravity, standardPhysics);
		}

		private static double Distance(Vector from, Vector to)
		{
			return Math.Sqrt((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y));
		}

		private static Level GetLevelByName(string name)
		{
			Rocket rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
			Vector target = new Vector(600, 200);
			Gravity gravity = (size, v) => Vector.Zero;

			switch (name)
			{
				case "Zero":
					break;
				case "Heavy":
					gravity = (size, v) => new Vector(0, 0.9);
					break;
				case "Up":
					target = new Vector(700, 500);
					gravity = (size, v) => new Vector(0, -300 / (size.Height - v.Y + 300.0));
					break;
				case "WhiteHole":
					gravity = (size, v) => {
						var dist = Distance(target, v);
						var length = 140.0 * dist / (dist * dist + 1.0);
						return (v - target).Normalize() * length;
					};
					break;
				case "BlackHole":
					gravity = (size, v) =>
					{
						var anom = (rocket.Location + target) / 2;
						var dist = Distance(anom, v);
						var length = 300.0 * dist / (dist * dist + 1.0);
						return (anom - v).Normalize() * length;
					};
					break;
				case "BlackAndWhite":
					gravity = (size, v) =>
					{
						var dist = Distance(target, v);
						var length1 = 140.0 * dist / (dist * dist + 1.0);

						var r = rocket.Location;
						var anoma = (r + target) / 2.0;
						dist = Distance(anoma, v);
						var length2 = 300.0 * dist / (dist * dist + 1.0);
						var v2 = ((v - target).Normalize() * length1 + (anoma - v).Normalize() * length2) / 2.0;
						return v2;
					};
					break;
			}

			return GetNewLevel(name, rocket, target, gravity);
		}

		public static IEnumerable<Level> CreateLevels()
		{
			var levels = new List<string>() { "Zero", "Heavy", "Up", "WhiteHole", "BlackHole", "BlackAndWhite" };

			foreach (var level in levels)
				yield return GetLevelByName(level);
		}
	}
}