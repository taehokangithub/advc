
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc20_17
{
	static bool s_debugWrite = false;

	static void debugWrite(string s)
	{
		if (s_debugWrite)
		{
			Console.WriteLine(s);
		}
	}

	static List<Point> DIRPOINTS = new List<Point>();

	static void GenerateDirPoints()
	{
		for (int x = -1; x <= 1; x ++)
		{
			for (int y = -1; y <= 1; y ++)
			{
				for (int z = -1; z <= 1; z ++)
				{
					for (int w = -1; w <= 1; w ++)
					{
						if (x == 0 && y == 0 && z == 0 && w == 0)
						{
							continue;
						}
						DIRPOINTS.Add(new Point(x, y, z, w));
					}

				}
			}
		}
	}
	
	static bool DecodeChar(char c)
	{
		Debug.Assert(c == '#' || c == '.');
		return c == '#';
	}

	class Point
	{
		public int X = 0;
		public int Y = 0;
		public int Z = 0;
		public int W = 0;
		
		public Point() 
		{
		}

		public Point(Point other)
		{
			X = other.X;
			Y = other.Y;
			Z = other.Z;
			W = other.W;
		}

		public Point(int x, int y, int z, int w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Point operator -(Point a, Point b)
		{
			Point p = new Point();

			p.X = a.X - b.X;
			p.Y = a.Y - b.Y;
			p.Z = a.Z - b.Z;
			p.W = a.W - b.W;

			return p;            
		}

		public static Point operator +(Point a, Point b)
		{
			Point p = new Point();

			p.X = a.X + b.X;
			p.Y = a.Y + b.Y;
			p.Z = a.Z + b.Z;
			p.W = a.W + b.W;

			return p;
		}

		public void SetMax(Point p)
		{
			X = Math.Max(X, p.X);
			Y = Math.Max(Y, p.Y);
			Z = Math.Max(Z, p.Z);
			W = Math.Max(W, p.W);
		}

		public void SetMin(Point p)
		{
			X = Math.Min(X, p.X);
			Y = Math.Min(Y, p.Y);
			Z = Math.Min(Z, p.Z);
			W = Math.Min(W, p.W);
		}

		public override string ToString()
		{
			return $"[{X},{Y},{Z},{W}]";
		}
	}

	class Grid
	{
		HashSet<string> m_grid = new HashSet<string>(); // stores string representation of each active points
		Point m_max = new Point();
		Point m_min = new Point();

		public void AddLine(string line)
		{
			m_max.X = 0;
			foreach (char c in line)
			{
				if (c == '#')
				{
					m_grid.Add(m_max.ToString());
				}

				m_max.X ++;
			}
			m_max.Y ++;
		}

		bool GetAt(Point p)
		{
			return m_grid.Contains(p.ToString());
		}

		void SetAt(Point p)
		{
			m_grid.Add(p.ToString());
		}

		void RemoveAt(Point p)
		{
			if (GetAt(p))
			{
				m_grid.Remove(p.ToString());
			}		
		}

		int CountNeighbours(Point p)
		{
			int count = 0;
			foreach (Point dir in DIRPOINTS)
			{
				if (GetAt(dir + p))
				{
					count ++;
				}
			}
			return count;
		}

		void Process()
		{
			Point curMin = new Point(m_min);
			Point curMax = new Point(m_max);

			Console.WriteLine($"Start processing. current active {m_grid.Count} from {curMin} to {curMax}");

			HashSet<string> newGrid = new HashSet<string>();

			for (int w = curMin.W - 1; w <= curMax.W + 1; w++)
			{
				for (int z = curMin.Z - 1; z <= curMax.Z + 1; z ++)
				{
					for (int y = curMin.Y - 1; y <= curMax.Y + 1; y ++)
					{
						for (int x = curMin.X - 1; x <= curMax.X + 1; x++)
						{
							Point p = new Point(x, y, z, w);
							
							int cnt = CountNeighbours(p);
							bool val = GetAt(p);

							bool add = false;
							if (val && (cnt == 2 || cnt == 3))
							{
								add = true;
							}
							if (!val && (cnt == 3))
							{
								add = true;
							}
							if (add)
							{
								newGrid.Add(p.ToString());
								m_max.SetMax(p);
								m_min.SetMin(p);
							}

							debugWrite($"Checking {p}, cnt {cnt}, value {add} ");
						}
					}
				}

			}
			
			m_grid = newGrid;

			Console.WriteLine($"Process finished, current active {m_grid.Count}, new min {m_min} max {m_max}\n");
		}

		public void Solve()
		{
			for (int i = 0; i < 6; i ++)
			{
				Process();
			}

			Console.WriteLine($"Solve1 ans = {m_grid.Count}");
		}
	}

	static void SolveMain(string path)
	{
		Console.WriteLine($"Reading File {path}");
		var lines = File.ReadLines(path);

		Grid grid = new Grid();

		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
				grid.AddLine(line);
			}
		}

		grid.Solve();
	}


	static void Run()
	{
		GenerateDirPoints();

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