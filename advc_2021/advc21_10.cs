
using System;
using System.Collections.Generic;
using System.IO;


class Advc21_10
{

    const string s_opener = "([{<";
    const string s_closer = ")]}>";

    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    static int getCorruptScore(char c)
    {
        switch(c)
        {
            case ')' : return 3;
            case ']' : return 57;
            case '}' : return 1197;
            case '>' : return 25137;
        }
        throw new Exception($"Error! unknown char {c}");
    }

    static int getAutotScore(char c)
    {
        switch(c)
        {
            case ')' : return 1;
            case ']' : return 2;
            case '}' : return 3;
            case '>' : return 4;
        }
        throw new Exception($"Error! unknown char {c}");
    }

    static char getCloser(char opener)
    {
        if (!s_opener.Contains(opener))
        {
            throw new Exception($"Error! {opener} is not an opener");
        }

        return s_closer[s_opener.IndexOf(opener)];
    }

    static int getCorruptedLineScore(string line, Stack<char> stack)
    {
        int ret = 0;

        debugWrite($"Handling {line}");

        for (int i = 0; i < line.Length; i ++)
        {
            char c = line[i];

            if (s_opener.Contains(c))
            {
                stack.Push(c);
            }
            else if (s_closer.Contains(c))
            {
                char popped = stack.Pop();
                char expected = getCloser(popped);

                if (expected != c)
                {
                    ret = getCorruptScore(c);
                    debugWrite($"[{i}] Expected {expected}, Found {c}, score {ret}");
                    break;
                }
            }
            else
            {
                throw new Exception($"Error! unknown character {c} from {line}");
            }
        }

        return ret;
    }

	static void solve(string path)
	{
		var lines = File.ReadLines(path);

        int solve1Ans = 0;
        var solve2Scores = new List<long>();

		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
                var stack = new Stack<char>();

                int corruptScore = getCorruptedLineScore(line, stack);

                solve1Ans += corruptScore;

                if (corruptScore == 0)
                {
                    long autoScore = 0;
                    while(stack.Any())
                    {
                        autoScore = (autoScore * 5) + getAutotScore(getCloser(stack.Pop()));
                    }

                    debugWrite($"Autoscore {autoScore}");
                    solve2Scores.Add(autoScore);
                }
			}
		}

        solve2Scores.Sort();
        long solve2Ans = solve2Scores[solve2Scores.Count / 2];

        Console.WriteLine($"Solve1 {solve1Ans}");
        Console.WriteLine($"Solve2 {solve2Ans}");
	}

	static void Run()
	{
		Console.WriteLine("Starting Advc21_05");

		solve("../../data/advc21_10 sample.txt");
        solve("../../data/advc21_10.txt");
	}

	static void Main()
	{
		Run();
	}    
}