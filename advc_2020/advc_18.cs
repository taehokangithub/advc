
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


class Advc20_18
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

	class StringStream
	{
		public string Str{ get; }
		public int Pos { get; set; }

		public StringStream(string s)
		{
			Str = s;
		}

		public string GetToken()
		{
			string token = "";

			while (Pos < Str.Length)
			{
				char c = Str[Pos++];

				if (c == ' ' || c == '\n')
				{
					break;
				}

				if (c == ')' && token.Length > 0)
				{
					Pos --;
					break;
				}

				token += c;
			}
			
			return token;
		}
	}

	static long expression(StringStream str, bool isOp2 = false)
	{
		if (str.Str[str.Pos] == '(')
		{
			str.Pos ++;
			return isOp2 ? operation2(str) : operation1(str);
		}

		string exp = str.GetToken();
		return Convert.ToInt64(exp);
	}

	static long operation1(StringStream str)
	{
		long val = expression(str);

		while(true)
		{
			string op = str.GetToken();

			if (op.Length > 0)
			{
				if (op == ")")
				{
					break;
				}

				long operand = expression(str);

				if (op == "+")
				{
					val += operand;
				}
				else if (op == "*")
				{
					val *= operand;
				}
			}
			else
			{
				break;
			}
		}

		return val;
	}

	static long operation2(StringStream str)
	{
		List<long> values = new List<long>();
		List<char> ops = new List<char>();

		values.Add(expression(str, isOp2:true));

		while(true)
		{
			string op = str.GetToken();

			if (op.Length > 0)
			{
				if (op == ")")
				{
					break;
				}

				Debug.Assert(op.Length == 1);
				ops.Add(op[0]);
				values.Add(expression(str, isOp2:true));
			}
			else
			{
				break;
			}
		}

		List<long> valuesForMul = new List<long>();

		for (int i = 0; i <= ops.Count; i ++)
		{
			long v = values[i];

			while(i < ops.Count && ops[i] == '+')
			{
				v += values[++i];
			}

			valuesForMul.Add(v);
		}

		long val = 1;

		foreach (long v in valuesForMul)
		{
			val *= v;
		}
		return val;
	}	

    static void solve(string path)
    {
        var lines = File.ReadLines(path);

		long solve1ans = 0;
		long solve2ans = 0;
        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
				long ret1 = operation1(new StringStream(line));
				long ret2 = operation2(new StringStream(line));

				Console.WriteLine($"ret : {ret1}, {ret2}");

				solve1ans += ret1;
				solve2ans += ret2;
			}
        }

		Console.WriteLine($"ans = {solve1ans}, {solve2ans}");
    }

    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        //solve($"../../data/{className}_sample.txt");
		//solve($"../../data/{className}_sample2.txt");
        solve($"../../data/{className}.txt");
    }

    static void Main()
    {
        Run();
    }    
}