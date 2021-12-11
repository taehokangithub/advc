//#define DEBUGWRITE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Advc20_09
{
    static void debugWrite(string s)
    {
        #if DEBUGWRITE
            Console.WriteLine(s);
        #endif
    }

	class Solver
	{
		public List<long> Numbers{ get; set; }
		public int PreambleSize { get; set; }

		public void solve()
		{
			for (int i = 0; i < Numbers.Count - PreambleSize - 1; i ++)
			{
				var curSet = Numbers.GetRange(i, PreambleSize);
				long target = Numbers[i + PreambleSize];

				if (!findAddingCouple(target, curSet))
				{
					Console.WriteLine($"Part 1 answer {target}");

					curSet = Numbers.GetRange(0, i);
					Console.WriteLine($"Part 2 answer {findContiguos(target, curSet)}");
					return;
				}
			}
		}

		private long findContiguos(long target, List<long> range)
		{
			for (int i = 0; i < range.Count - 2; i ++)
			{
				for (int k = 3; i + k <= range.Count; k ++)
				{
					var partList = new List<long>(range.Skip(i).Take(k));
					long sum = partList.Sum();

					debugWrite($"[{i}+{k}<={range.Count}]:");
					foreach (var val in partList)
					{
						debugWrite($"[{val}]");
					}
					debugWrite($"=> {sum}");
					
					if (sum == target)
					{
						partList.Sort();
						return partList[0] + partList[partList.Count - 1];
					}
				}
			}
			return -1;
		}

		private bool findAddingCouple(long target, List<long> range)
		{
			var copied = range.GetRange(0, range.Count);
			copied.Sort();

			for (int i = 0; i < copied.Count - 1; i ++)
			{
				for (int k = copied.Count - 1; k > i; k --)
				{
					long sum = copied[i] + copied[k];

					if (sum == target)
					{
						return true;
					}
					if (sum < target)
					{
						break;
					}
				}
			}
			return false;
		}
	};

    static void solve(string path, int preambleSize)
    {
		string text = File.ReadAllText(path);
		var numbers = new List<long> (Array.ConvertAll(text.Split('\n'), s => 
		{
			long val;
			return long.TryParse(s, out val) ? val : -1;
		}));

		Solver solver = new Solver{ Numbers = numbers, PreambleSize = preambleSize };
		solver.solve();
    }

    static void Run()
    {
        Console.WriteLine($"Starting {(new StackFrame().GetMethod().DeclaringType)}" );

        solve("../data/advc20_09_sample.txt", 5);
        solve("../data/advc20_09.txt", 25);
    }

    static void Main()
    {
        Run();
    }    
}