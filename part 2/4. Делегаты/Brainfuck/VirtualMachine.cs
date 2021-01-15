using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public  string Instructions       { get; }
		public  int    InstructionPointer { get; set; }
		public  byte[] Memory             { get; }
		public  int    MemoryPointer      { get; set; }

		private Dictionary<char, Action<IVirtualMachine>> commands = new Dictionary<char, Action<IVirtualMachine>>();

		public VirtualMachine(string program, int memorySize)
		{
			if (program != null)
				Instructions = program;
			else
				throw new ArgumentNullException("Empty program!");

			if (memorySize > 0 && memorySize <= 30000)
            {
				Memory = new byte[memorySize];
				MemoryPointer = 0;
			}
			else
				throw new ArgumentNullException("Wrong memorySize!");
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (!commands.ContainsKey(symbol))
				commands.Add(symbol, execute);
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				var current = Instructions[InstructionPointer];
				if (commands.ContainsKey(current))
					commands[current](this);
				InstructionPointer++;
			}
		}
	}
}