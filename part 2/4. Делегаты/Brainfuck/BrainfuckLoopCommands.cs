using System;
using System.Collections.Generic;
using System.Xml;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		//private static List<(int, int)> pairs = new List<(int, int)>();
		private static Dictionary<int, int> FindEndFromBegin = new Dictionary<int, int>();
		private static Dictionary<int, int> FindBeginFromEnd = new Dictionary<int, int>();

		private static void FillPairs(string input)
        {
			var openList  = new List<int>();
			var closeList = new List<int>();
			
			var index = input.IndexOfAny(new char[2] { '[', ']' });
			while (index != -1)
            {
				if (input[index] == '[')
					openList.Add(index);
				else
					closeList.Add(index);

				index = input.IndexOfAny(new char[2] { '[', ']' }, index + 1);
			}

			var openIndex = 0;
			foreach(var close in closeList)
            {
				while (openIndex < openList.Count - 1 && openList[openIndex + 1] < close)
					openIndex++;

				while(openIndex > 0 && openList[openIndex - 1] > close)
					openIndex--;

				var open = openList[openIndex];
				FindEndFromBegin.Add(open, close);
				FindBeginFromEnd.Add(close, open);
				openList.Remove(open);
				openIndex--;
			}
        }

		public static void RegisterTo(IVirtualMachine vm)
		{
			//pairs = new List<(int, int)>();
			FindEndFromBegin = new Dictionary<int, int>();
			FindBeginFromEnd = new Dictionary<int, int>();

			vm.RegisterCommand('[', b => {
				if (FindBeginFromEnd.Count == 0)
					FillPairs(b.Instructions);

				if (b.Memory[b.MemoryPointer] == 0)
					b.InstructionPointer = FindEndFromBegin[b.InstructionPointer] - 1;
			});

			vm.RegisterCommand(']', b => {
				if (FindBeginFromEnd.Count == 0)
					FillPairs(b.Instructions);

				if (b.Memory[b.MemoryPointer] != 0)
					b.InstructionPointer = FindBeginFromEnd[b.InstructionPointer] - 1;
			});
		}
	}
}