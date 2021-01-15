using System.Drawing;
using System.Linq;
using System;

namespace func_rocket
{
	public class ForcesTask
	{
		/// <summary>
		/// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
		/// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
		/// </summary>
		public static RocketForce GetThrustForce(double forceValue)
		{
			return r => new Vector(forceValue * Math.Cos(r.Direction), forceValue * Math.Sin(r.Direction));
		}

		/// <summary>
		/// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
		/// </summary>
		public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
		{
			return r => gravity(spaceSize, r.Location);
		}

		/// <summary>
		/// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
		/// </summary>
		public static RocketForce Sum(params RocketForce[] forces)
		{
			return r =>
			{
				if (forces.Length == 0)
					return Vector.Zero;

				var sum = forces[0](r);
				for (int i = 1; i < forces.Length; i++)
					sum += forces[i](r);
				return sum;
			};
		}
	}
}