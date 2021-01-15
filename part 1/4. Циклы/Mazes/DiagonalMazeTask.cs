namespace Mazes
{
	public static class DiagonalMazeTask
	{
        private static void Step(Robot robot, Direction direction, int length)
        {
            for (int i = 0; i < length; i++)
                robot.MoveTo(direction);

            if (!robot.Finished && direction == Direction.Right)
                robot.MoveTo(Direction.Down);
            else if (!robot.Finished)
                robot.MoveTo(Direction.Right);
        }

        private static void Move(Robot robot, int bigger, int less, Direction direction)
        {
            int length = bigger / less;
            while (!robot.Finished)
                Step(robot, direction, length);
        }

        public static void MoveOut(Robot robot, int width, int height)
		{
            if (width > height)
                Move(robot, width - 2, height - 2, Direction.Right);
            else
                Move(robot, height - 2, width - 2, Direction.Down);            
		}
	}
}