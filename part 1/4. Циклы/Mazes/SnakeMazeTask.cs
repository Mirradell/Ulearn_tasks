namespace Mazes
{
	public static class SnakeMazeTask
	{
        private static void Step(Robot robot, Direction direction, int length)
        {
            for (int i = 0; i < length; i++)
                robot.MoveTo(direction);
                
            if (!robot.Finished)
            {
                robot.MoveTo(Direction.Down);
                robot.MoveTo(Direction.Down);
            }
        }

        public static void MoveOut(Robot robot, int width, int height)
    		{
            int i = 0;
            while (!robot.Finished)
            {
                if (i % 2 == 0)
                    Step(robot, Direction.Right, width - 3);
                else
                    Step(robot, Direction.Left, width - 3);
                i++;
            }
        }
	}
}