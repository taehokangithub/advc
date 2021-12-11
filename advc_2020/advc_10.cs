
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


class Advc20_10
{
    class RecursiveSearch
    {
        private List<int> m_numbers;
        private Dictionary<int, long> m_answers = new Dictionary<int, long>();

        public RecursiveSearch(List<int> numbers)
        {
            m_numbers = numbers;
            m_answers[numbers.Count - 1] = 1;    // Leaf node
        }

        public long Solve(int fromIndex = 0)
        {
            if (m_answers.ContainsKey(fromIndex))
            {
                return m_answers[fromIndex];
            }

            int curNum = m_numbers[fromIndex];
            long cnt = 0;

            for (int i = fromIndex + 1; i <= fromIndex + 3 && i < m_numbers.Count; i ++)
            {
                if (m_numbers[i] - curNum <= 3)
                {
                    cnt += Solve(i);
                }
            }

            m_answers[fromIndex] = cnt;

            return cnt;
        }
    }

    static void solvePart2(List<int> numbers)
    {
        var search = new RecursiveSearch(numbers);

        long ans = search.Solve();

        Console.WriteLine($"Solve 2 : {ans}");
    }

    static void solvePart1(List<int> numbers)
    {
        int cntDiff1 = 0;
        int cntDiff3 = 0;

        for(int i = 0; i < numbers.Count - 1; i ++)
        {
            int diff = numbers[i + 1] - numbers[i];

            cntDiff3 += (diff == 3) ? 1 : 0;
            cntDiff1 += (diff == 1) ? 1 : 0;
        }

        Console.WriteLine($"Solve1 : {cntDiff1} * {cntDiff3} = {cntDiff1 * cntDiff3}");
    }

	static void SolveMain(string path)
	{
        Console.WriteLine($"Reading File {path}");
		var lines = File.ReadLines(path);

        var numbers = new List<int>();

		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
                numbers.Add(int.Parse(line));
			}
		}

        numbers.Add(0);
        numbers.Sort();
        numbers.Add(numbers[numbers.Count - 1] + 3);

        solvePart1(numbers);
        solvePart2(numbers);
	}

	static void Run()
	{
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

		Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
		SolveMain($"../../data/{className}.txt");
	}

    static void Main()
    {
        Run();
    }        
}