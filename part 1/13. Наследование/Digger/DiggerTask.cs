using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    //Напишите здесь классы Player, Terrain и другие.
    public class Terrain : ICreature
    {
        public string GetImageFileName()
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }
    }

    public class Player : ICreature
    {
        public string GetImageFileName()
        {
            return "Digger.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        private bool CanMove(int x, int y)
        {
            return Game.Map[x, y] == null || Game.Map[x, y] != null && 
                (Game.Map[x, y].GetType().Name == "Terrain" || Game.Map[x, y].GetType().Name == "Gold" || Game.Map[x, y].GetType().Name =="Monster");
        }

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand();
            if (Game.KeyPressed == System.Windows.Forms.Keys.Left && x - 1 >= 0 && CanMove(x - 1, y))
                command.DeltaX = -1;
            if (Game.KeyPressed == System.Windows.Forms.Keys.Right && x + 1 < Game.MapWidth && CanMove(x + 1, y))
                command.DeltaX = 1;
            if (Game.KeyPressed == System.Windows.Forms.Keys.Up && y - 1 >= 0 && CanMove(x, y - 1))
                command.DeltaY = -1;
            if (Game.KeyPressed == System.Windows.Forms.Keys.Down && y + 1 < Game.MapHeight && CanMove(x, y + 1))
                command.DeltaY = 1;
            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            var anotherObjectFileName = conflictedObject.GetType().Name;
            if (anotherObjectFileName == "Terrain")
                return false;
            else if (anotherObjectFileName == "Sack" || anotherObjectFileName == "Monster")
                return true;
            return false;
        }
    }

    public class Sack : ICreature
    {
        private int longToFly = 0;

        public string GetImageFileName()
        {
            return "Sack.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand();
            if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] == null)
                command.DeltaY++;

            longToFly += command.DeltaY;

            if (command.DeltaY == 0 && longToFly > 0 && y + 1 < Game.MapHeight && (Game.Map[x, y + 1].GetType().Name == "Player" || Game.Map[x, y + 1].GetType().Name == "Monster"))
            {
                Game.Map[x, y] = null;
                command.DeltaY++;
                longToFly++;
            }
            else if (command.DeltaY == 0 && longToFly > 1)
            {
                if (y == Game.MapHeight)
                    command.TransformTo = new Gold();
                else if (y < Game.MapHeight)
                {
                    var anotherObjectName = Game.Map[x, y].GetType().Name;
                    if (anotherObjectName == "Terrain" || anotherObjectName == "Sack" || anotherObjectName == "Gold")
                        command.TransformTo = new Gold();
                }
            }
            else if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] != null && Game.Map[x, y + 1].GetType().Name == "Terrain")
                longToFly = 0;

            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }
    }

    public class Gold: ICreature
    {
        public string GetImageFileName()
        {
            return "Gold.png";
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetType().Name == "Player")
            {
                Game.Scores += 10;
                return true;
            }
            if (conflictedObject.GetType().Name == "Monster")
                return true;
            return false;
        }
    }

    public class Monster : ICreature
    {
        public string GetImageFileName()
        {
            return "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        private bool CanMove(int x, int y)
        {
            if (x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight)
                return Game.Map[x, y] == null || Game.Map[x, y] != null && Game.Map[x, y].GetType().Name != "Terrain" &&
                       Game.Map[x, y].GetType().Name != "Sack" && Game.Map[x, y].GetType().Name != "Monster";
            else
                return false;
        }


        private (int, int) PlayersCoord()
        {
            int x = -1, y = -1;
            for (int i = 0; i < Game.MapWidth; i++)
                for (int j = 0; j < Game.MapHeight; j++)
                    if (Game.Map[i, j] != null && Game.Map[i, j].GetType().Name == "Player")
                        (x, y) = (i, j);
            return (x, y);
        }

        public CreatureCommand Act(int x, int y)
        {
            (int xPlayer, int yPlayer) = PlayersCoord();
            if (xPlayer == -1 && yPlayer == -1)
                return new CreatureCommand();

            var command = new CreatureCommand();
            int xDif = xPlayer - x, yDif = yPlayer - y;
            if (xDif != 0 && CanMove(x + Math.Sign(xDif), y))
                command.DeltaX = Math.Sign(xDif);
            else if (CanMove(x, y + Math.Sign(yDif)))
                command.DeltaY = Math.Sign(yDif);

            return command;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.GetType().Name == "Player")
                return false;
            else if (conflictedObject.GetType().Name == "Monster" || conflictedObject.GetType().Name == "Sack")
                return true;
            
            return false;
        }
    }
}
