
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

class Advc20_13
{
    static bool s_debugWrite = false;
    static readonly string[] s_inputSample = new string[] { "939", "7,13,x,x,59,x,31,19" };
    static readonly string[] s_input = new string[] {"1002578", "19,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,751,x,29,x,x,x,x,x,x,x,x,x,x,13,x,x,x,x,x,x,x,x,x,23,x,x,x,x,x,x,x,431,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,17"};

    class Solver
    {
        private int m_time;
        private List<int> m_buses = new List<int>();
        private Dictionary<int, int> m_busOffsets = new Dictionary<int, int>();
        public Solver(string numStr, string busStr)
        {
            m_time = int.Parse(numStr);
            int offset = 0;
            foreach (string str in busStr.Split(","))
            {
                int bus;
                if (int.TryParse(str, out bus))
                {
                    m_buses.Add(bus);
                    m_busOffsets[bus] = offset;
                }

                offset ++;
            }
        }

        public void Solve1()
        {
            int minWait = int.MaxValue;
            int minWaitBus = 0;

            foreach(int bus in m_buses)
            {
                int timeDiff = bus - (m_time % bus);

                if (minWait > timeDiff)
                {
                    minWait = timeDiff;
                    minWaitBus = bus;
                }
            }

            int ans = minWait * minWaitBus;
            Console.WriteLine($"Solve1 = {ans}");
        }

        public void Solve2()
        {
            long step = 1;
            long start = 1;

            foreach (var item in m_busOffsets)
            {
                for (long i = start; true; i += step)
                {
                    if ((i + item.Value) % item.Key == 0)
                    {
                        start = i;
                        step *= item.Key;

                        Console.WriteLine($"Found ans for {item.Key}:{item.Value} at {start}");
                        break;
                    }
                }
            }

            Console.WriteLine($"solve2 = {start}");
        }
    }

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

	static void SolveMain(string[] input)
	{
		Solver solver1 = new Solver(input[0], input[1]);
        solver1.Solve1();
        solver1.Solve2();
	}


	static void Run()
	{
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

		Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain(s_inputSample);
		SolveMain(s_input);
	}

    static void Main()
    {
        Run();
    }

}