
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class Advc20_19
{
	static bool s_debugWrite = false;

	static void debugWrite(string s)
	{
		if (s_debugWrite)
		{
			Console.WriteLine(s);
		}
	}

	class SolveState
	{
		public string Path { get; private set; } = "";
		public string Input { get; }
		public int Pos { get; set; } = 0;

		public SolveState(SolveState other)
		{
			Path = new string(other.Path);
			Input = other.Input;
			Pos = other.Pos;
		}

		public SolveState(string str)
		{
			Input = str;
		}

		public char GetNextChar()
		{
			return Input[Pos++];
		}

		public override string ToString()
		{
			return $"[STATE {Path} {Pos/Input.Length}]";
		}

		public void AddPath(int index)
		{
			Path += $"[{index}]";
		}

		public void AddPath(int index, char c)
		{
			Path += $"[{index}:{c}]";
		}

		public bool HasMoreChar()
		{
			return Pos < Input.Length;
		}
	}

	class SolveStateQueue
	{
		Queue<SolveState> m_states = new Queue<SolveState>();
		public SolveStateQueue(SolveState state)
		{
			m_states.Enqueue(state);
		}

		public SolveStateQueue()
		{
		}

		public void Enqueue(SolveState state)
		{
			m_states.Enqueue(state);
		}

		public SolveState Dequeue()
		{
			return m_states.Dequeue();
		}

		public void AddRange(SolveStateQueue other)
		{
			while (other.HasSomething())
			{
				m_states.Enqueue(other.m_states.Dequeue());
			}
		}

		public bool HasSomething()
		{
			return m_states.Count > 0;
		}

		public void Purge()
		{
			Queue<SolveState> newQueue = new Queue<SolveState>();

			foreach (SolveState state in m_states)
			{
				if (!state.HasMoreChar())
				{
					newQueue.Enqueue(state);
				}
			}

			m_states = newQueue;
		}
	}

	static readonly SolveStateQueue EmptySolveQueue = new SolveStateQueue();

	abstract class Rule
	{
		public static Solver s_solver = new Solver();

		public int Index = 0;
		public abstract SolveStateQueue Match(SolveState state);

		public static Rule Parse(string str)
		{
			if (str[0] == '"')
			{
				return new RuleLiteral(str[1]);
			}
			if (str.Contains("|"))
			{
				return new RuleORSet(str);
			}
			else
			{
				return new RuleANDSet(str);
			}
		}
	}

	class RuleLiteral : Rule
	{
		public char Value { get; }

		public RuleLiteral(char c)
		{
			Value = c;
		}

		public override SolveStateQueue Match(SolveState state)
		{
			state.AddPath(Index, Value);

			if (state.HasMoreChar())
			{
				char c = state.GetNextChar();

				if (c == Value)
				{
					return new SolveStateQueue(state);
				}
			}

			return EmptySolveQueue;
		}
	}

	class RuleIndirect : Rule
	{
		public int OtherIndex { get; }
		public RuleIndirect(int otherIndex)
		{
			OtherIndex = otherIndex;
		}

		public override SolveStateQueue Match(SolveState state)
		{
			state.AddPath(Index);

			return Rule.s_solver.GetRuleAt(OtherIndex).Match(state);
		}
	}

	class RuleANDSet : Rule
	{
		List<Rule> m_rules = new List<Rule>();
		public RuleANDSet(string str)
		{
			string[] ruleIndexes = str.Split(' ');

			foreach (string ruleIndexStr in ruleIndexes)
			{
				int index = int.Parse(ruleIndexStr);
				Rule rule = new RuleIndirect(index);
				m_rules.Add(rule);
			}
		}

		public override SolveStateQueue Match(SolveState state)
		{
			state.AddPath(Index);

			SolveStateQueue queue = new SolveStateQueue(state);

			foreach (Rule rule in m_rules)
			{
				SolveStateQueue queueForThisRule = new SolveStateQueue();

				while (queue.HasSomething())
				{
					SolveState curState = new SolveState(queue.Dequeue());

					queueForThisRule.AddRange(rule.Match(curState));
				}

				if (!queueForThisRule.HasSomething())
				{
					break;	// no answer
				}

				queue = queueForThisRule;
			}
			return queue;
		}		
	}

	class RuleORSet : Rule
	{
		List<RuleANDSet> m_andRules = new List<RuleANDSet>();

		public RuleORSet(string str)
		{
			string[] sets = str.Split(" | ");

			foreach (string set in sets)
			{
				m_andRules.Add(new RuleANDSet(set));
			}
		}

		public override SolveStateQueue Match(SolveState state)
		{
			state.AddPath(Index);

			SolveStateQueue queue = new SolveStateQueue();

			foreach (Rule rule in m_andRules)
			{
				queue.AddRange(rule.Match(new SolveState(state)));
			}
			return queue;			
		}
	}

	class Solver
	{
		Dictionary<int, Rule> m_rules = new Dictionary<int, Rule>();
		List<string> m_inputs = new List<string>();

		public Rule GetRuleAt(int index)
		{
			return m_rules[index];
		}

		public void parseLine(string line)
		{
			if (char.IsLetter(line[0]))
			{
				m_inputs.Add(line);
			}
			else
			{
				string[] strs = line.Split(": ");
				int index = int.Parse(strs[0]);
				
				Rule rule = Rule.Parse(strs[1]);
				rule.Index = index;

				m_rules[rule.Index] = rule;
			}
		}

		int SolveInternal()
		{
			int cnt = 0;
			Rule.s_solver = this;

			foreach (string input in m_inputs)
			{
				SolveStateQueue queue = m_rules[0].Match(new SolveState(input));

				queue.Purge();

				if (queue.HasSomething())
				{
					debugWrite($"Possible answers for {input}");

					while (queue.HasSomething())
					{
						SolveState state = queue.Dequeue();

						debugWrite($"  => {state.Path}");
					}
					cnt ++;
				}
			}

			return cnt;
		}

		public void SolvePart1()
		{
			int cnt = SolveInternal();
			Console.WriteLine($"Solve 1 ans = {cnt}");

			// Part 2 override
			m_rules[8] = Rule.Parse("42 | 42 8");
			m_rules[11] = Rule.Parse("42 31 | 42 11 31");

			cnt = SolveInternal();
			Console.WriteLine($"Solve 2 ans = {cnt}");
		}
	}

	static void solve(string path)
	{
		var lines = File.ReadLines(path);

		Solver solver = new Solver();
		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
				solver.parseLine(line);
			}
		}

		solver.SolvePart1();
	}

	static void Run()
	{
		var classType = new StackFrame().GetMethod()?.DeclaringType;
		string className = classType != null? classType.ToString() : "Advc";

		Console.WriteLine($"Starting {className}");
		className.ToString().ToLower();

		solve($"../../data/{className}_sample.txt");
		solve($"../../data/{className}.txt");
	}

	static void Main()
	{
		Run();
	}    
}