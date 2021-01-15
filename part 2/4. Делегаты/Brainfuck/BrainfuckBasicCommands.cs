using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => {
				var symbolUTF = b.Memory[b.MemoryPointer];
				write(Convert.ToChar(symbolUTF)); 
			});
			
			vm.RegisterCommand(',', b => {
				var memory = read();
				if (memory != -1)
					b.Memory[b.MemoryPointer] = Convert.ToByte(memory);
			});

			vm.RegisterCommand('>', b => {
				b.MemoryPointer = (b.MemoryPointer + 1) % b.Memory.Length;
			});

			vm.RegisterCommand('<', b => {
				b.MemoryPointer = (b.MemoryPointer + b.Memory.Length - 1) % b.Memory.Length;
			});

			vm.RegisterCommand('+', b => { b.Memory[b.MemoryPointer] = (byte)((b.Memory[b.MemoryPointer] + 1) % 256); });
			vm.RegisterCommand('-', b => { b.Memory[b.MemoryPointer] = (byte)((b.Memory[b.MemoryPointer] + 255) % 256); });

			for (char symbol = 'a'; symbol <= 'z'; symbol++)
			{
				var temp = symbol;
				vm.RegisterCommand(symbol, b =>
				{
					b.Memory[b.MemoryPointer] = Convert.ToByte(temp);
				});
			}

			for (char symbol = 'A'; symbol <= 'Z'; symbol++)
			{
				var temp = symbol;
				vm.RegisterCommand(symbol, b =>
				{
					b.Memory[b.MemoryPointer] = Convert.ToByte(temp);
				});
			}

			for (char i = '0'; i <= '9'; i++)
			{
				var temp = i;
				vm.RegisterCommand(temp, b =>
				{
					b.Memory[b.MemoryPointer] = Convert.ToByte(temp);
				});
			}
		}
	}
}