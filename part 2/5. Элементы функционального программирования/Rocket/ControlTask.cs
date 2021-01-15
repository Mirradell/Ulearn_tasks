using System;

namespace func_rocket
{
	public class ControlTask
	{
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			if ((new Vector(2, 2).Rotate(rocket.Direction) + rocket.Velocity * 2).Angle > (target - rocket.Location).Angle)
				return Turn.Left;
			else
				return Turn.Right;
		}
	}
}