using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Computer
    {
        private enum Opcode 
        {
            Add = 1,
            Multi = 2,
            Halt = 99
        }

        private List<int> m_memory;
        private int m_excutionPtr = 0;
        private bool m_isHalted = false;
        private bool m_logDetail = false;

        public Computer(List<int> initalMemory) 
        {
            m_memory = new(initalMemory);
        }

        public void Run()
        {
            while (!m_isHalted)
            {
                RunInstruction();
            }
            LogDetail($"Program running finished");
        }

        public int GetAt(int index)
        {
            if (index >= m_memory.Count)
            {
                throw new ArgumentException($"[GetAt] Memory out of index {index} size {m_memory.Count}");
            }
            return m_memory[index];
        }

        public void SetAt(int index, int val)
        {
            if (index >= m_memory.Count)
            {
                throw new ArgumentException($"[GetAt] Memory out of index {index} size {m_memory.Count}");
            }
            m_memory[index] = val;
        }

        public void Restore(int noun, int verb)
        {
            SetAt(1, noun);
            SetAt(2, verb);
        }

        public void Dump()
        {
            Console.WriteLine($"[Dump] {string.Join(",", m_memory)}");
        }

        private void RunInstruction()
        {
            LogDetail($"[RunInstruction] Reading opcode from {m_excutionPtr}");
            int instruction = Fetch();

            switch (instruction)
            {
                case (int)Opcode.Add: 
                case (int)Opcode.Multi:
                {
                    int op1 = FetchIndirect();
                    int op2 = FetchIndirect();
                    int dst = Fetch();
                    int val = (instruction == (int)Opcode.Add) ? (op1 + op2) : (op1 * op2);

                    LogDetail($"[Inst] [{m_excutionPtr}] {(Opcode)instruction} {op1} {op2} => {val} at {dst}");
                    SetAt(dst, val);
                    break; 
                }
                case (int)Opcode.Halt:
                {
                    LogDetail($"[Inst] Halting program");
                    m_isHalted = true;
                    break;
                }
                default:
                    throw new InvalidDataException($"[RunInstruction] unknown opcode {instruction}, at {m_excutionPtr}");
            }
        }

        private int Fetch()
        {
            LogDetail($"[Fetch] {m_excutionPtr} => {m_memory[m_excutionPtr]}");
            return m_memory[m_excutionPtr++];
        }

        private int FetchIndirect()
        {
            return GetAt(Fetch());
        }

        public void LogDetail(string str)
        {
            if (m_logDetail)
            {
                Console.WriteLine(str);
            }
        }
    }

	class Problem02 
	{
		public static int Solve1(List<int> data)
		{
            Computer computer = new(data);

            computer.Restore(12, 2);
            computer.Run();

			return computer.GetAt(0);
		}

		public static int Solve2(List<int> data, int toFind)
		{
            for (int i = 0; i <= 99; i ++)
            {
                for (int k = 0; k <= 99; k ++)
                {
                    Computer computer = new(data);                    
                    computer.Restore(i, k);
                    computer.Run();
                    int output = computer.GetAt(0);

                    computer.LogDetail($"[Solve2] Finished loop : noun {i} verb {k} => {output}");

                    if (output == toFind)
                    {
                        return i * 100 + k;
                    }
                }
            }

            return 0;
		}
		public static void Start() 
		{
			var textData = File.ReadAllText("data/input02.txt");
			var textArr = textData.Split(',');
			var intList = textArr.Select(t => int.Parse(t)).ToList();
			int ans1 = Solve1(intList);
			int ans2 = Solve2(intList, 19690720);

			Console.WriteLine($"ans = {ans1}, {ans2}");
		}
	}
}


