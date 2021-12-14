
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

class Advc21_14
{	
   static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    struct Pair
    {
        public char a, b;
        public Pair(char x, char y)
        {
            a = x; 
            b = y;
        }

        public override string ToString()
        {
            return $"[{a},{b}]";
        }
    }

    class ResultSet
    {
        Dictionary<char, long> m_result { get; set; } = new Dictionary<char, long>();

        public void Merge(ResultSet other, char intersection = '0')
        {
            foreach(var item in other.m_result)
            {
                if (!m_result.ContainsKey(item.Key))
                {
                    m_result[item.Key] = 0;
                }
                m_result[item.Key] += item.Value;
            }

            if (m_result.ContainsKey(intersection))
            {
                m_result[intersection] --;
            }
            
        }

        public void AddValue(char c, int value)
        {
            if (!m_result.ContainsKey(c))
            {
                m_result[c] = 0;
            }
            m_result[c] += value;
        }

        public void PrintAll()
        {
            foreach(var item in m_result)
            {
                Console.WriteLine($"  Result {item.Key} = {item.Value}");
            }
            Console.WriteLine("");
        }

        public long DiffMaxMin()
        {
            long min = long.MaxValue;
            long max = 0;

            foreach(long value in m_result.Values)
            {
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }

            return max - min;
        }
    }

    class ResultSetPerDepth
    {
        public Dictionary<int,  ResultSet> Result { get; set; } = new Dictionary<int, ResultSet>();
    }

    class RuleSet
    {
        public Dictionary<Pair, char> m_rules { get; set; }= new Dictionary<Pair, char>();

        public void AddRule(string line)
        {
            string[] split = line.Split(" -> ");

            Pair pair;
            pair.a = split[0][0];
            pair.b = split[0][1];

            m_rules[pair] = split[1][0];
        }

        public char GetRule(Pair p)
        {
            return m_rules[p];
        }

        public char GetRule(char a, char b)
        {
            return m_rules[new Pair(a, b)];
        }
    }

    class Solver2
    {
        public Dictionary<Pair, ResultSetPerDepth> Matrix { get; set; } = new Dictionary<Pair, ResultSetPerDepth>();
        readonly RuleSet m_ruleSet;
        readonly string m_sequence;

        public Solver2(RuleSet ruleSet, string sequence)
        {
            m_ruleSet = ruleSet;
            m_sequence = sequence;
        }

        private ResultSet GetResultSet(Pair pair, int depth)
        {
            ResultSetPerDepth? resultDepth;

            // Already known
            if (Matrix.TryGetValue(pair, out resultDepth))
            {
                ResultSet? result;
                if (resultDepth.Result.TryGetValue(depth, out result))
                {
                    return result;
                }
            }
            else
            {
                resultDepth = new ResultSetPerDepth();
                Matrix[pair] = resultDepth;
            }

            // Leaf condition
            if (depth == 0)
            {
                ResultSet set = new ResultSet();
                set.AddValue(pair.a, 1);
                set.AddValue(pair.b, 1);

                debugWrite($"Solved leaf {pair}:{depth}");
                //set.PrintAll();

                resultDepth.Result[depth] = set;
                return set;
            }

            // Combination
            char c = m_ruleSet.GetRule(pair);

            Pair first = new Pair(pair.a, c);
            Pair second = new Pair(c, pair.b);

            int prevDepth = depth - 1;
            ResultSet myResult = new ResultSet();
            ResultSet resultFirst = GetResultSet(first, prevDepth);
            ResultSet resultSecond = GetResultSet(second, prevDepth);

            myResult.Merge(resultFirst);
            myResult.Merge(resultSecond, c);

            resultDepth.Result[depth] = myResult;

            debugWrite($"Solved {pair}:{depth} by adding {first}:{prevDepth} and {second}:{prevDepth}");
            //myResult.PrintAll();

            return myResult;
        }

        public void Solve()
        {
            int depth = 40;
            ResultSet result = GetResultSet(new Pair(m_sequence[0], m_sequence[1]), depth);

            for (int i = 1; i < m_sequence.Length - 1; i ++)
            {
                Pair pair = new Pair(m_sequence[i], m_sequence[i + 1]);

                ResultSet oneResult = GetResultSet(pair, depth);
                result.Merge(oneResult, m_sequence[i]);
            }

            result.PrintAll();
            long ans = result.DiffMaxMin();

            Console.WriteLine($"Solve2 ans = {ans}");
        }
    }

    class Solver1
    {
        readonly string m_sequence;
        readonly RuleSet m_rule;

        public Solver1(RuleSet ruleSet, string seq)
        {
            m_rule = ruleSet;
            m_sequence = seq;
        }
        private Dictionary<char, int> CountElement(string sequence)
        {
            var dic = new Dictionary<char, int>();
            foreach (char c in sequence)
            {
                if (!dic.ContainsKey(c))
                {
                    dic[c] = 0;
                }

                dic[c] ++;
            }

            return dic;
        }        

        public string DoRound(string sequence)
        {
            string newSeq = "";

            debugWrite($"solving {sequence}");

            for (int i = 0; i < sequence.Length; i ++)
            {
                newSeq += sequence[i];

                debugWrite($"[{i}/{sequence}/{sequence[i]}] {newSeq}  ");

                if (i < sequence.Length - 1)
                {
                     string combination = sequence.Substring(i, 2);
                     newSeq += m_rule.GetRule(sequence[i], sequence[i + 1]);
                }
            }

            debugWrite($"{newSeq}");
            return newSeq;

        }

        public void Solve()
        {
            string sequence = m_sequence;
            
            for (int i = 0; i < 10; i ++)
            {
                sequence = DoRound(sequence);
                debugWrite($"round {i}, length {sequence.Length}");
            }

            var counter = CountElement(sequence);

            int min = int.MaxValue;
            int max = 0;

            foreach (var item in counter)
            {
                min = Math.Min(min, item.Value);
                max = Math.Max(max, item.Value);
            }

            int ans = max - min;

            Console.WriteLine($"Solve1 ans = {ans}");
        }
    }

    static void SolveMain(string path)
	{
        Console.WriteLine($"Reading File {path}");
		var lines = File.ReadLines(path);

        RuleSet ruleSet = new RuleSet();
        string sequence = "";
		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
                if (sequence.Length > 0)
                {
                    ruleSet.AddRule(line);
                }
                else 
                {
                    sequence = line;
                }
			}
		}

        Solver1 solver1 = new Solver1(ruleSet, sequence);
        solver1.Solve();

        Solver2 solver2 = new Solver2(ruleSet, sequence);
        solver2.Solve();
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