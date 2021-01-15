namespace Mazes
{
	public static class EmptyMazeTask
	{
        private static void Step(Robot robot, Direction direction, int length)
        {
            for (int i = 0; i < length; i++)
                robot.MoveTo(direction);
        }

		public static void MoveOut(Robot robot, int width, int height)
		{
            Step(robot, Direction.Right, width - 3);
            Step(robot, Direction.Down, height - 3);
		}
	}
}